#pragma warning disable 1591
using Grinderofl.GenericSearch.Transformers;
using Microsoft.AspNetCore.Routing;

namespace Grinderofl.GenericSearch.Configuration
{
    public class GenericSearchOptions
    {
        /// <summary>
        /// Gets or sets whether POST requests on Index actions should automatically be redirected.
        /// <remarks>
        ///     Defaults to True.
        /// </remarks>
        /// <para>
        ///     When true, a POST requests for Index actions with a parameter type of TRequest defined in a<br/>
        ///     <see cref="SearchProfile{TEntity,TRequest,TResult}"/> will have their TRequest type parameter transformed<br/>
        ///     using a <see cref="IRouteValueTransformer"/> into a <see cref="RouteValueDictionary"/> and the request<br/>
        ///     redirected to the same action, using the transformed parameter as the routeValues argument.<br/>
        ///     To override this behaviour for an individual TRequest type, use the <see cref="SearchProfile{TEntity,TRequest,TResult}.RedirectPostRequests"/><br/>
        ///     method and set the value to <see cref="ProfileBehaviour.Disabled"/>.
        /// </para>
        /// <para>
        ///     When false, POST requests for Index actions with a parameter type of TRequest defined in a <br/>
        ///     <see cref="SearchProfile{TEntity,TRequest,TResult}"/> will not be redirected.<br/>
        ///     To override this behaviour for an individual TRequest type, use the <see cref="SearchProfile{TEntity,TRequest,TResult}.RedirectPostRequests"/><br/>
        ///     method and set the value to <see cref="ProfileBehaviour.Enabled"/>.
        /// </para>
        /// </summary>
        public bool RedirectPostRequests { get; set; } = true;

        /// <summary>
        /// Gets or sets whether Index action request model properties should be automatically transferred to the view model.
        /// <remarks>
        ///     Defaults to True.
        /// </remarks>
        /// <para>
        ///     When true, requests for Index actions with a parameter type of TRequest and view model type of TResult defined in a<br/>
        ///     <see cref="SearchProfile{TEntity,TRequest,TResult}"/> will have the properties from the TRequest parameter automatically<br/>
        ///     transferred to the view model after the action has been executed.<br/>
        ///     To override this behaviour for an individual TRequest and TResult type, use the <see cref="SearchProfile{TEntity,TRequest,TResult}.TransferRequestProperties"/><br/>
        ///     method and set the value to <see cref="ProfileBehaviour.Disabled"/>.
        /// </para>
        /// <para>
        ///     When false, requests for Index actions with a parameter type of TRequest and view model type of TResult defined in a <br/>
        ///     <see cref="SearchProfile{TEntity,TRequest,TResult}"/> will not have the properties from TRequest parameter automatically<br/>
        ///     transferred to the view model after the action has executed.<br/>
        ///     To override this behaviour for an individual TRequest and TResult type, use the <see cref="SearchProfile{TEntity,TRequest,TResult}.TransferRequestProperties"/><br/>
        ///     method and set the value to <see cref="ProfileBehaviour.Enabled"/>.
        /// </para>
        /// </summary>
        public bool TransferRequestProperties { get; set; } = true;

        /// <summary>
        /// Options to configure GenericSearch conventions.
        /// </summary>
        public ConventionOptions ConventionOptions { get; set; } = new ConventionOptions();

        /// <summary>
        ///     Gets or sets whether request model should be cached for the duration of the request.<br/>
        ///     This allows using nicer looking <see cref="IGenericSearch"/> methods.<br/>
        /// <remarks>
        ///     Defaults to True.
        /// </remarks>
        /// </summary>
        public bool CacheRequestModel { get; set; } = true;
    }

}
