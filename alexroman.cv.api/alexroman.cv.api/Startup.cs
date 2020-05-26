using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace alexroman.cv.api
{
    public class Startup
    {
        private readonly string _allowSpecificOrigins = nameof(_allowSpecificOrigins);

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
                        builder.WithOrigins("https://alexroman.dev",
                                            "https://www.alexroman.dev");
                    });
            });

            // Fix for Anemonis.AspNetCore.JsonRpc using synchronous calls
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddJsonRpc();
            services.AddLazyCache();

            services.AddScoped<ICvDatabase, CvDatabase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseJsonRpc();

            app.UseCors(_allowSpecificOrigins);
        }
    }
}
