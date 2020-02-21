#pragma warning disable 1591
namespace Grinderofl.GenericSearch.Transformers
{
    public interface IQueryStringTransformer
    {
        /// <summary>
        /// Transforms the provided request object into a query <see cref="string"/> using the request properties as keys.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        string Transform(object request);
    }
}