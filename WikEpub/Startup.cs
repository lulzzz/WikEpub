using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WikEpub.Services;
using WikEpubLib;
using WikEpubLib.CreateDocs;
using WikEpubLib.Interfaces;
using WikEpubLib.IO;
using WikEpubLib.Records;

namespace WikEpub
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
            services
                .AddSingleton<HttpClient>()
                .AddScoped<IGetTocXml, GetTocXml>()
                .AddScoped<IGetContentXml, GetContentXml>()
                .AddScoped<IGetContainerXml, GetContainerXml>()
                .AddScoped<IHtmlInput, HtmlInput>()
                .AddScoped<IParseHtml, HtmlParser>()
                .AddScoped<IGetWikiPageRecords, GetWikiPageRecords>()
                .AddScoped<IGetXmlDocs, GetXmlDocs>()
                .AddScoped<IEpubOutput, EpubOutput>()
                .AddScoped<IHtmlsToEpub, GetEpub>();
            
            services.AddControllersWithViews();
            services.AddSingleton<FileManagerService>();
            services.AddHostedService(provider => provider.GetService<FileManagerService>());



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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseDefaultFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
