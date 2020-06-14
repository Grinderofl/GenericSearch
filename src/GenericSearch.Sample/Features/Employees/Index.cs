using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GenericSearch.Extensions;
using GenericSearch.Sample.Data;
using GenericSearch.Sample.Data.Entities;
using GenericSearch.Sample.Infrastructure.Extensions;
using GenericSearch.Searches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GenericSearch.Sample.Features.Employees
{
    public static class Index
    {
        public class Query : IRequest<Model>
        {
            public TextSearch FirstName { get; set; }
            public TextSearch LastName { get; set; }
            public DateSearch BirthDate { get; set; }
            public DateSearch HireDate { get; set; }
            public SingleTextOptionSearch Country { get; set; }
            public TextSearch HomePhone { get; set; }
            public TextSearch ReportsToFirstName { get; set; }
            public TextSearch ReportsToLastName { get; set; }

            public string Ordx { get; set; }
            public Direction Ordd { get; set; }

            public int Page { get; set; }

            public int Rows { get; set; }
        }

        public class Model : PagedResult
        {
            public Model(IEnumerable<Projection> items, int total) : base(total)
            {
                Items = items;
            }

            [Display(Name = "First Name")]
            public TextSearch FirstName { get; set; }

            [Display(Name = "Last Name")]
            public TextSearch LastName { get; set; }

            [Display(Name = "Birth Date")]
            public DateSearch BirthDate { get; set; }

            [Display(Name = "Hire Date")]
            public DateSearch HireDate { get; set; }

            public SingleTextOptionSearch Country { get; set; }

            [Display(Name = "Phone")]
            public TextSearch HomePhone { get; set; }

            [Display(Name = "Reports To First Name")]
            public TextSearch ReportsToFirstName { get; set; }

            [Display(Name = "Reports To Last Name")]
            public TextSearch ReportsToLastName { get; set; }

            public string Ordx { get; set; }
            public Direction Ordd { get; set; }

            public IEnumerable<Projection> Items { get; }
        }

        public class Projection
        {
            public string Id { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }

            [Display(Name = "Birth Date")]
            public DateTime? BirthDate { get; set; }

            [Display(Name = "Hire Date")]
            public DateTime? HireDate { get; set; }

            public string Country { get; set; }

            [Display(Name = "Phone")]
            public string HomePhone { get; set; }
            
            [Display(Name = "Reports To")]
            public string ReportsToFirstName { get; set; }
            public string ReportsToLastName { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Employee, Projection>();
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly NorthwindDbContext context;
            private readonly IMapper mapper;
            private readonly IGenericSearch search;

            public Handler(NorthwindDbContext context, IMapper mapper, IGenericSearch search)
            {
                this.context = context;
                this.mapper = mapper;
                this.search = search;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = context.Employees
                                   .ProjectTo<Projection>(mapper)
                                   .Search(search)
                                   .Sort(search);

                var count = await items.CountAsync(cancellationToken);
                var results = await items.Paginate(search)
                                         .ToListAsync(cancellationToken);

                return new Model(results, count);
            }
        }
    }
}