using AlbumsToBuy.Helpers;
using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using AlbumsToBuy.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy
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
			services.Configure<RazorViewEngineOptions>(o =>
			{
				o.ViewLocationFormats.Add("/Views/Management/{1}/{0}" + RazorViewEngine.ViewExtension);
			});

			services.AddDbContext<ApplicationDbContext>(
				options => options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.Cookie.Name = "AuthorizationCookie";
			});

			//repositories
			services.AddScoped<AddressRepository>();
			services.AddScoped<AlbumOrderRepository>();
			services.AddScoped<AlbumRepository>();
			services.AddScoped<OrderRepository>();
			services.AddScoped<PaymentRepository>();
			services.AddScoped<ShoppingListItemRepository>();
			services.AddScoped<TrackRepository>();
			services.AddScoped<UserRepository>();
			
			//services
			services.AddScoped<AddressService>();
			services.AddScoped<AlbumOrderService>();
			services.AddScoped<AlbumService>();
			services.AddScoped<OrderService>();
			services.AddScoped<PaymentService>();
			services.AddScoped<ShoppingListItemService>();
			services.AddScoped<TrackService>();
			services.AddScoped<UserService>();
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

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCookiePolicy();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
					name: "management",
					pattern: "Management/{controller}/{action=Index}/{id?}");
			});
		}
	}
}
