using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoronaBL;
using CoronaBL.Interfaces;
using Microsoft.AspNetCore.Http;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Models;
using LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorFood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Helpers.Configuration = configuration.AsEnumerable().ToList();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILiteDbContext, LiteDbContext>(x => new LiteDbContext(new LiteDatabase("Filename=Database.db")));

            //services.AddSingleton<ILiteDbContext, LiteDbContext>();
            services.AddIdentity<ApplicationUser, AspNetCore.Identity.LiteDB.IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
           .AddUserStore<LiteDbUserStore<ApplicationUser>>()
           .AddRoleStore<LiteDbRoleStore<AspNetCore.Identity.LiteDB.IdentityRole>>()
           .AddDefaultTokenProviders();

        //    services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();

            //services.AddSingleton<ILocalStorage, LocalStorage>();
            
            services.AddScoped<IUser, User>();
            services.AddScoped<IUserApi, UserApi>();
            services.AddScoped<IBrowserStorage, BrowserStorage>();
            services.AddScoped<ICustomer, Customer>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
