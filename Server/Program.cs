using DryIoc;
using DryIoc.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Server.DataBase.Classes;
using Server.DataBase.Interfaces;
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
            try
            {
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
            catch (Exception ex)
            {
                Logger.Log.Error("Global exception", ex);
            }
        }
    }

    class Startup
    {
        /// <summary>
        /// Создание конейнера и активатора хабов.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();

            ConfigureContainer(container);

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new DryIocHubActivator(container));
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }

        /// <summary>
        /// Создание контейнера.
        /// </summary>
        /// <param name="container">Контейнер.</param>
        void ConfigureContainer(IContainer container)
        {
            container.RegisterHubs(typeof(LoginHub),
                                   typeof(UsersHub),
                                   typeof(StaffHub),
                                   typeof(GroupsHub),
                                   typeof(StudentsHub),
                                   typeof(ChatHub),
                                   typeof(DetailInfoHub));

            container.RegisterSingleton<IRepository, Repository>();
            container.RegisterSingleton<IUsersRepository, UsersRepository>();
            container.RegisterSingleton<IGroupsRepository, GroupsRepository>();
            container.RegisterSingleton<IStaffRepository, StaffRepository>();
            container.RegisterSingleton<IStudentsRepository, StudentsRepository>();
            container.RegisterSingleton<IDetailInfoRepository, DetailInfoRepository>();
        }
    }
}
