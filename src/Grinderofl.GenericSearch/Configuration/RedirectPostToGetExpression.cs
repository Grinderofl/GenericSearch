using Grinderofl.GenericSearch.Internal;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides redirect POST to GET specification expression
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class RedirectPostToGetExpression<TRequest, TResult> : IRedirectPostToGetConfiguration,
                                                                  IRedirectPostToGetExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the name of the action which should trigger POST to GET redirects
        /// </summary>
        public string ActionName { get; private set; } = ConfigurationConstants.DefaultListActionName;

        /// <summary>
        /// Specifies whether POST to GET redirects are enabled, disabled, or what is specified in options
        /// </summary>
        public ConfigurationState ConfigurationState { get; private set; } = ConfigurationState.Default;
        
        /// <summary>
        /// Specifies the name of the action which should be used to redirect
        /// POST requests to GET requests, e.g. if it's something other than Index
        /// </summary>
        /// <param name="actionName">Name of the action to check for</param>
        /// <returns>Redirect POST to GET expression</returns>
        public IRedirectPostToGetExpression<TRequest, TResult> UseActionName(string actionName)
        {
            ActionName = actionName;
            return this;
        }

        /// <summary>
        /// Disables POST to GET redirects, e.g. when it's enabled by default
        /// </summary>
        /// <returns>Redirect POST to GET expression</returns>
        public IRedirectPostToGetExpression<TRequest, TResult> Disable()
        {
            ConfigurationState = ConfigurationState.Disabled;
            return this;
        }

        /// <summary>
        /// Enables POST to GET redirects, e.g. when it's disabled by default
        /// </summary>
        /// <returns>Redirect POST to GET expression</returns>
        public IRedirectPostToGetExpression<TRequest, TResult> Enable()
        {
            ConfigurationState = ConfigurationState.Enabled;
            return this;
        }
    }
}