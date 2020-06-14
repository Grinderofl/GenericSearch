using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Exceptions;
using GenericSearch.Searches;

namespace GenericSearch.Providers
{
    /// <summary>
    /// Provides methods to set <see cref="ISearch"/> type property values using a factory method.
    /// </summary>
    public class SearchFactoryProvider : ISearchFactoryProvider
    {
        private readonly Dictionary<Type, Func<string, Func<ISearch>>> factories;

        /// <summary>
        /// Initializes a new instance of <see cref="SearchFactoryProvider"/>
        /// </summary>
        public SearchFactoryProvider()
        {
            factories = CreateFactories();
        }

        private Dictionary<Type, Func<string, Func<ISearch>>> CreateFactories()
        {
            return CreatePropertyFactories().ToDictionary(x => x.Item1, x => x.Item2);
        }

        /// <summary>
        /// Creates all built-in <see cref="ISearch"/> property factories
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<(Type, Func<string, Func<ISearch>>)> CreatePropertyFactories()
        {
            yield return (typeof(TextSearch),  propertyName => () => new TextSearch(propertyName));
            yield return (typeof(SingleTextOptionSearch),  propertyName => () => new SingleTextOptionSearch(propertyName));
            yield return (typeof(MultipleTextOptionSearch),  propertyName => () => new MultipleTextOptionSearch(propertyName));
            yield return (typeof(DateSearch),  propertyName => () => new DateSearch(propertyName));
            yield return (typeof(SingleDateOptionSearch),  propertyName => () => new SingleDateOptionSearch(propertyName));
            yield return (typeof(DecimalSearch),  propertyName => () => new DecimalSearch(propertyName));
            yield return (typeof(BooleanSearch),  propertyName => () => new BooleanSearch(propertyName));
            yield return (typeof(OptionalBooleanSearch),  propertyName => () => new OptionalBooleanSearch(propertyName));
            yield return (typeof(TrueBooleanSearch),  propertyName => () => new TrueBooleanSearch(propertyName));
        }

        /// <summary>
        /// Provides <see cref="Func{ISearch}"/> values to assign to properties of type <see cref="ISearch"/>
        /// </summary>
        /// <param name="searchProperty">Property to assign the function value to</param>
        /// <returns><see cref="Func{ISearch}"/></returns>
        public virtual Func<ISearch> Provide(PropertyInfo searchProperty)
        {
            if (factories.ContainsKey(searchProperty.PropertyType))
            {
                return factories[searchProperty.PropertyType](searchProperty.Name);
            }

            var constructors = searchProperty.PropertyType.GetConstructors();
            if (!constructors.Any())
            {
                throw new SearchPropertyActivationException($"No public constructors found on '{searchProperty.PropertyType.FullName}'");
            }

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameters.Single().ParameterType == typeof(string))
                {
                    return () => (Activator.CreateInstance(searchProperty.PropertyType, searchProperty.Name) as ISearch);
                }

                if (parameters.Length == 0)
                {
                    return () => (Activator.CreateInstance(searchProperty.PropertyType) as ISearch);
                }
            }

            throw new SearchPropertyActivationException($"No suitable constructors found on '{searchProperty.PropertyType.FullName}'");
        }
    }
}