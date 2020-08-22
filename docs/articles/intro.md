# Introduction

## Description

GenericSearch is a new library for ASP.NET Core to simplify adding **filtering**, **sorting**, and **pagination** functionality with minimal boilerplate code. It follows the convention over configuration paradigm while still being strongly typed and extensible enough to support more complex scenarios, and is especially useful for projects which feature a large number of list views such as admin interfaces.

> * Are your search boxes and facets a spaghetti of *if's*, *switches*, complex LINQ building? 
> * Do you dread having to add pagination and sorting on top of that?

**Then GenericSearch is for you!**

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