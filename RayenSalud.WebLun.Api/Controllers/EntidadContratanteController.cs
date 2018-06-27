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
            else if (data.Token == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Token Vacío");
            }
            else if (data.NombreUsuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Nombre Usuario Vacío");
            }
            else
            {
                try
                {
                    string ecolId = data.EcolId;
                    //agregados para validar el token
                    string token = data.Token;
                    string nombreUsuario = data.NombreUsuario;

                    //aca vamos a validar el token antes que realice las otras operaciones
                    bool esValido = Negocio.Utiles.ValidarTokenSession(int.Parse(ecolId), nombreUsuario, token);
                    if (esValido)
                    {
                        
                        List<Entidad.Global.ContratanteLun> lista = Negocio.Global.Global.ObtenerTodosContratantesLun();
                        //correcto
                        httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, lista);
                    }
                    else
                    {
                        //el token es incorrecto
                        httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Token_invalido);
                    }


                }
                catch (Exception ex)
                {
                    Negocio.Utiles.NLogs(ex);
                    httpResponse = ManejoMensajes.RetornaMensajeExcepcion(httpResponse, ex);
                }
            }
            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Get([FromUri]string ecolId)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            if (ecolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Ecol Id");
            }
            else
            {
                Entidad.Global.ContratanteLun contratante = Negocio.Global.Global.ObtenerContratanteLunPorId(int.Parse(ecolId));
                if (contratante != null && contratante.Id > 0)
                {
                    httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, contratante);
                }
                else
                {
                    httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, 456, "No_existe_contratante");
                }

                

            }

            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(dynamic DynamicClass)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);


            if (data.Empleador == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Empleador");
            }
            else if (data.IdTipoContrato == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Id Tipo Contrato");
            }
            else if (data.IdRegion == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Id Region");
            }
            else if (data.IdComuna == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Id Comuna");
            }
            else if (data.Direccion == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Direccion");
            }
            else if (data.EcolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Ecol Id");
            }
            else if (data.Sobrecupo == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Sobrecupo");
            }
            else if (data.TotalLIcencias == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Total Licencias");
            }
            else if (data.Token == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Token Vacío");
            }
            else if (data.NombreUsuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Nombre Usuario Vacío");
            }

            else
            {
                string empleador = data.Empleador;
                string idTipoContrato = data.IdTipoContrato;
                string ecolId = data.EcolId;

                string direccion = data.Direccion;
                string idRegion = data.IdRegion;
                string idComuna = data.IdComuna;

                string sobrecupo = data.Sobrecupo;
                string totalLicencias = data.TotalLicencias;
                //no requerido
                string numero = "";
                string restoDireccion = "";
                //agregados para validar el token
                string token = data.Token;
                string nombreUsuario = data.NombreUsuario;
                try
                {
                    //aca vamos a validar el token antes que realice las otras operaciones
                    bool esValido = Negocio.Utiles.ValidarTokenSession(int.Parse(ecolId), nombreUsuario, token);
                    if (esValido)
                    {
                        //el tokern es correcto
                        if (data.Numero != null)
                        {
                            numero = data.Numero;
                        }

                        if (data.RestoDireccion != null)
                        {
                            restoDireccion = data.RestoDireccion;
                        }
                        bool retorno = Negocio.Global.Global.ModificarENCO(empleador, int.Parse(idTipoContrato), int.Parse(idRegion), int.Parse(idComuna), int.Parse(ecolId), direccion, numero, restoDireccion, int.Parse(sobrecupo), int.Parse(totalLicencias));
                        if (retorno)
                        {
                            //si esta correcto, ahora hay que retornar el nuevo elemento
                            Entidad.Global.ContratanteLun contratante = Negocio.Global.Global.ObtenerContratanteLunPorId(int.Parse(ecolId));
                            if (contratante != null && contratante.Id > 0)
                            {
                                httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, contratante);
                            }
                            else
                            {
                                httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, 456, "No_existe_contratante");
                            }

                        }
                        else
                        {
                            httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, 8000, "Error al modificar Entidad Contratante");
                        }
                    }
                    else
                    {
                        //el token es incorrecto
                        httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Token_invalido);
                    }

                }
                catch(Exception ex)
                {
                    httpResponse = ManejoMensajes.RetornaMensajeExcepcion(httpResponse, ex);
                }




            }
            //retornamos
            return httpResponse;
        }
    }
}