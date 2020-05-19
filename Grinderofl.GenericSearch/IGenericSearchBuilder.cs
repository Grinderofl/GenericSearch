using System;
using System.Reflection;
using Grinderofl.GenericSearch.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grinderofl.GenericSearch
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
        /// Configures GenericSearch to use convention based defaults
        /// </summary>
        /// <param name="conventionOptionsBuilderAction">Action to further configure convention options</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder UseConventions(Action<IGenericSearchConventionOptionsBuilder> conventionOptionsBuilderAction = null);

        /// <summary>
        /// Specifies whether POST request should be redirected to GET globally
        /// </summary>
        /// <param name="redirectPostToGet">Sets whether to redirect POST to GET</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder RedirectPostToGet(bool redirectPostToGet);

        /// <summary>
        /// Specifies globally whether filter values should be copied from request to result after
        /// an action has finished executing
        /// </summary>
        /// <param name="copyRequestFilterValues">Sets whether to copy request filter values to result</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder CopyRequestFilterValues(bool copyRequestFilterValues);

        /// <summary>
        /// Specifies globally the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against
        /// <remarks>
        /// The default name is 'Index'
        /// </remarks>
        /// </summary>
        /// <param name="defaultName">Default name to use</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder DefaultListActionName(string defaultName);

        /// <summary>
        /// Specifies the default number of rows per page
        /// </summary>
        /// <param name="defaultRows">Default number of rows</param>
        /// <returns>Generic Search Builder</returns>
        IGenericSearchBuilder DefaultRowsPerPage(int defaultRows);
    }
}