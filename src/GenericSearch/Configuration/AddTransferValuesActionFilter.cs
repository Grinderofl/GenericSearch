using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GenericSearch.ActionFilters;
using GenericSearch.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AddTransferValuesActionFilter : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<TransferValuesActionFilter>();
        }
    }
}