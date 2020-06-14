namespace GenericSearch
{
    /// <summary>
    /// Provides redirect POST to GET specification expression
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface IRedirectPostToGetExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the name of the action which should be used to redirect
        /// POST requests to GET requests, e.g. if it's something other than Index
        /// </summary>
        /// <param name="actionName">Name of the action to check for</param>
        /// <returns>Redirect POST to GET expression</returns>
        IRedirectPostToGetExpression<TRequest, TResult> UseActionName(string actionName);

        /// <summary>
        /// Disables POST to GET redirects, e.g. when it's enabled by default
        /// </summary>
        /// <returns>Redirect POST to GET expression</returns>
        IRedirectPostToGetExpression<TRequest, TResult> Disable();

        /// <summary>
        /// Enables POST to GET redirects, e.g. when it's disabled by default
        /// </summary>
        /// <returns>Redirect POST to GET expression</returns>
        IRedirectPostToGetExpression<TRequest, TResult> Enable();
    }
}