using System;

namespace GenericSearch
{
    public interface IRequestFactory
    {
        object Create(Type requestType);
    }
}