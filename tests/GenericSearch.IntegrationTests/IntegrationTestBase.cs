using GenericSearch.Extensions;
using GenericSearch.IntegrationTests.Internal;
using GenericSearch.IntegrationTests.Internal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        // Never actually gets used
        private const string ConnectionString = "Data Source=.;Initial Catalog=GenericSearch.IntegrationTests;PersistSecurityInfo=True;Trusted_Connection=True";

        protected TestContext Context { get; }

        protected IntegrationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            Context = new TestContext(optionsBuilder.Options);
        }

        protected static IGenericSearch CreateSearch(ListProfile profile)
        {
            var services = new ServiceCollection();
            services.AddDefaultGenericSearch().AddProfile(profile);
            var provider = services.BuildServiceProvider();
            var scope = provider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IGenericSearch>();
        }
    }
}