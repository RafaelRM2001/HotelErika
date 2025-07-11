using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;

namespace HotelErika.Controllers
{
    public class ComprobanteController : Controller
    {
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            int? rolId = HttpContext.Session.GetInt32("RolId");

            if (usuarioId == null || rolId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<entComprobante> comprobantes;

            if (rolId == 2) // Admin
            {
                comprobantes = logComprobante.Instancia.ListarComprobantes(); // todos
            }
            else
            {
                comprobantes = logComprobante.Instancia.ListarComprobantesPorUsuario(usuarioId.Value); // solo del usuario
            }

            return View(comprobantes);
        }
    }
}
