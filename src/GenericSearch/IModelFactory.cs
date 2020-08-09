using System;

namespace GenericSearch
{
    public interface IModelFactory
    {
        object Create(Type modelType);
    }
}