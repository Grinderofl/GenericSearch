namespace GenericSearch.Internal
{
    public interface IModelCache
    {
        object Get();

        void Put(object model);
    }
}