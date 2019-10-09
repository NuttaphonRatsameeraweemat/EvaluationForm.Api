using AutoMapper;
using EVF.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVF.Api
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
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Configure Repository.
            services.ConfigureRepository(Configuration);
            services.ConfigureRedisCache(Configuration);
            //Add Bll class.
            services.ConfigureMasterBll();
            services.ConfigureAuthorizationBll();
            services.ConfigureCentralSettingBll();
            services.ConfigureHrBll();
            services.ConfigureWorkflowBll();
            services.ConfigureEvaluationBll();
            services.ConfigureVendorBll();
            //Add Configure Extension.
            services.ConfigureHttpContextAccessor();
            services.ConfigureLoggerService();
            services.ConfigureCors();
            //Release
            services.ConfigureCookieAuthen(Configuration);
            //Debug
            //services.ConfigureJwtAuthen(Configuration);
            services.ConfigureEmailService();
            services.ConfigureComponent();
            services.AddAutoMapper();
            services.ConfigureMvc();
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
                app.ConfigureUseSwagger();

            //For JWT test
            //app.ConfigureHandlerStatusPages();
           
            app.UseAuthentication();
            app.ConfigureMiddleware();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
