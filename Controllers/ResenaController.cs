using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class ResenaController : Controller
    {
        // GET: ResenaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ResenaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ResenaController/Create
        public ActionResult Create(int idServicio, int idCliente, int idVeterinario)
        {
            // crea una variable Resena con los valores que se han recibido desde el formulario(el boton)
            var resena = new Resena
            {
                IdServicio = idServicio,
                IdCliente = idCliente,
                IdVeterinario = idVeterinario
            };

            return View(resena);
        }

        // POST: ResenaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Resena resenita)
        {
            try
            {
                // con el objeto creado de la clase reseña se manda a llamar al metodo
                resenita.AddResena(resenita);
                return RedirectToAction("Info", "Cliente");
            }
            catch
            {
                return View();
            }
        }

        // GET: ResenaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ResenaController/Edit/5
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

        // GET: ResenaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResenaController/Delete/5
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
