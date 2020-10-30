# GenericSearch

GenericSearch is a library to provide strongly-typed and reusable **filtering**, **sorting**, and **pagination** functionality for ASP.NET Core web applications which make use of LINQ-based data source providers. It covers a number of common scenarios out of the box, and can be extended with minimal effort to support more complicated ones.

## Why?

While it's normal for proof of concepts and  smaller projects to search lengthy lists with a couple of *if*-statements, larger projects need a more sophisticated and maintainable approach.

GenericSearch also solves another two problems - pagination and back-forward browser functionality. This is done through intercepting a `POST` request to the controller action method for the list page, creating a `RouteValueDictionary` of submitted post data which differ from their default values, and finally redirecting the user to the same action method with `GET` as the request method.

The library comes with number of commonly used filter types, ranging from "contains text" to "is one of the dates from a multi select list". It's also very easy to extend built-in filters, add your own filters, and use said filters in your lists.


## Features

* Strongly typed search classes
* Dynamic expression tree building
* [AutoMapper-style configuration]()
* [POST-to-GET redirects](  )
* [Request Model to View Model property mapping](  )
* Zero third-party library dependencies
* [Extension methods for building views](  )