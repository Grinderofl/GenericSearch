using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using Xunit;

namespace GenericSearch.UnitTests.Searches.Activation
{
    public class SearchActivatorTests
    {
        public static IEnumerable<object[]> SearchTypes => AllTypes
            .Where(x => !x.IsGenericType)
            .Where(x => !x.IsAbstract)
            .Where(x => typeof(AbstractSearch).IsAssignableFrom(x))
            .Select(x => new object[] { x });

        public static IEnumerable<object[]> ActivatorTypes => AllTypes
            .Where(x => !x.IsGenericType)
            .Where(x => !x.IsAbstract)
            .Where(x => x != typeof(FallbackSearchActivator))
            .Where(x => x.GetInterfaces().Contains(typeof(ISearchActivator)))
            .Select(x => new object[] { x });

        private static readonly Type GenericType = typeof(ISearchActivator<>);
        private static readonly Type[] AllTypes = typeof(GenericSearchOptions).Assembly.GetExportedTypes();

        [Theory]
        [MemberData(nameof(SearchTypes))]
        public void Has_Activator(Type type)
        {
            var activatorType = GenericType.MakeGenericType(type);
            var implementation = AllTypes.FirstOrDefault(x => x.GetInterfaces().Contains(activatorType));
            implementation.Should().NotBeNull();
        }

        public string TestProperty { get; set; }

        private static readonly string ItemPropertyPath =
            typeof(SearchActivatorTests).GetProperty(nameof(TestProperty)).Name;

        [Theory]
        [MemberData(nameof(ActivatorTypes))]
        public void Can_Instantiate(Type implementation)
        {
            var activator = Activator.CreateInstance(implementation) as ISearchActivator;

            Debug.Assert(activator != null, nameof(activator) + " != null");
            var search = activator.Activate(ItemPropertyPath);

            search.Should().NotBeNull();

            var searchType = search.GetType();
            var activatorType = GenericType.MakeGenericType(searchType);

            implementation.Should().Implement(activatorType);
        }
    }

}
