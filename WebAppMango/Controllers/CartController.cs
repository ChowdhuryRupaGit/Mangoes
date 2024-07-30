using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using WebAppMango.Models.DTO;
using WebAppMango.Services;
using WebAppMango.Utilities;

namespace WebAppMango.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> CartIndex()
         {
            try
            {
                var userId = User.Claims.Where(x=>x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                ResponseDTO? responseDTO = await _cartService.GetCart(userId);
                CartDTO cartDTO = null;
                    
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(responseDTO.Result));
                }
                return View(cartDTO);
            }
             catch (Exception ex) 
            {
                TempData["error"] = ex.Message;
            }
             return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            return View(await LoggedInUser());
        }


        [HttpGet]
        public async Task<IActionResult> Confirmation(int orderId)
        {
            try
            {
                ResponseDTO? responseDTO = await _orderService.ValidateStripeSession(orderId);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(responseDTO.Result));
                    if(orderHeaderDTO.StatusCode == SD.Status_Approved)
                    {
                        return View(orderId);
                    }                   
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();     
        }


        [HttpPost]
        [ActionName("CheckOut")]
        public async Task<IActionResult> CheckOut(CartDTO cartDTO)
        {
            try
            {
                CartDTO cart = await LoggedInUser();
                cart.CartHeaderDTO.PhoneNumber = cartDTO.CartHeaderDTO.PhoneNumber;
                cart.CartHeaderDTO.Name = cartDTO.CartHeaderDTO.Name;
                cart.CartHeaderDTO.EmailId = cartDTO.CartHeaderDTO.EmailId;

                ResponseDTO? responseDTO = await _orderService.CreateOrder(cart);
                OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(responseDTO.Result));
                if (responseDTO.IsSuccess && responseDTO != null)
                {
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                    StripeRequestDTO stripeRequestDTO = new StripeRequestDTO()
                    {
                        ApprovelURL = domain + "cart/Confirmation?orderId=" + orderHeaderDTO.OrderHeaderId,
                        CancelURL = domain + "cart/CheckOut",
                        OrderHeaderDTO = orderHeaderDTO
                    };
                    var response = await _orderService.CreateSession(stripeRequestDTO);
                    StripeRequestDTO stripeRequest = JsonConvert.DeserializeObject<StripeRequestDTO>(Convert.ToString(response.Result));
                    Response.Headers.Add("Location",stripeRequest.StripeSessionUrl);
                    return new StatusCodeResult(303);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDTO cartDTO)
        {
                CartDTO cart = await LoggedInUser();
                cart.CartHeaderDTO.EmailId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
                ResponseDTO? responseDTO = await _cartService.EmailCart(cart);

                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Email will be processed and sent shortly.";
                    return RedirectToAction(nameof(CartIndex));
                }
              
                return View(cartDTO);
            }

        private async Task<CartDTO> LoggedInUser()
        {
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTO? responseDTO = await _cartService.GetCart(userId);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(responseDTO.Result));
                return cartDTO;
            }
            return new CartDTO();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            try
            {
               var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                ResponseDTO? responseDTO = await _cartService.RemoveCart(cartDetailsId);

                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Item has been removed from cart";
                    return RedirectToAction(nameof(CartIndex));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                ResponseDTO? responseDTO = await _cartService.ApplyCoupon(cartDTO);

                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Coupon has been added to the Total Amount";
                    return RedirectToAction(nameof(CartIndex));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }



        [HttpPost]

        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            try
            {
                cartDTO.CartHeaderDTO.CouponCode = "";
                var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                ResponseDTO? responseDTO = await _cartService.RemoveCoupon(cartDTO);

                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Coupon has been removed from the Total Amount";
                    return RedirectToAction(nameof(CartIndex));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }
    }
}
