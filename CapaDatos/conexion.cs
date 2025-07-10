using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    class conexion
    {
        #region singleton
        private static readonly conexion UnicaInstancia = new conexion();
        public static conexion Instancia
        {
            get
            {
                return conexion.UnicaInstancia;
            }
        }
        #endregion singleton
       
        public SqlConnection Conectar()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Data Source=DESKTOP-51RAHDP\\SQLEXPRESS; Initial Catalog=DBHotelErika; Integrated Security=True;";
            return cn;
        }



    }
}


