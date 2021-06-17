using DataMicroservice.Hubs;
using DataMicroservice.IRepository;
using DataMicroservice.Repository;
using DataMicroservice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DataMicroservice
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataMicroservice", Version = "v1" });
                c.EnableAnnotations();
            });
            services.AddCors(options => {
                options.AddPolicy("CORS", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            services.AddSignalR();
            Hivemq mqtt = new Hivemq();
            services.AddSingleton(mqtt);
            services.AddScoped<ISensorContext, SensorContext>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddSingleton(new DataService(mqtt));
            services.AddSingleton<DataHub>(new DataHub());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataMicroservice v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseWebSockets();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DataHub>("hub/Data");
            });
        }
    }
}
