using log4net;
using log4net.Config;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Server
{
    static class Logger
    {
        public static ILog Log { get; } = LogManager.GetLogger("LOGGER");

        static Logger()
        {
            XmlConfigurator.Configure();
        }
    }
}
