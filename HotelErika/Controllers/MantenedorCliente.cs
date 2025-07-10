using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;
using System;


namespace HotelErika.Controllers
{
    public class MantenedorCliente : Controller
    {
        public IActionResult Listar()
        {
            //return View();
            List<entCliente> lista = logCliente.Instancia.ListarCliente();
            ViewBag.lista = lista;
            return View(lista);

        }
        [HttpGet]
        public ActionResult InsertarCliente()
        {

            return View();
        }

        [HttpPost]
        public IActionResult InsertarCliente(entCliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente); // Devuelve la vista con errores de validación
            }

            cliente.FechaRegistro = DateTime.Now;
            bool resultado = logCliente.Instancia.InsertarCliente(cliente);

            if (resultado)
                return RedirectToAction("Listar");
            else
                return View(cliente); // Puedes agregar un mensaje de error si quieres
        }

        // GET: Eliminar
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var cliente = logCliente.Instancia.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente); // Muestra la vista de confirmación
        }

        // POST: Elimina
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            bool eliminado = logCliente.Instancia.EliminarCliente(id);
            if (eliminado)
                return RedirectToAction("Listar");
            else
                return View("Error"); // Vista de error 
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var cliente = logCliente.Instancia.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(entCliente cliente)
        {
            if (ModelState.IsValid)
            {
                bool actualizado = logCliente.Instancia.EditarCliente(cliente);
                if (actualizado)
                {
                    return RedirectToAction("Listar");
                }
                ModelState.AddModelError("", "No se pudo actualizar el cliente.");
            }
            return View(cliente);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var cliente = logCliente.Instancia.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

    }
}
