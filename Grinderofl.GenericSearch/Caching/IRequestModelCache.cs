#pragma warning disable 1591
namespace Grinderofl.GenericSearch.Caching
{
    public interface IRequestModelCache
    {
        void Put(object model);
        object Get();
    }
}