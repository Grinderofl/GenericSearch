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
        private readonly IModelActivator modelActivator;
        private readonly ISearchPropertyActivator requestPropertyActivator;
        private readonly IModelCache modelCache;
        private readonly IModelBinder fallbackModelBinder;

        public GenericSearchModelBinder(ListConfiguration configuration, 
                                        IModelActivator modelActivator,
                                        ISearchPropertyActivator requestPropertyActivator,
                                        IModelCache modelCache,
                                        IModelBinder fallbackModelBinder)
        {
            this.configuration = configuration;
            this.modelActivator = modelActivator;
            this.requestPropertyActivator = requestPropertyActivator;
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

            requestPropertyActivator.Activate(configuration, model);
            bindingContext.Model = model;
            await fallbackModelBinder.BindModelAsync(bindingContext);
            modelCache.Put(bindingContext.Model);
        }
    }
}
