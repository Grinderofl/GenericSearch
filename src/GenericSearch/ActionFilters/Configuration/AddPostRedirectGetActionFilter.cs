using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GenericSearch.ActionFilters.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AddPostRedirectGetActionFilter : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<PostRedirectGetActionFilter>();
        }
    }
}