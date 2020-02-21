#pragma warning disable 1591
using Grinderofl.GenericSearch.Searches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration
{
    public class SearchFactory
    {
        private static readonly Dictionary<Type, Func<string, AbstractSearch>> SearchFactories =
            new Dictionary<Type, Func<string, AbstractSearch>>()
            {
                [typeof(TextSearch)] = propertyName => new TextSearch(propertyName),
                [typeof(SingleTextOptionSearch)] = propertyName => new SingleTextOptionSearch(propertyName),
                [typeof(MultipleTextOptionSearch)] = propertyName => new MultipleTextOptionSearch(propertyName),
                [typeof(DateSearch)] = propertyName => new DateSearch(propertyName),
                [typeof(SingleDateOptionSearch)] = propertyName => new SingleDateOptionSearch(propertyName),
                [typeof(DecimalSearch)] = propertyName => new DecimalSearch(propertyName),
            };

        public static AbstractSearch Create<TSearch>(PropertyInfo info) where TSearch : AbstractSearch
        {
            if (SearchFactories.ContainsKey(typeof(TSearch)))
            {
                return SearchFactories[typeof(TSearch)](info.Name);
            }

            throw new KeyNotFoundException($"Factory for the type '{typeof(TSearch)}' was not found.");
        }

        public static ISearch Create(PropertyInfo info)
        {
            if (SearchFactories.ContainsKey(info.PropertyType))
            {
                return SearchFactories[info.PropertyType](info.Name);
            }

            var ctors = info.PropertyType.GetConstructors();
            if (!ctors.Any())
            {
                throw new ArgumentException($"No public constructors found on '{info.PropertyType.FullName}'.");
            }

            if (ctors.Any(x =>
                          {
                              var parameters = x.GetParameters();
                              if (parameters.Length == 1 && parameters.Single().ParameterType == typeof(string))
                              {
                                  return true;
                              }

                              return false;
                          }))
            {
                return Activator.CreateInstance(info.PropertyType, info.Name) as ISearch;
            }

            if (ctors.Any(x => x.GetParameters().Length == 0))
            {
                return Activator.CreateInstance(info.PropertyType) as ISearch;
            }

            throw new ArgumentException($"No suitable constructors found on '{info.PropertyType.FullName}'.");
        }
    }
}