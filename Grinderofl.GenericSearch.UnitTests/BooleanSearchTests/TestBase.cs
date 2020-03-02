using System;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.ModelBinding;
using Grinderofl.GenericSearch.Processors;
using Moq;

// ReSharper disable All

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public abstract class TestBase
    {
        protected Mock<IPropertyProcessorProvider> ProcessorProvider { get; }
        protected ISearchConfiguration SearchConfiguration { get; }
        protected IRequestBinder RequestBinder { get; }

        protected TestBase()
        {
            var options = new GenericSearchOptions {ConventionOptions = {UseConventions = true}};
            var profile = CreateProfile();

            SearchConfiguration = new ConventionSearchConfiguration(profile, options);

            var processor = new PropertyProcessor(SearchConfiguration);

            ProcessorProvider = new Mock<IPropertyProcessorProvider>();
            ProcessorProvider.Setup(x => x.ProviderForRequestType(It.IsAny<Type>()))
                             .Returns(processor);

            RequestBinder = new GenericSearchRequestBinder();
        }

        protected virtual TestProfile CreateProfile()
        {
            return new TestProfile();
        }
    }
}
