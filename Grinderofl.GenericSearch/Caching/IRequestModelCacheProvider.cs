#pragma warning disable 1591
namespace Grinderofl.GenericSearch.Caching
{
    public interface IRequestModelCacheProvider
    {
        IRequestModelCache Provide();
    }
}