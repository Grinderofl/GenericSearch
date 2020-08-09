using System;

namespace GenericSearch.Internal.Activation.Finders
{
    public interface IPropertyPathFinder
    {
        string Find(Type entityType, string source);
    }
}