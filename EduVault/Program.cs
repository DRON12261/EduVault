using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EduVault.Data;
using EduVault.Repositories;
using EduVault.Services;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

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
            // Добавление необходимого для работы с данными PostgreSQL
            builder.Services.AddDbContext<PostgresDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")), ServiceLifetime.Scoped);
            builder.Services.AddDbContextFactory<PostgresDBContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")), ServiceLifetime.Scoped);
            // Регистрация репозиториев
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            builder.Services.AddScoped<IRecordRepository, RecordRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IRelationRepository, RelationRepository>();
            builder.Services.AddScoped<IFileTypeFieldRepository, FileTypeFieldRepository>();
            builder.Services.AddScoped<IFieldRepository, FieldRepository>();
            // Регистрация сервисов
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFileTypeService, FileTypeService>();
            builder.Services.AddScoped<IRecordService, RecordService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IRelationService, RelationService>();
            builder.Services.AddScoped<IFileNameTemplateService, FileNameTemplateService>();
            builder.Services.AddScoped<IFileTypeFieldService, FileTypeFieldService>();
            builder.Services.AddScoped<IFieldService, FieldService>();
            // Добавление необходимого для работы с данными MongoDB
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
                return client.GetDatabase(new MongoUrl(config.GetConnectionString("MongoDocker")).DatabaseName);
            });
            builder.Services.AddScoped<IGridFSBucket>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                return new GridFSBucket(database);
            });
            builder.Services.AddScoped<IMongoFileService, MongoFileService>();
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
			app.UseAuthorization();
            app.MapRazorPages();
            return app;
		}
	}
}
