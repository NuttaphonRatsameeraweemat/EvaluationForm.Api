﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.Swagger;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using EVF.Helper;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Helper.Components;

using EVF.Data;
using EVF.Data.Repository.Interfaces;

using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll;
using EVF.Master.Bll.Interfaces;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.Master.Bll;
using EVF.CentralSetting.Bll;

using EVF.Hr.Bll.Interfaces;
using EVF.Hr.Bll;

namespace EVF.Api.Extensions
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        public static void ConfigureRepository(this IServiceCollection services, IConfiguration Configuration)
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
        public static void ConfigureRedisCache(this IServiceCollection services, IConfiguration Configuration)
        {
            RedisCacheHandler.ConnectionString = Configuration["ConnectionStrings:RedisCacheConnection"];
        }

        /// <summary>
        /// Dependency Injection Master Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureMasterBll(this IServiceCollection services)
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
        public static void ConfigureAuthorizationBll(this IServiceCollection services)
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
        public static void ConfigureHrBll(this IServiceCollection services)
        {
            services.AddScoped<IHrCompanyBll, HrCompanyBll>();
            services.AddScoped<IHrOrgBll, HrOrgBll>();
            services.AddScoped<IHrEmployeeBll, HrEmployeeBll>();
        }

        /// <summary>
        /// Dependency Injection Central Setting Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureCentralSettingBll(this IServiceCollection services)
        {
            services.AddScoped<IHolidayCalendarBll, HolidayCalendarBll>();
            services.AddScoped<IValueHelpBll, ValueHelpBll>();
            services.AddScoped<IApprovalBll, ApprovalBll>();
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
        /// Dependency Injection Email Service. 
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureEmailService(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService, EmailService>();
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

        /// <summary>
        /// Config handle message status code 403.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureHandlerStatusPages(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 403)
                {
                    var model = new ResultViewModel
                    {
                        IsError = true,
                        StatusCode = context.HttpContext.Response.StatusCode,
                        Message = $"{MessageValue.UserRoleIsEmpty}"
                    };
                    string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    context.HttpContext.Response.ContentType = ConstantValue.ContentTypeJson;
                    await context.HttpContext.Response.WriteAsync(json);
                }
            });
        }

        /// <summary>
        /// Configuration Authentication Jwt type.
        /// </summary>
        /// <param name="services">The services conllection.</param>
        /// <param name="Configuration">The configuration.</param>
        public static void ConfigureJwtAuthen(this IServiceCollection services, IConfiguration Configuration)
        {
            var option = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = System.TimeSpan.Zero,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = option;
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         var model = new ResultViewModel
                         {
                             IsError = true,
                             StatusCode = context.Response.StatusCode,
                             Message = $"{MessageValue.Unauthorized}"
                         };
                         string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                         {
                             ContractResolver = new CamelCasePropertyNamesContractResolver()
                         });
                         context.Response.OnStarting(async () =>
                         {
                             context.Response.ContentType = ConstantValue.ContentTypeJson;
                             await context.Response.WriteAsync(json);
                         });
                         return System.Threading.Tasks.Task.CompletedTask;
                     },  
                     OnChallenge = context =>
                     {
                         context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                         var model = new ResultViewModel
                         {
                             IsError = true,
                             StatusCode = context.Response.StatusCode,
                             Message = $"{MessageValue.Unauthorized}"
                         };
                         string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                         {
                             ContractResolver = new CamelCasePropertyNamesContractResolver()
                         });
                         context.Response.OnStarting(async () =>
                         {
                             context.Response.ContentType = ConstantValue.ContentTypeJson;
                             await context.Response.WriteAsync(json);
                         });
                         return System.Threading.Tasks.Task.CompletedTask;
                     },
                 };
             });
        }

        /// <summary>
        /// Configuration Services Cookie Authentication Jwt format.
        /// </summary>
        /// <param name="services">The services conllection.</param>
        /// <param name="Configuration">The configuration.</param>
        public static void ConfigureCookieAuthen(this IServiceCollection services, IConfiguration Configuration)
        {
            var option = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = System.TimeSpan.Zero,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "access_token";
                        options.SlidingExpiration = true;
                        options.Events.OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                            var model = new ResultViewModel
                            {
                                IsError = true,
                                StatusCode = context.Response.StatusCode,
                                Message = $"{MessageValue.Unauthorized}"
                            };
                            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            });
                            context.Response.OnStarting(async () =>
                            {
                                context.Response.ContentType = ConstantValue.ContentTypeJson;
                                await context.Response.WriteAsync(json);
                            });
                            return System.Threading.Tasks.Task.CompletedTask;
                        };
                        options.Events.OnRedirectToAccessDenied = context =>
                        {
                            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                            var model = new ResultViewModel
                            {
                                IsError = true,
                                StatusCode = context.Response.StatusCode,
                                Message = $"{MessageValue.UserRoleIsEmpty}"
                            };
                            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            });
                            context.Response.OnStarting(async () =>
                            {
                                context.Response.ContentType = ConstantValue.ContentTypeJson;
                                await context.Response.WriteAsync(json);
                            });
                            return System.Threading.Tasks.Task.CompletedTask;
                        };
                        options.TicketDataFormat = new CookieAuthenticateFormat(SecurityAlgorithms.HmacSha256, option);
                    });
        }

    }
}
