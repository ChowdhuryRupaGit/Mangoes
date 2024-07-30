using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppMango.Models.DTO;
using WebAppMango.Services;

namespace WebAppMango.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> productlist = new List<ProductDTO>();
            ResponseDTO? responseDTO = await _productService.GetAllProductAsync();
            if(responseDTO!=null && responseDTO.IsSuccess)
            {
                productlist = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }

            return View(productlist);
        }
        [HttpGet]

        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _productService.AddAsync(productDTO);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Successfully Created Product";
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            else
            {
                TempData["error"] = "Failed to create Product";
            }
            return View(productDTO);
        }


        public async Task<IActionResult> ProductUpdate(int productId)
        {
            ProductDTO productdto = new();
            ResponseDTO? responseDTO = await _productService.GetByIdAsync(productId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {

                productdto = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
                return View(productdto);
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }

            return NotFound();
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ProductDTO productdto = new();
            ResponseDTO? responseDTO = await _productService.GetByIdAsync(productId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {

                productdto = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
                return View(productdto);
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDTO product)
        {
            ResponseDTO? responseDTO = await _productService.UpdateAsync(product);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Successfully Updated Product";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _productService.RemoveAsync(product.ProductId);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Successfully Deleted Product";
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            else
            {
                TempData["error"] = "Failed to delete Product";
            }
            return NotFound();
        }
    }
}
