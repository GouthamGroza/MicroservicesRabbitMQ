using Microsoft.AspNetCore.Mvc;
using ProductOwnerMicroservice.Models;
using ProductOwnerMicroservice.Services;

namespace ProductOwnerMicroservice.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public  ProductsController (IProductService _productService)
        {
            productService = _productService;
        }

        [HttpGet("list")]
        public Task<IEnumerable<ProductDetails>> ProductListAsync()
        {
            var productList = productService.GetProductListAsync();
            return productList;
        }

        [HttpGet("filterlist")]
        public Task<ProductDetails> GetProductByIdAsync(int Id)
        {
            return productService.GetProductByIdAsync(Id);
        }

        [HttpPost("sendoffer")]
        public bool SendProductOfferAsync(ProductOfferDetail productOfferDetails)
        {
            bool isSent = false;
            if (productOfferDetails != null)
            {
                isSent = productService.SendProductOffer(productOfferDetails);
                return isSent;
            }
            return isSent;
        }


            
       
    }
}
