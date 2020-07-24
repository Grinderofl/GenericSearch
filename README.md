# GenericSearch for ASP.NET Core

![CI](https://github.com/Grinderofl/GenericSearch/workflows/CI/badge.svg?branch=master)


## Table of Contents

* [Description](#description)
* [Features](#features)
* [Getting Started](#getting-started)
* [Samples](#samples)
* [Release Notes](#release-notes)
* [Acknowledgements](#acknowledgements)



## Description

GenericSearch is a new library for ASP.NET Core to simplify adding **filtering**, **sorting**, and **pagination** functionality with minimal boilerplate code. It follows the convention over configuration paradigm while still being extensible enough to support more complex scenarios, and is especially useful for projects which feature a large number of list views such as admin interfaces.

> * Are your search boxes and facets a spaghetti of *if's*, *switches*, complex LINQ building? 
>
> * Do you dread having to add pagination and sorting on top of that?

**Then GenericSearch is for you!**

While it's normal for proof of concepts and  smaller projects to search lengthy lists with a couple of *if*-statements, larger projects need a more sophisticated and maintainable approach.

GenericSearch also solves another two problems - pagination and back-forward browser functionality. This is done through intercepting a `POST` request to the controller action method for the list page, creating a `RouteValueDictionary` of submitted post data which differ from their default values, and finally redirecting the user to the same action method with `GET` as the request method.

The library comes with number of commonly used filter types, ranging from "contains text" to "is one of the dates from a multi select list". It's also very easy to extend built-in filters, add your own filters, and use said filters in your lists.



## Features

* Strongly typed search classes
* Dynamic expression tree building
* [AutoMapper-style configuration]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/27/Profiles )
* [POST-to-GET redirects]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/13/POST-to-GET-redirects )
* [Request Model to View Model property mapping]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/29/Request-Model-to-View-Model-property-mapping )
* Zero third-party library dependencies
* [Extension methods for building views]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/31/Extension-methods )



## Getting Started

1. Install the NuGet package:

   `Install-Package GenericSearch.0.1.0`
   
2. Register and configure GenericSearch with your application services:

   ```c#
   // Option 1: ConfigureOptions() in GenericSearch service builder
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddDefaultGenericSearch()
           .ConfigureOptions(x => 
   		{
               x.CreateFilter<Request, Entity, Result>();
           });
   }
   
   // Option 2: Configure<GenericSearchOptions>() in application service builder
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddDefaultGenericSearch();
       services.Configure<GenericSearchOptions>(x => 
   	{
           x.CreateFilter<Request, Entity, Result>();
       });
   }
   
   // Option 3: Implement IConfigureOptions<GenericSearchOptions>
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddDefaultGenericSearch();
       services.ConfigureOptions<ConfigureGenericSearchOptions>();
   }
   
   public class ConfigureGenericSearchOptions : IConfigureOptions<GenericSearchOptions>
   {
       public void Configure(GenericSearchOptions options)
       {
           options.CreateFilter<Request, Entity, Result>();
       }
   }
   
   // Option 4: Extend ListProfile
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddDefaultGenericSearch(typeof(SearchProfile).Assembly);
   }
   
   public class SearchProfile : ListProfile
   {
       public SearchProfile()
       {
           CreateFilter<Request, Entity, Result>();
       }
   }
   
   ```
   
3. Add features!

## Quickstart tutorial

**Re-implementing *Products* admin list view in `GenericSearch.Sample`*:**

(*) *Using AutoMapper and MediatR*

1. Create the item projection

   ```c#
   public class Item
   {
       public int Id { get; set; }
       public string ProductName { get; set; }
       public string Supplier { get; set; }
       public string Category { get; set; }
       public double? UnitPrice { get; set; }
       public short? UnitsInStock { get; set; }
       public short? UnitsOnOrder { get; set; }
       public short? ReorderLevel { get; set; }
       public bool Discontinued { get; set; }
   }
   ```

2. Create the request type

   ```c#
   public class Query : IRequest<Model>, ISortOrder
   {
       public TextSearch ProductName { get; set; }
       public SingleTextOptionSearch Supplier { get; set; }
       public SingleTextOptionSearch Category { get; set; }
       public OptionalBooleanSearch Discontinued { get; set; }
   
       public Direction Ordd { get; set; }
       public string Ordx { get; set; }
   
       public int Page { get; set; }
       public int Rows { get; set; }
   }
   ```

3. Create the result type

   ```c#
   public class Model : PagedResult, ISortOrder
   {
       public Model(IEnumerable<Item> items, int total) : base(total)
       {
           Items = items;
       }
   
       public IEnumerable<Item> Items { get; }
   
       public TextSearch ProductName { get; set; }
       public SingleTextOptionSearch Supplier { get; set; }
       public SingleTextOptionSearch Category { get; set; }
       public OptionalBooleanSearch Discontinued { get; set; }
   
       public Direction Ordd { get; set; }
       public string Ordx { get; set; }
   }
   ```

4. Create the mapping profile

   ```c#
   public class MappingProfile : Profile
   {
       public MappingProfile()
       {
           CreateMap<Product, Item>()
               .ForMember(x => x.Supplier, x => x.MapFrom(c => c.Supplier.CompanyName))
               .ForMember(x => x.Category, x => x.MapFrom(c => c.Category.CategoryName));
       }
   }
   ```

5. Create the search profile

   ```c#
   public class SearchProfile : ListProfile
   {
       public SearchProfile()
       {
           CreateFilter<Query, Item, Model>();
       }
   }
   ```

6. Create the request handler

   ```c#
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
               .ProjectTo<Item>(mapper)
               .Search(genericSearch)
               .Sort(genericSearch);
   
           var count = await items.CountAsync(cancellationToken);
           var results = await items.Paginate(genericSearch)
               .ToListAsync(cancellationToken);
   
           return new Model(results, count);
       }
   }
   ```

7. Create the controller

   ```c#
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
   ```

8. Add select lists for Supplier and Category filters

   ```c#
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
               viewData["Supplier"] = await context.Suppliers
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
   ```

9. Add view

   ```c#
   @model  GenericSearch.Sample.Features.Products.Index.Model
   
   @{
       Layout = "_Layout.Admin";
   }
   
   <div class="row mb-2">
       <div class="col-12">
           <button class="btn btn-primary"
                   type="button"
                   data-toggle="collapse"
                   data-target="#filters"
                   aria-expanded="false"
                   aria-controls="filters">
               Toggle Filters
           </button>
       </div>
   </div>
   
   <div class="row">
       <div class="col collapse col-md-3" id="filters">
           <form role="form" method="post">
               <div class="card">
                   <div class="card-header">
                       <h6>Filters</h6>
                   </div>
                   @Html.EditorFor(x => x.ProductName)
                   @Html.EditorFor(x => x.Category)
                   @Html.EditorFor(x => x.Supplier)
                   @Html.EditorFor(x => x.Discontinued)
                   <partial name="Sorting"/>
                   <div class="card-body py-2">
                       <div class="input-group input-group-sm">
                           <button class="btn btn-primary" type="submit">Apply</button>
                       </div>
                   </div>
               </div>
           </form>
       </div>
       <div class="col">
           <table class="table">
               <thead>
                   <tr>
                       <th>Product Name</th>
                       <th>Supplier</th>
                       <th>Category</th>
                       <th>Unit Price</th>
                       <th>Stock</th>
                       <th>Ordered</th>
                       <th>Discontinued</th>
                   </tr>
               </thead>
               <tbody>
                   @foreach (var item in Model.Items)
                   {
                       <tr>
                           <td>@Html.DisplayFor(x => item.ProductName)</td>
                           <td>@Html.DisplayFor(x => item.Supplier)</td>
                           <td>@Html.DisplayFor(x => item.Category)</td>
                           <td>@Html.DisplayFor(x => item.UnitPrice)</td>
                           <td>@Html.DisplayFor(x => item.UnitsInStock)</td>
                           <td>@Html.DisplayFor(x => item.UnitsOnOrder)</td>
                           <td>@Html.DisplayFor(x => item.Discontinued)</td>
                       </tr>
                   }
               </tbody>
           </table>
           <partial name="Paging"/>
       </div>
   </div>
   
   ```

More information can be found in the [Wiki]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/1/Home ).



## Samples

The repository contains a [sample project]( https://dev.azure.com/sulenero/_git/GenericSearch?path=%2Fsrc%2FGenericSearch.Sample) which provides further examples on how to use GenericSearch and combine it with other libraries such as [AutoMapper](https://automapper.org/) and [MediatR](https://github.com/jbogard/MediatR). 



## Release Notes

### GenericSearch 1.0 Preview 2

> Note: This is a preview release. While it's unlikely for any features to be significantly altered, please be
>
> sure to check the release notes of newer versions for any breaking changes.

* Changed the configuration of convention defaults
* Added more construction and initialization options to filters and properties
* Simplified adding GenericSearch defaults

### GenericSearch 1.0 Preview 1

> Note: This is a preview release. While it's unlikely for any features to be significantly altered, please be sure to check the release notes of newer versions for any breaking changes.

* First preview release



## Acknowledgements

Many thanks to Daniel Palme for the [blog post]( https://www.palmmedia.de/blog/2012/2/18/aspnet-mvc-generic-filtering-based-on-expressions ) which gave the idea for this project.
