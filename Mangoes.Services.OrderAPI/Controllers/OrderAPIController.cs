using AutoMapper;
using Mangoes.Services.OrderAPI.Data;
using Mangoes.Services.OrderAPI.Model;
using Mangoes.Services.OrderAPI.Model.DTO;
using Mangoes.Services.OrderAPI.Service.IService;
using Mangoes.Services.OrderAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Collections;

namespace Mangoes.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        public AppDBContext _db;
        private IMapper _mapper = null;
        ResponseDTO _responseDTO;
        private IProductServicecs _productServicecs;
        public OrderAPIController(AppDBContext db, IMapper mapper, IProductServicecs productServicecs)
        {
            _db = db;
            _mapper = mapper;
            _responseDTO = new ResponseDTO();
            _productServicecs = productServicecs;
        }

        [HttpPost("CreateOrders")]
        public async Task<ResponseDTO> CreateOrders(CartDTO cartDTO )
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(cartDTO.CartHeaderDTO);
                orderHeaderDTO.StatusCode = SD.Status_Pending;
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.OrderDetailsList = _mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.CartDetailsDTOLists);
                OrderHeader orderCreated =  _db.OrderHeader.Add(_mapper.Map<OrderHeader>(orderHeaderDTO)).Entity;
                await _db.SaveChangesAsync();
                orderHeaderDTO.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDTO.Result = orderHeaderDTO;   
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDTO> CreateSession([FromBody]StripeRequestDTO stripeRequestDTO)
        {
            try
            {
               // StripeConfiguration.ApiKey = "sk_test_51PhomWHtx5G7DxaW84S2EMe4mkdx2Gw3N051LEZC2FVVXCMR7BOR4CSuyvhKLNgATcjdsH4fv13WZVoWmoOtYuwV00fG7CnVuK";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDTO.ApprovelURL,
                    CancelUrl = stripeRequestDTO.CancelURL,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };
                var DiscountObj = new List<SessionDiscountOptions>
                  {
                      new SessionDiscountOptions
                      {
                          Coupon = stripeRequestDTO.OrderHeaderDTO.CouponCode
                      }
                  };

                foreach (var items in stripeRequestDTO.OrderHeaderDTO.OrderDetailsList)
                {
                    var sessionLineItems = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(items.Quantity * 23),
                            Currency = "AED",
                            ProductData = new SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = items.Product.Name
                            }
                        },
                        Quantity = items.Quantity
                    };
                    options.LineItems.Add(sessionLineItems);
                }
                if(stripeRequestDTO.OrderHeaderDTO.Discount>0)
                {
                    options.Discounts = DiscountObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDTO.StripeSessionId = session.Id;
                stripeRequestDTO.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeader.First(x => x.OrderHeaderId == stripeRequestDTO.OrderHeaderDTO.OrderHeaderId);
                if(orderHeader!=null)
                {
                    orderHeader.StripeSessionId = session.Id;
                }
                _db.SaveChanges();
                _responseDTO.Result = stripeRequestDTO;

            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;

        }

        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDTO> ValidateStripeSession(int orderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeader.First(x=>x.OrderHeaderId == orderId);


                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);
                var paymentService = new PaymentIntentService();
                PaymentIntent payment = paymentService.Get(session.PaymentIntentId);
                if(payment.Status== "succeeded")
                {
                    orderHeader.PaymentIntentId = payment.Id;
                    orderHeader.StatusCode = SD.Status_Approved;
                   await _db.SaveChangesAsync();
                }
                _responseDTO.Result = _mapper.Map<OrderHeaderDTO>(orderHeader);

            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;

        }
    }
}
