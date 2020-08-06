using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Configuration;
using GenericSearch.Internal.Definition;
using GenericSearch.Searches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class GenericSearchServicesBuilderTests
    {
        [Fact]
        public void AddProfile_Type_Succeeds()
        {
            var services = new ServiceCollection();

            var builder = new GenericSearchServicesBuilder(services);
            builder.AddProfile(typeof(ProfileBaz));

            var service = services.Single(x => x.ServiceType == typeof(IListDefinitionSource));

            service.ImplementationType.Should().Be<ProfileBaz>();
        }

        [Fact]
        public void AddProfile_Generic_Succeeds()
        {
            var services = new ServiceCollection();

            var builder = new GenericSearchServicesBuilder(services);
            builder.AddProfile<ProfileBaz>();

            var service = services.Single(x => x.ServiceType == typeof(IListDefinitionSource));

            service.ImplementationType.Should().Be<ProfileBaz>();
        }

        [Fact]
        public void AddProfile_Profile_Succeeds()
        {
            var services = new ServiceCollection();

            var builder = new GenericSearchServicesBuilder(services);
            var profile = new ProfileBaz();
            builder.AddProfile(profile);

            var service = services.Single(x => x.ServiceType == typeof(IListDefinitionSource));

            service.ImplementationInstance.Should().Be(profile);
        }

        [Fact]
        public void AddProfilesFromAssembly_Assembly_Succeeds()
        {
            var services = new ServiceCollection();

            var builder = new GenericSearchServicesBuilder(services);
            builder.AddProfilesFromAssembly(GetType().Assembly);

            var service = services.Single(x => x.ServiceType == typeof(IListDefinitionSource));

            service.ImplementationType.Should().Be<ProfileBaz>();
        }

        [Fact]
        public void AddProfilesFromAssemblyOf_T_Succeeds()
        {
            var services = new ServiceCollection();

            var builder = new GenericSearchServicesBuilder(services);
            builder.AddProfilesFromAssemblyOf<ProfileBaz>();

            var service = services.Single(x => x.ServiceType == typeof(IListDefinitionSource));

            service.ImplementationType.Should().Be<ProfileBaz>();
        }

        [Fact]
        public void AddSearchActivator_Succeeds()
        {
            var services = new ServiceCollection();
            
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddSearchActivator<ActivatorFoo>();

            var service = services.Single(x => x.ServiceType == typeof(ISearchActivator<TextSearch>));
            
            service.ServiceType.Should().Be(typeof(ISearchActivator<TextSearch>));
            service.ImplementationType.Should().Be(typeof(ActivatorFoo));
        }

        [Fact]
        public void AddSearchActivator_Duplicate_Throws()
        {
            var services = new ServiceCollection();
            
            var builder = new GenericSearchServicesBuilder(services);

            builder.Invoking(x => x.AddSearchActivator<ActivatorBar>())
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Constructor_Succeeds()
        {
            var services = new ServiceCollection();
            // ReSharper disable once ObjectCreationAsStatement
            new GenericSearchServicesBuilder(services);

            var provider = services.BuildServiceProvider();

            provider.GetService<IOptions<GenericSearchOptions>>()
                .Value
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ConfigureOptions_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x =>
            {
                x.ListActionName = "List";
                x.DefaultPage = 10;
                x.DefaultRows = 100;
                x.PagePropertyName = "CurrentPage";
                x.RowsPropertyName = "CurrentRows";
                x.SortColumnPropertyName = "Sortx";
                x.SortDirectionPropertyName = "Sortd";
                x.SortDirection = Direction.Descending;
                x.PostRedirectGetEnabled = false;
                x.TransferValuesEnabled = false;
            });
            
            var provider = services.BuildServiceProvider();

            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Value.ListActionName.Should().Be("List");
            options.Value.DefaultPage.Should().Be(10);
            options.Value.DefaultRows.Should().Be(100);
            options.Value.PagePropertyName.Should().Be("CurrentPage");
            options.Value.RowsPropertyName.Should().Be("CurrentRows");
            options.Value.SortColumnPropertyName.Should().Be("Sortx");
            options.Value.SortDirectionPropertyName.Should().Be("Sortd");
            options.Value.SortDirection.Should().Be(Direction.Descending);
            options.Value.PostRedirectGetEnabled.Should().BeFalse();
            options.Value.TransferValuesEnabled.Should().BeFalse();
        }

        [Fact]
        public void ConfigureOptions_UseModelFactory_Generic_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x => x.UseModelFactory<TestModelFactory>());

            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Value.DefaultModelActivatorType.Should().Be<TestModelFactory>();
        }

        [Fact]
        public void ConfigureOptions_UseModelFactory_Type_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x => x.UseModelFactory(typeof(TestModelFactory)));

            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Value.DefaultModelActivatorType.Should().Be<TestModelFactory>();
        }

        [Fact]
        public void ConfigureOptions_UseModelFactory_Type_Throws()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x => x.UseModelFactory(typeof(Request)));
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Invoking(x => x.Value)
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ConfigureOptions_UseModelActivator_Method_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x => x.UseModelActivator(Activator.CreateInstance));

            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Value.DefaultModelActivatorMethod.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureOptions_UseModelActivator_Generic_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.ConfigureOptions(x => x.UseModelActivator((sp, t) => Activator.CreateInstance(t)));

            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<GenericSearchOptions>>();
            options.Value.DefaultModelActivatorResolver.Should().NotBeNull();
        }

        [Fact]
        public void Configure_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.Configure(x =>
            {
                x.AddList<Request, Item, Result>();
            });

            var service = services.SingleOrDefault(x => x.ServiceType == typeof(IListDefinition));
            service.Should().NotBeNull();

            Debug.Assert(service != null, nameof(service) + " != null");
            service.Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddDefaultServices_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultServices();
            
            var provider = services.BuildServiceProvider();

            var configurationProvider = provider.GetService<IListConfigurationProvider>();
            configurationProvider.Should().NotBeNull();
        }

        [Fact]
        public void AddDefaultServices_Executes_Once()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultServices();

            var count = services.Count;
            builder.AddDefaultServices();

            services.Count.Should().Be(count);
        }

        [Fact]
        public void AddDefaultActivators_Executes_Once()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultActivators();

            var count = services.Count;
            builder.AddDefaultActivators();

            services.Count.Should().Be(count);
        }

        [Fact]
        public void AddModelBinder_Executes_Once()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddModelBinder();

            var count = services.Count;
            builder.AddModelBinder();

            services.Count.Should().Be(count);
        }
        
        [Fact]
        public void AddPostToGetRedirects_Executes_Once()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddTransferValuesActionFilter();

            var count = services.Count;
            builder.AddTransferValuesActionFilter();

            services.Count.Should().Be(count);
        }

        [Fact]
        public void CreateFilter_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultActivators()
                .AddDefaultServices();
            builder.ConfigureOptions(x => x.AddList<Request, Item, Result>());

            var provider = services.BuildServiceProvider();

            var configurationProvider =
                provider.CreateScope().ServiceProvider.GetRequiredService<IListConfigurationProvider>();

            var configuration = configurationProvider.GetConfiguration(typeof(Request));

            configuration.Should().NotBeNull();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<Result>();
        }

        [Fact]
        public void AddList_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultActivators()
                .AddDefaultServices();
            builder.AddList<Request, Item, Result>();

            var provider = services.BuildServiceProvider();

            var configurationProvider =
                provider.CreateScope().ServiceProvider.GetRequiredService<IListConfigurationProvider>();

            var configuration = configurationProvider.GetConfiguration(typeof(Request));

            configuration.Should().NotBeNull();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<Result>();
        }

        [Fact]
        public void AddRequestFactory_Succeeds()
        {
            var services = new ServiceCollection();
            var builder = new GenericSearchServicesBuilder(services);
            builder.AddDefaultActivators()
                .AddDefaultServices();
            builder.AddRequestFactory<TestModelFactory>();

            var provider = services.BuildServiceProvider();

            var requestFactory = provider.CreateScope().ServiceProvider.GetRequiredService<IModelFactory>();
            requestFactory.Should().BeOfType<TestModelFactory>();
        }

        public class AddDefaultActivators
        {
            private static readonly Type[] AllTypes = typeof(GenericSearchOptions).Assembly.GetExportedTypes();

            private static IEnumerable<Type> SearchTypes => AllTypes
                .Where(x => !x.IsGenericType)
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(AbstractSearch).IsAssignableFrom(x));

            private static readonly Type ActivatorType = typeof(ISearchActivator<>);

            private static IEnumerable<Type> ActivatorTypes => SearchTypes
                .Select(x => ActivatorType.MakeGenericType(x));

            public static IEnumerable<object[]> Types => ActivatorTypes
                .Select(x => new object[] {x});

            private readonly IServiceProvider provider;

            public AddDefaultActivators()
            {
                var services = new ServiceCollection();
                var builder = new GenericSearchServicesBuilder(services);
                builder.AddDefaultActivators();
                provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
            }

            [Theory]
            [MemberData(nameof(Types))]
            public void Succeeds(Type type)
            {
                var activator = provider.GetService(type);
                activator.Should().NotBeNull();
            }
        }

        private class TestModelActivator : IModelActivator
        {
            public object Activate(IListConfiguration source)
            {
                throw new NotImplementedException();
            }
        }

        private class TestModelFactory : IModelFactory
        {
            public object Create(Type modelType)
            {
                throw new NotImplementedException();
            }
        }

        private class ActivatorFoo : ISearchActivator<TextSearch>
        {
            public ISearch Activate(string entityPath)
            {
                throw new NotImplementedException();
            }
        }

        private class ActivatorBar : ISearchActivator
        {
            public ISearch Activate(string entityPath)
            {
                throw new NotImplementedException();
            }
        }

        public class ProfileBaz : ListProfile
        {
        }

        private class Request
        {
        }

        private class Item
        {
        }

        private class Result
        {
        }
    }
}
