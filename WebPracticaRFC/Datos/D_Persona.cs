using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebPracticaRFC.Models;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace WebPracticaRFC.Datos
{
    public class D_Persona
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        public string ConvertirMayusculas(string input)
        {
            TextInfo myTI = CultureInfo.CurrentCulture.TextInfo;

            return myTI.ToTitleCase(input);
        }

        public string crearRFC(string nombre, string apellidoPat, string apellidoMat, DateTime fechaNacimiento)
        {

            if (string.IsNullOrEmpty(apellidoMat))
            {
                apellidoMat = "x";
            }

             string nombreOtro = nombre.Split(' ')[0].ToUpper();

            if (nombreOtro == "MARIA" || nombreOtro == "MA." || nombreOtro == "MA" || nombreOtro == "JOSE" || nombreOtro == "j" || nombreOtro == "J.")
            {
                string[] divideNombre = nombre.Split(' ');

                nombre = divideNombre[1];

            }

            apellidoPat = apellidoPat.ToUpper().Replace('Ñ', 'x');

            string formatoFecha = fechaNacimiento.ToString("yyMMdd");

            string rfc = apellidoPat.Substring(0, 2) + apellidoMat.Substring(0, 1) + nombre.Substring(0, 1);

            rfc = rfc.ToUpper();
            if (rfc == "PENE" || rfc == "BUEI" || rfc == "CACA" || rfc == "CAKA" || rfc == "COGE"
                    || rfc == "COJE" || rfc == "COJO" || rfc == "FETO" || rfc == "JOTO" || rfc == "FETO"
                        || rfc == "JOTO" || rfc == "KAGO" || rfc == "KAGO" || rfc == "KOJO" || rfc == "MAMO"
                            || rfc == "MEAS" || rfc == "MION" || rfc == "MULA" || rfc == "PEDO" || rfc == "PUTA"
                                || rfc == "QULO" || rfc == "BUEY" || rfc == "CACO" || rfc == "CAKO" || rfc == "COJA"
                                    || rfc == "COJI" || rfc == "CULO" || rfc == "GUEY" || rfc == "KACA" || rfc == "KAGA"
                                        || rfc == "KAGA" || rfc == "KOGE" || rfc == "KAKA" || rfc == "MAME" || rfc == "MEAR"
                                            || rfc == "MEON" || rfc == "MOCO" || rfc == "PEDA" || rfc == "PENE" || rfc == "PUTO" || rfc == "RATA")
            {
                rfc = rfc.Substring(0, rfc.Length - 1);
                rfc = rfc + "x";
            }

            rfc = rfc + formatoFecha;


            return rfc.ToUpper();
        }


        public E_Persona esRepetido(string rfc)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            E_Persona persona = null;

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("EsRepetido", conexion);

                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@RFC", rfc);

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    persona = new E_Persona
                    {
                        idRFC = Convert.ToInt32(reader["idRFC"]),
                        Nombre = reader["nombre"].ToString(),
                        ApellidoPat = reader["apellidoPat"].ToString(),
                        ApellidoMat = reader["apellidoMat"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]),
                        RFC = reader["RFC"].ToString(),
                        EsRepetido = Convert.ToBoolean(reader["esRepetido"])
                    };
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conexion.Close(); }

            return persona;

        }

        public E_Persona Agregar(E_Persona persona)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);

            if (string.IsNullOrEmpty(persona.ApellidoMat))
            {
                persona.ApellidoMat = "x";
            }

            persona.RFC = crearRFC(persona.Nombre, persona.ApellidoPat, persona.ApellidoMat, persona.FechaNacimiento);

            E_Persona RFCRepetido = esRepetido(persona.RFC);

            if (RFCRepetido != null)
            {
                persona.EsRepetido = true;
            }
            else
            {
                persona.EsRepetido = false;
            }

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("InsertarRFC", conexion);

                comando.CommandType = CommandType.StoredProcedure;

                persona.Nombre = ConvertirMayusculas(persona.Nombre);
                persona.ApellidoMat = ConvertirMayusculas(persona.ApellidoMat);
                persona.ApellidoPat = ConvertirMayusculas(persona.ApellidoPat);


                comando.Parameters.AddWithValue("@nombre", persona.Nombre);
                comando.Parameters.AddWithValue("@apellidoMat", persona.ApellidoMat);
                comando.Parameters.AddWithValue("@apellidoPat", persona.ApellidoPat);
                comando.Parameters.AddWithValue("@fechaNacimiento", persona.FechaNacimiento);
                comando.Parameters.AddWithValue("@RFC", persona.RFC);
                comando.Parameters.AddWithValue("@esRepetido", persona.EsRepetido);

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conexion.Close(); }

            return persona;

        }

        public List<E_Persona> MostrarTodos()
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            List<E_Persona> lista = new List<E_Persona>();

            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("MostrarTodosRFC", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    E_Persona objeto = new E_Persona();

                    objeto.idRFC = Convert.ToInt32(reader["idRFC"]);
                    objeto.Nombre = reader["nombre"].ToString();
                    objeto.ApellidoPat = reader["apellidoPat"].ToString();
                    objeto.ApellidoMat = reader["apellidoMat"].ToString();
                    objeto.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    objeto.RFC = reader["RFC"].ToString();
                    objeto.EsRepetido = Convert.ToBoolean(reader["esRepetido"]);
                    lista.Add(objeto);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conexion.Close(); }

            return lista;
        }

        public E_Persona ObtenerParaEditar(int idRFC)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            E_Persona persona = new E_Persona();

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("MostrarRFC", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@idRFC", idRFC);

                SqlDataReader reader = comando.ExecuteReader();

                reader.Read();

                persona = new E_Persona
                {
                    idRFC = Convert.ToInt32(reader["idRFC"]),
                    Nombre = reader["nombre"].ToString(),
                    ApellidoPat = reader["apellidoPat"].ToString(),
                    ApellidoMat = reader["apellidoMat"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]),
                    RFC = reader["RFC"].ToString(),
                    EsRepetido = Convert.ToBoolean(reader["esRepetido"])
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conexion.Close(); }

            return persona;
        }

        public E_Persona Editar(E_Persona persona)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);

            if (string.IsNullOrEmpty(persona.ApellidoMat))
            {
                persona.ApellidoMat = "x";
            }

            persona.RFC = crearRFC(persona.Nombre, persona.ApellidoPat, persona.ApellidoMat, persona.FechaNacimiento);

            E_Persona RFCRepetido = esRepetido(persona.RFC);

            if (RFCRepetido != null)
            {
                persona.EsRepetido = true;
            }
            else
            {
                persona.EsRepetido = false;
            }

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("EditarRFCNuevo", conexion);

                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@IdRFC", persona.idRFC);
                comando.Parameters.AddWithValue("@nombre", persona.Nombre);
                comando.Parameters.AddWithValue("@apellidoPat", persona.ApellidoPat);
                comando.Parameters.AddWithValue("@apellidoMat", persona.ApellidoMat);
                comando.Parameters.AddWithValue("@fechaNacimiento", persona.FechaNacimiento);
                comando.Parameters.AddWithValue("@RFC", persona.RFC);
                comando.Parameters.AddWithValue("@esRepetido", persona.EsRepetido);

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conexion.Close(); }

            return persona;

        }

        public List<E_Persona> Buscar(string texto)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            List<E_Persona> lista = new List<E_Persona>();

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("BuscarRFC", conexion);

                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@texto", texto);

                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    E_Persona objeto = new E_Persona();

                    objeto.idRFC = Convert.ToInt32(reader["idRFC"]);
                    objeto.Nombre = reader["nombre"].ToString();
                    objeto.ApellidoPat = reader["apellidoPat"].ToString();
                    objeto.ApellidoMat = reader["apellidoMat"].ToString();
                    objeto.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    objeto.RFC = reader["RFC"].ToString();
                    lista.Add(objeto);
                }




            }
            catch (Exception ex) { throw ex; }
            finally { conexion.Close(); }

            return lista;
        }


        public void Eliminar(int idRFC)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);

            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("EliminarRFC", conexion);

                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idRFC", idRFC);

                comando.ExecuteNonQuery();



            }catch (Exception ex) { throw ex; }
            finally { conexion.Close(); }
        }

        public int ContarRegistros()
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            int registros = 0;
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("ContarRegistros",conexion);
                comando.CommandType =CommandType.StoredProcedure;
                //registros = comando.ExecuteNonQuery();

                registros = (int)comando.ExecuteScalar();


            }
            catch (Exception ex) { throw ex; }
            finally { conexion.Close(); }

            return registros;
        }
    }
}