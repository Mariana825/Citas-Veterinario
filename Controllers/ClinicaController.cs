using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class ClinicaController : Controller
    {
        // GET: ClinicaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClinicaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClinicaController/Create
          public ActionResult Create(Clinica clinic)
            {
               return View();
               
        }
          

        // POST: ClinicaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Clinica clinic )
        {
            
            //llama al metodo add clinica, y guiarda la variable del id_clinica
            int idClinica = clinic.AddClinica(clinic);

            string[] dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" }; //se crea un areglo de tipo string para los dias
            foreach (string dia in dias) //recorre el arreglo, y va guardando la hora de apertura y cierree
            {
                
                TimeSpan horaApertura = TimeSpan.Parse(Request.Form[$"{dia.ToLower()}_hora_apertura"]);
                TimeSpan horaCierre = TimeSpan.Parse(Request.Form[$"{dia.ToLower()}_hora_cierre"]);
                // manda el idclinica a la tabla de horario
                //clinic llama al metodo addclinica
                clinic.AddHorarioClinica(idClinica, dia, horaApertura, horaCierre);
            }

            //si registra correctamente manda al info veterinario
            return RedirectToAction("Info", "Veterinario");
        }

        // GET: ClinicaController/Edit/5
        public ActionResult Edit(int id)
        {
            //este metodo es para buscar la clinica a editar con el id
            Clinica clinica = new Clinica().GetClinicaById(id);
            
            return View(clinica);
        }

        // POST: ClinicaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Clinica clinic)
        {
            try
            {
                //mandamos a llamar el metodo para poder editar la clinica
                clinic.EditClinica(clinic);
                return RedirectToAction("Info", "Veterinario");
            }
            catch
            {
                return View(clinic);
            }
        }

        // GET: ClinicaController/Delete/5
        public ActionResult Delete(int id)
        {
            //parapoder eliminar la clinica, primero buscamos la clinica con el id
            Clinica clinica = new Clinica().GetClinicaById(id);
            return View(clinica);
        }

        // POST: ClinicaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection, Clinica clinic)
        {
            try
            {//llamamos al metodo para eliminar la clinica segun el id obtenido
                clinic.DeleteClinica(id);
                return RedirectToAction("Info", "Veterinario");
            }
            catch
            {
                return View();
            }
        }
    }
}
