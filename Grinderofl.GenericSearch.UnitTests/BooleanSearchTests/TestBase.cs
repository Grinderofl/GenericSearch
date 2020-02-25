using System;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.ModelBinding;
using Grinderofl.GenericSearch.Processors;
using Moq;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public class TestBase
    {
        protected Mock<IPropertyProcessorProvider> ProcessorProvider { get; }
        protected ISearchConfiguration SearchConfiguration { get; }
        protected IRequestBinder RequestBinder { get; }

        public TestBase()
        {
            var options = new GenericSearchOptions {ConventionOptions = {UseConventions = true}};
            var profile = new TestProfile();

            SearchConfiguration = new ConventionSearchConfiguration(profile, options);

            var processor = new PropertyProcessor(SearchConfiguration);

            ProcessorProvider = new Mock<IPropertyProcessorProvider>();
            ProcessorProvider.Setup(x => x.ProviderForRequestType(It.IsAny<Type>()))
                             .Returns(processor);

            RequestBinder = new GenericSearchRequestBinder();
        }
    }
}
