using GenericSearch.Configuration;
using GenericSearch.Extensions;
using GenericSearch.ModelBinders.Activation;
using GenericSearch.UnitTests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.UnitTests
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

        protected static (IGenericSearch search, TRequest request) Create<TRequest, TProfile>() where TRequest : new() where TProfile : ListProfile, new()
        {
            var services = new ServiceCollection();
            services.AddDefaultGenericSearch(typeof(GenericSearch).Assembly).AddProfile<TProfile>();
            var rootProvider = services.BuildServiceProvider();
            var scopedProvider = rootProvider.CreateScope().ServiceProvider;

            var search = scopedProvider.GetRequiredService<IGenericSearch>();
            var activator = scopedProvider.GetRequiredService<ISearchPropertyActivator>();
            var configuration = scopedProvider.GetRequiredService<IListConfigurationProvider>().GetConfiguration(typeof(TRequest));

            var request = new TRequest();
            activator.Activate(configuration, request);
            return (search, request);
        }

        public class TestServices<TRequest>
        {
            public TestServices(IGenericSearch search, TRequest request)
            {
                Search = search;
                Request = request;
            }

            public IGenericSearch Search { get; }
            public TRequest Request { get; }
        }
    }
}