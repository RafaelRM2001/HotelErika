using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Http;
using System;

namespace HotelErika.Controllers
{
    public class MantenedorReserva : Controller
    {
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            int? rolId = HttpContext.Session.GetInt32("RolId"); // ← Obtiene el RolId desde la sesión

            if (usuarioId == null || rolId == null)
            {
                // Usuario no autenticado o sesión expirada
                return RedirectToAction("Login", "Login");
            }

            List<entReserva> lista;

            if (rolId == 2) // ← Administrador
            {
                lista = logReserva.Instancia.ListarReserva(); // ← Muestra todas las reservas
            }
            else
            {
                lista = logReserva.Instancia.ObtenerReservasPorUsuario(usuarioId.Value); // ← Solo las del usuario
            }

            return View(lista);
        }



        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
            return View(new entReserva());
        }

        [HttpPost]
        public IActionResult Crear(entReserva r)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
                return View(r);
            }

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                ViewBag.Mensaje = "Error: Sesión expirada o usuario no autenticado.";
                ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
                return View(r);
            }

            // ✅ Validar que la fecha de entrada no sea en el pasado
            DateTime hoy = DateTime.Today;
            if (r.FechaEntrada.Date < hoy)
            {
                ModelState.AddModelError("FechaEntrada", "La fecha de entrada no puede ser anterior a hoy.");
                ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
                return View(r);
            }

            r.UsuarioId = usuarioId.Value;
            r.FechaRegistro = DateTime.Now;
            r.Activo = true;

            bool exito = logReserva.Instancia.InsertarReserva(r);
            if (exito)
            {
                logHabitacion.Instancia.CambiarEstadoHabitacion(r.NumeroHabitacion, "Ocupado");
                return RedirectToAction("Index");
            }

            ViewBag.Mensaje = "Ocurrió un error al guardar la reserva.";
            ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
            return View(r);
        }



        [HttpGet]
        public JsonResult BuscarClientePorDNI(string dni)
        {
            var cliente = logReserva.Instancia.BuscarClientePorDNI(dni);
            if (cliente != null)
            {
                return Json(new { nombre = cliente.Nombre, apellido = cliente.Apellido });
            }
            return Json(null);
        }

        [HttpGet]
        public JsonResult ObtenerHabitaciones(string tipo)
        {
            var lista = logReserva.Instancia.ListarHabitacionesPorTipo(tipo);
            return Json(lista);
        }

        [HttpGet]
        public JsonResult ObtenerPrecio(string numero)
        {
            decimal precio = logReserva.Instancia.ObtenerPrecioHabitacion(numero);
            return Json(precio);
        }


        [HttpPost]
        public IActionResult Cancelar(int id)
        {
            bool resultado = logReserva.Instancia.CancelarReserva(id);

            if (resultado)
            {
                TempData["Mensaje"] = "Reserva cancelada correctamente.";
            }
            else
            {
                TempData["Error"] = "";
            }

            return RedirectToAction("Index"); // O el nombre de tu acción de listado
        }

        [HttpGet]
        public IActionResult Reprogramar(int id)
        {
            var reserva = logReserva.Instancia.ObtenerReservaPorId(id); // Debe existir este método
            if (reserva == null)
            {
                return RedirectToAction("Index");
            }

            return View(reserva);
        }

        [HttpPost]
        public IActionResult Reprogramar(int id, string numeroHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            try
            {
                // ✅ Validación: fecha de entrada no puede ser anterior a hoy
                if (fechaEntrada.Date < DateTime.Today)
                {
                    TempData["error"] = "La fecha de entrada no puede ser anterior a hoy.";
                    return RedirectToAction("Reprogramar", new { id = id });
                }

                bool actualizado = logReserva.Instancia.ReprogramarReserva(id, numeroHabitacion, fechaEntrada, fechaSalida);
                if (actualizado)
                {
                    TempData["mensaje"] = "Reserva reprogramada correctamente.";
                }
                else
                {
                    TempData["error"] = "No se pudo reprogramar la reserva. Verifique si está activa.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }



    }
}
