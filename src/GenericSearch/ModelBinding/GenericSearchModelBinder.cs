using System.Threading.Tasks;
using GenericSearch.Internal;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenericSearch.ModelBinding
{
    public class GenericSearchModelBinder : IModelBinder
    {
        private readonly IListConfiguration configuration;
        private readonly IModelActivator modelActivator;
        private readonly IModelPropertyActivator modelPropertyActivator;
        private readonly IModelCache modelCache;
        private readonly IModelBinder fallbackModelBinder;

        public GenericSearchModelBinder(IListConfiguration configuration, 
                                        IModelActivator modelActivator,
                                        IModelPropertyActivator modelPropertyActivator,
                                        IModelCache modelCache,
                                        IModelBinder fallbackModelBinder)
        {
            this.configuration = configuration;
            this.modelActivator = modelActivator;
            this.modelPropertyActivator = modelPropertyActivator;
            this.modelCache = modelCache;
            this.fallbackModelBinder = fallbackModelBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = modelActivator.Activate(configuration);
            if (model == null)
            {
                await fallbackModelBinder.BindModelAsync(bindingContext);
                return;
            }

            modelPropertyActivator.Activate(configuration, model);
            bindingContext.Model = model;
            await fallbackModelBinder.BindModelAsync(bindingContext);
            modelCache.Put(bindingContext.Model);
        }
    }
}
