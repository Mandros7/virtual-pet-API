using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MediatonicPets.Models;
using MediatonicPets.Services;
using MediatonicPets.Factories;
using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace MediatonicPets
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
            services.Configure<PetDatabaseSettings>(
                Configuration.GetSection(nameof(PetDatabaseSettings)));
            
            services.Configure<PetConfigurationSettings>(
                Configuration.GetSection(nameof(GlobalPetConfigurationSettings)));

            services.Configure<List<PetConfigurationSettings>>(
                Configuration.GetSection("GlobalPetConfigurationSettings:PetConfigurationSettings"));

            services.AddSingleton<IPetDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PetDatabaseSettings>>().Value);

            services.AddSingleton<List<PetConfigurationSettings>>(sp =>
                sp.GetRequiredService<IOptions<List<PetConfigurationSettings>>>().Value);

            services.AddSingleton<PetService>();
            services.AddSingleton<UserService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Pet API",
                    Description = "A simple Pet API using ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Hector Rodriguez",
                        Email = "hectorrcov93@gmail.com",
                        Url = new Uri("https://github.com/Mandros7"),
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

                // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet API V1");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
