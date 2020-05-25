using Grinderofl.GenericSearch.Internal;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Provides expressions for copying request filter values to result
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public class CopyRequestFilterValuesExpression<TRequest, TResult> : ICopyRequestFilterValuesConfiguration, 
                                                                        ICopyRequestFilterValuesExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the name of the action which should trigger copying request filter values to result filter values
        /// </summary>
        public string ActionName { get; private set; } = ConfigurationConstants.DefaultListActionName;

        /// <summary>
        /// Specifies whether copying is enabled, disabled, or follow the default configuration
        /// </summary>
        public ConfigurationState ConfigurationState { get; private set; } = ConfigurationState.Default;

        /// <summary>
        /// Specifies the name of the action which should trigger copying request filter values
        /// to result filters e.g. if it's something other than Index
        /// </summary>
        /// <param name="actionName">Name of the action which should trigger the copying</param>
        /// <returns>Copy request filter values expression</returns>
        public ICopyRequestFilterValuesExpression<TRequest, TResult> UseActionName(string actionName)
        {
            ActionName = actionName;
            return this;
        }

        /// <summary>
        /// Disables copying request filter values to result filter values, e.g. when it's enabled by default
        /// </summary>
        /// <returns>Copy request filter values expression</returns>
        public ICopyRequestFilterValuesExpression<TRequest, TResult> Disable()
        {
            ConfigurationState = ConfigurationState.Disabled;
            return this;
        }

        /// <summary>
        /// Enables copying request filter values to result filter values, e.g. when it's disabled by default
        /// </summary>
        /// <returns>Copy request filter values expression</returns>
        public ICopyRequestFilterValuesExpression<TRequest, TResult> Enable()
        {
            ConfigurationState = ConfigurationState.Enabled;
            return this;
        }
    }
}