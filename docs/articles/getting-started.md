## Registering GenericSearch services

There are a few methods to register required services and optional features with the application services.

**Recommended method**

Registers the following services:
* Required services
* Activators for built-in Search types
* Model Binder
* POST-Redirect-GET Action Filter
* Transfer Values Action Filter
* `ListProfile` types from provided assemblies
```c#
services.AddDefaultGenericSearch(typeof(Startup).Assembly);
```

**Pick-and-choose**

Registers only the required services, registers `ListProfile` types from provided assemblies, and lets you manually add optional services.

```c#
var builder = services.AddGenericSearch(typeof(Startup).Assembly);
builder.AddDefaultActivators(); // Registers Activators for built-in Search types
builder.AddModelBinder(); // Adds the GenericSearch Model Binder to MVC pipeline
builder.AddTransferValuesActionFilter(); // Adds Transfer Values Action Filter
builder.AddPostRedirectGetActionFilter(); // Adds POST-Redirect-GET Action Filter
```

## Configuration

