using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Negocio.Global
{
    public class Global
    {
        #region List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> ObtenerContratantesLun()
        public static List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> ObtenerTodosContratantesLun()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from ENCO_ENTIDAD_CONTRATANTE_LUN where ACTIVO = 1 AND ELIMINADO = 0 order by ID", conn);

            conn.Open();
            List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> listaDevolver = new List<RayenSalud.WebLun.Entidad.Global.ContratanteLun>();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RAZON_SOCIAL = rdr.GetOrdinal("RAZON_SOCIAL");
                int FECHA_MODIFICACION = rdr.GetOrdinal("FECHA_MODIFICACION");
                int FECHA_INICIO_CONTRATO = rdr.GetOrdinal("FECHA_INICIO_CONTRATO");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int RESTO_DIRECCION = rdr.GetOrdinal("RESTO_DIRECCION");
                int NUMERO = rdr.GetOrdinal("NUMERO_DIRECCION");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int CANTIDAD_AVISO = rdr.GetOrdinal("CANTIDAD_AVISO");
                try
                {
                    while (rdr.Read())
                    {
                        RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();
                        contratante.Id = rdr.GetInt32(ID);
                        contratante.RazonSocial = rdr.GetString(RAZON_SOCIAL);
                        contratante.FechaModificacion = rdr.IsDBNull(FECHA_MODIFICACION) ? DateTime.MinValue : rdr.GetDateTime(FECHA_MODIFICACION);
                        contratante.FechaInicioContrato = rdr.IsDBNull(FECHA_INICIO_CONTRATO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_INICIO_CONTRATO);
                        contratante.IdRegion = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        contratante.IdComuna = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        contratante.Direccion = rdr.GetString(DIRECCION);
                        contratante.RestoDireccion = rdr.IsDBNull(RESTO_DIRECCION) ? "" : rdr.GetString(RESTO_DIRECCION);
                        contratante.Numero = rdr.IsDBNull(NUMERO) ? "" : rdr.GetString(NUMERO);
                        contratante.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        contratante.Eliminado = rdr.IsDBNull(ELIMINADO) ? 0 : rdr.GetInt32(ELIMINADO);
                        contratante.CantidadAviso = rdr.IsDBNull(CANTIDAD_AVISO) ? 0 : rdr.GetInt32(CANTIDAD_AVISO);
                        listaDevolver.Add(contratante);
                    }

                }
                catch(Exception ex)
                {
                    Negocio.Utiles.NLogs(ex);
                }
                finally
                {
                    rdr.Close();
                }

            }
            catch (Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }
            finally
            {
                conn.Close();
            }

            return listaDevolver;
        }
        #endregion

        public static RayenSalud.WebLun.Entidad.Global.ContratanteLun ObtenerContratantesPorNombre(string nombre)
        {
            List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> lista = ObtenerTodosContratantesLun();
            RayenSalud.WebLun.Entidad.Global.ContratanteLun entidad = new Entidad.Global.ContratanteLun();

            if (lista != null && lista.Count > 0)
                entidad = lista.Find(p => p.RazonSocial == nombre);

            return entidad;
        }

        #region List<ReynSalud.WebLun.Entidad.Global.ContratanteLun> ObtenerContratantesLun()
        public List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> ObtenerContratantesLun()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from ENCO_ENTIDAD_CONTRATANTE_LUN where ACTIVO = 1 AND ELIMINADO = 0 order by ID", conn);

            conn.Open();
            List<RayenSalud.WebLun.Entidad.Global.ContratanteLun> listaDevolver = new List<RayenSalud.WebLun.Entidad.Global.ContratanteLun>();

            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RAZON_SOCIAL = rdr.GetOrdinal("RAZON_SOCIAL");
                int FECHA_MODIFICACION = rdr.GetOrdinal("FECHA_MODIFICACION");
                int FECHA_INICIO_CONTRATO = rdr.GetOrdinal("FECHA_INICIO_CONTRATO");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int RESTO_DIRECCION = rdr.GetOrdinal("RESTO_DIRECCION");
                int NUMERO = rdr.GetOrdinal("NUMERO_DIRECCION");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int CANTIDAD_AVISO = rdr.GetOrdinal("CANTIDAD_AVISO");
                try
                {
                    while (rdr.Read())
                    {
                        RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();
                        contratante.Id = rdr.GetInt32(ID);
                        contratante.RazonSocial = rdr.GetString(RAZON_SOCIAL);
                        contratante.FechaModificacion = rdr.IsDBNull(FECHA_MODIFICACION) ? DateTime.MinValue : rdr.GetDateTime(FECHA_MODIFICACION);
                        contratante.FechaInicioContrato = rdr.IsDBNull(FECHA_INICIO_CONTRATO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_INICIO_CONTRATO);
                        contratante.IdRegion = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        contratante.IdComuna = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        contratante.Direccion = rdr.GetString(DIRECCION);
                        contratante.RestoDireccion = rdr.IsDBNull(RESTO_DIRECCION) ? "" : rdr.GetString(RESTO_DIRECCION);
                        contratante.Numero = rdr.IsDBNull(NUMERO) ? "" : rdr.GetString(NUMERO);
                        contratante.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        contratante.Eliminado = rdr.IsDBNull(ELIMINADO) ? 0 : rdr.GetInt32(ELIMINADO);
                        contratante.CantidadAviso = rdr.IsDBNull(CANTIDAD_AVISO) ? 0 : rdr.GetInt32(CANTIDAD_AVISO);
                        listaDevolver.Add(contratante);
                    }
                }
                finally
                {
                    rdr.Close();
                }

            }
            finally
            {
                conn.Close();
            }
            return listaDevolver;
        }
        #endregion

        #region static RayenSalud.WebLun.Entidad.Global.ContratanteLun ObtenerContratanteLunPorId(int id)
        public static RayenSalud.WebLun.Entidad.Global.ContratanteLun ObtenerContratanteLunPorId(int id)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from ENCO_ENTIDAD_CONTRATANTE_LUN where ACTIVO = 1 AND ELIMINADO = 0 and ID = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RAZON_SOCIAL = rdr.GetOrdinal("RAZON_SOCIAL");
                int FECHA_MODIFICACION = rdr.GetOrdinal("FECHA_MODIFICACION");
                int FECHA_INICIO_CONTRATO = rdr.GetOrdinal("FECHA_INICIO_CONTRATO");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int RESTO_DIRECCION = rdr.GetOrdinal("RESTO_DIRECCION");
                int NUMERO = rdr.GetOrdinal("NUMERO_DIRECCION");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int CANTIDAD_AVISO = rdr.GetOrdinal("CANTIDAD_AVISO");
                try
                {
                    while (rdr.Read())
                    {
                        //Saydex.WebLun.Entidad.Global.ContratanteLun contratante = new Saydex.WebLun.Entidad.Global.ContratanteLun();
                        contratante.Id = rdr.GetInt32(ID);
                        contratante.RazonSocial = rdr.GetString(RAZON_SOCIAL);
                        contratante.FechaModificacion = rdr.IsDBNull(FECHA_MODIFICACION) ? DateTime.MinValue : rdr.GetDateTime(FECHA_MODIFICACION);
                        contratante.FechaInicioContrato = rdr.IsDBNull(FECHA_INICIO_CONTRATO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_INICIO_CONTRATO);
                        contratante.IdRegion = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        contratante.IdComuna = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        contratante.Direccion = rdr.GetString(DIRECCION);
                        contratante.RestoDireccion = rdr.IsDBNull(RESTO_DIRECCION) ? "" : rdr.GetString(RESTO_DIRECCION);
                        contratante.Numero = rdr.IsDBNull(NUMERO) ? "" : rdr.GetString(NUMERO);
                        contratante.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        contratante.Eliminado = rdr.IsDBNull(ELIMINADO) ? 0 : rdr.GetInt32(ELIMINADO);
                        contratante.CantidadAviso = rdr.IsDBNull(CANTIDAD_AVISO) ? 0 : rdr.GetInt32(CANTIDAD_AVISO);
                        //listaDevolver.Add(contratante);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            return contratante;
        }
        #endregion

        #region static bool InsertarRegistroHistorialLun(RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun historial)
        public static bool InsertarRegistroHistorialLun(RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun historial)
        {
            bool retorno;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;

            //cmd = new SqlCommand("select * from ENCO_ENTIDAD_CONTRATANTE_LUN where ACTIVO = 1 AND ELIMINADO = 0 and ID = @ID", conn);
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO HELU_HISTORIAL_ENCARGADOS_LUN ");
            sb.Append("(FECHA_REGISTRO,USER_ENCARGADO,RUN_ENCARGADO,TIPO_MOVIMIENTO,USUARIO_CREADOR) VALUES ");
            sb.Append("(@FECHA_REGISTRO,@USER_ENCARGADO,@RUN_ENCARGADO,@TIPO_MOVIMIENTO,@USUARIO_CREADOR)");
            cmd = new SqlCommand(sb.ToString(), conn);

            cmd.Parameters.AddWithValue("@FECHA_REGISTRO", historial.FechaRegistro);
            cmd.Parameters.AddWithValue("@USER_ENCARGADO", historial.UserEncargado);
            cmd.Parameters.AddWithValue("@RUN_ENCARGADO", historial.RunEncargado);
            cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", historial.TipoMovimiento);
            cmd.Parameters.AddWithValue("@USUARIO_CREADOR", historial.UsuarioCreador);
            conn.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                retorno = true;

            }
            catch (Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
                retorno = false;
            }
            finally
            {
                conn.Close();
            }


            return retorno;

        }
        #endregion

        #region static bool InsertarRegistroHistorialRebalse(RayenSalud.WebLun.Entidad.Global.HistorialSobrecupo historial)
        public static bool InsertarRegistroHistorialRebalse(RayenSalud.WebLun.Entidad.Global.HistorialSobrecupo historial)
        {
            bool retorno;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;

            //cmd = new SqlCommand("select * from ENCO_ENTIDAD_CONTRATANTE_LUN where ACTIVO = 1 AND ELIMINADO = 0 and ID = @ID", conn);
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO HSLUN_HISTORIAL_SOBRECUPO_LUN ");
            sb.Append("(ECOL_ID,SOBRECUPO_ANTERIOR,FECHA,USUARIO_CREADOR,SOBRECUPO_FINAL,TIPO_MOVIMIENTO) VALUES ");
            sb.Append("(@ECOL_ID,@SOBRECUPO_INICIAL,@FECHA,@USUARIO_CREADOR,@SOBRECUPO_FINAL,@TIPO_MOVIMIENTO)");
            cmd = new SqlCommand(sb.ToString(), conn);

            cmd.Parameters.AddWithValue("@ECOL_ID", historial.EncoId);
            cmd.Parameters.AddWithValue("@SOBRECUPO_INICIAL", historial.Sobrecupo);
            cmd.Parameters.AddWithValue("@SOBRECUPO_FINAL", historial.SobrecupoFinal);
            cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", historial.TipoMovimiento);
            cmd.Parameters.AddWithValue("@FECHA", historial.Fecha);
            cmd.Parameters.AddWithValue("@USUARIO_CREADOR", historial.UsuarioCreador);

            conn.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                retorno = true;

            }
            catch (Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
                retorno = false;
            }
            finally
            {
                conn.Close();
            }


            return retorno;

        }
        #endregion

        #region static RayenSalud.WebLun.Entidad.Global.RebalseLun ObtenerRebalseLunPorId(int id)
        public static RayenSalud.WebLun.Entidad.Global.RebalseLun ObtenerRebalseLunPorId(int id)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from RLUN_REBALSE_LUN where ID = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse = new RayenSalud.WebLun.Entidad.Global.RebalseLun();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int ENCO_ID = rdr.GetOrdinal("ECOL_ID");
                int TOTAL_LICENCIAS = rdr.GetOrdinal("TOTAL_LICENCIAS");
                int SOBRECUPO = rdr.GetOrdinal("SOBRECUPO");
                int TOTAL_LICENCIAS_INICIAL = rdr.GetOrdinal("TOTAL_LICENCIAS_INICIAL");
                int SOBRECUPO_INICIAL = rdr.GetOrdinal("SOBRECUPO_INICIAL");
                try
                {
                    while (rdr.Read())
                    {
                        rebalse.Id = rdr.GetInt32(ID);
                        rebalse.EncoId = rdr.IsDBNull(ENCO_ID) ? 0 : rdr.GetInt32(ENCO_ID);
                        rebalse.TotalLicencias = rdr.IsDBNull(TOTAL_LICENCIAS) ? 0 : rdr.GetInt32(TOTAL_LICENCIAS);
                        rebalse.Sobrecupo = rdr.IsDBNull(SOBRECUPO) ? 0 : rdr.GetInt32(SOBRECUPO);
                        rebalse.TotalLicenciasInicial = rdr.IsDBNull(TOTAL_LICENCIAS_INICIAL) ? 0 : rdr.GetInt32(TOTAL_LICENCIAS_INICIAL);
                        rebalse.TotalSobrecupoInicial = rdr.IsDBNull(SOBRECUPO_INICIAL) ? 0 : rdr.GetInt32(SOBRECUPO_INICIAL);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            catch(Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }
            finally
            {
                conn.Close();
            }
            return rebalse;
        }
        #endregion

        #region static RayenSalud.WebLun.Entidad.Global.RebalseLun ObtenerRebalseLunPorEncoId(int encoId)
        public static RayenSalud.WebLun.Entidad.Global.RebalseLun ObtenerRebalseLunPorEncoId(int encoId)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from RLUN_REBALSE_LUN where ECOL_ID = @ECOL_ID", conn);
            cmd.Parameters.AddWithValue("@ECOL_ID", encoId);
            conn.Open();
            RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse = new RayenSalud.WebLun.Entidad.Global.RebalseLun();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int ECOL_ID = rdr.GetOrdinal("ECOL_ID");
                int TOTAL_LICENCIAS = rdr.GetOrdinal("TOTAL_LICENCIAS");
                int SOBRECUPO = rdr.GetOrdinal("SOBRECUPO");
                int TOTAL_LICENCIAS_INICIAL = rdr.GetOrdinal("TOTAL_LICENCIAS_INICIAL");
                int SOBRECUPO_INICIAL = rdr.GetOrdinal("SOBRECUPO_INICIAL");
                try
                {
                    while (rdr.Read())
                    {
                        rebalse.Id = rdr.GetInt32(ID);
                        rebalse.EncoId = rdr.IsDBNull(ECOL_ID) ? 0 : rdr.GetInt32(ECOL_ID);
                        rebalse.TotalLicencias = rdr.IsDBNull(TOTAL_LICENCIAS) ? 0 : rdr.GetInt32(TOTAL_LICENCIAS);
                        rebalse.Sobrecupo = rdr.IsDBNull(SOBRECUPO) ? 0 : rdr.GetInt32(SOBRECUPO);
                        rebalse.TotalLicenciasInicial = rdr.IsDBNull(TOTAL_LICENCIAS_INICIAL) ? 0 : rdr.GetInt32(TOTAL_LICENCIAS_INICIAL);
                        rebalse.TotalSobrecupoInicial = rdr.IsDBNull(SOBRECUPO_INICIAL) ? 0 : rdr.GetInt32(SOBRECUPO_INICIAL);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            catch(Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }
            finally
            {
                conn.Close();
            }
            return rebalse;

        }
        #endregion

        #region static bool ModificarRebalseLun(RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse)
        public static bool ModificarRebalseLun(RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse)
        {
            bool retorno;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE RLUN_REBAlSE_LUN SET ");
            sb.Append("ECOL_ID = @ENCO_ID, SOBRECUPO = @SOBRECUPO, TOTAL_LICENCIAS = @TOTAL_LICENCIAS ");
            sb.Append("WHERE ECOL_ID = @ENCO_ID");
            cmd = new SqlCommand(sb.ToString(), conn);

            cmd.Parameters.AddWithValue("@ENCO_ID", rebalse.EncoId);
            cmd.Parameters.AddWithValue("@SOBRECUPO", rebalse.Sobrecupo);
            cmd.Parameters.AddWithValue("@TOTAL_LICENCIAS", rebalse.TotalLicencias);

            conn.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                retorno = true;

            }
            catch (Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
                retorno = false;
            }
            finally
            {
                conn.Close();
            }


            return retorno;

        }
        #endregion

        #region static bool ModificarRebalseLunTotal(RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse)
        public static bool ModificarRebalseLunTotal(RayenSalud.WebLun.Entidad.Global.RebalseLun rebalse)
        {
            bool retorno;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE RLUN_REBAlSE_LUN SET ");
            sb.Append("ECOL_ID = @ENCO_ID, SOBRECUPO_INICIAL = @SOBRECUPO_INICIAL, TOTAL_LICENCIAS_INICIAL = @TOTAL_LICENCIAS_INICIAL ");
            sb.Append("WHERE ECOL_ID = @ENCO_ID");
            cmd = new SqlCommand(sb.ToString(), conn);

            cmd.Parameters.AddWithValue("@ENCO_ID", rebalse.EncoId);
            cmd.Parameters.AddWithValue("@SOBRECUPO_INICIAL", rebalse.TotalSobrecupoInicial);
            cmd.Parameters.AddWithValue("@TOTAL_LICENCIAS_INICIAL", rebalse.TotalLicenciasInicial);

            conn.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                retorno = true;

            }
            catch (Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
                retorno = false;
            }
            finally
            {
                conn.Close();
            }


            return retorno;

        }
        #endregion


    }
}
