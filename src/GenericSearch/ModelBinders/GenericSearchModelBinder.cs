using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericSearch.Configuration;
using GenericSearch.Definition;
using GenericSearch.Internal;
using GenericSearch.ModelBinders.Activation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenericSearch.ModelBinders
{
    public class GenericSearchModelBinder : IModelBinder
    {
        private readonly ListConfiguration configuration;
        private readonly IRequestActivator requestActivator;
        private readonly IRequestPropertyActivator requestPropertyActivator;
        private readonly IModelCache modelCache;
        private readonly IModelBinder fallbackModelBinder;

        public GenericSearchModelBinder(ListConfiguration configuration, 
                                        IRequestActivator requestActivator,
                                        IRequestPropertyActivator requestPropertyActivator,
                                        IModelCache modelCache,
                                        IModelBinder fallbackModelBinder)
        {
            this.configuration = configuration;
            this.requestActivator = requestActivator;
            this.requestPropertyActivator = requestPropertyActivator;
            this.modelCache = modelCache;
            this.fallbackModelBinder = fallbackModelBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = requestActivator.Activate(configuration);
            if (model == null)
            {
                await fallbackModelBinder.BindModelAsync(bindingContext);
                return;
            }

            requestPropertyActivator.Activate(configuration, model);
            bindingContext.Model = model;
            await fallbackModelBinder.BindModelAsync(bindingContext);
            modelCache.Put(bindingContext.Model);
        }
    }
}
