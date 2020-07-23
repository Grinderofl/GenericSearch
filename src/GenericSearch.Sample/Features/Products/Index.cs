using System.Collections.Generic;
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

namespace GenericSearch.Sample.Features.Products
{
    public static class Index
    {
        public class Query : IRequest<Model>, ISortOrder
        {
            public TextSearch ProductName { get; set; }
            public SingleTextOptionSearch SupplierCompanyName { get; set; }
            public SingleTextOptionSearch Category { get; set; }
            public OptionalBooleanSearch Discontinued { get; set; }

            public Direction Ordd { get; set; }
            public string Ordx { get; set; }

            public int Page { get; set; }
            public int Rows { get; set; }
        }

        public class Model : PagedResult, ISortOrder
        {
            public Model(IEnumerable<Item> items, int total) : base(total)
            {
                Items = items;
            }

            public IEnumerable<Item> Items { get; }

            public TextSearch ProductName { get; set; }
            public SingleTextOptionSearch SupplierCompanyName { get; set; }
            public SingleTextOptionSearch Category { get; set; }
            public OptionalBooleanSearch Discontinued { get; set; }

            public Direction Ordd { get; set; }
            public string Ordx { get; set; }
        }

        public class Item
        {
            public int Id { get; set; }
            [Display(Name = "Product Name")]
            public string ProductName { get; set; }

            [Display(Name = "Supplier")]
            public string Supplier { get; set; }

            [Display(Name = "Category")]
            public string Category { get; set; }

            [Display(Name = "Price")]
            public double? UnitPrice { get; set; }

            [Display(Name = "Stock")]
            public short? UnitsInStock { get; set; }

            [Display(Name = "Ordered")]
            public short? UnitsOnOrder { get; set; }
            public short? ReorderLevel { get; set; }

            [Display(Name = "Discontinued")]
            public bool Discontinued { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Product, Item>()
                    .ForMember(x => x.Supplier, x => x.MapFrom(c => c.Supplier.CompanyName))
                    .ForMember(x => x.Category, x => x.MapFrom(c => c.Category.CategoryName));
            }
        }

        public class SearchProfile : ListProfile
        {
            public SearchProfile()
            {
                AddList<Query, Product, Model>()
                    .Search(x => x.Category, x => x.On(c => c.Category.CategoryName));
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly NorthwindDbContext context;
            private readonly IMapper mapper;
            private readonly IGenericSearch genericSearch;

            public Handler(NorthwindDbContext context, IMapper mapper, IGenericSearch genericSearch)
            {
                this.context = context;
                this.mapper = mapper;
                this.genericSearch = genericSearch;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = context.Products
                    .Search(genericSearch)
                    .Sort(genericSearch);

                var count = await items.CountAsync(cancellationToken);
                var results = await items.Paginate(genericSearch)
                    .ProjectTo<Item>(mapper)
                    .ToListAsync(cancellationToken);

                return new Model(results, count);
            }
        }
    }
}