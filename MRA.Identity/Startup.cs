using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MRA.Identity.Data;
using MRA.Identity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MRA.Identity
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }

        public Startup(IConfiguration appConfiguration)
        {
            AppConfiguration = appConfiguration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = AppConfiguration.GetValue<string>("ConnectionString");
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.clients)
                .AddDeveloperSigningCredential();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "MRA.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Styles")),
                RequestPath = "/styles"
            });
            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
