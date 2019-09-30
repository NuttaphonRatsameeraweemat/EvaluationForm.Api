using AutoMapper;
using EVF.Tranfer.Service.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVF.Tranfer.Service.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            NLog.LogManager.LoadConfiguration(string.Concat(System.IO.Directory.GetCurrentDirectory(), "/NLog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Configure Extension and Bll class.
            services.ConfigureEvfCoreRepository(Configuration);
            services.ConfigureEvfTranferRepository(Configuration);
            services.ConfigureBrbUtilRepository(Configuration);
            services.ConfigureBll();
            services.ConfigureHttpContextAccessor();
            services.ConfigureLoggerService();
            services.ConfigureCors();
            services.ConfigureBasicAuthen();
            services.ConfigureComponent();
            services.ConfigureCustomResponseBadRequest();
            services.ConfigureServiceResolver();
            services.ConfigureMvc();
            services.AddAutoMapper();
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ConfigureUseSwagger();

            app.UseAuthentication();
            app.ConfigureMiddleware();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
