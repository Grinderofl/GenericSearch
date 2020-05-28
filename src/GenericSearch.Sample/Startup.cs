using AutoMapper;
using FluentValidation.AspNetCore;
using GenericSearch.Sample.Data;
using GenericSearch.Sample.Features.Startup;
using Grinderofl.FeatureFolders;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: AspMvcViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

[assembly: AspMvcPartialViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat(@"~\Features\{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\{1}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\Features\{1}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\Shared\{0}.cshtml")]

[assembly: AspMvcAreaPartialViewLocationFormat(@"~\Areas\{2}\{1}\{0}.cshtml")]
[assembly: AspMvcAreaPartialViewLocationFormat(@"~\Areas\{2}\Features\{1}\{0}.cshtml")]
[assembly: AspMvcAreaPartialViewLocationFormat(@"~\Areas\{2}\{0}.cshtml")]
[assembly: AspMvcAreaPartialViewLocationFormat(@"~\Areas\{2}\Shared\{0}.cshtml")]

namespace GenericSearch.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NorthwindDbContext>(ConfigureDbContextOptionsBuilder);

            services.AddControllersWithViews()
                    .AddFluentValidation()
                    .AddFeatureFolders()
                    .AddAreaFeatureFolders();

            services.AddRouting(x =>
                                {
                                    x.LowercaseQueryStrings = true;
                                    x.LowercaseUrls = true;
                                });

            services.AddMediatR(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
            services.AddGenericSearch(GetType().Assembly)
                    .UseConventions()
                    //.UseConventions(c => c.DefaultSortOrderPropertyName("Ordx").DefaultSortDirectionPropertyName("Ordd"))
                    .DefaultRowsPerPage(20);

            ConfigureOptions(services);
        }

        protected virtual void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected virtual void ConfigureOptions(IServiceCollection services)
        {
            services.ConfigureOptions<FluentValidationMvcConfigurationConfigurer>();
        }

        protected virtual void PreConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                return;
            }

            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NorthwindDbContext>();
            context.Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            PreConfigure(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
