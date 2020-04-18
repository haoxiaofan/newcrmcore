using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewCrmCore.Application.Services;
using NewCrmCore.Application.Services.Interface;
using NewCrmCore.Domain.Services.BoundedContext;
using NewCrmCore.Domain.Services.Interface;
using NewCrmCore.NotifyCenter;
using NewCrmCore.Web.Filter;
using NewLibCore.Data.SQL.Mapper;

namespace NewCrmCore.Web
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
            EntityMapper.InitDefaultSetting();
            EntityMapper.ConnectionStringName = "NewCrmDatabase";

            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ISecurityServices, SecurityServices>();
            services.AddTransient<IAppServices, AppServices>();
            services.AddTransient<IDeskServices, DeskServices>();
            services.AddTransient<IWallpaperServices, WallpaperServices>();
            services.AddTransient<ILoggerServices, LoggerServices>();
            services.AddTransient<CommonNotify>();

            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<IAppContext, Domain.Services.BoundedContext.AppContext>();
            services.AddTransient<IDeskContext, DeskContext>();
            services.AddTransient<ILoggerContext, LoggerContext>();
            services.AddTransient<IMemberContext, MemberContext>();
            services.AddTransient<ISecurityContext, SecurityContext>();
            services.AddTransient<IWallpaperContext, WallpaperContext>();

            services.AddMvc(config =>
            {
                config.Filters.Add(new HandleException());
                config.Filters.Add(new CheckPermissions());
                config.Filters.Add(new VisitorRecordFilter());
            }).AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                op.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //app.UseMvcWithDefaultRoute();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotifyHub>("/hubs");
                endpoints.MapControllerRoute("Default", "{controller}/{action}/{id?}", defaults: new { controller = "desk", action = "index" });
            });
        }
    }
}
