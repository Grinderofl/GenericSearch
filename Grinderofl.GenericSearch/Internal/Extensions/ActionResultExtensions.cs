using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Grinderofl.GenericSearch.Internal.Extensions
{
    internal static class ActionResultExtensions
    {
        public static object GetModel(this IActionResult actionResult)
        {
            return actionResult switch
            {
                ViewResult result => result.Model,
                PartialViewResult result => result.Model,
                PageResult result => result.Model,
                JsonResult result => result.Value,
                ViewComponentResult result => result.Model,
                _ => null
            };
        }
    }
}