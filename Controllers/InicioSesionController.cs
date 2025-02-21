using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class InicioSesionController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }



        // GET: InicioSesionController/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: InicioSesionController/Details/5
       
        public ActionResult Details(int id)
        {
           

            return View(); 
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: InicioSesionController/Details
        public ActionResult Details()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult Login(string telefono, string contraseña)
        {
            telefono = telefono.Trim(); //se eliminan los espacio de telefono

           //se llama el metodo get usuario y se mandan los parametros de telefono y contraseña
            Usuario usuario = GetUsuario(telefono, contraseña); 

            if (usuario != null)
            {
                var inicioSesion = new InicioSesion
                { // se crea una variable de inicio se sesion y se guardan estos valores
                    IdUsuario = usuario.IdUsuario,
                    Telefono = usuario.Telefono
                };

                InicioSesion.IniciarSesion(inicioSesion, HttpContext.Session);
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
                HttpContext.Session.SetInt32("Tipo", usuario.Tipo);
                HttpContext.Session.SetString("Telefono", usuario.Telefono);

               //aqui va a regiridr segun el tipo de usuaior, si es 1, va a cliente, si es 2 va a veterinario
                switch (usuario.Tipo)
                {
                    case 1:
                       
                        return RedirectToAction("Info", "Cliente", new { id = usuario.IdUsuario }); //y les pasa el idusuairo,
                                                                                                    //porque de esta manera pueden continuar su registro
                                                                                                    //ya que aqui, todavia no se trabaja ninguna sesion htppp
                    case 2: 
                        return RedirectToAction("Info", "Veterinario", new { id = usuario.IdUsuario }); // Pasar ID
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Teléfono o contraseña incorrectos.";
            return View("Login");
        }

        private Usuario GetUsuario(string telefono, string contraseña)
        {
            Usuario usuario = null;

            string connectionString = "Server=localhost;Username=postgres;Database=puppycit;Password=root;";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM public.usuario WHERE telefono = @telefono AND contrasena = @contrasena";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@telefono", telefono);
                    command.Parameters.AddWithValue("@contrasena", contraseña);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                                Tipo = reader.GetInt32(reader.GetOrdinal("tipo"))
                            };
                        }
                    }
                }
            }

            return usuario; 
        }





        // GET: InicioSesionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InicioSesionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InicioSesionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InicioSesionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Logout()
        {
            // limpiar la sesión
            HttpContext.Session.Clear(); // elimina todas las variables de sesión

            // redirigir a la página de inicio o login
            return RedirectToAction("Index", "Home"); 
        }
    }
}
