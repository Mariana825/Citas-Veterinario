using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PuppyCit.Models;
using System.Data;



namespace PuppyCit.Controllers
{

    public class ClienteController : Controller
    {


        [HttpPost]
        //se coloca la ruta para que el fecth no encuentre en automatico
        [Route("Cliente/ActualizarCita")]
        public IActionResult ActualizarCita([FromBody] Cita cita)
        { //este metodo es para la api d epaypal, que pueda actualizar el estado de la cita
            try
            {
                //necesita el id de la cita y el estado
                int idCita = cita.IdCita;
                string estado = cita.Estado;


                Servicio servicio = new Servicio();
                //se crea un objeto de la clase servicio y se manda a llamar el metodo, pasando estos 2 parametros
                servicio.ActualizarEstadoCita(idCita, estado);

                //retorna la vista del cliente, con el nuevo idcita que se actualizo
                return RedirectToAction("Info", new { id = idCita });
            }
            catch (Exception ex)
            {

                return BadRequest("Hubo un error al actualizar el estado de la cita.");
            }
        }

        public IActionResult Info(int idCita)
        {
            var sesionActual = InicioSesion.ObtenerSesionActual(HttpContext.Session);

            if (sesionActual != null)
            {
                // obtiene al cliente actual
                Cliente cliente = new Cliente();
                //extrae el cliente con el usuario
                cliente = cliente.GetClienteById(sesionActual.IdUsuario);
                //guarda el id cliente en la sesion
                HttpContext.Session.SetInt32("IdCliente", cliente.IdCliente);

               
                // obtiene la lista de citas
                Cita cita = new Cita();
                //llama al metodo de obtener la citas con el objeto de la clase cita, donde se le pasa el idclientew
                List<Cita> citas = cita.GetCitasCliente(cliente.IdCliente);
                ViewBag.Citas = citas;
                
                // obtiene mascotas del cliente
                Mascota mascota = new Mascota();
                //se crea el objeto mascotas y se manda a llamar dicho metdodo con el idcliente como parametro.
                List<Mascota> mascotas = mascota.GetMascotasPorCliente(cliente.IdCliente);
                ViewBag.Mascotas = mascotas;
                
                // obtiene lista de clínicas, esto es para la api de geolocalizacion, donde le va a pasar todas las clinicas disponibles y que se puedan visualizar
                Clinica clinica = new Clinica();
                //pasa todo el listado de las clinicas
                List<Clinica> listaClinicas = clinica.GetClinica();
                ViewBag.Clinicas = listaClinicas;
                
                return View(cliente);
            }

            return RedirectToAction("Login", "InicioSesion");
        }



        public ActionResult Index()
        {
            //pasa todos los clientes a la vista
            return View(new Cliente().GetCliente());
        }

        // GET: ClienteController/Details
        public ActionResult Details(int id)
        {


            return View(); 
        }

        // POST: ClienteController/Details
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Details(Cliente cliente)
        {
            
            return View();
        }


        // GET: ClienteController/Create
        public ActionResult Create(int idUsuario, string nombre, string apellidos, string telefono)
        {
            //al momento de crear un cliente, va a pasar todos sus datos de usuaio, para que pueda visualizarlos
            //al momento de contestar su formulario
            ViewBag.Nombre = nombre;
            ViewBag.Apellidos = apellidos;
            ViewBag.Telefono = telefono;

            // crea el nuevo cliente con el IdUsuario obtenido del formulario pasado
            var nuevoCliente = new Cliente
            {
                IdUsuario = idUsuario 
            };
            //retorna el cliente
            return View(nuevoCliente);
        }




        // POST: ClienteController/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Cliente client)
        {
     
            try
            {
            
                client.Usuario = new Usuario(); // crea una nueva instancia de Usuario dentro de Cliente
                //y se mandan el idusuario que se obtuvo del formualrio
                client.Usuario.IdUsuario = Convert.ToInt32(collection["id_usuario"]);

                client.AddCliente(client);


                //ya que registro el cliente, lo manda a iniciar sesion
                return RedirectToAction("Login", "InicioSesion");
            }
            catch 
            {
                // si ocurre un error, vuelve a mostrar el formulario de creación
            }

            return View(client); 
        }

        [HttpGet]
        public IActionResult EditDatosPersonales(int idUsuario)
        {
            //este metodo solo busca al cliente a editar, en este se crea el objeto y se manda a llamar el metodo con el idusuario
            Cliente cliente = new Cliente().GetClienteById(idUsuario);

            return View(cliente);
        }
        [HttpGet]
        public IActionResult EditDireccion(int idUsuario)
        {
            //este metodo solo busca al cliente a editar, en este se crea el objeto y se manda a llamar el metodo con el idusuario

            Cliente cliente = new Cliente().GetClienteById(idUsuario);

            return View(cliente);
        }

            // POST: ClienteController/Edit/5
            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDatosPersonales(int idusuario, Cliente client)
        {
            try
            {
                //este metodo solo modifica la parte de usuario
                client.EditarDatosPersonales(client);
                //mandamos a llamr al metodo
                return RedirectToAction("Info", "Cliente");
            }
            catch
            {
                return View(client);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDireccion(int idusuario, Cliente client)
        {
            try
            {
                //este metodo solo modifica los atributos de cliente
                //mandamos a llamr al metodo
                client.EditarDireccion(client);
                return RedirectToAction("Info", "Cliente");
            }
            catch
            {
                return View(client);
            }
        }


        // GET: ClienteController/Delete/5
        public ActionResult Delete(int id)
        {
            //con el id, buscamos el cliente a eliminar
            return View(new Cliente().GetClienteId(id)); 
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //con este metodo eliminamos al cliente
                Cliente client = new Cliente();
                client.DeleteCliente(id);  //le pasamos el id del cliente
                return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                return View(); 
                    }
        }

       
    }
}
