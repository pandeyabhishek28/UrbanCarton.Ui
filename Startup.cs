using GraphQL.Client.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using UrbanCarton.Mvc.Clients;

namespace urbancartonmvc
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
            services.AddMvc(o => o.EnableEndpointRouting = true);

            services.AddSingleton(t =>
            new GraphQLHttpClient(Configuration["ProductApiUri"],
            new GraphQL.Client.Serializer.Newtonsoft.NewtonsoftJsonSerializer()));

            services.AddSingleton<ProductGraphClient>();
            services.AddHttpClient<ProductHttpClient>(o =>
            o.BaseAddress = new Uri(Configuration["ProductApiUri"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
