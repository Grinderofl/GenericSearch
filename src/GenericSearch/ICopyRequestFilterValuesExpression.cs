namespace GenericSearch
{
    /// <summary>
    /// Provides expressions for copying request filter values to result
    /// </summary>
    /// <typeparam name="TRequest">Request/Parameter type</typeparam>
    /// <typeparam name="TResult">Result/ViewModel type</typeparam>
    public interface ICopyRequestFilterValuesExpression<TRequest, TResult>
    {
        /// <summary>
        /// Specifies the name of the action which should trigger copying request filter values
        /// to result filters e.g. if it's something other than Index
        /// </summary>
        /// <param name="actionName">Name of the action which should trigger the copying</param>
        /// <returns>Copy request filter values expression</returns>
        ICopyRequestFilterValuesExpression<TRequest, TResult> UseActionName(string actionName);

        /// <summary>
        /// Disables copying request filter values to result filter values, e.g. when it's enabled by default
        /// </summary>
        /// <returns>Copy request filter values expression</returns>
        ICopyRequestFilterValuesExpression<TRequest, TResult> Disable();

        /// <summary>
        /// Enables copying request filter values to result filter values, e.g. when it's disabled by default
        /// </summary>
        /// <returns>Copy request filter values expression</returns>
        ICopyRequestFilterValuesExpression<TRequest, TResult> Enable();
    }
}