using Microsoft.Extensions.Options;

namespace DotDispatch
{
    public static class Extensions
    {
        public static void UseDispatch(this WebApplication app, DotDispatchOptions options = null)
        {
            if (options == null)
            {
                options = new DotDispatchOptions();
            }

            options.Validate();

            var fw = new FileWatcher(System.IO.Path.Combine(options.Root, options.WebPath), options.ContentFile);
            var pvp = new ProjectVersionProvider("DotDispatch");

            app.MapFallback(async (context) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                context.Response.Headers["Content-Type"] = "text/html";

                var content = fw.GetContent();

                if (string.IsNullOrEmpty(content))
                {
                    content = app.Configuration["DefaultContent"];
                }

                if (string.IsNullOrEmpty(content))
                {
                    content = $"<h2>Dot Dispatch {pvp.GetVersion()}</h2><br/><p>{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff")}</p>";
                }

                await context.Response.WriteAsync(content);
            });

            fw.Init();
        }
    }
}
