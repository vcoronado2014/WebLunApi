using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Negocio
{
    public class Utiles
    {
        public const string HTML_DOCTYPE = "text/html";
        public const string JSON_DOCTYPE = "application/json";
        public const string XML_DOCTYPE = "application/xml";

        public static string NLogs(string mensaje)
        {
            var logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            logger.Log(NLog.LogLevel.Info, mensaje);
            return mensaje;
        }
        public static string NLogs(Exception ex)
        {
            var logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            logger.LogException(NLog.LogLevel.Error, "Error", ex);
            return ex.Message;
        }

    }
}
