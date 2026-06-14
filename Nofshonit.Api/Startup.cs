using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nofshonit.Common.EF.Club;
//using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Nofshonit.Common.DTOs.MapperManagement;
using AutoMapper;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Infrastructure.Utils.Log;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Nofshonit.Common;
using Microsoft.AspNetCore.Http.Features;
using Nofshonit.Common.Constants;
using Nofshonit.BL.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Nofshonit.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using DtsStructuredLogger;
using Serilog;
using Serilog.Events;
using System;

namespace Nofshonit.Api
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
            Serilog.Log.Logger = Logger.GetLogger();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            //services.AddTransient<ILog, Log>();
            services.AddDistributedMemoryCache();
            services.AddSession();
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<UserBL>();
            services.AddAuthentication().AddCookie(opts =>
            {
                opts.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // ******************** 
            // Setup CORS 
            // ******************** 
            var CorsOrigins = ContainerManager.Container.Resolve<Infrastructure.Configuration.IConfigurationManager>().GetCorsOrigins();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(CorsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader() 
                    .AllowCredentials()
                    );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My First API",
                    Description = "My First ASP.NET Core 7 Web API"
                });
            });

            services.AddRazorPages();

            services.AddDbContext<DTS_OnlineContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DTSOnlineContext"));
            });

            services.AddDbContext<ClubContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("ClubContext"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Configurations:Jwt:Issuer"],
                        ValidAudience = Configuration["Configurations:Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Configurations:Jwt:Key"])),
                        ClockSkew = System.TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            string memberId = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                            string notApprovedMember = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                            Endpoint endpoint = context.HttpContext.GetEndpoint();

                            // this is the place where we can check if the user is approved or not.
                            // if the user is not approved (and the request path is not userslinksms), we direct him to login page.
                            // this solve a bug related to the user that is not approved refresh was able him to access the system.
                            if (!string.IsNullOrEmpty(memberId)
                                && !string.IsNullOrEmpty(notApprovedMember)
                                && !context.HttpContext.Request.Path.ToString().ToLower().Contains("userslinksms")
                                && endpoint?.Metadata.GetMetadata<IAllowAnonymous>() == null)
                            {
                                context.Fail("User is not approved and cannot access this resource.");
                                context.HttpContext.Response.Cookies.Delete(CookiesKeys.AccessToken);
                                return Task.CompletedTask;
                            }

                            if (
                                !string.IsNullOrEmpty(memberId)
                                && !context.HttpContext.Request.Path.ToString().ToLower().Contains("logout")
                                && endpoint?.Metadata.GetMetadata<IAllowAnonymous>() == null
                            )
                            {
                                var userBl = context.HttpContext.RequestServices.GetRequiredService<UserBL>();

                                if (!string.IsNullOrEmpty(notApprovedMember))
                                {
                                    userBl.CreateUserToken(memberId, true);
                                }
                                else
                                {
                                    userBl.CreateUserToken(memberId);
                                }
                            }
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[CookiesKeys.AccessToken];
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.HttpContext.Response.Cookies.Delete(CookiesKeys.AccessToken);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache, IMapper mapper)
        {
            //add NLog to .NET Core
            loggerFactory.AddNLog();
            //Enable ASP.NET Core features (NLog.web) - only needed for ASP.NET Core users
            // app.AddNLogWeb();

            //needed for non-NETSTANDARD platforms: configure nlog.config in your project root. NB: you need NLog.Web.AspNetCore package for this. 
            env.ConfigureNLog("nlog.config");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use((context, next) =>
            {
                var httpConnectionFeature = context.Features.Get<IHttpConnectionFeature>();
                var localIpAddress = httpConnectionFeature?.LocalIpAddress;
                string serverIpSddress = localIpAddress.ToString();
                if (serverIpSddress.Length > 3)
                {
                    serverIpSddress = serverIpSddress.Substring(serverIpSddress.Length - 3, 3);
                }
                context.Response.Headers.Add(HeadersKeys.ServerIpAddress, serverIpSddress);
                return next.Invoke();
            });

            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });
            app.UseSession();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    var statusCode = httpContext.Response.StatusCode;
                    var path = httpContext.Request.Path;

                    if (path == "/favicon.ico" ||
                             path.StartsWithSegments("/api/values", StringComparison.OrdinalIgnoreCase))
                        return LogEventLevel.Verbose;

                    if (statusCode != 200 || ex != null)
                        return LogEventLevel.Warning;


                    return LogEventLevel.Verbose;
                };
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });
            ContainerManager.Container.Resolve<ICacheManager>().Init(cache);
            ContainerManager.Container.Resolve<IMapperManager>().Init(mapper);
            ContainerManager.Container.Resolve<ILog>().Init(loggerFactory);

            //var autoFacBuilder = new ContainerBuilder();
            //autoFacBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //autoFacBuilder.RegisterType<Infrastructure.IOC.Container>().As<Infrastructure.IOC.IContainer>();
            //var autoFacContainer = autoFacBuilder.Build();

            //var resolver = new AutofacWebApiDependencyResolver(autoFacContainer);

        }

    }
}
