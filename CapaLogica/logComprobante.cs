using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaLogica
{
    public class logComprobante
    {
        public static readonly logComprobante Instancia = new logComprobante();

        public bool InsertarComprobante(entComprobante c)
        {
            return datComprobante.Instancia.InsertarComprobante(c);
        }

        public List<entComprobante> ListarComprobantes()
        {
            return datComprobante.Instancia.ListarComprobantes();
        }

        public List<entComprobante> ListarComprobantesPorUsuario(int usuarioId)
        {
            return datComprobante.Instancia.ObtenerComprobantesPorUsuario(usuarioId);
        }
    }
}
