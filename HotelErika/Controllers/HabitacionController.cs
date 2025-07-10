using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaLogica;
using System;

namespace HotelErika.Controllers
{
    public class HabitacionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var lista = logHabitacion.Instancia.ListarHabitaciones(); // Ahora muestra todas
            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new entHabitacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(entHabitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Complete todos los campos requeridos.";
                return View(habitacion);
            }

            // Validar si ya existe el número
            if (logHabitacion.Instancia.ExisteNumeroHabitacion(habitacion.Numero))
            {
                ViewBag.Mensaje = "El número de habitación ya está registrado.";
                return View(habitacion);
            }

            try
            {
                bool registrado = logHabitacion.Instancia.InsertarHabitacion(habitacion);
                if (registrado)
                {
                    TempData["MensajeExito"] = "Habitación registrada correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo registrar la habitación.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al registrar habitación: " + ex.Message;
            }

            return View(habitacion);
        }



        [HttpGet]
        public IActionResult Editar(int id)
        {
            var habitacion = logHabitacion.Instancia.ObtenerHabitacionPorId(id);
            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(entHabitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Complete todos los campos requeridos.";
                return View(habitacion);
            }

            try
            {
                bool actualizado = logHabitacion.Instancia.EditarHabitacion(habitacion);
                if (actualizado)
                {
                    TempData["MensajeExito"] = "Habitación actualizada correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo actualizar la habitación.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al editar habitación: " + ex.Message;
            }

            return View(habitacion);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var habitacion = logHabitacion.Instancia.ObtenerHabitacionPorId(id);
            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminar(int id)
        {
            try
            {
                bool eliminado = logHabitacion.Instancia.EliminarHabitacion(id);
                if (eliminado)
                {
                    TempData["MensajeExito"] = "Habitación eliminada correctamente.";
                }
                else
                {
                    TempData["MensajeError"] = "No se pudo eliminar la habitación.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al eliminar habitación: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var habitacion = logHabitacion.Instancia.ObtenerHabitacionPorId(id);
            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

    }
}
