using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuthorizationIntDotNet5.Contants;

namespace PermissionBasedAuthorizationIntDotNet5.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.Products.Edit)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}