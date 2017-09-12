using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Common.Factories;
using Intranet.Web.Domain.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using Intranet.Web.Domain.Models.Entities;

namespace Intranet.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment CurrentEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Dependency Injection
            services.AddTransient<IDateTimeFactory, DateTimeFactory>();
            #endregion

            #region Database
            services.AddDbContext<IntranetApiContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
            #endregion

            #region Options
            // Required to use the Options<T> pattern
            services.AddOptions();
            #endregion

            #region Mvc
            // Add framework services.
            services.AddMvc();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IntranetApiContext dbContext)
        {
            #region Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            #endregion

            #region Hack to seed DB
            var news = new List<News>
            {
                new News
                {
                    Title = "First news",
                    Text = "<strong>News body</strong>",
                    HeaderImage = new Image { FileName = "header.jpg" },
                    UserId = "oskar",
                    User = new User { Username = "oskar", DisplayName = "Oskar K" },
                },
            };

            dbContext.AddRange(news);
            dbContext.SaveChanges();
            #endregion

            #region Mvc
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=News}/{action=Index}/{id?}");
            });
            #endregion
        }
    }
}
