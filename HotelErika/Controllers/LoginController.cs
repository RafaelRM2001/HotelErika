using CapaEntidad;
using CapaLogica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Security.Cryptography;

namespace HotelErika.Controllers
{
    public class LoginController : Controller
    {
        //esta es la clave--
        private const string CLAVE_AUTORIZACION_ADMIN = "ERIKA2025";

        // 🔒 Método para hashear contraseña con SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Mensaje = "Debe ingresar su correo y contraseña.";
                return View();
            }

            // 🔒 Encriptar antes de comparar
            string passwordHash = HashPassword(password.Trim());

            var usuario = logUsuario.Instancia.LoginUsuario(email.Trim(), passwordHash);

            if (usuario != null && usuario.Activo)
            {
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetInt32("RolId", usuario.RolId);
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Mensaje = "Credenciales incorrectas o cuenta inactiva.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var rolIdUsuario = logUsuario.Instancia.ObtenerRolIdPorNombre("Usuario");

            return View(new entUsuario
            {
                RolId = rolIdUsuario
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(entUsuario usuario, [FromForm] string? claveAdmin)
        {
            try
            {
                int rolIdAdmin = logUsuario.Instancia.ObtenerRolIdPorNombre("Administrador");

                if (usuario.RolId == rolIdAdmin)
                {
                    if (string.IsNullOrWhiteSpace(claveAdmin) || claveAdmin != CLAVE_AUTORIZACION_ADMIN)
                    {
                        ViewBag.Mensaje = "Clave de autorización incorrecta para administradores.";
                        return View(usuario);
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Complete todos los campos obligatorios.";
                    return View(usuario);
                }

                // 🔒 Hashear la contraseña antes de guardar
                usuario.Password = HashPassword(usuario.Password);
                usuario.FechaRegistro = DateTime.Now;
                usuario.Activo = true;

                bool registrado = logUsuario.Instancia.RegistrarUsuario(usuario);

                if (registrado)
                {
                    TempData["MensajeExito"] = "Cuenta registrada correctamente.";
                    return RedirectToAction("Register");
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo registrar el usuario.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al registrar: " + ex.Message;
            }

            return View(usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
