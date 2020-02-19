using log4net;
using log4net.Config;

namespace ISTraining_Part
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
