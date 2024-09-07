var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var html = "";
var version = "";

string GetVersion()
{
    if (string.IsNullOrEmpty(version))
    {
        version = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "DotDispatch")?.GetName().Version.ToString();
    }

    return version;
}

app.MapFallback(async (context) =>
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
        context.Response.Headers["Content-Type"] = "text/html";

        if (System.IO.File.Exists("wwwroot/index.html"))
        {
            if (string.IsNullOrEmpty(html))
            {
                var path = System.IO.Path.Combine(Environment.CurrentDirectory, "wwwroot/index.html");

                html = System.IO.File.ReadAllText(path);
            }

            await context.Response.WriteAsync(html);
        }
        else
        {
            var content = app.Configuration["DefaultContent"];

            if (string.IsNullOrEmpty(content))
            {
                content = $"<h2>Dot Dispatch {GetVersion()}</h2><br/><p>{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff")}</p>";
            }

            await context.Response.WriteAsync(content);
        }
    });

app.Run();
