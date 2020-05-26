using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenericSearch.UnitTests
{
    internal class NullModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}