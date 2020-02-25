#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Grinderofl.GenericSearch.ModelBinding
{
    public class GenericSearchModelBinder : IModelBinder
    {
        private readonly IRequestBinder requestBinder;
        private readonly ISearchConfiguration configuration;
        private readonly IModelBinder fallbackModelBinder;

        public GenericSearchModelBinder(IRequestBinder requestBinder, ISearchConfiguration configuration, IModelBinder fallbackModelBinder)
        {
            this.requestBinder = requestBinder;
            this.configuration = configuration;
            this.fallbackModelBinder = fallbackModelBinder;
        }

        public virtual async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.Model == null)
            {
                bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
            }

            requestBinder.BindRequest(bindingContext.Model, configuration);

            await fallbackModelBinder.BindModelAsync(bindingContext);
        }


    }
}
