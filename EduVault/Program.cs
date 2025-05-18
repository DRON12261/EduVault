using EduVault.Repositories;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EduVault
{
	class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = TuneBuilder(WebApplication.CreateBuilder(args));
			WebApplication app = TuneApp(builder.Build());
			app.Run();
		}

		public static WebApplicationBuilder TuneBuilder(WebApplicationBuilder builder)
		{
			builder.Services.AddRazorPages();
			builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")));
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
							.AddCookie(options => options.LoginPath = "/login");
			builder.Services.AddAuthorization();
			builder.Services.AddControllersWithViews();
			return builder;
		}
		public static WebApplication TuneApp(WebApplication app)
		{
			/*using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				db.Database.EnsureCreated();
				Console.WriteLine("Подключение к PostgreSQL успешно!");
			}*/
			app.UseAuthentication();   // добавление middleware аутентификации 
			app.UseAuthorization();   // добавление middleware авторизации 
			app.MapRazorPages();

			app.UseStaticFiles();
			app.UseRouting();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Users}/{action=Index}/{id?}");
			/*app.Run(async (context) =>
			{
				context.Response.ContentType = "text/html; charset=utf-8";

				var path = context.Request.Path;
				var now = DateTime.Now;
				var response = context.Response;

				if (path == "/date")
					await response.WriteAsync($"Date: {now}");
				else if (path == "/hello")
					await response.WriteAsync("Драсте");
				else
					await response.WriteAsync("ничего не понял"); ;
			});*/
			return app;
		}
	}
}
=======
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")));

var app = builder.Build();

<<<<<<< HEAD
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.EnsureCreated();
	Console.WriteLine("����������� � PostgreSQL �������!");
}
=======
>>>>>>> parent of 43814f3 (18.05.2025 - Frontend init)
app.MapRazorPages();

/*app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    var path = context.Request.Path;
    var now = DateTime.Now;
    var response = context.Response;

    if (path == "/date")
        await response.WriteAsync($"Date: {now}");
    else if (path == "/hello")
        await response.WriteAsync("������");
    else
        await response.WriteAsync("������ �� �����"); ;
});*/

app.Run();
>>>>>>> parent of aafdbad (18.05 Поковырял бэк еще)
