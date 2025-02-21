using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PuppyCit.Models;
using System;

namespace PuppyCit.Controllers
{
    public class ServicioController : Controller
    {
        // GET: ServicioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServicioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
      
        // GET: ServicioController/Create
        public ActionResult Create(int idCita)
        {
            //se crea una variable servicio, donde se va a guardar el id cita enviado desde el formulario
            var servicio = new Servicio { IdCita = idCita }; 
            return View(servicio);
           
        }

        

        // POST: ServicioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        
        public ActionResult Create(IFormCollection collection, Servicio servicio)
        {

            try
            {
                //recibe una variable de tipo cadena sobre el metodo de pago
                string metodoPago = collection["MetodoPago"];
                // insertar el servicio en la base de datos.
                servicio.AddServicio(servicio);
                

                // aqui actualizar el estado de la cita según el método de pago.
                string estadoCita;

                if (metodoPago == "Efectivo")
                {
                    estadoCita = "Finalizada"; 
                }

                else if (metodoPago == "Paypal")
                {
                    estadoCita = "Pendiente de Pago";
                }
                else
                {
                    estadoCita = "Programada"; 
                    //esta es la que tiene por defecto cuando crea el cliente la cita
                }


                // llamamos a una función para actualizar el estado de la cita, con los parametros que debe recibir el metodo
                servicio.ActualizarEstadoCita(servicio.IdCita, estadoCita);

                return RedirectToAction("Info", "Veterinario", new { idCita = servicio.IdCita });
               
                
            }
            catch
            {
                return View();
            }
        }

        // GET: ServicioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServicioController/Edit/5
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

        // GET: ServicioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServicioController/Delete/5
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
