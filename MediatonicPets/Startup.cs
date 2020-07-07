using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MediatonicPets.Models;
using MediatonicPets.Services;
using MediatonicPets.Factories;

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
            
            services.Configure<GlobalPetConfigurationSettings>(
                Configuration.GetSection(nameof(GlobalPetConfigurationSettings)));

            services.AddSingleton<IPetDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PetDatabaseSettings>>().Value);

            services.AddSingleton<IGlobalPetConfigurationSettings>(sp =>
                sp.GetRequiredService<IOptions<GlobalPetConfigurationSettings>>().Value);

            services.AddSingleton<PetService>();
            services.AddSingleton<UserService>();

            services.AddControllers();
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
        }
    }
}
