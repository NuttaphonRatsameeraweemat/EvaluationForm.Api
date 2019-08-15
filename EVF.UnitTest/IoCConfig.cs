using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EVF.Api;
using EVF.Data;
using EVF.Data.Repository.Interfaces;
using EVF.Bll.Components.InterfaceComponents;
using EVF.Bll.Components;
using EVF.Helper.Interfaces;
using EVF.Helper;
using EVF.Bll.Interfaces;
using EVF.Bll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.UnitTest
{
    /// <summary>
    /// The IoCConfig class provide installing all components needed to use.
    /// </summary>
    public class IoCConfig
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfig" /> class.
        /// </summary>
        public IoCConfig()
        {
            // Load configuration file.
            var config = new ConfigurationBuilder()
                            .AddJsonFile(this.GetAppSettingDirectory())
                            .Build();

            var services = new ServiceCollection();
            // Add services to the container.
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<EVFContext>(options =>
              options.UseNpgsql(config["ConnectionStrings:DefaultConnection"]));

            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<IUnitOfWork, EVFUnitOfWork>();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IConfigSetting, ConfigSetting>();
            services.AddScoped<ILoginBll, LoginBll>();
            services.AddScoped<IMenuBll, MenuBll>();
            services.AddScoped<IRoleBll, RoleBll>();
            services.AddScoped<IAdService, AdService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAdService, AdService>();
            services.AddSingleton<IK2Service, K2Service>();

            services.AddTransient<IManageToken, ManageToken>(c => new ManageToken(this.InitialHttpContext()));
            
            ServiceProvider = services.BuildServiceProvider();

        }

        /// <summary>
        /// Get appsetting json file in unit test directory.
        /// </summary>
        /// <returns>appsetting directory path.</returns>
        private string GetAppSettingDirectory()
        {
            return string.Concat(System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("bin")), "appsettings.json");
        }

        /// <summary>
        /// The Serivce Provider, this provides access to the IServiceCollection.
        /// </summary>
        public ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Initial Mockup HttpContext inject to test.
        /// </summary>
        /// <returns></returns>
        private HttpContextAccessor InitialHttpContext()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers["TEST"] = "12345";

            httpContextAccessor.HttpContext = httpContext;
            return httpContextAccessor;
        }

    }
}
