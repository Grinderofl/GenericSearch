using System;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Activation.Factories;
using GenericSearch.Internal.Configuration;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Activation
{
    public class ModelPropertyActivatorTests
    {
        [Fact]
        public void NullConfiguration_Throws()
        {
            var activatorFactory = new Mock<ISearchActivatorFactory>();
            var serviceProvider = new Mock<IServiceProvider>();
            var activator = new ModelPropertyActivator(activatorFactory.Object, serviceProvider.Object);

            var configuration = (IListConfiguration) null;
            var model = new Mock<object>();

            Assert.Throws<ArgumentNullException>(() => activator.Activate(configuration, model.Object));
        }

        [Fact]
        public void NullModel_Throws()
        {
            var activatorFactory = new Mock<ISearchActivatorFactory>();
            var serviceProvider = new Mock<IServiceProvider>();
            var activator = new ModelPropertyActivator(activatorFactory.Object, serviceProvider.Object);

            var configuration = new Mock<IListConfiguration>();
            var model = (object)null;

            Assert.Throws<ArgumentNullException>(() => activator.Activate(configuration.Object, model));
        }
    }
}