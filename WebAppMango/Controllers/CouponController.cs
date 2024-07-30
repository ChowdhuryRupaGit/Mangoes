using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppMango.Models.DTO;
using WebAppMango.Services;

namespace WebAppMango.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO> couponlist = new List<CouponDTO>();
            ResponseDTO? responseDTO  = await _couponService.GetAllCouponAsync();
            if(responseDTO!=null && responseDTO.IsSuccess)
            {
                couponlist = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(responseDTO.Result));
            }

            return View(couponlist);
        }

        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDTO couponDTO)
        {
            if(ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _couponService.AddAsync(couponDTO);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Successfully Created Coupon";
                    return RedirectToAction(nameof(CouponIndex));
                }
            }
            else
            {
                TempData["error"] = "Failed to create Coupon";
            }
            return View(couponDTO);
        }
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            CouponDTO coupondto = new();
            ResponseDTO? responseDTO = await _couponService.GetByIdAsync(couponId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
				coupondto = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responseDTO.Result));
                return View(coupondto);
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _couponService.RemoveAsync(coupon.CouponId);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Successfully Deleted Coupon";
                    return RedirectToAction(nameof(CouponIndex));
                }
            }
            else
            {
                TempData["error"] = "Failed to delete Coupon";
            }
            return NotFound();
        }
    }
}
