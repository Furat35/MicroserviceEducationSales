using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Exceptions;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogService _catalogService;
        public HomeController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            return View(await _catalogService.GetByCourseIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseAsync());
        }

        [HttpGet]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (errorFeature != null && errorFeature.Error is UnAuthorizedException)
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
