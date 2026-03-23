using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventGo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using payment.PaymentData;
using Stripe;
using Microsoft.Extensions.Logging;
using EventGo.Models;

namespace EventGo
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
            services.AddControllersWithViews();
            //payment
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddDbContext<MovieContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("ConnectDb")));

            services.AddDbContext<PadelContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("padel")));
            services.AddDbContext<SecureContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("padel")));
     //       services.AddIdentity<MyUser, MyRole>().AddEntityFrameworkStores<SecureContext>();

    //        services.ConfigureApplicationCookie(
         
            
            //Added Services_____________________________
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<IMovieInCinemaRepository, MovieInCinemaRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProducerRepository, ProducerRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IMovieActorRepository, MovieActorRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IMovieOrderRepository, MovieOrderRepository>();
            services.AddScoped<IUpdateProfileRepository, UpdateProfileRepository>();

            //___________________________________________
            //Authentication and Authorization

          
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
.AddEntityFrameworkStores<MovieContext>()
.AddDefaultTokenProviders();
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            logger.LogError("An error occurred while seeding the database.");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();
         
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Main}/{action=Index}/{id?}");
            });


            //Database Data Initializer
            DBInitializer.SeedDB(app).Wait();
            DBInitializer.CreateUsersAndRolesAsync(app).Wait();
            DbPadel.SeedDB(app).Wait();

        }
    }
}
