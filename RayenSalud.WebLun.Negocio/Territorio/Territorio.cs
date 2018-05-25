using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Negocio.Territorio
{
    public class Territorio
    {

        public static List<RayenSalud.WebLun.Entidad.Territorio.Region> ObtenerRegiones()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from REG_REGION where ELIMINADO = 0 order by ID", conn);

            conn.Open();
            List<RayenSalud.WebLun.Entidad.Territorio.Region> listaDevolver = new List<RayenSalud.WebLun.Entidad.Territorio.Region>();
            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRE = rdr.GetOrdinal("NOMBRE");
                int CODIGO_DEIS = rdr.GetOrdinal("CODIGO_DEIS");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                try
                {
                    while (rdr.Read())
                    {
                        RayenSalud.WebLun.Entidad.Territorio.Region region = new RayenSalud.WebLun.Entidad.Territorio.Region();
                        region.Id = rdr.GetInt32(ID);
                        region.Nombre = rdr.GetString(NOMBRE);
                        region.CodigoDeis = rdr.IsDBNull(CODIGO_DEIS) ? "" : rdr.GetString(CODIGO_DEIS);

                        listaDevolver.Add(region);
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
            finally
            {
                conn.Close();
            }
            return listaDevolver;


        }

        public static RayenSalud.WebLun.Entidad.Territorio.Region ObtenerRegionPorId(int id)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from REG_REGION where ELIMINADO = 0 AND ID=@ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            List<RayenSalud.WebLun.Entidad.Territorio.Region> listaDevolver = new List<RayenSalud.WebLun.Entidad.Territorio.Region>();
            RayenSalud.WebLun.Entidad.Territorio.Region region = new RayenSalud.WebLun.Entidad.Territorio.Region();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRE = rdr.GetOrdinal("NOMBRE");
                int CODIGO_DEIS = rdr.GetOrdinal("CODIGO_DEIS");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                try
                {
                    while (rdr.Read())
                    {
                        region.Id = rdr.GetInt32(ID);
                        region.Nombre = rdr.GetString(NOMBRE);
                        region.CodigoDeis = rdr.IsDBNull(CODIGO_DEIS) ? "" : rdr.GetString(CODIGO_DEIS);
                    }
                }
                catch (Exception ex)
                {
                    Negocio.Utiles.NLogs(ex);
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
            return region;


        }

        public static List<RayenSalud.WebLun.Entidad.Territorio.Comuna> ObtenerComunasPorRegion(int idRegion)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from COM_COMUNA where ELIMINADO = 0 AND COD_REGION=@REG_ID", conn);
            cmd.Parameters.AddWithValue("@REG_ID", idRegion);
            conn.Open();
            List<RayenSalud.WebLun.Entidad.Territorio.Comuna> listaDevolver = new List<RayenSalud.WebLun.Entidad.Territorio.Comuna>();

            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRE = rdr.GetOrdinal("NOMBRE");
                int CODIGO_DEIS = rdr.GetOrdinal("CODIGO_DEIS");
                int COD_REGION = rdr.GetOrdinal("COD_REGION");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                try
                {
                    while (rdr.Read())
                    {
                        RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = new RayenSalud.WebLun.Entidad.Territorio.Comuna();
                        comuna.Id = rdr.GetInt32(ID);
                        comuna.Nombre = rdr.GetString(NOMBRE);
                        comuna.CodigoDeis = rdr.IsDBNull(CODIGO_DEIS) ? "" : rdr.GetString(CODIGO_DEIS);
                        comuna.IdRegion = int.Parse(rdr.GetString(COD_REGION));
                        listaDevolver.Add(comuna);
                    }

                }
                catch (Exception ex)
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

        public static RayenSalud.WebLun.Entidad.Territorio.Comuna ObtenerComunaPorId(int id)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from COM_COMUNA where ELIMINADO = 0 AND ID=@ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = new RayenSalud.WebLun.Entidad.Territorio.Comuna();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRE = rdr.GetOrdinal("NOMBRE");
                int CODIGO_DEIS = rdr.GetOrdinal("CODIGO_DEIS");
                int COD_REGION = rdr.GetOrdinal("COD_REGION");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                try
                {
                    while (rdr.Read())
                    {
                        comuna.Id = rdr.GetInt32(ID);
                        comuna.Nombre = rdr.GetString(NOMBRE);
                        comuna.CodigoDeis = rdr.IsDBNull(CODIGO_DEIS) ? "" : rdr.GetString(CODIGO_DEIS);
                        comuna.IdRegion = int.Parse(rdr.GetString(COD_REGION));
                    }
                }
                catch (Exception ex)
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
            return comuna;


        }


        public static List<Entidad.Nodo> ObtenerNodosDeLaComuna(int idComuna)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");

            string conexionStr = ConfigurationManager.ConnectionStrings["BDWebLunConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("OBTENER_NODOS_POR_COMUNA", conn);
            cmd.Parameters.AddWithValue("@COM_ID", idComuna);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            List<Entidad.Nodo> listaDevolver = new List<Entidad.Nodo>();

            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RAZON_SOCIAL = rdr.GetOrdinal("RAZON_SOCIAL");
                try
                {
                    Entidad.Nodo nodo1 = new Entidad.Nodo();
                    nodo1.IdNodo = 900000;
                    nodo1.RazonSocial = "No Aplica";
                    listaDevolver.Add(nodo1);

                    while (rdr.Read())
                    {
                        Entidad.Nodo nodo = new Entidad.Nodo();
                        nodo.IdNodo = rdr.IsDBNull(ID) ? 0 : rdr.GetInt32(ID);
                        nodo.RazonSocial = rdr.GetString(RAZON_SOCIAL);
                        listaDevolver.Add(nodo);
                    }
                }
                catch (Exception ex)
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
    }
}
