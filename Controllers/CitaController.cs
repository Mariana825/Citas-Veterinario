using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

namespace PuppyCit.Controllers
{
    public class CitaController : Controller
    {
        // GET: CitaController
        
        public ActionResult Index()
        {
            return View();
        }

        // GET: CitaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult SelecClinica()
            //es para el formulario que seleccione la clinica
        
        { // crea un objeto de la clase clinica
            Clinica clinica = new Clinica();
            //en la variable clinica va a guardar lo que obtuvo del metodo get clinicas
            var clinicas = clinica.GetClinica();
           
            ViewData["Clinicas"] = clinicas; //va a mostrar las variables con viewdata
            return View(); //retorna la vista donde va a seleccionar la clinica
           

        }

        [HttpGet]
        public ActionResult Selecvet()
        {
            //obtiene el id:clincia, ya que se guardo en la sesion
            int? idClinica = HttpContext.Session.GetInt32("IdClinica");

            if (idClinica == null)
            {// si el id_clinica es nulo ya despues de pasar a vet,, lo va a regresar a que seleccione la clinica
                return RedirectToAction("SeleccionarClinica");
            }

            Veterinario veterinario = new Veterinario();
            //crea un objeto de la clase  eterinario
            List<Veterinario> veterinarios = veterinario.GetVeterinariosPorClinica(idClinica.Value);
            //con ese objeto obtiene la lista de veteriarios, que se obtuvo llamando al metodo, mandandole el id:clinica

            ViewData["Veterinarios"] = veterinarios; //muestra la lista de vteerinarios con viewdata

            return View(); //retrnar la vista para sekeccionar el vet
        }


          [HttpGet]
        public ActionResult Selecmas()
        {
            //obtiene el idcliente que se guardo en la sesion
            int idCliente = (int)HttpContext.Session.GetInt32("IdCliente");

            //crea un objeto de la clase mascoata
          Mascota mascota = new Mascota();
            //y con el cliente obtenido de la sesion, obtiene las mascotas de ese cliente
            var mascotas = mascota.GetMascotasPorCliente(idCliente);
            //muestra todas las msascotas con el view data
            ViewData["Mascotas"] = mascotas;
            return View(); //retorna la vista para seleccionar la mascota
        }
        [HttpGet]
        public ActionResult Selechorario()
        { //para seleccionar el horario
            int? idClinica = HttpContext.Session.GetInt32("IdClinica");
            //este es para obtener los horarios de la clinica, se crea una instancia

            HorarioClinica horarioClinica = new HorarioClinica();
            //y se manda a llamar el metodo
            List<HorarioClinica> horarios = horarioClinica.GetHorarioPorClinica(idClinica.Value); 

            ViewData["Horarios"] = horarios;
            //pasas a la vista los horarios
            ViewBag.Servicios = new List<Servicio>();
            return View();
        }




        // POST: CitaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SelecClinica(IFormCollection collection)
        {
            // obtiene el id de la clínica desde el formulario q se selecciono
            int idClinica = Convert.ToInt32(collection["idClinica"]);
            //convert.ToInt32() convierte un valor de tipo string, ya que los datos enviados por formularios generalmente son string
           
            // guarda el id de la clínica en la sesión
            HttpContext.Session.SetInt32("IdClinica", idClinica);

            return RedirectToAction("Selecvet");
            //redirige a la vista de vet, para que seleccione el veterinario
        }

        public ActionResult Selecvet(IFormCollection collection)
        {
            //hace lo mismo que clinica, obtiene el idveterinario del fromualrio y lo convierte a int
            int idVeterinario = Convert.ToInt32(collection["idVeterinario"]);
            //guarda este id veterinario, porque se va a utilizar en el 3er paso del formualrio
            HttpContext.Session.SetInt32("IdVeterinario", idVeterinario);

            return RedirectToAction("Selecmas"); //redirige a seleccionar mascota

        }


        [HttpPost]
        public ActionResult Selecmas (IFormCollection collection)
        {
            //toma el idmascota seleccionado del formulario, y lo convierte a int
            int idMascota = Convert.ToInt32(collection["idMascota"]);

           //guarda este id de la mascota en la sesion
            HttpContext.Session.SetInt32("IdMascota", idMascota);

            //lo redirige a sleccionar hel horario de la cita
            return RedirectToAction("Selechorario");
        }

        [HttpPost]
        public ActionResult Selechorario (IFormCollection collection, Cita cita)
        {
           

            // obtiene la fecha seleccionada por el cliente desde el formulario igual el horario,
            // y los convierte a su tipo de dato que deben de ser
            DateTime fecha = Convert.ToDateTime(collection["fecha"]);
            TimeSpan horario = TimeSpan.Parse(collection["horario"]);

            //obtiene todos los id que se fueron guardando en la sesion para poder realizar el registro
            int idClinica = (int)HttpContext.Session.GetInt32("IdClinica");
            int idMascota = (int)HttpContext.Session.GetInt32("IdMascota");
            int idVeterinario = (int) HttpContext.Session.GetInt32("IdVeterinario");
            int idCliente = (int)HttpContext.Session.GetInt32("IdCliente");

            Cliente cliente = new Cliente();

            // Llamar al método ObtenerTelefonoPorId pasando el idCliente
           

            //crear un objeto de Cita, donde vamos a mandar todos  los datos de las variables que creamos
            Cita nuevaCita = new Cita
            {
                Fecha = fecha,
                Horario = horario,
                Estado = "Programada", //este lo cree en especial, porque queria mandar el estado como programada en determinado
                IdMascota = idMascota,
                IdVeterinario = idVeterinario,
                IdClinica = idClinica,
                IdCliente = idCliente
            };
            //se manda a llamr el metodo addcita y le pasamos es eobjeto creado
               cita.AddCita(nuevaCita); //

           


            return RedirectToAction("Info", "Cliente");

        }
      

        // GET: CitaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CitaController/Edit/5
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

        // GET: CitaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CitaController/Delete/5
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
