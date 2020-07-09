using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericSearch.Sample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace GenericSearch.Sample.Features.Products
{
    public class AddIndexFiltersAttribute : TypeFilterAttribute
    {
        public AddIndexFiltersAttribute() : base(typeof(AddIndexFilters))
        {
        }

        private class AddIndexFilters : AsyncViewDataFilter
        {
            private readonly NorthwindDbContext context;

            public AddIndexFilters(NorthwindDbContext context) => this.context = context;

            protected override async Task ModifyViewDataAsync(ViewDataDictionary viewData, CancellationToken cancellationToken)
            {
                viewData["SupplierCompanyName"] = await context.Suppliers
                    .Select(x => x.CompanyName)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(x => new SelectListItem(x, x.ToLowerInvariant()))
                    .ToListAsync(cancellationToken);

                viewData["Category"] = await context.Categories
                    .Select(x => x.CategoryName)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(x => new SelectListItem(x, x.ToLowerInvariant()))
                    .ToListAsync(cancellationToken);
            }
        }
    }
}