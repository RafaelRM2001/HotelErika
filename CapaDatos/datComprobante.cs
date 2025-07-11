using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaDatos
{
    public class datComprobante
    {
        public static readonly datComprobante Instancia = new datComprobante();

        public bool InsertarComprobante(entComprobante c)
        {
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Comprobantes (ReservaId, MetodoPago, Monto, FechaEmision) VALUES (@ReservaId, @MetodoPago, @Monto, GETDATE())", cn);
                cmd.Parameters.AddWithValue("@ReservaId", c.ReservaId);
                cmd.Parameters.AddWithValue("@MetodoPago", c.MetodoPago);
                cmd.Parameters.AddWithValue("@Monto", c.Monto);
                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<entComprobante> ListarComprobantes()
        {
            List<entComprobante> lista = new List<entComprobante>();

            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT C.Id, C.ReservaId, C.MetodoPago, C.Monto, C.FechaEmision,
                           R.Nombre AS NombreCliente, R.Apellido AS ApellidoCliente
                    FROM Comprobantes C
                    INNER JOIN Reserva R ON C.ReservaId = R.Id
                    WHERE C.ReservaId IS NOT NULL
                    ORDER BY C.FechaEmision DESC", cn);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new entComprobante
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ReservaId = Convert.ToInt32(dr["ReservaId"]),
                        MetodoPago = dr["MetodoPago"].ToString(),
                        Monto = Convert.ToDecimal(dr["Monto"]),
                        FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        ApellidoCliente = dr["ApellidoCliente"].ToString()
                    });
                }

                dr.Close();
            }

            return lista;
        }

        public List<entComprobante> ObtenerComprobantesPorUsuario(int usuarioId)
        {
            List<entComprobante> lista = new List<entComprobante>();

            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT C.Id, C.ReservaId, C.MetodoPago, C.Monto, C.FechaEmision
            FROM Comprobantes C
            INNER JOIN Reserva R ON C.ReservaId = R.Id
            WHERE R.UsuarioId = @UsuarioId", cn);

                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new entComprobante
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ReservaId = Convert.ToInt32(dr["ReservaId"]),
                        MetodoPago = dr["MetodoPago"].ToString(),
                        Monto = Convert.ToDecimal(dr["Monto"]),
                        FechaEmision = Convert.ToDateTime(dr["FechaEmision"])
                    });
                }
            }

            return lista;
        }
    }
}
