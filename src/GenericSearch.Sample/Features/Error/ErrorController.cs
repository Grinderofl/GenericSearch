using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.Sample.Features.Error
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View(new ErrorModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }
    }
}
