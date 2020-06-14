using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericSearch.Sample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace GenericSearch.Sample.Features.Employees
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
                viewData["Country"] = await context.Customers
                                                   .Select(x => x.Country)
                                                   .Distinct()
                                                   .OrderBy(x => x)
                                                   .Select(x => new SelectListItem(x, x.ToLowerInvariant()))
                                                   .ToListAsync(cancellationToken);
            }
        }
    }
}
