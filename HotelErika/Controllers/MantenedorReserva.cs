using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace HotelErika.Controllers
{
    public class MantenedorReserva : Controller
    {
        private bool UsuarioLogueado()
        {
            return HttpContext.Session.GetString("UsuarioNombre") != null;
        }

        // Listar todas las reservas
        public IActionResult Listar()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Login");

            var reservas = logReserva.Instancia.ListarReservas();
            return View(reservas);
        }

        // Mostrar formulario de creación
        public IActionResult Crear()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Login");

            ViewBag.Clientes = new SelectList(logCliente.Instancia.ListarCliente(), "Id", "Nombre");
            ViewBag.Habitaciones = new SelectList(
                logHabitacion.Instancia.ListarHabitaciones().Where(h => h.Estado == "Disponible"),
                "Id", "Numero"
            );
            return View();
        }

        // Guardar reserva
        [HttpPost]
        public IActionResult Crear(entReserva reserva)
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Login");

            int? rolId = HttpContext.Session.GetInt32("RolId");
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (rolId == null || usuarioId == null)
                return RedirectToAction("Login", "Login");

            var habitaciones = logHabitacion.Instancia.ListarHabitacionesDisponibles();
            ViewBag.Habitaciones = new SelectList(habitaciones, "Id", "DescripcionExtendida");

            if (rolId == 2) // Admin
            {
                var clientes = logCliente.Instancia.ListarCliente();
                ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto");
            }
            else // Cliente normal
            {
                var cliente = logCliente.Instancia.ObtenerClientePorId(usuarioId.Value); // Asumiendo que implementas esto
                ViewBag.ClienteId = cliente.Id;
            }

            ViewBag.RolId = rolId;
            return View();
        }

        // Aplazar reserva (formulario)
        public IActionResult Aplazar(int id)
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Login");

            var reserva = logReserva.Instancia.ObtenerReservaPorId(id);
            return View(reserva);
        }

        // Aplazar reserva (guardar)
        [HttpPost]
        public IActionResult Aplazar(int id, DateTime nuevaFechaIngreso, DateTime nuevaFechaSalida)
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Login");

            logReserva.Instancia.AplazarReserva(id, nuevaFechaIngreso, nuevaFechaSalida);
            return RedirectToAction("Listar");
        }
    }
}
