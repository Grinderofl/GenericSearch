using System;
using System.Linq;
using System.Reflection;
using GenericSearch.ActionFilters;
using GenericSearch.ActionFilters.Configuration;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Activation.Factories;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition;
using GenericSearch.ModelBinding.Configuration;
using GenericSearch.Searches.Activation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GenericSearch.Internal.Configuration
{
    public class GenericSearchServicesBuilder : IGenericSearchServicesBuilder
    {
        private readonly IServiceCollection services;

        public GenericSearchServicesBuilder(IServiceCollection services)
        {
            this.services = services;
            this.services.AddOptions<GenericSearchOptions>();
        }

        private bool defaultActivatorsAdded;

        public IGenericSearchServicesBuilder AddProfilesFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Contains(typeof(IListDefinitionSource)));

            foreach (var type in types)
            {
                AddProfile(type);
            }

            return this;
        }

        public IGenericSearchServicesBuilder AddProfile<T>() where T : class, IListDefinitionSource
        {
            services.AddSingleton<IListDefinitionSource, T>();
            return this;
        }

        public IGenericSearchServicesBuilder AddProfile<T>(T profile) where T : IListDefinitionSource
        {
            services.AddSingleton<IListDefinitionSource>(profile);
            return this;
        }

        public IGenericSearchServicesBuilder AddProfile(Type profileType)
        {
            services.AddSingleton(typeof(IListDefinitionSource), profileType);
            return this;
        }

        public IGenericSearchServicesBuilder AddProfilesFromAssemblyOf<T>() => 
            AddProfilesFromAssembly(typeof(T).Assembly);

        public IGenericSearchServicesBuilder AddDefaultServices()
        {
            services.AddHttpContextAccessor();

            services.TryAddSingleton<IModelCache, HttpContextModelCache>();
            services.TryAddSingleton<IListConfigurationProvider, ListConfigurationProvider>();
            
            services.TryAddSingleton<IListConfigurationFactory, ListConfigurationFactory>();
            services.TryAddSingleton<ISearchConfigurationFactory, SearchConfigurationFactory>();
            services.TryAddSingleton<IPageConfigurationFactory, PageConfigurationFactory>();
            services.TryAddSingleton<IRowsConfigurationFactory, RowsConfigurationFactory>();
            services.TryAddSingleton<ISortColumnConfigurationFactory, SortColumnConfigurationFactory>();
            services.TryAddSingleton<ISortDirectionConfigurationFactory, SortDirectionConfigurationFactory>();
            services.TryAddSingleton<IPropertyConfigurationFactory, PropertyConfigurationFactory>();
            services.TryAddSingleton<IPostRedirectGetConfigurationFactory, PostRedirectGetConfigurationFactory>();
            services.TryAddSingleton<ITransferValuesConfigurationFactory, TransferValuesConfigurationFactory>();
            services.TryAddSingleton<IModelActivatorConfigurationFactory, ModelActivatorConfigurationFactory>();
            services.TryAddSingleton<IPropertyPathFinder, PascalCasePropertyPathFinder>();

            services.TryAddScoped<IRequestModelProvider, RequestModelProvider>();
            services.TryAddScoped<IGenericSearch, GenericSearch>();
            services.TryAddSingleton<IModelActivator, ModelActivator>();
            services.TryAddScoped<IGenericSearchRouteValueTransformer, GenericSearchRouteValueTransformer>();
            services.TryAddScoped<IModelPropertyActivator, ModelPropertyActivator>();
            services.TryAddScoped<ISearchActivatorFactory, SearchActivatorFactory>();
            

            return this;
        }

        public IGenericSearchServicesBuilder AddDefaultActivators()
        {
            if (defaultActivatorsAdded)
            {
                return this;
            }

            AddSearchActivator<TextSearchActivator>();
            AddSearchActivator<BooleanSearchActivator>();
            AddSearchActivator<DateSearchActivator>();
            AddSearchActivator<DecimalSearchActivator>();
            AddSearchActivator<IntegerSearchActivator>();
            AddSearchActivator<MultipleDateOptionSearchActivator>();
            AddSearchActivator<MultipleIntegerOptionSearchActivator>();
            AddSearchActivator<MultipleTextOptionSearchActivator>();
            AddSearchActivator<OptionalBooleanSearchActivator>();
            AddSearchActivator<SingleDateOptionSearchActivator>();
            AddSearchActivator<SingleIntegerOptionSearchActivator>();
            AddSearchActivator<SingleTextOptionSearchActivator>();
            AddSearchActivator<TrueBooleanSearchActivator>();

            defaultActivatorsAdded = true;

            return this;
        }

        public IGenericSearchServicesBuilder AddModelBinder()
        {
            if (services.All(x => x.ImplementationType != typeof(AddGenericSearchModelBinder)))
            {
                services.ConfigureOptions<AddGenericSearchModelBinder>();
            }
            
            return this;
        }

        public IGenericSearchServicesBuilder AddTransferValuesActionFilter()
        {
            if(services.All(x => x.ImplementationType != typeof(AddTransferValuesActionFilter)))
            {
                services.ConfigureOptions<AddTransferValuesActionFilter>();
            }

            return this;
        }

        public IGenericSearchServicesBuilder AddPostRedirectGetActionFilter()
        {
            if(services.All(x => x.ImplementationType != typeof(AddPostRedirectGetActionFilter)))
            {
                services.ConfigureOptions<AddPostRedirectGetActionFilter>();
            }

            return this;
        }

        public IGenericSearchServicesBuilder AddSearchActivator<TActivator>() 
            where TActivator : class, ISearchActivator
        {
            var genericInterface = typeof(TActivator).GetInterfaces()
                .SingleOrDefault(x => x.IsGenericType && x.GenericTypeArguments.Length == 1);

            if (genericInterface == null)
            {
                throw new ArgumentException($"The class '{typeof(TActivator).FullName}' does not implement '{typeof(ISearchActivator<>)}'");
            }
            
            var searchType = genericInterface.GenericTypeArguments.First();
            var activatorType = typeof(ISearchActivator<>).MakeGenericType(searchType);

            services.AddScoped(activatorType, typeof(TActivator));
            return this;
        }

        public IGenericSearchServicesBuilder ConfigureOptions(Action<GenericSearchOptions> optionsAction)
        {
            services.Configure(optionsAction);
            return this;
        }

        public IGenericSearchServicesBuilder Configure(Action<ListProfile> configureAction)
        {
            var profile = new ListProfile();
            configureAction(profile);

            var source = (IListDefinitionSource) profile;

            foreach (var definition in source.Definitions)
            {
                services.AddSingleton(definition);
            }

            return this;
        }

        public IGenericSearchServicesBuilder AddList<TRequest, TEntity, TResult>(Action<IListExpression<TRequest, TEntity, TResult>> action = null)
        {
            var profile = new ListProfile();
            var expression = profile.AddList<TRequest, TEntity, TResult>();
            action?.Invoke(expression);
            services.AddSingleton<IListDefinitionSource>(profile);
            return this;
        }

        public IGenericSearchServicesBuilder AddModelFactory<TRequestFactory>() where TRequestFactory : class, IModelFactory
        {
            services.AddScoped<IModelFactory, TRequestFactory>();
            return this;
        }
    }
}
