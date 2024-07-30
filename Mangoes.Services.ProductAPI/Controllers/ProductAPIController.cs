using AutoMapper;
using Mangoes.Services.ProductAPI.Data;
using Mangoes.Services.ProductAPI.Model;
using Mangoes.Services.ProductAPI.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mangoes.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private AppDBContext _dbContext;
        IMapper _mapper = null;
        ResponseDTO _responseDTO;

        public ProductAPIController(AppDBContext db, IMapper mapp)
        {
            _dbContext = db;
            _mapper = mapp;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                IEnumerable<Product> productList = await _dbContext.Products.ToListAsync();
                _responseDTO.Result = _mapper.Map<IEnumerable<ProductDTO>>(productList);

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
                Product product = _dbContext.Products.SingleOrDefault(x => x.ProductId == id);
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }
        [HttpGet]
        [Route("GetByName/{productName}")]
        public ResponseDTO GetByName(string productName)
        {
            try
            {
                Product product = _dbContext.Products.SingleOrDefault(x => x.Name == productName);
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDTO Post([FromBody] ProductDTO productdto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productdto);
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
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
        public ResponseDTO Put([FromBody] ProductDTO productdto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productdto);
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
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
        [Authorize(Roles = "Admin")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product product = _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = ex.Message;
            }
            return _responseDTO;
        }


    }
}
