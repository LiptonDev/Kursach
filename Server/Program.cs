using DryIoc;
using DryIoc.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Server.DataBase;
using Server.Hubs;
using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";
            if (args.Length == 0)
                args = new[] { url };

            using (WebApp.Start(args[0]))
            {
                Console.WriteLine("Server running on {0}", args[0]);

                Logger.Log.Info("Server started");

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
            var container = new Container();

            ConfigureContainer(container);

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new DryIocHubActivator(container));
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }

        void ConfigureContainer(IContainer container)
        {
            container.RegisterHubs(typeof(LoginHub), typeof(UsersHub), typeof(StaffHub), typeof(GroupsHub), typeof(StudentsHub));

            container.RegisterSingleton<IDataBase, DataBase.DataBase>();
        }
    }
}
