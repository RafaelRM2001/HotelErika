using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HotelErika.Controllers
{
    public class MantenedorReserva : Controller
    {
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            int? rolId = HttpContext.Session.GetInt32("RolId");

            if (usuarioId == null || rolId == null)
                return RedirectToAction("Login", "Login");

            List<entReserva> lista = (rolId == 2)
                ? logReserva.Instancia.ListarReserva()
                : logReserva.Instancia.ObtenerReservasPorUsuario(usuarioId.Value);

            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId.HasValue)
            {
                var usuario = logUsuario.Instancia.ObtenerUsuarioPorId(usuarioId.Value);
                if (usuario != null)
                {
                    return View(new entReserva
                    {
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        DNI_RUC = usuario.DNI_RUC,
                        FechaEntrada = DateTime.Today,
                        FechaSalida = DateTime.Today.AddDays(1)
                    });
                }
            }

            return View(new entReserva
            {
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarReserva(entReserva r)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposHabitacion = logReserva.Instancia.ListarTiposHabitacion();
                ViewBag.Habitaciones = logReserva.Instancia.ListarHabitacionesPorTipo(r.TipoHabitacion);
                return View("Crear", r);
            }

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (!usuarioId.HasValue)
                return RedirectToAction("Login", "Login");

            r.UsuarioId = usuarioId.Value;
            r.FechaRegistro = DateTime.Now;
            r.Activo = true;

            int dias = (r.FechaSalida - r.FechaEntrada).Days;
            if (dias < 1) dias = 1;

            // 🔁 Obtener el precio real desde la BD
            decimal precioPorNoche = logHabitacion.Instancia.ObtenerPrecioPorNumero(r.NumeroHabitacion);
            r.Precio = precioPorNoche * dias;

            Console.WriteLine($"Precio por noche: {precioPorNoche}, Días: {dias}, Total: {r.Precio}");

            // 👉 Depuración temporal
            Console.WriteLine("Precio recibido: " + r.Precio);

            // 👉 Guardamos la reserva temporalmente para procesarla en el método de pago
            TempData["ReservaTemporal"] = JsonConvert.SerializeObject(r);

            return RedirectToAction("Pagar");
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

            TempData["Mensaje"] = resultado
                ? "Reserva cancelada correctamente."
                : "Error al cancelar la reserva.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Reprogramar(int id)
        {
            var reserva = logReserva.Instancia.ObtenerReservaPorId(id);
            if (reserva == null)
                return RedirectToAction("Index");

            return View(reserva);
        }

        [HttpPost]
        public IActionResult Reprogramar(int id, string numeroHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            if (fechaEntrada.Date < DateTime.Today)
            {
                TempData["error"] = "La fecha de entrada no puede ser anterior a hoy.";
                return RedirectToAction("Reprogramar", new { id = id });
            }

            bool actualizado = logReserva.Instancia.ReprogramarReserva(id, numeroHabitacion, fechaEntrada, fechaSalida);
            TempData[actualizado ? "mensaje" : "error"] = actualizado
                ? "Reserva reprogramada correctamente."
                : "No se pudo reprogramar la reserva.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Pagar()
        {
            if (TempData["ReservaTemporal"] == null)
                return RedirectToAction("Index");

            TempData.Keep("ReservaTemporal");

            var reservaJson = TempData["ReservaTemporal"].ToString();
            var reserva = JsonConvert.DeserializeObject<entReserva>(reservaJson);

            ViewBag.Monto = reserva.Precio;  // ← PASA EL MONTO A LA VISTA

            return View();
        }

        [HttpPost]
        public IActionResult Pagar(string metodoPago, decimal monto)
        {
            if (TempData["ReservaTemporal"] == null)
                return RedirectToAction("Index");

            var reservaJson = TempData["ReservaTemporal"].ToString();
            var reserva = JsonConvert.DeserializeObject<entReserva>(reservaJson);

            // Inserta la reserva primero
            bool exito = logReserva.Instancia.InsertarReserva(reserva);

            if (exito)
            {
                logHabitacion.Instancia.CambiarEstadoHabitacion(reserva.NumeroHabitacion, "Ocupado");

                entComprobante c = new entComprobante
                {
                    ReservaId = reserva.Id, // Este ID debe ser generado por el SP
                    MetodoPago = metodoPago,
                    Monto = monto
                };

                bool creado = logComprobante.Instancia.InsertarComprobante(c);
                if (creado)
                {
                    TempData["mensaje"] = "Pago registrado correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["error"] = "Reserva realizada, pero el comprobante no se generó.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "No se pudo registrar la reserva.";
            return RedirectToAction("Index");
        }
    }
}
