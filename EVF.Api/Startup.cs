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
            //Add Configure Extension and Bll class.
            services.ConfigureRepository(Configuration);
            services.ConfigureRedisCache(Configuration);
            services.ConfigureMasterBll();
            services.ConfigureAuthorizationBll();
            services.ConfigureCentralSettingBll();
            services.ConfigureHrBll();
            services.ConfigureHttpContextAccessor();
            services.ConfigureLoggerService();
            services.ConfigureCors();
            services.ConfigureJwtAuthen(Configuration);
            services.ConfigureCookieAuthen(Configuration);
            services.ConfigureBasicAuthen();
            services.ConfigureEmailService();
            services.ConfigureComponent();
            services.AddAutoMapper();
            services.ConfigureCustomResponseBadRequest();
            services.ConfigureMvc();
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaager();
            }
            app.ConfigureHandlerStatusPages();
            app.UseAuthentication();
            app.ConfigureMiddleware();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
