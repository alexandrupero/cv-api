using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace alexroman.cv.api
{
    public class Startup
    {
        private readonly string _allowSpecificOrigins = nameof(_allowSpecificOrigins);
        private readonly string _allowAllOrigins = nameof(_allowAllOrigins);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .WithOrigins("https://alexroman.dev",
                                            "https://www.alexroman.dev")
                            .WithHeaders("Content-Type");
                    });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowAllOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            // Fix for Anemonis.AspNetCore.JsonRpc using synchronous calls
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddJsonRpc(options =>
                options.JsonSerializer = new JsonSerializer()
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
            });
            services.AddLazyCache();

            services.AddScoped<ICvDatabase, CvDatabase>();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(_allowAllOrigins);
            }
            else
            {
                app.UseCors(_allowSpecificOrigins);
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });

            app.UseJsonRpc();
        }
    }
}
