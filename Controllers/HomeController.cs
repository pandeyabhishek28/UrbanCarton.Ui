using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrbanCarton.Mvc.Clients;
using UrbanCarton.Mvc.Models;

namespace UrbanCarton.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductHttpClient _productHttpClient;
        private readonly ProductGraphClient _productGraphClient;

        public HomeController(ProductHttpClient productHttpClient,
            ProductGraphClient productGraphClient)
        {
            _productHttpClient = productHttpClient;
            _productGraphClient = productGraphClient;
        }


        public async Task<IActionResult> Index()
        {
            var responseModel = await _productHttpClient.GetProducts();
            responseModel.ThrowErrors();
            return View(responseModel.Data.Products);
        }

        public async Task<IActionResult> ProductDetail(int productId)
        {
            _productGraphClient.SubscribeToUpdates();
            var product = await _productGraphClient.GetProduct(productId);
            return View(product);
        }

        public IActionResult AddReview(int productID)
        {
            return View(new ProductReviewModel { ProductId = productID });
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ProductReviewModel productReviewModel)
        {
            await _productGraphClient.AddReview(productReviewModel);
            return RedirectToAction("ProductDetail",
                new { productId = productReviewModel.ProductId });
        }
    }
}