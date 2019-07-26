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
              options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]));

            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<IUnitOfWork, EVFUnitOfWork>();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IConfigSetting, ConfigSetting>();
            services.AddScoped<IAdService, AdService>();

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

    }
}
