### setup in Programs.cs

```c#

using System;
using System.Collections.Generic;
using System.Threading;
using Delphi.Extensions.Configuration.Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace ConsulRegister
{
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static CancellationToken ConsulReloadCancellationToken;

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddConsul(option =>
                    {
                        option.Token = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
                        option.Address = new List<Uri>
                        {
                            new Uri("http://demo.demo.com:8500"), new Uri("http://demo1.demo.com:8500"),
                            new Uri("http://demo2.demo.com:8500")
                        };
                        option.Datacenter = "dc1";
                        option.Prefix = "";
                        option.WaitTime = new TimeSpan(0, 0, 1, 0);
                    }, ConsulReloadCancellationToken, autoReload: true);
                })
                .UseStartup<Startup>();
    }
}
```


### inject config/strong type in startup.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using ConsulRegister.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ConsulRegister
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //configure consul configuration injection
            services.Configure<ConsulDemo>(Configuration.GetSection("ConsulDemo"));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });


            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
```

### Get the config value in controller

```c#
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using ConsulRegister.Config;
using Microsoft.AspNetCore.Mvc;
using ConsulRegister.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ConsulRegister.Controllers
{
    
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration Settings = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly IOptionsSnapshot<ConsulDemo> ConsulDemo = null;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="consulDemo"></param>
        public HomeController(IConfiguration config, IOptionsSnapshot<ConsulDemo> consulDemo
        )
        {
            Settings = config;
            ConsulDemo = consulDemo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }




       
        

        [HttpGet]
        public IActionResult Test()
        {
            var y = ConsulDemo.Value;
            
            
            
            
            return new ContentResult() {Content = y.MySql};
        }
    }
}
```
