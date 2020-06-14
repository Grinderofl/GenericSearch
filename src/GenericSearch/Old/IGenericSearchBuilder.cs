using System;
using System.ComponentModel;
using System.Reflection;
using GenericSearch.Configuration;
using GenericSearch.Searches;

namespace GenericSearch
{
    /// <summary>
    /// Provides a builder for GenericSearch services
    /// </summary>
    public interface IGenericSearchBuilder
    {
        /// <summary>
        /// Applies configuration from all types implementing <see cref="IGenericSearchProfile"/> to GenericSearch.
        /// </summary>
        /// <param name="assemblies">Assemblies to find <see cref="IGenericSearchProfile"/> types from</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder AddProfilesFromAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Applies configuration from all types implementing <see cref="IGenericSearchProfile"/> to GenericSearch
        /// from assembly containing the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type which' containing assembly to search </typeparam>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder AddProfilesFromAssemblyOf<T>();

        /// <summary>
        /// Adds the provided type as a <see cref="IGenericSearchProfile"/> configuration.
        /// </summary>
        /// <typeparam name="T">Profile of type <see cref="GenericSearchProfile"/></typeparam>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder AddProfile<T>() where T : GenericSearchProfile;

        /// <summary>
        /// Adds the provided instance as a <see cref="IGenericSearchProfile"/> configuration
        /// </summary>
        /// <typeparam name="T">Profile of type <see cref="GenericSearchProfile"/></typeparam>
        /// <param name="profile">Instance of <typeparamref name="T"/></param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder AddProfile<T>(T profile) where T : GenericSearchProfile;

        /// <summary>
        /// Adds the provided type <paramref name="profileType"/> a <see cref="IGenericSearchProfile"/> configuration.
        /// </summary>
        /// <param name="profileType">Type of the profile</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder AddProfile(Type profileType);

        /// <summary>
        /// Configures GenericSearch options
        /// </summary>
        /// <param name="optionsAction">Configuration action to perform</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder Configure(Action<GenericSearchOptions> optionsAction);
    }
}