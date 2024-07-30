using AutoMapper;
using Mango.MessageService;
using Mangoes.Services.CartAPI.Data;
using Mangoes.Services.CartAPI.Model;
using Mangoes.Services.CartAPI.Model.DTO;
using Mangoes.Services.CartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mangoes.Services.CartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
   // [Authorize]
    public class CartAPIController : ControllerBase
    {
        public AppDBContext _db;
        IMapper mapper = null;
        ResponseDTO _responseDTO;
        IProductServicecs _productServicecs;
        ICouponService _coupService;
        IMessageService messageService;
        IConfiguration configuration;
        public CartAPIController(AppDBContext db, IMapper mapp, IProductServicecs productServicecs, 
            ICouponService coupService, IMessageService _messageService, IConfiguration _configuration)
        {
            _db = db;
            mapper = mapp;
            _responseDTO = new ResponseDTO();
            _productServicecs = productServicecs;
            _coupService = coupService;
            messageService = _messageService;
            configuration = _configuration;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> Upsert(CartDTO cartDTO)
        {
            try
            {
                var cardheaderFromDb = await _db.CartHeader.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDTO.CartHeaderDTO.UserId);
                if(cardheaderFromDb == null)
                {
                    //create CartHeader
                    CartHeader cartHeader = mapper.Map<CartHeader>(cartDTO.CartHeaderDTO);
                    _db.CartHeader.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    //create cartdetail
                    cartDTO.CartDetailsDTOLists.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = mapper.Map<CartDetails>(cartDTO.CartDetailsDTOLists.First());
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsFromDB = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(x=>x.ProductId == cartDTO.CartDetailsDTOLists.FirstOrDefault().ProductId
                                    && x.CartHeaderId == cardheaderFromDb.CartHeaderId);
                    if(cartDetailsFromDB == null)
                    {
                        //add product
                        cartDTO.CartDetailsDTOLists.First().CartHeaderId = cardheaderFromDb.CartHeaderId;
                        CartDetails cartDetails = mapper.Map<CartDetails>(cartDTO.CartDetailsDTOLists.First());
                        _db.CartDetails.Add(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update the quantity
                        cartDTO.CartDetailsDTOLists.First().Quantity += cartDetailsFromDB.Quantity;
                        cartDTO.CartDetailsDTOLists.First().CartHeaderId = cartDetailsFromDB.CartHeaderId;
                        cartDTO.CartDetailsDTOLists.First().CartDetailsId = cartDetailsFromDB.CartDetailsId;
                        _db.CartDetails.Update(mapper.Map<CartDetails>(cartDTO.CartDetailsDTOLists.First()));
                        await _db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails =  _db.CartDetails.First(x => x.CartDetailsId == cartDetailsId);
                int cartCount = _db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (cartCount == 1)
                {
                    var cartHeaderToBeRemoved = await _db.CartHeader.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    if (cartHeaderToBeRemoved != null)
                    {
                        _db.CartHeader.Remove(cartHeaderToBeRemoved);
                       
                    }
                   
                }
                await _db.SaveChangesAsync();
                _responseDTO.Result = true;
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = _db.CartHeader.First(x=>x.UserId == cartDTO.CartHeaderDTO.UserId);
                cartFromDb.CouponCode = cartDTO.CartHeaderDTO.CouponCode;
                _db.CartHeader.Update(cartFromDb);
                await  _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = _db.CartHeader.First(x => x.UserId == cartDTO.CartHeaderDTO.UserId);
                cartFromDb.CouponCode = "";
                _db.CartHeader.Update(cartFromDb);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDTO cartDTO)
        {
            try
            {
              string topicQueueName = configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCart");
              await messageService.PublishMessage(cartDTO, topicQueueName);
              _responseDTO.Result = true;
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }


        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cartDto = new()
                {
                    CartHeaderDTO = mapper.Map<CartHeaderDTO>(_db.CartHeader.FirstOrDefault(x => x.UserId == userId))
                };
                cartDto.CartDetailsDTOLists = mapper.Map<IEnumerable<CartDetailsDTO>>(_db.CartDetails.Where(x => x.CartHeaderId == cartDto.CartHeaderDTO.CartHeaderId));
                IEnumerable<ProductDTO> listOfProductsDTO = await _productServicecs.GetProductsAsync();
                foreach(var item in cartDto.CartDetailsDTOLists)
                {
                    item.Product = listOfProductsDTO.FirstOrDefault(u=>u.ProductId == item.ProductId);
                    cartDto.CartHeaderDTO.TotalPrice += item.Quantity * item.Product.Price;
                }
                if(!string.IsNullOrEmpty(cartDto.CartHeaderDTO.CouponCode))
                {
                    CouponDTO coupon = await _coupService.GetCoupon(cartDto.CartHeaderDTO.CouponCode);
                    if(coupon!=null && cartDto.CartHeaderDTO.TotalPrice>coupon.MinAmount)
                    {
                        cartDto.CartHeaderDTO.TotalPrice-= coupon.MinAmount;
                        cartDto.CartHeaderDTO.Discount = coupon.MinAmount;
                    }
                }
                _responseDTO.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }
    }
}
