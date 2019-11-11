using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EVF.Api;
using EVF.Data;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper;
using Microsoft.AspNetCore.Http;
using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll;
using System.Security.Claims;
using EVF.Helper.Components;
using System.Security.Principal;
using EVF.Hr.Bll.Interfaces;
using EVF.Hr.Bll;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll;
using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll;
using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll;

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
            services.AddSingleton<IConfiguration>(config);
            services.AddAutoMapper(typeof(Startup));

            this.ConfigureRepository(services, config);
            this.ConfigureRedisCache(services, config);
            this.ConfigureMasterBll(services);
            this.ConfigureAuthorizationBll(services);
            this.ConfigureHrBll(services);
            this.ConfigureCentralSettingBll(services);
            this.ConfigureReportBll(services);
            this.ConfigureComponent(services);
            this.ConfigureHttpContextAccessor(services);
            this.ConfigureLoggerService(services);
            this.ConfigureEvaluationBll(services);
            this.ConfigureWorkflowBll(services);
            this.ConfigureVendorBll(services);

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
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        private void ConfigureRepository(IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<EVFContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddTransient<IUnitOfWork, EVFUnitOfWork>();
        }

        /// <summary>
        /// Dependency Injection rediscahce.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        private void ConfigureRedisCache(IServiceCollection services, IConfigurationRoot Configuration)
        {
            RedisCacheHandler.ConnectionString = Configuration["ConnectionStrings:RedisCacheConnection"];
        }

        /// <summary>
        /// Dependency Injection Master Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureMasterBll(IServiceCollection services)
        {
            services.AddScoped<IKpiBll, KpiBll>();
            services.AddScoped<IKpiGroupBll, KpiGroupBll>();
            services.AddScoped<IPeriodBll, PeriodBll>();
            services.AddScoped<IGradeBll, GradeBll>();
            services.AddScoped<ILevelPointBll, LevelPointBll>();
            services.AddScoped<ICriteriaBll, CriteriaBll>();
            services.AddScoped<IEvaluationTemplateBll, EvaluationTemplateBll>();
        }

        /// <summary>
        /// Dependency Injection Authorization Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureAuthorizationBll(IServiceCollection services)
        {
            services.AddScoped<ILoginBll, LoginBll>();
            services.AddScoped<IRoleBll, RoleBll>();
            services.AddScoped<IMenuBll, MenuBll>();
            services.AddScoped<IUserRoleBll, UserRoleBll>();
            services.AddScoped<IAuthorityCompanyBll, AuthorityCompanyBll>();
        }

        /// <summary>
        /// Dependency Injection HR Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureHrBll(IServiceCollection services)
        {
            services.AddScoped<IHrCompanyBll, HrCompanyBll>();
            services.AddScoped<IHrOrgBll, HrOrgBll>();
            services.AddScoped<IHrEmployeeBll, HrEmployeeBll>();
        }

        /// <summary>
        /// Dependency Injection Central Setting Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureCentralSettingBll(IServiceCollection services)
        {
            services.AddScoped<IHolidayCalendarBll, HolidayCalendarBll>();
            services.AddScoped<IValueHelpBll, ValueHelpBll>();
            services.AddScoped<IApprovalBll, ApprovalBll>();
        }

        /// <summary>
        /// Dependency Injection Evaluation Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureEvaluationBll(IServiceCollection services)
        {
            services.AddScoped<IEvaluationBll, EvaluationBll>();
            services.AddScoped<IEvaluationAssignBll, EvaluationAssignBll>();
            services.AddScoped<IEvaluationLogBll, EvaluationLogBll>();
            services.AddScoped<ISummaryEvaluationBll, SummaryEvaluationBll>();
            services.AddScoped<IEvaluationSapResultBll, EvaluationSapResultBll>();
        }

        /// <summary>
        /// Dependency Injection Workflow Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureWorkflowBll(IServiceCollection services)
        {
            services.AddScoped<IWorkflowBll, WorkflowBll>();
            services.AddScoped<IWorkflowDelegateBll, WorkflowDelegateBll>();
        }

        /// <summary>
        /// Dependency Injection Vendor Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureVendorBll(IServiceCollection services)
        {
            services.AddScoped<IVendorBll, VendorBll>();
            services.AddScoped<IVendorFilterBll, VendorFilterBll>();
            services.AddScoped<IVendorTransectionBll, VendorTransectionBll>();
        }

        /// <summary>
        /// Dependency Injection Inbox Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureReportBll(IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IVendorEvaluationReportBll, VendorEvaluationReportBll>();
            services.AddScoped<IEvaluationSummaryReportBll, EvaluationSummaryReportBll>();
        }

        /// <summary>
        /// Register service components class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureComponent(IServiceCollection services)
        {
            services.AddSingleton<IConfigSetting, ConfigSetting>();
            services.AddSingleton<IAdService, AdService>();
            services.AddSingleton<IK2Service, K2Service>();
            services.AddSingleton(typeof(IElasticSearch<>), typeof(ElasticSearch<>));

            services.AddTransient<IManageToken, ManageToken>(c => new ManageToken(this.InitialHttpContext()));
        }

        /// <summary>
        /// Dependency Injection Httpcontext.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureHttpContextAccessor(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Add Singletion Logger Class
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureLoggerService(IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Initial Mockup HttpContext inject to test.
        /// </summary>
        /// <returns></returns>
        private HttpContextAccessor InitialHttpContext()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = new DefaultHttpContext();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "BOONRAWD_LOCAL\\ds01"));
            identity.AddClaim(new Claim(ConstantValue.ClamisEncrypt, UtilityService.EncryptString("hw_2931", "UlZaR1JVNURVbGxRVkVsUFRrdEZXUT09")));
            identity.AddClaim(new Claim(ConstantValue.ClamisEmpNo, "001754"));
            identity.AddClaim(new Claim(ConstantValue.ClamisName, string.Format(ConstantValue.EmpTemplate, "สัญชัย", "ต้นพุดซา")));
            identity.AddClaim(new Claim(ConstantValue.ClamisOrg, "10001416"));
            identity.AddClaim(new Claim(ConstantValue.ClamisPosition, "20000641"));
            identity.AddClaim(new Claim(ConstantValue.ClamisComCode, "1600"));
            identity.AddClaim(new Claim(ConstantValue.ClamisPurchasing, "1600"));
            var user = new GenericPrincipal(new ClaimsIdentity(identity), new string[] { "ADMIN" });
            httpContext.User = user;

            httpContextAccessor.HttpContext = httpContext;
            return httpContextAccessor;
        }

    }
}
