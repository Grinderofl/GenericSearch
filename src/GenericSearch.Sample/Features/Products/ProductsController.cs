using System.Threading;
using System.Threading.Tasks;
using GenericSearch.Sample.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.Sample.Features.Products
{
    public class ProductsController : Controller
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator) => this.mediator = mediator;

        [AddIndexFilters]
        public async Task<IActionResult> Index(Index.Query query, CancellationToken cancellationToken)
        {
            var model = await mediator.Send(query, cancellationToken);
            return View(model);
        }
    }
}
