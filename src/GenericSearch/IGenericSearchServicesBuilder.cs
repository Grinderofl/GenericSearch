using System;
using System.Reflection;
using GenericSearch.Searches.Activation;

namespace GenericSearch
{
    public interface IGenericSearchServicesBuilder
    {
        IGenericSearchServicesBuilder AddDefinitionsFromAssembly(Assembly assembly);
        IGenericSearchServicesBuilder AddDefinitionsFromAssemblyOf<T>();
        IGenericSearchServicesBuilder AddDefaultServices();
        IGenericSearchServicesBuilder AddDefaultActivators();
        IGenericSearchServicesBuilder AddModelBinder();
        IGenericSearchServicesBuilder AddPostToGetRedirects();
        IGenericSearchServicesBuilder AddSearchActivator<TActivator>() where TActivator : class, ISearchActivator;
        IGenericSearchServicesBuilder ConfigureOptions(Action<GenericSearchOptions> optionsAction);
    }
}