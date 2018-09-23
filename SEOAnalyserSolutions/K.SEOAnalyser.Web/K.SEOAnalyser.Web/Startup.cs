using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using K.SEOAnalyser.Web.Models;
using K.SEOAnalyser.Web.Models.Entities;
using K.SEOAnalyser.Web.Services;
using K.SEOAnalyser.Web.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace K.SEOAnalyser.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //inject services
            services.AddScoped<IWordService, WordService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddTransient<IDbInit, DbInit>();

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            if (HostingEnvironment.IsDevelopment())
            {
                //running on memory since we dont have server for db
                services.AddDbContext<SeoContext>(options => options.UseInMemoryDatabase("SeoContext"));

                //services.AddDbContext<SeoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SeoContext")));
            }
            else
            {
                //set up other enviroment db connection
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeoContext seoContext, IDbInit dbInit)
        {
            if (env.IsDevelopment())
            {
                //for developement use only.
                dbInit.Seeds();

                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Analyser}/{action=Index}/{id?}");
            });
        }
    }
}
