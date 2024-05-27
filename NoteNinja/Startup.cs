using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using NoteNinja.Models;
using NoteNinja.Repositories;
using NoteNinja.Services;

namespace NoteNinja
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // MongoDB Configuration
            var mongoClient = new MongoClient(Configuration["MongoDbSettings:ConnectionString"]);
            var database = mongoClient.GetDatabase(Configuration["MongoDbSettings:DatabaseName"]);
            services.AddSingleton(database);

            // Register Repositories with collection names
            services.AddScoped<IRepository<Note>>(provider => new Repository<Note>(database, "Notes"));
            services.AddScoped<IRepository<User>>(provider => new Repository<User>(database, "Users"));
            services.AddScoped<IRepository<Models.Tag>>(provider => new Repository<Models.Tag>(database, "Tags"));

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();


            services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

            // Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>
                  {
                      options.LoginPath = "/Account/Login";
                      options.AccessDeniedPath = "/Account/AccessDenied";
                  });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "notes",
                    pattern: "{controller=Notes}/{action=Index}/{id?}");
            });
        }
    }
}
