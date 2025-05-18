var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

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
        await response.WriteAsync("Драсте");
    else
        await response.WriteAsync("ничего не понял"); ;
});*/

app.Run();
