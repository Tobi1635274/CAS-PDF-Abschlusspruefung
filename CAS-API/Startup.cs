using CAS_API.Services;
using GdPicture14.WEB;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CAS_API
{
#pragma warning disable 1591
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            DocuViewareManager.SetupConfiguration(true, DocuViewareSessionStateMode.File, Path.Combine(Directory.GetCurrentDirectory(), "Cache"), "http://localhost:5018/", "DocuVieware");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppConfiguration(_configuration);
            services.AddControllers();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CAS-PDF", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(x => x.MapControllers());
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CAS-PDF v1");
            });
        }
    }
#pragma warning restore 1591
}
