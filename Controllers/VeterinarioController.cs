using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class VeterinarioController : Controller


    {

        public IActionResult Info(int idCita)
        {
            //obtiene la sesion actual
            var sesionActual = InicioSesion.ObtenerSesionActual(HttpContext.Session);

            if (sesionActual != null)
            {//si la sesion No es null, entonces hace un objeto de la clase veterinario, y
             //mandar a traer toda la informacion, de dicho veterinario con el metodo getveterinario
                Veterinario veterinario = new Veterinario();
                
                veterinario = veterinario.GetVeterinarioById(sesionActual.IdUsuario);
                HttpContext.Session.SetInt32("IdVeterinario", veterinario.IdVeterinario);

                // se crea un pbjeto de la clase cita que va a obtener todas la citas del veterinario
                Cita cita = new Cita();
             
                List<Cita> citas = cita.GetCitasVeterinario(veterinario.IdVeterinario);
                //pasa las citas a al vista
                ViewBag.Citas = citas;

                //se crea una objeto de la clase veterianrio que va a otener todas las reseñas
               Veterinario vet = new Veterinario();
                List<Resena> resenas = vet.GetResenas(veterinario.IdVeterinario);
                //pasa las reseñas a la vista
                ViewBag.Resenas = resenas;

                //obtiene las clinicas a la que este asociado el veterinario, con el idvterinario
              List<Clinica> clinicas = veterinario.GetClinicasPorVeterinario(veterinario.IdVeterinario);
                ViewBag.Clinicas = clinicas; //pasa las clinicas a la vista



                return View(veterinario); //retorna la vista del vet
            }

            return RedirectToAction("Login", "InicioSesion"); //sino encuentra la sesion, lo manda a iniciar sesion
        }
        // GET: VeterinarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VeterinarioController/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: VeterinarioController/Create
        public ActionResult Create(int idUsuario, string nombre, string apellidos, string telefono)
        {
            //crea un objeto de clinica
            Clinica clinica = new Clinica();
            //con este objeto, va a obtener todas las clinicas disponibles en la base de datos
            var clinicas = clinica.GetClinica(); 

            //manda con viewbagm todos los datos para poder visualizarlos
            ViewBag.ListaClinicas = clinicas;
            ViewBag.Nombre = nombre;
            ViewBag.Apellidos = apellidos;
            ViewBag.Telefono = telefono;

            //crea uuna variable de veterinario, que va a mandar el usuario del formulario pasado
            var nuevoVeterinario = new Veterinario
            {
                IdUsuario = idUsuario
            };
            //y retorna la vista
            return View(nuevoVeterinario);
        }

        // POST: VeterinarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Veterinario vet)
        {
            try
            {
                //cre un objeto de la clase usuario, en donde va a mandar el id usuario y la clinica
                vet.Usuario = new Usuario();

                vet.Usuario.IdUsuario = Convert.ToInt32(collection["idUsuario"] );

                var idClinica = collection["idClinica"];
                //recordar que el id clinica permite ser nulo, por si aun no tiene clinica
                vet.IdClinica = string.IsNullOrEmpty(idClinica) ? (int?)null : Convert.ToInt32(idClinica);

                //ese objeto creado se va a mandar al metodo add veterinario
                vet.AddVeterinario(vet);

                return RedirectToAction("Login", "InicioSesion"); // lo manda a iniciar sesion si ya se registro
            }
            catch 
            {}
            return View(vet);
        }


        [HttpGet]
        public IActionResult EditDatosPersonalesv(int idUsuario)
        {// se crea un objeto de la clase veterinario y se manda a llamar al metodo para obtener el veterianrio por medio del usuario
            Veterinario vet = new Veterinario().GetVeterinarioById(idUsuario);

            return View(vet);
        }
        [HttpGet]
        public IActionResult EditInfo(int idUsuario)
        {
            //pasamos a la vista todas las clinicas, para que el usuario pueda cambiar de clinica
            Clinica clinica = new Clinica();
            var clinicas = clinica.GetClinica();
            
            ViewBag.ListaClinicas = clinicas;
            //y obtener el veterinario por medio del id usuario
            Veterinario vet = new Veterinario().GetVeterinarioById(idUsuario);

            return View(vet);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDatosPersonalesv(int idusuario, Veterinario vet)
        {
            try
            {
                //mandamos a llamr al metodo para editar sus datos eprsonales
                vet.EditarDatosPersonalesv(vet);//esto es para editar la perte del usuario
                return RedirectToAction("Info", "Veterinario");
            }
            catch
            {
                return View(vet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditInfo(IFormCollection collection,int idusuario, Veterinario vet)
        {
            try
            {
                //se tomae el valor de id clinica, desde el formulario
                var idClinica = collection["idClinica"];
                 vet.IdClinica = string.IsNullOrEmpty(idClinica) ? (int?)null : Convert.ToInt32(idClinica); //sii está vacío, se asigna como null, porque tmb esta permitido, si no, lo convierte a entero.
                vet.EditarInfo(vet);//llama al metodo de editar la informacion
                return RedirectToAction("Info", "Veterinario");
            }
            catch
            {
                return View(vet);
            }
        }


        // GET: VeterinarioController/Delete/5
        public ActionResult Delete(int id)
        {
            //con el id, buscamos el veterinario a eliminar
            return View(new Veterinario().GetVeterinarioById(id));
        }

        // POST: VeterinarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            //con este metodo eliminamos al veterinario
            Veterinario vet = new Veterinario();
            vet.DeleteVeterinario(id);  //le pasamos el id del cliente
            return RedirectToAction(nameof(Index));
           
            
        }
    }
}
