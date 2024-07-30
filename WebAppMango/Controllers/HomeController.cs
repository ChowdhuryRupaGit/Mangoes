using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.IdentityModel.JsonWebTokens;
using WebAppMango.Models;
using WebAppMango.Models.DTO;
using WebAppMango.Services;

namespace WebAppMango.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO> productlist = new List<ProductDTO>();
            ResponseDTO? responseDTO = await _productService.GetAllProductAsync();
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                productlist = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }

            return View(productlist);
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            ProductDTO product = new ProductDTO();
            ResponseDTO? responseDTO = await _productService.GetByIdAsync(productId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
            }

            return View(product);
        }

       // [Authorize]
        [HttpPost]
        [ActionName("ProductDetail")]
        public async Task<IActionResult> ProductDetail(ProductDTO productDTO)
        {
            CartDTO cart = new()
            {
                CartHeaderDTO = new CartHeaderDTO()
                {
                    UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
                }
            };
            CartDetailsDTO cartDetailsDTO = new CartDetailsDTO()
            {
                Quantity = productDTO.Count,
                ProductId = productDTO.ProductId
              
            };
            List<CartDetailsDTO> cartDetailsDTOs = new List<CartDetailsDTO> { cartDetailsDTO };
            cart.CartDetailsDTOLists = cartDetailsDTOs;
            ResponseDTO? responseDTO = await _cartService.UpsertCart(cart);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Item has been added to the cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to add the item to the cart";
            }

            return View(productDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}