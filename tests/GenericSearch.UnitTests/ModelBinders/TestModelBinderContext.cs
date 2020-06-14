using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace GenericSearch.UnitTests.ModelBinders
{
    internal class TestModelBinderContext : ModelBinderProviderContext
    {
        public TestModelBinderContext(Type modelType, ServiceProvider services)
        {
            Services = services;
            Metadata = new TestModelMetadata(modelType);
        }

        public override IModelBinder CreateBinder(ModelMetadata metadata)
        {
            throw new NotImplementedException();
        }

        public override BindingInfo BindingInfo { get; }
        public override ModelMetadata Metadata { get; }
        public override IModelMetadataProvider MetadataProvider { get; }
        public override IServiceProvider Services { get; }
    }
}