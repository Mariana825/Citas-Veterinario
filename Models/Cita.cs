using System;
using System.Collections.Generic;

using System.Data;
using System.Drawing;
using Npgsql;

namespace PuppyCit.Models
{
    public class Cita : Conexion
    {
        public int IdCita { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Horario { get; set; }
        public string Estado { get; set; }
        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }
        public int IdClinica { get; set; }

        public int IdCliente { get; set; }

        // estas son propiedades de tipo objeto que se van a usar para almacenar las relaciones entre la cita y otras calses

        public Veterinario Veterinario { get; set; }
        public Mascota Mascota { get; set; }
        public Clinica Clinica { get; set; }
        public Especie Especie { get; set; }

        public Raza Raza { get; set; }
        
        public Servicio Servicio { get; set; }
        public Resena Resena { get; set; }
        public List<Servicio> Servicios { get; set; }


        public Cita() { }

        public Cita(int idCita)
        {
            this.IdCita = idCita;
        }

        public Cita(int idCita, DateTime fecha, TimeSpan horario, string estado, int idMascota, int idVeterinario, int idClinica, int idCliente)
            : this(idCita)
        {
            Fecha = fecha;
            Horario = horario;
            Estado = estado;
            IdMascota = idMascota;
            IdVeterinario = idVeterinario;
            IdClinica = idClinica;
            IdCliente = idCliente;
        }


        public void AddCita(Cita cita)

        // este método agrega una nueva cita a la base de datos
        {
            const string sql = "INSERT INTO cita (fecha, horario, estado, id_mascota, id_veterinario, id_clinica, id_cliente) VALUES (:fecha, :horario, :estado, :idMascota, :idVeterinario, :idClinica, :idCliente);";

            // crea los parámetros con los valores de la cita para insertarlos en la base de datos
            NpgsqlParameter paramFecha = new NpgsqlParameter(":fecha", cita.Fecha);
            NpgsqlParameter paramHorario = new NpgsqlParameter(":horario", cita.Horario);
            NpgsqlParameter paramEstado = new NpgsqlParameter(":estado", cita.Estado);
            NpgsqlParameter paramIdMascota = new NpgsqlParameter(":idMascota", cita.IdMascota);
            NpgsqlParameter paramIdVeterinario = new NpgsqlParameter(":idVeterinario", cita.IdVeterinario);
            NpgsqlParameter paramIdClinica = new NpgsqlParameter(":idClinica", cita.IdClinica);
            NpgsqlParameter paramIdCliente = new NpgsqlParameter(":idCliente", cita.IdCliente);

            // agrega todos los parámetros a una lista
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>();
            lstParams.Add(paramFecha);
            lstParams.Add(paramHorario);
            lstParams.Add(paramEstado);
            lstParams.Add(paramIdMascota);
            lstParams.Add(paramIdVeterinario);
            lstParams.Add(paramIdClinica);
            lstParams.Add(paramIdCliente);

            // ejecuta la consulta para insertar los datos de la cita
            GetQuery(sql, lstParams);
        }


        public void EditCita(Cita cita)
        {
            //proximo a ultilizar pero solo para el veterinario
            const string sql = "UPDATE cita SET fecha = :fecha, horario = :horario, estado = :estado, id_mascota = :idMascota, id_veterinario = :idVeterinario, id_clinica = :idClinica, id_cliente = :idCliente WHERE id_cita = :id;";

            NpgsqlParameter paramId = new NpgsqlParameter(":id", cita.IdCita);
            NpgsqlParameter paramFecha = new NpgsqlParameter(":fecha", cita.Fecha);
            NpgsqlParameter paramHorario = new NpgsqlParameter(":horario", cita.Horario);
            NpgsqlParameter paramEstado = new NpgsqlParameter(":estado", cita.Estado);
            NpgsqlParameter paramIdMascota = new NpgsqlParameter(":idMascota", cita.IdMascota);
            NpgsqlParameter paramIdVeterinario = new NpgsqlParameter(":idVeterinario", cita.IdVeterinario);
            NpgsqlParameter paramIdClinica = new NpgsqlParameter(":idClinica", cita.IdClinica);
            NpgsqlParameter paramIdCliente = new NpgsqlParameter(":idCliente", cita.IdCliente);

            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>
            {
                paramId,
                paramFecha,
                paramHorario,
                paramEstado,
                paramIdMascota,
                paramIdVeterinario,
                paramIdClinica,
                paramIdCliente
            };

            GetQuery(sql, lstParams);
        }



        public List<Cita> GetCitasCliente(int idCliente)
        {

        
            //metodo apra obtener todo lo correspondiente a las citas del cliente
            const string sql = @" SELECT cita.id_cita, cita.fecha, cita.horario, cita.estado,
                            mascota.id_mascota, mascota.nombre AS nombre_mascota, mascota.edad,
                            especie.nombre AS especie, raza.nombre AS raza,
                            usuario.nombre AS nombre_veterinario, clinica.nombre AS nombre_clinica,
                            servicio.id_servicio, servicio.nombre AS nombre_servicio, servicio.tratamiento, 
                            servicio.costo, servicio.diagnostico, servicio.metodo_pago, servicio.cuentapaypal,
                            resena.id_resena, resena.comentario, resena.calificacion,cita.id_veterinario
                        FROM cita
                        INNER JOIN mascota ON cita.id_mascota = mascota.id_mascota
                        INNER JOIN raza ON mascota.id_raza = raza.id_raza
                        INNER JOIN especie ON raza.id_especie = especie.id_especie
                        INNER JOIN veterinario ON cita.id_veterinario = veterinario.id_veterinario
                        INNER JOIN usuario ON veterinario.id_usuario = usuario.id_usuario
                        INNER JOIN clinica ON cita.id_clinica = clinica.id_clinica
                        LEFT JOIN servicio ON cita.id_cita = servicio.id_cita
                        LEFT JOIN resena ON servicio.id_servicio = resena.id_servicio AND resena.id_cliente = @idCliente
                        WHERE cita.id_cliente = @idCliente;";


            NpgsqlParameter paramIdCliente = new NpgsqlParameter("@idCliente", idCliente);
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter> { paramIdCliente };

            // Ejecuta la consulta SQL con los parámetros y almacena el resultado en una DataTable
            DataTable tabla = GetQuery(sql, lstParams);
           
            List<Cita> lstCitas = new List<Cita>();

            if (tabla.Rows.Count < 1)
            {
                return lstCitas; // Si no se encontraron citas, devuelve una lista vacía
            }

            // Listado de citas y servicios
            foreach (DataRow fila in tabla.Rows)
            {
                int idCita = (int)fila["id_cita"];

                // Crear el objeto Cita
                Cita cit = new Cita
                {
                    IdCita = idCita,
                    Fecha = (DateTime)fila["fecha"],
                    Horario = (TimeSpan)fila["horario"],
                    Estado = (string)fila["estado"],
                    Mascota = new Mascota
                    { //para obtener los datos de la mascota
                        IdMascota = (int)fila["id_mascota"],
                        Nombre = (string)fila["nombre_mascota"],
                        Edad = (int)fila["edad"],
                        Raza = new Raza
                        {
                            Nombre = (string)fila["raza"]
                        },
                        Especie = new Especie
                        {
                            Nombre = (string)fila["especie"]
                        }
                    },
                    Veterinario = new Veterinario
                    {//para obtener el veterinario asginado a la cita
                        IdVeterinario = (int)fila["id_veterinario"],
                        Nombre = (string)fila["nombre_veterinario"]
                    },
                    Clinica = new Clinica
                    {//obtener la clinica de la cita
                        Nombre = (string)fila["nombre_clinica"]
                    },
                    Resena = fila["id_resena"] != DBNull.Value ? new Resena
                    {//si existe una reseña  se va a crear esta instancia, donde manda a llamar todo lo correspondiente a cita
                        IdResena = (int)fila["id_resena"],
                        Comentario = (string)fila["comentario"],
                        Calificacion = (int)fila["calificacion"]
                    } : null //sino se deja como null
                };

                // Si la cita tiene un servicio asociado, se asignan todos los valores que tenga el servicio
                if (fila["id_servicio"] != DBNull.Value)
                {
                    var servicio = new Servicio
                    {//para mostrar todo lo referente a servicio
                        IdServicio = (int)fila["id_servicio"],
                        Nombre = (string)fila["nombre_servicio"],
                        Tratamiento = (string)fila["tratamiento"],
                        Costo = (decimal)fila["costo"],
                        Diagnostico = (string)fila["diagnostico"],
                        MetodoPago = (string)fila["metodo_pago"],
                        Cuentapaypal = (string)fila["cuentapaypal"]
                    };

                    cit.Servicio = servicio; // asigna ese objeto servicio, al objeto de citas cit
                }

                
                // Si no tiene servicio, la propiedad Servicio se deja como null (por defecto)

                lstCitas.Add(cit);
            }

            return lstCitas;
        }


        public List<Cita> GetCitasVeterinario(int idVeterinario)
        // obtiene todas las citas asociadas de un cliente a partir de su id (idCliente)
        {

            const string sql =  @"SELECT cita.id_cita, cita.fecha, cita.horario, cita.estado,
                                         mascota.id_mascota, mascota.nombre AS nombre_mascota, mascota.edad,
                                         especie.nombre AS especie, raza.nombre AS raza,
                                         usuario.nombre AS nombre_cliente, usuario.apellidos AS apellidos_cliente, 
                                         clinica.nombre AS nombre_clinica,
                                         servicio.id_servicio, servicio.nombre AS nombre_servicio, 
                                         servicio.tratamiento, servicio.costo, servicio.diagnostico, 
                                         servicio.metodo_pago, servicio.cuentapaypal
                                  FROM cita
                                  INNER JOIN mascota ON cita.id_mascota = mascota.id_mascota
                                  INNER JOIN raza ON mascota.id_raza = raza.id_raza
                                  INNER JOIN especie ON raza.id_especie = especie.id_especie
                                  INNER JOIN cliente ON cita.id_cliente = cliente.id_cliente
                                  INNER JOIN usuario ON cliente.id_usuario = usuario.id_usuario
                                  INNER JOIN clinica ON cita.id_clinica = clinica.id_clinica
                                  LEFT JOIN servicio ON cita.id_cita = servicio.id_cita
                                  WHERE cita.id_veterinario = @idVeterinario;";

            // información adicional de la mascota, especie, raza, veterinario
            // y clínica asociadas.


            // crea el parámetro para la consulta, usando el id del cliente.
            NpgsqlParameter paramIdVeterinario = new NpgsqlParameter("@idVeterinario", idVeterinario);
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter> { paramIdVeterinario };

            // ejecuta la consulta SQL con los parámetros y almacena el resultado en una DataTable.
            DataTable tabla = GetQuery(sql, lstParams);
            List<Cita> lstCitas = new List<Cita>();


            // no se encontraron citas, se retorna una lista vacía.
            if (tabla.Rows.Count < 1) // si no hay filas en la tabla, no hay clientes
            {
                return lstCitas; // devuelve una lista vacia
            }

            foreach (DataRow fila in tabla.Rows)
            {
                //  un objeto cita y asigna los valores de la fila de la consulta.
                Cita cit = new Cita
                {
                    IdCita = (int)fila["id_cita"],
                    Fecha = (DateTime)fila["fecha"],
                    Horario = (TimeSpan)fila["horario"],
                    Estado = (string)fila["estado"],

                    // para mostrar ibnformación de la mascota de la cita
                    Mascota = new Mascota
                    {
                        IdMascota = (int)fila["id_mascota"],
                        Nombre = (string)fila["nombre_mascota"],
                        Edad = (int)fila["edad"],
                        Raza = new Raza
                        {
                            Nombre = (string)fila["raza"]
                        },
                        Especie = new Especie
                        {
                            Nombre = (string)fila["especie"]
                        }
                    },
                    //para mostrar los apellidos y nombre del cliente
                    Cliente = new Cliente 
                    {
                        Nombre = (string)fila["nombre_cliente"],
                        Apellidos = (string)fila["apellidos_cliente"]
                    },

                    //obtener la clinica que se asigno para la cita
                    Clinica = new Clinica
                    {
                        Nombre = (string)fila["nombre_clinica"]
                    }
                };
                if (fila["id_servicio"] != DBNull.Value) //si el campo idservicio si tiene un valor válido
                {
                    var servicio = new Servicio //crea una instancia de la clase Servicio y asigna sus propiedades usando los valores 
                    {
                        IdServicio = (int)fila["id_servicio"],
                        Nombre = (string)fila["nombre_servicio"],
                        Tratamiento = (string)fila["tratamiento"],
                        Costo = (decimal)fila["costo"],
                        Diagnostico = (string)fila["diagnostico"],
                        MetodoPago = (string)fila["metodo_pago"],
                        Cuentapaypal = (string)fila["cuentapaypal"]
                    };
                    //
                    cit.Servicio = servicio; // asigna ese objeto servicio, al objeto de citas cit
                }

                // se agregan las citas obtenidas desde la base de datos a la lista de citas del cliente.
                lstCitas.Add(cit);

            }

            // regresa la lista de citas con toda la información asociada.
            return lstCitas;
        }

        public int GetServicioByIdCita(int idCita)
        {
            //metodo para obtener el servicio segun la cita (viene enviado desde el formuario)
            const string sql = "SELECT id_servicio FROM cita WHERE id_cita = :idCita;";
            NpgsqlParameter paramIdCita = new NpgsqlParameter(":idCita", idCita);
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>() { paramIdCita };
            DataTable tabla = GetQuery(sql, lstParams);

            // si se encuentra la cita, devuelve el id_servicio
            if (tabla.Rows.Count > 0)
            {
                return (int)tabla.Rows[0]["id_servicio"];
            }

            // si no se encuentra la cita, devuelve un valor predeterminado (por ejemplo, 0)
            return 0;
        }


    }
}
