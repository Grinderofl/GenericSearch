# Generic Search for ASP.NET Core
[![Build Status](https://dev.azure.com/sulenero/Grinderofl.GenericSearch/_apis/build/status/Grinderofl.GenericSearch?branchName=master)](https://dev.azure.com/sulenero/Grinderofl.GenericSearch/_build/latest?definitionId=11&branchName=master)

The purpose of this library is to reduce the amount of boilerplate code required when creating search, sorting, and pagination functionality in ASP.NET MVC Core applications using a convention-based approach for rapid development.

Normally, adding sorting and filtering features to a project involves a nest of conditional statements. While this may be acceptable in small projects with a very limited number of list views and columns in those views, once the projects grow bigger through more list views or an exhaustive administration interface, adding and maintaining features becomes increasingly more burdensome and dull. In addition, server-side pagination adds another layer of pain when the developer has to build out the query string by hand while making sure the length of the query string doesn't exceed 2000 characters.

While historically there have been many attempts at trying to solve this problem, ranging from abstract base classes, boxing magic, and crazy service definitions all the way to heavyweight frontend libraries, yet so far none of them come close to ease of configuration, simplicity of use, or being able to fit into whatever workflow the developer might have.

GenericSearch aims to change this and minimize the amount of boilerplate code developers need to write by taking advantage of convention over configuration design and once-only data collection if certain changes are required application-wide.

## Installation

1. Install the NuGet package:

```
Install-Package Grinderofl.GenericSearch
```

2. Add the following line to your `ConfigureServices` method in `Startup`:

```c#
services.AddGenericSearch(GetType().Assembly)
    .UseConventions()
    .DefaultRowsPerPage(20);
```

And you're done!

## Basic Usage

GenericSearch is configured very similarly to AutoMapper, except it makes use of three different generic types:

* Entity, Projection, or ViewModel Item type
* Request or Query type
* Result or View Model type

### Projections

The Entity Type represents the line item in the list view model, sometimes also called DTO or Projection. It has long been considered good practice to have a separate class for representing domain entities in viewmodels, and creating projections of your domain entities before they get materialised helps reduce the resource consumption of your database.

The projection for the NorthWind Customer object might look something like this:

```c#
public class CustomerProjection
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
```

AutoMapper can be used to simplify projection mapping:

```c#
public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerProjection>();
    }
}
```

### Query objects

Strongly typed request or query model will hold our search, sort, and paging options. The Sample project makes use of Mediatr, but this library works for any application model.

```c#
public class Query : IRequest<Model>
{
    public CustomerFreeTextSearch FreeText { get; set; }
    public TextSearch CompanyName { get; set; }
    public TextSearch ContactName { get; set; }
    public TextSearch City { get; set; }
    public TextSearch PostalCode { get; set; }
    public MultipleTextOptionSearch Country { get; set; }

    public string Ordx { get; set; }
    public Direction Ordd { get; set; }

    public int Page { get; set; }
    public int Rows { get; set; }
}
```

When creating the search properties on the query object, it's easiest to match the target projection property you want to search on with the name of the search property in query object. It's not strictly required since the property names can be configured easily enough, but helps avoid unnecessary code and keep the different models in sync.

The properties for Sort By and Sort Direction default to `Ordx` and `Ordd` by convention, while current page and row count properties default to `Page` and `Rows` respectively. The default value for `Ordd` is `Ascending`, and the default value for `Rows` is 25.

For filtering, a number of implementations is available out-of-the-box.

**Single value input types:**

* `DateSearch` 
* `DecimalSearch`
* `IntegerSearch`
* `TextSearch`

**Single value selectlist or dropdown types**

* `SingleDateOptionSearch` 
* `SingleIntegerOptionSearch`
* `SingleTextOptionSearch`

**Multiple value selectlist types**

* `MultipleDateOptionSearch`
* `MultipleIntegerOptionSearch`
* `MultipleTextOptionSearch`

### ViewModel objects

Strongly typed viewmodel object allows us to populate its search, sorting, and paging values from the query object automatically.

There is a helper base class `PagedResult` which takes a single constructor argument for the total number of items prior to pagination, and provides additional properties that help simplify creating a reusable pagination view.

```c#
public class Model : PagedResult
{
    public Model(IEnumerable<CustomerProjection> items, int total) : base(total)
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

    public IEnumerable<CustomerProjection> Items { get; }

}
```

Main difference between ViewModel object and Query object is the `DisplayAttribute`. Being able to create reusable editor templates for search types was one of the driving factors for GenericSearch. There is a number of extension methods for `IHtmlHelper` which allow interacting with GenericSearch components in views.

### Search Profiles

To let GenericSearch know which query objects, view models, and item models are related, we inherit from `SearchProfile` and declare the relation with `CreateFilter<TItem, TRequest, TResult>()`: 

```c#
public class SearchProfile : GenericSearchProfile
{
    public SearchProfile()
    {
        CreateFilter<CustomerProjection, Query, Model>();
    }
}
```

When using conventions, all properties in query object which implement `ISearch` are automatically added to the profile. The projection column for the search target is determined by the name of the property in the query object.

When not using conventions, all search properties will need to be defined in `CreateFilter` builder method.

```c#
public class SearchProfile : SearchProfile
{
    public SearchProfile()
    {
        CreateFilter<Projection, Query, Model>()
            .Custom(x => x.FreeText)
            .Text(x => x.CompanyName)
            .Text(x => x.ContactName)
            .Text(x => x.City)
            .Text(x => x.PostalCode)
            .TextOption(x => x.Country)
            .Sort()
            .Page(x => x.DefaultRows(25));
    }
}
```

If there are properties unrelated to search that you would like to be automatically populated from the query object to the view model, you can add them with `AddTransfer(x => x.RequestProperty)` method in `SearchProfile`.

Similar to AutoMapper, it's possible to define multiple filters in a single profile if needed.

### Custom searches

Sometimes it's needed to perform more complex searches than the ones GenericSearch provides by default. By convention, any properties with a type which implements the `ISearch` interface are automatically recognised, initialised, and used when performing the search. GenericSearch provides an abstract `Search<TEntity>` base class to create a search targeting a projection type.

```c#
public class CustomerFreeTextSearch : Search<Index.Projection>
{
    public string Term { get; set; }

    public override bool IsActive() => !string.IsNullOrWhiteSpace(Term);

    protected override IQueryable<Index.Projection> ApplyToQuery(IQueryable<Index.Projection> query)
    {
        if (!IsActive())
        {
            return query;
        }

        return query.Where(x => x.CompanyName.Contains(Term) ||
                           x.ContactName.Contains(Term) ||
                           x.Region.Contains(Term) ||
                           x.City.Contains(Term));
    }
}
```

In addition, it's possible to specify a predicate method for initializing custom searches in case they have a custom constructor:

```
public class SearchProfile : GenericSearchProfile
{
	public SearchProfile()
	{
		CreateFilter<Projection, Request, Result>()
			.Custom(x => x.MySearch, () => new MySearch("WithCustomConstructor"));
	}
}
```

Since all `GenericSearchProfile` types are resolved from service provider when configurations are first created, dependency injection is an option:

```
public class SearchProfile : GenericSearchProfile
{
	public SearchProfile(IMyHttpContextServicesISearchResolver resolver)
	{
		CreateFilter<Projection, Request, Result>()
			.Custom(x => x.MySearch, () => resolver.Resolve<MyISearchImplementation>()));
	}
}
```

### Performing search

To perform the actual search itself, we need to take a dependency on `IGenericSearch`. This can be done anywhere - in controller, service layer, or Mediatr handler. For this example, we'll make use of Mediatr.

```c#
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
```

Since we're using conventions, the search related properties on the view model are automatically populated from the query object. We also don't need to pass the query object into the handler methods since the modelbinder causes the query object to be made available for GenericSearch services throughout the request lifetime. Both of these behaviours can be changed in the ServiceCollection configuration stage.

When GenericSearch services are registered, an `IModelBinderProvider` implementation is added to the top of model binder providers collection. The resulting model binder has two functions for any `ModelType` registered in a search profile:

1. Initialize all `ISearch` type properties on the model
2. Store the final model in a cache scoped for the rest of the request

The second function allows us to pass only `IGenericSearch` dependency to the `Search`, `Sort`, and `Paginate` extension methods, as `IGenericSearch` has access to the request scope and can obtain the cached model.

### Controllers

While handling the search can be done in controllers, Mediatr allows us to keep the controllers thin:

```c#
public class CustomersController : Controller
{
    private readonly IMediator mediator;

    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IActionResult> Index(Index.Query query, CancellationToken cancellationToken)
    {
        var model = await mediator.Send(query, cancellationToken);
        return View(model);
    }
}
```

### Select lists and dropdowns

SelectLists can be added to ViewData by making use of actionfilters. GenericSearch provides an `AsyncViewDataFilter` base class for simplifying view data modifications. For example, to add a selectlist of countries for the Country search:

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
            viewData["Country"] = await context.Customers
                .Select(x => x.Country)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem(x, x.ToLowerInvariant()))
                .ToListAsync(cancellationToken);
        }
    }
}
```

`@Html.GetSelectListForModel()` can then be used in Editor or Display Templates, or `@Html.GetSelectListFor(x => x.Country)`  to get the select list by the name of the search property.

### POST to GET redirects

When submitting search forms, browsers typically send values of all inputs, which means that in general forms are submitted with POST method. While on one hand the URL's are nice and tidy, trying to page, refresh, or use back and forward is a proper pain for both users and developers. This is why GenericSearch also provides an action filter which redirects a POST request to GET for same action, assuming the request model type has been defined in a Search Profile, and the action name matches `DefaultListActionName`  specified in either the filter definition or in global options. The default values are `Index` and `IndexAsync`. The resulting URL's respect routing endpoint definitions, and only expose parameters which differ from their default values. This means that given a request such as:

```c#
public class TestRequest
{

    public TextSearch One { get; set; } = new TextSearch("One");

    public TextSearch Two { get; set; } = new TextSearch("Two");

    public TextSearch Three { get; set; } = new TextSearch("Three");

    [DefaultValue(3)]
    public SingleIntegerOptionSearch Four { get; set; } = new SingleIntegerOptionSearch("Four")
    {
    	Is = 3
    };

    public SingleIntegerOptionSearch Five { get; set; } = new SingleIntegerOptionSearch("Four")
    {
    	Is = 3
    };

    [DefaultValue(1)]
    public int Page { get; set; } = 3;

    [DefaultValue(20)]
    public int Rows { get; set; } = 20;

    [DefaultValue("Four")]
    public string Ordx { get; set; } = "Four";
}
```

Then the final GET query would only be `?five.is=3,page=3`.

### Filter property transfer

When you have a bunch of filters, there are two ways to deal with carrying them over from the request model to view model: 

1. Place the request model inside the view model as a property 
2. Manually assign properties from request model to view model after enumerating the list

Both of them have downsides - first one suffers from added complexity in views, and second one just feels too verbose.

GenericSearch offers a solution to this as well - an action filter which copies search, paging, and sort properties from request model to view model once action has finished executing and we have access to the view model. By convention, it matches properties by name, but it's easy to override:

```c#
public class SearchProfile : SearchProfile
{
    public SearchProfile()
    {
        CreateFilter<Projection, Query, Model>()
            .Text(x => x.RequestProperty, x => x.ResultProperty)
            .Sort(x => x.Property(s => s.RequestSortProperty, s => s.ResultSortProperty))
            .Page(x => x.DefaultRows(25).Number(p => p.RequestPageNumber, p => .ResultPageNumber));
    }
}
```

This behaviour can be disabled either for individual configurations or, if desired, globally.

### Extension methods

GenericSearch provides the following `IHtmlHelper` extension methods:

* `GetPlaceHolderForModel()` - Gets the value of `Prompt` parameter in `DisplayAttribute` of the property if used in Display or Editor Template
* `GetPropertyNameForModel()` - Gets the name of the model property if used in Display or Editor Template
* `GetDisplayNameForModel()` - Gets the value of `Name` parameter in `DisplayAttribute` if used in Display or Editor Template
* `GetPropertyInfoForModel()` - Gets the `PropertyInfo` for the model property if used in Display or Editor Template
* `GetUrlForPage(int page)` - Takes the current request URL and either adds, updates, or removes the page number query parameter as defined for the request type.
* `GetSelectList(string key)` - Gets a `SelectListItem`  collection from `ViewData` by its key
* `GetSelectListForModel()` - Gets a `SelectListItem` collection from `ViewData` by the name of the property if used in Display or Editor Template
* `GetSelectListFor(Expression property)` - Gets a `SelectListItem` collection from `ViewData` for the provided property expression
* `GetPropertiesSelectList()` - Gets a `SelectListItem` collection for the current model properties which have a `DisplayAttribute`, using the `Name` of the `DisplayAttribute` as the Text and lower case name of the property as the Value.