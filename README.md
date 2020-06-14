# GenericSearch for ASP.NET Core

[![Build Status](https://dev.azure.com/sulenero/GenericSearch/_apis/build/status/GenericSearch?branchName=master)](https://dev.azure.com/sulenero/GenericSearch/_build/latest?definitionId=11&branchName=master)



## Table of Contents

* [Description](#description)
* [Features](#features)
* [Getting Started](#getting-started)
* [Samples](#samples)
* [Release Notes](#release-notes)
* [Acknowledgements](#acknowledgements)



## Description

GenericSearch is a library to add **filtering, sorting, and pagination** functionality with minimal boilerplate code to web applications built with ASP.NET Core by following a convention-based approach. It is especially useful for projects featuring a number of list views which require this type of functionality.

Commonly used approaches and examples provided in various tutorials tend to involve a nest of conditional statements. While this is acceptable in small projects which have limited list views and columns, bigger projects need a more maintainable way of providing said features. In addition, there is another layer of pain when persisting filters through different pages and retaining the functionality of the browser back-button.

There have been many attempts to solve these problems, ranging from base classes and boxing magic to heavyweight services and frontend libraries, but so far the ease of use and ability to fit into developers' workflow has been lacking.

GenericSearch aims to change this and take advantage of convention-over-configuration design to provide reusability, and once-only data collection during startup for efficient runtime performance.



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

   `Install-Package GenericSearch.1.0.0-preview.1` 
   
2. Register GenericSearch with your application services:

   ```c#
   public void ConfigureServices(IServiceCollection services)
   {
       var assemblyContainingProfiles = GetType().Assembly;
       services.AddGenericSearch(assemblyContainingProfiles);
   }
   ```
   
3. Define your filter models using [profiles]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/27/Profiles ) by creating classes which inherit from `GenericSearchProfile` and placing the configuration in the constructor:

   ```c#
   public class SearchProfile : ListProfile
   {
       public SearchProfile()
       {
           CreateFilter<ItemModel, RequestModel, ViewModel>();
       }
   }
   ```

4. Populate your `RequestModel` and `ViewModel` classes with Search type properties matching the names of your `ItemModel` properties which should be filterable:

   ```c#
   public class ItemModel
   {
       public long Id { get; set; }
       public string Foo { get; set; }
       public DateTime Bar { get; set; }
       public decimal Baz { get; set; }
   }
   
   public class RequestModel
   {
       public TextSearch Foo { get; set; }
       public DateSearch Bar { get; set; }
       public DecimalSearch Baz { get; set; }
       
       public string Ordx { get; set; }
       public Direction Ordd { get; set; }
       
       public int Page { get; set; }
       public int Rows { get; set; }
   }
   
   // Use the built-in PagedResult class to get automatic pagination properties
   public class ViewModel : PagedResult
   {
       public ViewModel(IEnumerable<ItemModel> items, int total) : base(total)
       {
           Items = items;
       }
       
       public IEnumerable<ItemModel> Items { get; }
       
       public TextSearch Foo { get; set; }
       public DateSearch Bar { get; set; }
       public DecimalSearch Baz { get; set; }
       
       public string Ordx { get; set; }
       public Direction Ordd { get; set; }
   }
   ```

   (See [Core Concepts]( https://dev.azure.com/sulenero/GenericSearch/_wiki/wikis/GenericSearch.wiki/11/Core-Concepts ) for more information on projections and models.)

5. Inject `IGenericSearch` and use the provided extension methods to perform filtering and sorting:

   ```c#
   public class ItemsController
   {
       private readonly ApplicationDbContext context;
       private readonly IGenericSearch search;
       
       public ItemsController(ApplicationDbContext context, IGenericSearch search)
       {
           this.context = context;
           this.search = search;
       }
       
       public async Task<IActionResult> Index(RequestModel request)
       {
           var items = context.Items
               .Search(search)
               .Sort(search);
           
           var count = await items.CountAsync();
           var results = await items.Paginate(search)
               .ToListAsync();
           
           var model = new ViewModel(results, count);
           return View(model);
       }
   }
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
