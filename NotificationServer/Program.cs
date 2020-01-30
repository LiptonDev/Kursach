using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace NotificationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";
            if (args.Length == 0)
                args = new[] { url };
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", args[0]);

                while (true)
                {
                    Console.ReadLine();
                }
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
