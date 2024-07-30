using AutoMapper;
using Mangoes.Services.CouponAPIS.Data;
using Mangoes.Services.CouponAPIS.Model;
using Mangoes.Services.CouponAPIS.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mangoes.Services.CouponAPIS.Controllers
{
    [Route("api/coupon")]
    [ApiController]
  //  [Authorize]
    public class CouponAPIController : ControllerBase
    {
        public AppDBContext _db;
        IMapper mapper = null;
        ResponseDTO _responseDTO;
        public CouponAPIController(AppDBContext db,IMapper mapp)
        {
            _db = db;
            mapper = mapp;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _responseDTO.Result = mapper.Map<IEnumerable<CouponDTO>>(objList);

            }
            catch (Exception ex) 
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("GetById/{id:int}")]
        public ResponseDTO GetById(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.SingleOrDefault(x => x.CouponId == id);
                _responseDTO.Result = mapper.Map<CouponDTO>(coupon);
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }




        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public ResponseDTO GetByCode(string couponCode)
        {
            try
            {
                Coupon coupon = _db.Coupons.SingleOrDefault(x => x.CouponCode == couponCode);
                _responseDTO.Result = mapper.Map<CouponDTO>(coupon);
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public ResponseDTO Post([FromBody] CouponDTO coupondto)
        {
            try
            {
                Coupon coupon = mapper.Map<Coupon>(coupondto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();
                _responseDTO.Result = mapper.Map<CouponDTO>(coupon);

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(coupondto.DiscountAmount*23),
                    Name = coupondto.CouponCode,
                    Id = coupondto.CouponCode,
                    Currency = "AED"  
                };
                var service = new Stripe.CouponService();
                service.Create(options);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDTO Put([FromBody] CouponDTO coupondto)
        {
            try
            {
                Coupon coupon = mapper.Map<Coupon>(coupondto);
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                _responseDTO.Result = mapper.Map<CouponDTO>(coupon);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = ex.Message;
            }
            return _responseDTO;
        }

        [HttpDelete]
        [Route("{id:int}")]
      //  [Authorize(Roles = "Admin")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(x=>x.CouponId == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
                _responseDTO.Result = mapper.Map<CouponDTO>(coupon);
                var service = new Stripe.CouponService();
                service.Delete(coupon.CouponCode);
            }
            catch(Exception ex )
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = ex.Message;
            }
            return _responseDTO;
        }
    }
}
