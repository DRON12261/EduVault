using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EduVault.Data;
using EduVault.Repositories;
using EduVault.Services;
using MongoDB.Driver;

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
            builder.Services.AddDbContext<PostgresDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")), ServiceLifetime.Scoped);
            builder.Services.AddDbContextFactory<PostgresDBContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")), ServiceLifetime.Scoped);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            builder.Services.AddScoped<IRecordRepository, RecordRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFileTypeService, FileTypeService>();
            builder.Services.AddScoped<IRecordService, RecordService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<MongoFileService>();
            builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("MongoDocker");
                return new MongoClient(connectionString);
            });

            builder.Services.AddScoped(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return client.GetDatabase(config["MongoDB:EduVault"]);
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "EduVault.AuthCookie";
                    options.LoginPath = "/Auth/Login"; // Куда перенаправлять неавторизованных пользователей
                    options.AccessDeniedPath = "/Auth/AccessDenied"; // Куда перенаправлять при отказе в доступе
                    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Время жизни куки
                    options.Cookie.HttpOnly = true; // Защита от XSS (недоступно через JS)
                    options.SlidingExpiration = true; // Обновлять срок жизни куки при активности
                });
            builder.Services.AddAuthorization();
            builder.Services.AddRazorPages();
            return builder;
		}
		public static WebApplication TuneApp(WebApplication app)
		{
            /*using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				db.Database.EnsureCreated();
				Console.WriteLine("              PostgreSQL        !");
			}*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();   //            middleware                
			app.UseAuthorization();   //            middleware             
			
			/*&app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
            */
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
					await response.WriteAsync("      ");
				else
					await response.WriteAsync("               "); ;
			});*/
            return app;
		}
	}
}
