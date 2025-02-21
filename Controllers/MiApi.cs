using Microsoft.AspNetCore.Mvc;
using PuppyCit.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PuppyCit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiApi : ControllerBase
    {
        // GET: api/<MiApi>
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetRazas()
        {
         
            //creamos un objeto de la clase raza
            Raza raza = new Raza();

            // llamamos a todas las razas con su especie asociada desde la base de datos
            List<Raza> razas = raza.GetRazas();

            // esto devolver solo el nombre de la raza y el nombre de la especie
            var resultado = new List<object>();

            foreach (var item in razas)
            {
               
                var razaConEspecie = new
                {
                    Raza = item.Nombre,                // nombre de la raza
                    Especie = item.Especie?.Nombre     // nombre de la especie (si es null, no se muestra)
                };

                resultado.Add(razaConEspecie);
            }

            // retornar el resultado 
            return Ok(resultado);
        }

       
        [HttpGet("{id}")]
        public ActionResult<object> GetRazaById(int id)
        {
           
            Raza raza = new Raza();
            // busca la raza en la base de datos usando el id
            Raza razaEncontrada = raza.GetRazaById(id);

            // si no encuentro la raza, devuelve un error 404
            if (razaEncontrada == null)
            {
                return NotFound();
            }

            // creo un objeto para devolver solo el nombre de la raza y la especie
            var razaConEspecie = new
            {
                Raza = razaEncontrada.Nombre,  // nombre de la raza
                Especie = razaEncontrada.Especie?.Nombre  // nombre de la especie, si existe
            };

            // devuelvo la respuesta con el objeto que contiene la raza y la especie
            return Ok(razaConEspecie);
        }

        // POST api/<MiApi>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MiApi>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MiApi>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
