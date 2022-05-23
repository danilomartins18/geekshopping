using GeekShopping.Web.Interfaces.Services;
using GeekShopping.Web.Models;
using GeekShopping.Web.Utils;
using GeekShopping.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.FindAllProducts("");
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _service.CreateProduct(model, token!);
                if (response != null) return RedirectToAction(
                     nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _service.FindProductById(id, token!);
            if (model != null) return View(model);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _service.UpdateProduct(model, token!);
                if (response != null) return RedirectToAction(
                     nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _service.FindProductById(id, token!);
            if (model != null) return View(model);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(ProductViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _service.DeleteProductById(model.Id, token!);
            if (response) return RedirectToAction(
                    nameof(Index));
            return View(model);
        }
    }
}