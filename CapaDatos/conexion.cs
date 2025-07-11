using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    class Conexion
    {
        #region singleton
        private static readonly Conexion UnicaInstancia = new Conexion();
        public static Conexion Instancia
        {
            get
            {
                return Conexion.UnicaInstancia;
            }
        }
        #endregion singleton
       
        public SqlConnection Conectar()
        {
            SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = "Data Source=DESKTOP-51RAHDP\\SQLEXPRESS; Initial Catalog=DBHotelErika; Integrated Security=True;";
            cn.ConnectionString = "Data Source=DESKTOP-1T9J9CT; Initial Catalog=DBHotelErika; Integrated Security=True;";
            return cn;
        }
    }
}


