using System.Diagnostics.CodeAnalysis;
using GenericSearch.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
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