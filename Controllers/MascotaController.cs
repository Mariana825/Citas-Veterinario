using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;


namespace PuppyCit.Controllers
{
    public class MascotaController : Controller
    {
       
       
        public IActionResult SelecEspecie()
        {
            //crea un objeto de la clase especie
            Especie especie = new Especie();
            //con este objeto llamas todas las especie
            var especies = especie.GetEspecies();
            ViewData["Especies"] = especies; // con view data se van a visualizar todas las especies de la bd
            return View(); //retorna la vista de este formulario de especies


        }
        [HttpGet]
        public IActionResult Create(int IdEspecie)
        {
           //se guarda el id_especie en la sesion
            HttpContext.Session.SetInt32("IdEspecie", IdEspecie);
            
            //se crea un objeto de la clase raza
            Raza raza = new Raza();
            //y con ese objeto llamamos al metodo que no va a dar todas la s razas de esa especie
            var razas = raza.GetRazasPorEspecie(IdEspecie); 
            //con view data vamos a poder visualizar todas las razasa
            ViewData["Razas"] = razas; 
            //retorna la vista con dicho formualrio de razas
            return View();
         

        }



        // GET: MascotaController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SelecEspecie(IFormCollection collection)
        {
            //obtiene el idespecie que se selecciono y se guardo en la sesion
            int idEspecie = Convert.ToInt32(collection["idEspecie"]);
            // lo manda al create de mascota bien, con el idespecie seleccionado
            return RedirectToAction("Create", new { IdEspecie = idEspecie });
        }

        public IActionResult Create(IFormCollection collection)
        {
            //este create ya tiene todo de la mascota

            int idRaza = Convert.ToInt32(collection["IdRaza"]);
            //obtiene ambos id que se guardaron, el de raza, y el de cliente
            int idCliente = (int) HttpContext.Session.GetInt32("IdCliente");

            Mascota mascotita = new Mascota //se crea asi un objeto, para que mande especificamente estos atributos a cliente
            {
                Nombre = collection["Nombre"], 
                Tamaño = collection["Tamaño"],
                Edad = Convert.ToInt32(collection["Edad"]),
                IdRaza = idRaza,
                IdCliente = idCliente
            };

            mascotita.AddMascota(mascotita); //se manda a llamar el addmascota, y
                                             //le pasa el objeto creado que va a tener los dos ids

            return RedirectToAction("Info", "Cliente"); //lo redirreciona a cliente
        }

        // GET: MascotaController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //se manda a llamr el metodo para obtener la mascota a editar
            Mascota mascota = new Mascota().GetMascotaById(id);
          
            return View(new Mascota
            { //solo se vana  modificar estos atributos, por ellos los retornamos
                IdMascota = mascota.IdMascota,
                Nombre = mascota.Nombre,
                Tamaño = mascota.Tamaño,
                Edad = mascota.Edad
            });
        }

        // POST: MascotaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Mascota mascota)
        {
            try
            {
               //en el objeto mascota guardamos el id mascota y ocn ese objeto llamamos al metodo a editar
                mascota.IdMascota = id;
                mascota.EditMascota(mascota);
                return RedirectToAction("Info", "Cliente");
            }
            catch
            {
                return View(mascota);
            }
        }

        // GET: MascotaController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //creamos un objeto de la clase mascota y llamamos al metodo para obtener la mascota por el id
            Mascota mascota = new Mascota().GetMascotaById(id);
            return View(mascota);
        }

        // POST: MascotaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection, Mascota mascota)
        {
            try
            {
                //eliminamos la mascota con el id obtenido
                mascota.DeleteMascota(id);
                return RedirectToAction("Info", "Cliente");
            }
            catch
            {
                return View();
            }
        }
    }
}
