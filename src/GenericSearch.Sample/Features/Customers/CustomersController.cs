using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.Sample.Features.Customers
{
    public class CustomersController : Controller
    {
        private readonly IMediator mediator;

        public CustomersController(IMediator mediator) => this.mediator = mediator;

        [AddIndexFilters]
        public async Task<IActionResult> Index(Index.Query query, CancellationToken cancellationToken)
        {
            var model = await mediator.Send(query, cancellationToken);
            return View(model);
        }
    }
}
