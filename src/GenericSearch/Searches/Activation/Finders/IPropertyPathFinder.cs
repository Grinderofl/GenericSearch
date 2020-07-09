using System;

namespace GenericSearch.Searches.Activation.Finders
{
    public interface IPropertyPathFinder
    {
        string Find(Type entityType, string source);
    }
}