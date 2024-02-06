using Microsoft.AspNetCore.Mvc;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;
using System.Text;

namespace StoreMarket.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        // Добавленный метод для возврата CSV-файла с товарами
        [HttpGet]
        [Route("products/csv")]
        public async Task<IActionResult> GetProductsCsv()
        {
            var products = _productServices.GetProducts();
            var csvData = new StringBuilder();

            csvData.AppendLine("Id,Name,Description,Price,CategoryId");

            foreach (var product in products)
            {
                csvData.AppendLine($"{product.Id},{product.Name},{product.Description},{product.Price},{product.CategoryId}");
            }

            var fileName = $"Products_{DateTime.Now:yyyyMMddHHmmss}.csv";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            await System.IO.File.WriteAllTextAsync(filePath, csvData.ToString());

            return PhysicalFile(filePath, "text/csv", fileName);
        }

        [HttpGet]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var product = _productServices.GetProductById(id);

            return Ok(product);
        }

        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var products = _productServices.GetProducts();

            return Ok(products);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<int> AddProducts(ProductCreateRequest request)
        {
            try
            {
                var id = _productServices.AddProduct(request);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
