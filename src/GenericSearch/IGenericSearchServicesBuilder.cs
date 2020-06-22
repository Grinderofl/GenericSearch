using System;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Searches.Activation;

namespace GenericSearch
{
    public interface IGenericSearchServicesBuilder
    {
        IGenericSearchServicesBuilder AddProfile<T>() where T : class, IListDefinitionSource;
        IGenericSearchServicesBuilder AddProfile<T>(T profile) where T : IListDefinitionSource;
        IGenericSearchServicesBuilder AddProfile(Type profileType);
        IGenericSearchServicesBuilder AddProfilesFromAssembly(Assembly assembly);
        IGenericSearchServicesBuilder AddProfilesFromAssemblyOf<T>();
        IGenericSearchServicesBuilder AddDefaultServices();
        IGenericSearchServicesBuilder AddDefaultActivators();
        IGenericSearchServicesBuilder AddModelBinder();
        IGenericSearchServicesBuilder AddPostToGetRedirects();
        IGenericSearchServicesBuilder AddSearchActivator<TActivator>() where TActivator : class, ISearchActivator;
        IGenericSearchServicesBuilder ConfigureOptions(Action<GenericSearchOptions> optionsAction);
    }
}