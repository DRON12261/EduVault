using EduVault.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDocker")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.EnsureCreated();
	Console.WriteLine("����������� � PostgreSQL �������!");
}
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
