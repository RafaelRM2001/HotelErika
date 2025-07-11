using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class datReserva
    {
        #region singleton
        private static readonly datReserva _instancia = new datReserva();
        public static datReserva Instancia => _instancia;
        #endregion

        #region métodos auxiliares


        /// Busca el nombre y apellido del cliente por su DNI/RUC.
        /// </summary>
        public entReserva BuscarClientePorDNI(string dni)
        {
            SqlCommand cmd = null;
            entReserva resultado = null;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spObtenerNombreApellidoPorDNI", cn) // ← nombre corregido
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@DNI_RUC", dni);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        resultado = new entReserva
                        {
                            DNI_RUC = dni,
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString()
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return resultado;
        }


        /// <summary>
        /// Recupera todos los tipos de habitación disponibles.
        /// </summary>
        public List<string> ListarTiposHabitacion()
        {
            SqlCommand cmd = null;
            var lista = new List<string>();
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarTiposHabitacion", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(dr["Tipo"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las habitaciones (número y precio) de un tipo dado que estén disponibles.
        /// </summary>
        public List<entReserva> ListarHabitacionesPorTipo(string tipo)
        {
            SqlCommand cmd = null;
            var lista = new List<entReserva>();
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarHabitacionesPorTipo", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new entReserva
                        {
                            NumeroHabitacion = dr["Numero"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene el precio de una habitación dado su número.
        /// </summary>
        public decimal ObtenerPrecioHabitacion(string numero)
        {
            SqlCommand cmd = null;
            decimal precio = 0m;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spObtenerPrecioHabitacion", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Numero", numero);
                cn.Open();
                precio = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0m);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return precio;
        }

        #endregion

        #region CRUD Reserva

        public List<entReserva> ListarReserva()
        {
            SqlCommand cmd = null;
            var lista = new List<entReserva>();
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarReserva", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new entReserva
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            DNI_RUC = dr["DNI_RUC"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            TipoHabitacion = dr["TipoHabitacion"].ToString(),
                            NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            FechaEntrada = Convert.ToDateTime(dr["FechaEntrada"]),
                            FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return lista;
        }

        public bool InsertarReserva(entReserva r)
        {
            SqlCommand cmd = null;
            bool insertado = false;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertarReserva", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@DNI_RUC", r.DNI_RUC);
                cmd.Parameters.AddWithValue("@Nombre", r.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", r.Apellido);
                cmd.Parameters.AddWithValue("@TipoHabitacion", r.TipoHabitacion);
                cmd.Parameters.AddWithValue("@NumeroHabitacion", r.NumeroHabitacion);
                cmd.Parameters.AddWithValue("@Precio", r.Precio);
                cmd.Parameters.AddWithValue("@FechaEntrada", r.FechaEntrada);
                cmd.Parameters.AddWithValue("@FechaSalida", r.FechaSalida);
                cmd.Parameters.AddWithValue("@FechaRegistro", r.FechaRegistro);
                cmd.Parameters.AddWithValue("@Activo", r.Activo);
                cmd.Parameters.AddWithValue("@UsuarioId", r.UsuarioId); // ← ¡ESTO es clave!

                cn.Open();
                insertado = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return insertado;
        }


        public entReserva ObtenerReservaPorId(int id)
        {
            SqlCommand cmd = null;
            entReserva r = null;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spObtenerReservaPorId", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r = new entReserva
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            DNI_RUC = dr["DNI_RUC"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            TipoHabitacion = dr["TipoHabitacion"].ToString(),
                            NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            FechaEntrada = Convert.ToDateTime(dr["FechaEntrada"]),
                            FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return r;
        }





        public List<entReserva> ObtenerReservasPorUsuario(int usuarioId)
        {
            SqlCommand cmd = null;
            var lista = new List<entReserva>();
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spObtenerReservasPorUsuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new entReserva
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            DNI_RUC = dr["DNI_RUC"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            TipoHabitacion = dr["TipoHabitacion"].ToString(),
                            NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            FechaEntrada = Convert.ToDateTime(dr["FechaEntrada"]),
                            FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                            Activo = Convert.ToBoolean(dr["Activo"]),
                            UsuarioId = Convert.ToInt32(dr["UsuarioId"])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return lista;
        }


        public bool CancelarReserva(int id)
        {
            SqlCommand cmd = null;
            bool cancelado = false;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCancelarReserva", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cancelado = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return cancelado;
        }

        public bool ReprogramarReserva(int id, string numeroHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            SqlCommand cmd = null;
            bool actualizado = false;
            try
            {
                var cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spReprogramarReserva", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@NumeroHabitacion", numeroHabitacion);
                cmd.Parameters.AddWithValue("@FechaEntrada", fechaEntrada);
                cmd.Parameters.AddWithValue("@FechaSalida", fechaSalida);

                cn.Open();
                actualizado = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return actualizado;
        }

        #endregion
    }
}
