using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;

namespace GenericSearch.Sample.Features.Startup
{
    public class FluentValidationMvcConfigurationConfigurer : IConfigureOptions<FluentValidationMvcConfiguration>
    {
        public void Configure(FluentValidationMvcConfiguration options)
        {
            options.RegisterValidatorsFromAssemblyContaining<Sample.Startup>();
        }
    }
}
