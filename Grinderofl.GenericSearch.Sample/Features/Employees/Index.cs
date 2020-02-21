using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.Sample.Data;
using Grinderofl.GenericSearch.Sample.Data.Entities;
using Grinderofl.GenericSearch.Sample.Infrastructure.Extensions;
using Grinderofl.GenericSearch.Searches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Grinderofl.GenericSearch.Sample.Features.Employees
{
    public static class Index
    {
        public class Query : IRequest<Model>
        {
            public TextSearch Name { get; set; }
            public DateSearch BirthDate { get; set; }
            public DateSearch HireDate { get; set; }
            public SingleTextOptionSearch Country { get; set; }
            public TextSearch HomePhone { get; set; }
            public TextSearch ReportsTo { get; set; }

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

            [Display(Name = "Name")]
            public TextSearch Name { get; set; }

            [Display(Name = "Birth Date")]
            public DateSearch BirthDate { get; set; }

            [Display(Name = "Hire Date")]
            public DateSearch HireDate { get; set; }

            [Display(Name = "Country")]
            public SingleTextOptionSearch Country { get; set; }

            [Display(Name = "Phone")]
            public TextSearch HomePhone { get; set; }

            [Display(Name = "Reports To")]
            public TextSearch ReportsTo { get; set; }

            public string Ordx { get; set; }
            public Direction Ordd { get; set; }

            public IEnumerable<Projection> Items { get; }
        }

        public class Projection
        {
            public string Id { get; set; }

            public string Name { get; set; }

            [Display(Name = "Birth Date")]
            public DateTime? BirthDate { get; set; }

            [Display(Name = "Hire Date")]
            public DateTime? HireDate { get; set; }

            public string Country { get; set; }

            [Display(Name = "Phone")]
            public string HomePhone { get; set; }
            
            [Display(Name = "Reports To")]
            public string ReportsTo { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Employee, Projection>()
                    .ForMember(x => x.Name, x => x.MapFrom(c => $"{c.FirstName} {c.LastName}"))
                    .ForMember(x => x.ReportsTo, x => x.MapFrom(c => $"{c.ReportsTo.FirstName} {c.ReportsTo.LastName}"));
            }
        }

        public class SearchProfile : SearchProfile<Projection, Query, Model>
        {
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