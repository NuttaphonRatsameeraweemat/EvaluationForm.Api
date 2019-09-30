using EVF.Data;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Tranfer.Service.Bll;
using EVF.Tranfer.Service.Bll.Interfaces;
using EVF.Tranfer.Service.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Net;
using static EVF.Tranfer.Service.Data.UnitOfWorkControl;

namespace EVF.Tranfer.Service.Api.Extensions
{
    public static class ServiceExtensions
    {
       

        /// <summary>
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        public static void ConfigureEvfCoreRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<EVFContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddTransient<EVFUnitOfWork>();
        }

        /// <summary>
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        public static void ConfigureEvfTranferRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<DMContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DataMartConnection"]));

            services.AddTransient<DMUnitOfWork>();
        }

        /// <summary>
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        public static void ConfigureBrbUtilRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<BrbUtilContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:BrbUtilConnection"]));

            services.AddTransient<BrbUtilUnitOfWork>();
        }

        /// <summary>
        /// Control dependency injection unit of work.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServiceResolver(this IServiceCollection services)
        {
            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "DM":
                        return serviceProvider.GetService<DMUnitOfWork>();
                    case "EVF":
                        return serviceProvider.GetService<EVFUnitOfWork>();
                    case "BRB":
                        return serviceProvider.GetService<BrbUtilUnitOfWork>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
        }

        /// <summary>
        /// Dependency Injection Authorization Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureBll(this IServiceCollection services)
        {
            services.AddScoped<ITranferBll, TranferBll>();
        }

        /// <summary>
        /// Register service components class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureComponent(this IServiceCollection services)
        {
            services.AddSingleton<IConfigSetting, ConfigSetting>();
            services.AddSingleton<IAdService, AdService>();
            services.AddSingleton<IK2Service, K2Service>();

            services.AddTransient<IManageToken, ManageToken>();
        }

        /// <summary>
        /// Dependency Injection Httpcontext.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Add Singletion Logger Class
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Custom resposse when model state invalid.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCustomResponseBadRequest(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult(new
                    {
                        result = UtilityService.InitialResultError(ConstantValue.HttpBadRequestMessage, (int)HttpStatusCode.BadRequest),
                        modelError = actionContext.ModelState
                    });
            });
        }

        /// <summary>
        /// Configure adding mvc and configure prefix route and filter.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.UseApiGlobalConfigRoutePrefix(new RouteAttribute("api"));
                opt.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(
                        UtilityService.InitialResultError(ConstantValue.HttpBadRequestMessage, (int)HttpStatusCode.BadRequest,
                                        actionContext.ModelState.Keys));
                };
            });
        }

        /// <summary>
        /// Config Api Routes Prefix.
        /// </summary>
        /// <param name="opts">The MvcOptions.</param>
        /// <param name="routeAttribute">The IRouteTemplateProvider.</param>
        public static void UseApiGlobalConfigRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new ApiGlobalPrefixRouteProvider(routeAttribute));
        }

        /// <summary>
        /// Add Middleware when request begin and end.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<Middleware>();
        }

        /// <summary>
        /// Add CORS Configuration.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        /// <summary>
        /// Configuration Swaager Doc and Authentication type.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "Header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
        }

        /// <summary>
        /// Setup Application Builder using Swagger and Swagger Ui.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureUseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
            });
        }

        /// <summary>
        /// Configuration Basic Authentication type.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureBasicAuthen(this IServiceCollection services)
        {
            // configure basic authentication 
            services.AddAuthentication(ConstantValue.BasicAuthentication)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(ConstantValue.BasicAuthentication, null);

        }

    }
}
