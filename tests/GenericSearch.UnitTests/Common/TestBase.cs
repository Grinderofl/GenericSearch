using System;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.UnitTests.Common
{
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected IGenericSearch GetGenericSearch() =>
            ServiceProvider.CreateScope()
                .ServiceProvider
                .GetRequiredService<IGenericSearch>();

        protected TestBase() => Initialize();
        
        private void Initialize()
        {
            var services = new ServiceCollection();
            var builder = services.AddGenericSearch();
            ConfigureSearch(builder);
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual void ConfigureSearch(IGenericSearchBuilder builder)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}