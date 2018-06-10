using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Negocio
{
    public static class Utiles
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
        public static int ObtenerTiempoSesion()
        {
            int retorno = 10;
            if (System.Configuration.ConfigurationManager.AppSettings["CaducidadSesion"] != null)
            {
                retorno = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CaducidadSesion"].ToString());
            }


            return retorno;
        }
        public static int ConvertirAFecha(string fecha)
        {
            //el formato es YYYYMMDDHHMM
            int retorno = -1;
            string anio = fecha.Substring(0, 4);
            string mes = fecha.Substring(4, 2);
            string dia = fecha.Substring(6, 2);
            string hora = fecha.Substring(8, 2);
            string minuto = fecha.Substring(10, 2);

            DateTime fechaProcesar = new DateTime(int.Parse(anio), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);
            int minutosAdicionar = ObtenerTiempoSesion();
            //agregamos los minutos configurados en el archivo web.config
            fechaProcesar = fechaProcesar.AddHours(minutosAdicionar);
            //fecha hora actual
            DateTime fechaHoraActual = DateTime.Now;
            //diferencia
            TimeSpan dif = fechaHoraActual - fechaProcesar;
            retorno = (int)(dif.Ticks);



            return retorno;

        }

        public static DateTime ConvertirAFechaNuevo(string fecha)
        {
            //el formato es YYYYMMDDHHMM
            int retorno = -1;
            string anio = fecha.Substring(0, 4);
            string mes = fecha.Substring(4, 2);
            string dia = fecha.Substring(6, 2);
            string hora = fecha.Substring(8, 2);
            string minuto = fecha.Substring(10, 2);

            DateTime fechaProcesar = new DateTime(int.Parse(anio), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);
            int minutosAdicionar = ObtenerTiempoSesion();
            //agregamos los minutos configurados en el archivo web.config
            fechaProcesar = fechaProcesar.AddHours(minutosAdicionar);

            return fechaProcesar;

        }
        public static string EntregaFechaHoraActual()
        {
            DateTime ahora = DateTime.Now;
            string mes = "";
            string anio = "";
            string dia = "";
            string hora = "";
            string minuto = "";

            string retorno = "";

            anio = ahora.Year.ToString();

            if (ahora.Month < 10)
                mes = "0" + ahora.Month.ToString();
            else
                mes = ahora.Month.ToString();

            if (ahora.Day < 10)
                dia = "0" + ahora.Day.ToString();
            else
                dia = ahora.Day.ToString();

            if (ahora.Hour < 10)
                hora = "0" + ahora.Hour.ToString();
            else
                hora = ahora.Hour.ToString();

            if (ahora.Minute < 10)
                minuto = "0" + ahora.Minute.ToString();
            else
                minuto = ahora.Minute.ToString();

            retorno = anio + mes + dia + hora + minuto;

            return retorno;
        }
        public static bool ValidarTokenSession(int ecolId, string nombreUsuario, string token)
        {
            //para pruebas
            //token = "MQB8AHAAdgBpAGMAdQBuAGEAcwBhAHkAZABlAHgAbAB1AG4AfAAyADAAMQA4ADAANQAyADgAMQAxADUAMAA="; //11:50
            bool retorno = false;
            DateTime fechaHoraActual = DateTime.Now;

            //desencriptamos el token
            string tokenProcesar = DesEncriptar(token);
            if (tokenProcesar != null && tokenProcesar.Length > 0)
            {
                //aca vamos bien
                //vamos a descomponer el string del token
                string[] arreglo = tokenProcesar.Split('|');
                if (arreglo != null && arreglo.Length == 3)
                {
                    //seguimos bien los 3
                    //0 = ecolId, 1=nombreUsuario, 2=fechahoraactual en formato YYYYMMDDHHMM
                    //int tiempo = ObtenerTiempoSesion();

                    int res = ConvertirAFecha(arreglo[2]);
                    DateTime fechaExpiracion = ConvertirAFechaNuevo(arreglo[2]);

                    //ahora comparamos, si el valor es positivo para nosotros significa que es incorrecto
                    if (fechaHoraActual <= fechaExpiracion)
                    {
                        //esto esta correcto
                        //ahora debemos comparar los demas elementos
                        if (int.Parse(arreglo[0]) == ecolId && arreglo[1] == nombreUsuario)
                        {
                            //ahora si estoy correcto
                            retorno = true;

                        }

                    }

                }
            }

            return retorno;
        }
        public static string CrearTokenSession(int ecolId, string nombreUsuario)
        {
            string retorno = "";
            //debemos crearlo en base a Ecol_id, nombreUsuario, FechaHora
            string cadena = ecolId.ToString() + "|" + nombreUsuario + "|" + EntregaFechaHoraActual();

            retorno = Encriptar(cadena);

            return retorno;
        }
        public static string Encriptar(string cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static string DesEncriptar(this string cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

    }
}
