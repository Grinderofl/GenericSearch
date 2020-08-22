# POST-redirect-GET

Typical list views with filtering and sorting generally also need to have their results paged. Ignoring javascript-based implementations, this is normally done by submitting the filters through a form using POST or GET method. While GET method makes it easy to add pagination and keep the back-button functional, the URL's can become a mess and potentially hit `HTTP 414 Request-URI Too Long` error. POST method however makes pagination more difficult and removes the usability of back-button.

The POST-REDIRECT-GET design pattern resolves this issue by taking the form submitted by POST, grabs the fields with non-default values, and redirects the browser to a URL which contains only those fields in query string.

---

GenericSearch comes with this functionality out of the box and adds an action filter which checks if the incoming request method is POST and the request model and executing action (Index and IndexAsync by default) match one defined in a search profile. The action filter then parses the model, creates a `RouteValueDictionary` with all non-default value properties as parameters in it, and short-circuits the execution with a redirect to the same action with the created dictionary as parameters, allowing MVC to generate the query string.

The POST-to-GET redirection respects the `DefaultValueAttribute` on the children of `ISearch` type properties, as well as on any top-level value type properties.

## Example
**Request Model**
```c#
public class Query : IRequest<Model>
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
```
**Headers**
```
[POST] https://localhost:5001/customers
content-type: application/x-www-form-urlencoded
```
**Body**
```
FreeText.Term: 
CompanyName.Is: 0
CompanyName.Term: alf
ContactName.Is: 1
ContactName.Term: 
City.Is: 1
City.Term: 
PostalCode.Is: 1
PostalCode.Term: 
Ordd: 0
Ordx: 
```
**Redirect result**
`/customers?companyname.term=alf&companyname.is=0`

To explain what's happening in the backscene, lets first look at the `Body` section.

All fields relevant to the request have been submitted using POST method. Most of them are empty, and if this was submitted through GET method, we'd end up with something like:

`/customers/?freetext.term=&companyname.is=0&companyname.term=alf&contactname.term=&city.is=1&city.term=&postalcode.is=1&postalcode.term=&ordd=0&ordx=`

This isn't very ideal if there could potentially be forms with options which can exceed the maximum URI length when submitted using GET.

Whenever the POST-to-GET redirection executes, it attempts to see if any values on the request model have a default value defined somewhere, in which case the request property value will not be added to the routedata dictionary. If a search type contains two or more properties, only ones marked tru will be added, meaning if there are no non-default value subproperties, they will never see the light of day in the resulting query string.

In the example above, the query string didn't ignore paging or row counts - those two had just been to default value during service configuration.