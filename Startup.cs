using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using iParking.Data;
using iParking.Models;
using iParking.Services;

namespace iParking
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            if (Environment.IsProduction())
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(string.Format(Configuration.GetConnectionString("SqlServerConnection"),
                    Configuration.GetValue<string>("SQL_USER"),
                    Configuration.GetValue<string>("SQL_PASS"))));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequiredLength = 4;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            if (Environment.IsProduction())
            {
                services.AddAuthentication()
                    .AddFacebook(opts =>
                    {
                        opts.AppId = Configuration.GetValue<string>("FACEBOOK_APP_ID");
                        opts.AppSecret = Configuration.GetValue<string>("FACEBOOK_APP_SECRET");
                    });

            }
            else
            {
                services.AddAuthentication()
                    .AddFacebook(opts =>
                    {
                        opts.AppId = "214957042409829";
                        opts.AppSecret = "fac43c43f17dc5624c37e19517745209";
                    });
            }

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
