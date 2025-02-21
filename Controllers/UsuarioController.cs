using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: UsuarioController
        public ActionResult Index()
        {

            return View(); 
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create/5
        public ActionResult Create()
        {

            return View(); 
        }

        // POST: UsuarioController/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Usuario user)
        {
            try
            {
                // se guarda el usuario en la base de datos
                user.AddUsuario(user); 

                if (user.Tipo == 1) 
                { // si el usuario es tipo 1, lo va a mandar el formulario de cliente
                    return RedirectToAction("Create", "Cliente", new
                    {

                        // y va a mandar todos estos datos, que necesito para que se visualice (nadamas apra visualizar)
                        idUsuario = user.IdUsuario,
                        nombre = user.Nombre,
                        apellidos = user.Apellidos,
                        telefono = user.Telefono
                    });
                }
                else if (user.Tipo == 2) 
                {
                    //lo mismo ocurre con veterinario, lo redirige al formulario, y con sus datos ya creados
                    return RedirectToAction("Create", "Veterinario", new
                    {
                        idUsuario = user.IdUsuario,
                        nombre = user.Nombre,          
                        apellidos = user.Apellidos,
                        telefono = user.Telefono
                    });
                }
            }
            catch
            {
               
                return View(user); 
            }

            return View(user); 
        }




        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {

            return View(new Usuario().GetUsuarioById(id)); 
        }
        
        // POST: UsuarioController/Edit/5
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

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonaController/Delete/5
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
        

    }
}
