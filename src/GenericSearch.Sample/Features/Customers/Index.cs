using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GenericSearch.Extensions;
using GenericSearch.Sample.Data;
using GenericSearch.Sample.Data.Entities;
using GenericSearch.Sample.Features.Shared;
using GenericSearch.Sample.Infrastructure.Extensions;
using GenericSearch.Searches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GenericSearch.Sample.Features.Customers
{
    public static class Index
    {
        public class Query : IRequest<Model>, ISortOrder
        {
            public CustomerFreeTextSearch FreeText { get; set; }
            public TextSearch CompanyName { get; set; }
            public TextSearch ContactName { get; set; }
            public TextSearch City { get; set; }
            public TextSearch PostalCode { get; set; }
            public MultipleTextOptionSearch Country { get; set; }

            [DefaultValue(nameof(Projection.ContactName))]
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }

            public int Page { get; set; }
            public int Rows { get; set; }
        }

        public class Model : PagedResult, ISortOrder
        {
            public Model(IEnumerable<Projection> items, int total) : base(total)
            {
                Items = items;
            }

            [Display(Name = "Query")]
            public CustomerFreeTextSearch FreeText { get; set; }

            [Display(Name = "Company Name")]
            public TextSearch CompanyName { get; set; }

            [Display(Name = "Contact Name")]
            public TextSearch ContactName { get; set; }

            [Display(Name = "City")]
            public TextSearch City { get; set; }

            [Display(Name = "Postal Code")]
            public TextSearch PostalCode { get; set; }

            [Display(Name = "Country")]
            public MultipleTextOptionSearch Country { get; set; }

            public string Ordx { get; set; }

            public Direction Ordd { get; set; }

            public IEnumerable<Projection> Items { get; }

        }

        public class Projection
        {
            public string Id { get; set; }

            [Display(Name = "Company Name")]
            public string CompanyName { get; set; }

            [Display(Name = "Contact Name")]
            public string ContactName { get; set; }

            [Display(Name = "Contact Title")]
            public string ContactTitle { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "Region")]
            public string Region { get; set; }

            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "Phone")]
            public string Phone { get; set; }

            [Display(Name = "Fax")]
            public string Fax { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Customer, Projection>()
                    .ForMember(x => x.Id, x => x.MapFrom(c => c.Id));
            }
        }

        public class SearchProfile : ListProfile
        {
            public SearchProfile()
            {
                AddList<Query, Projection, Model>()
                    .Search(x => x.FreeText, x => x.Ignore());
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

            public async Task<Model> Handle(Query query, CancellationToken cancellationToken)
            {
                var items = context.Customers
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