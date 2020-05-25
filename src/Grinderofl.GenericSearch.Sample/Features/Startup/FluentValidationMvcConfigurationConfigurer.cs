using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;

namespace Grinderofl.GenericSearch.Sample.Features.Startup
{
    public class FluentValidationMvcConfigurationConfigurer : IConfigureOptions<FluentValidationMvcConfiguration>
    {
        public void Configure(FluentValidationMvcConfiguration options)
        {
            options.RegisterValidatorsFromAssemblyContaining<Sample.Startup>();
        }
    }
}
