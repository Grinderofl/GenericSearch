using System;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Searches.Activation;

namespace GenericSearch
{
    public interface IGenericSearchServicesBuilder
    {
        /// <summary>
        /// Adds a configuration profile to GenericSearch services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddProfile<T>() where T : class, IListDefinitionSource;

        /// <summary>
        /// Adds a configuration profile to GenericSearch services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddProfile<T>(T profile) where T : IListDefinitionSource;

        /// <summary>
        /// Adds a configuration profile to GenericSearch services.
        /// </summary>
        /// <param name="profileType"></param>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddProfile(Type profileType);

        /// <summary>
        /// Adds all configuration profiles in an assembly to GenericSearch services.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddProfilesFromAssembly(Assembly assembly);

        /// <summary>
        /// Adds all configuration profiles in same assembly as a type to GenericSearch services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddProfilesFromAssemblyOf<T>();

        /// <summary>
        /// Adds default dependency implementations to GenericSearch services.
        /// </summary>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddDefaultServices();

        /// <summary>
        /// Adds default search activators to GenericSearch services.
        /// </summary>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddDefaultActivators();

        /// <summary>
        /// Adds default model binder to GenericSearch services.
        /// </summary>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddModelBinder();

        /// <summary>
        /// Adds POST-redirect-GET and Transfer Values action filters to GenericSearch services.
        /// </summary>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddActionFilters();

        /// <summary>
        /// Adds a search activator to GenericSearch services.
        /// </summary>
        /// <typeparam name="TActivator"></typeparam>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddSearchActivator<TActivator>() where TActivator : class, ISearchActivator;

        /// <summary>
        /// Configures GenericSearch options.
        /// </summary>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        IGenericSearchServicesBuilder ConfigureOptions(Action<GenericSearchOptions> optionsAction);

        /// <summary>
        /// Configures GenericSearch list definitions.
        /// </summary>
        /// <param name="configureAction"></param>
        /// <returns></returns>
        IGenericSearchServicesBuilder Configure(Action<ListProfile> configureAction);

        /// <summary>
        /// Adds a new list definition to GenericSearch pipeline.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        IGenericSearchServicesBuilder AddList<TRequest, TEntity, TResult>(Action<IListExpression<TRequest, TEntity, TResult>> action = null);
    }
}