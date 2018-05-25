using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;

namespace RayenSalud.WebLun.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EntidadContratanteController : ApiController
    {
        [AcceptVerbs("OPTIONS")]
        public void Options()
        { }

        [System.Web.Http.AcceptVerbs("POST")]
        public HttpResponseMessage Post(dynamic DynamicClass)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            if (data.EcolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Entidad Contratante");
            }
            else
            {
                try
                { 
                    string ecolId = data.EcolId;
                    List<Entidad.Global.ContratanteLun> lista = Negocio.Global.Global.ObtenerTodosContratantesLun();
                    //correcto
                    httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, lista);


                }
                catch (Exception ex)
                {
                    Negocio.Utiles.NLogs(ex);
                    httpResponse = ManejoMensajes.RetornaMensajeExcepcion(httpResponse, ex);
                }
            }
            return httpResponse;
        }
    }
}