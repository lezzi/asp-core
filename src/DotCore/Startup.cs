using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.DataAccess;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.EntityFrameworkCore;
using DotCore.Portable.DataAccess.EntityFrameworkCore.Repositories;
using DotCore.Portable.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DotCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using DotCore.Portable.BusinessLogic.MapperProfiles;

namespace DotCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Application services
            services.AddAutoMapper(item => item.AddProfile(new DefaultProfile()));

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IQuestionRepository, EfQuestionRepository>();
            services.AddScoped<IAnswerRepository, EfAnswerRepository>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddTransient<AskService>();
            services.AddTransient<QuestionsService>();
            services.AddTransient<AnswerService>();

            services.AddScoped<DotCoreContext>(x => x.GetService<ApplicationContext>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
