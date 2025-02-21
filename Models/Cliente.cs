using System;
using System.Data;
using Npgsql;
using Twilio.TwiML.Voice;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PuppyCit.Models
{
    public class Cliente : Conexion
    {
        public int IdCliente { get; set; }
        public string Estado { get; set; }
        public int Codigop { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public int IdUsuario { get; set; }


        public int IdCita { get; set; }
        public Usuario Usuario { get; set; } // referencia a la clase usuario para obtener datos extra del cliente
        public Cita Cita { get; set; } // relacion con la clase cita, cliente puede tener citas
        
        public string Nombre { get; set; } // nombre del cliente
        public string Apellidos { get; set; } // apellidos del cliente
        public string Telefono { get; set; } // telefono del cliente
        public Cliente() { }

        public Cliente(int idCliente)
        {
            this.IdCliente = idCliente;
        }

        public Cliente(int idCliente, string estado, int codigop, string municipio, string colonia, string calle, int idUsuario)
        : this(idCliente)
        {
            Estado = estado;
            Codigop = codigop;
            Municipio = municipio;
            Colonia = colonia;
            Calle = calle;
            IdUsuario = idUsuario;
        }

        public List<Cliente> GetCliente()
        {
            const string sql = "SELECT * FROM cliente;"; // consulta para obtener todos los clientes
            DataTable tabla = GetQuery(sql); // ejecuta la consulta y devuelve los datos en una tabla
            List<Cliente> lstCliente = new List<Cliente>(); // crea una lista vacia de clientes

            if (tabla.Rows.Count < 1) // si no hay filas en la tabla, no hay clientes
            {
                return lstCliente; // devuelve una lista vacia
            }

            foreach (DataRow fila in tabla.Rows) // recorre cada fila de la tabla
            {
                // crea un nuevo objeto cliente con los datos de la fila y lo agrega a la lista
                lstCliente.Add(new Cliente(
                (int)fila["id_cliente"],
                (string)fila["estado"],
                (int)fila["codigo_p"],
                (string)fila["municipio"],
                (string)fila["colonia"],
                (string)fila["calle"],
                (int)fila["id_usuario"]));
            }
            return lstCliente; // retorna la lista con todos los clientes
        }

        public void AddCliente(Cliente clientito)
        {
            const string sql = "INSERT INTO cliente (estado, codigo_p, municipio, colonia, calle, id_usuario) VALUES (:esta, :codi, :munici, :colo, :cal, :id_usu);";
            //la lista
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>();

            // se crean los prametros que van a la lista
            NpgsqlParameter paramEstado = new NpgsqlParameter(":esta", clientito.Estado);
            NpgsqlParameter paramCodigoP = new NpgsqlParameter(":codi", clientito.Codigop);
            NpgsqlParameter paramMunicipio = new NpgsqlParameter(":munici", clientito.Municipio);
            NpgsqlParameter paramColonia = new NpgsqlParameter(":colo", clientito.Colonia);
            NpgsqlParameter paramCalle = new NpgsqlParameter(":cal", clientito.Calle);
            NpgsqlParameter paramIdUsuario = new NpgsqlParameter(":id_usu", clientito.IdUsuario);

            lstParams.Add(paramEstado);
            lstParams.Add(paramCodigoP);
            lstParams.Add(paramMunicipio);
            lstParams.Add(paramColonia);
            lstParams.Add(paramCalle);
            lstParams.Add(paramIdUsuario);
            //agrega los aprametros a la lista
            GetQuery(sql, lstParams);
        }


        public Cliente GetClienteById(int idUsuario)
        // este metodo es para mostrar toda la informacion del cliente en base al id que va a obtener del sessin

        {
            // une tabla cliente y usuario para obtener datos completos de un cliente por id_usuario

           const string SQL = @"SELECT cliente.id_cliente, cliente.estado, cliente.codigo_p, cliente.municipio, cliente.colonia, 
            cliente.calle, cliente.id_usuario, usuario.nombre, usuario.apellidos, usuario.telefono 
            FROM cliente INNER JOIN usuario ON cliente.id_usuario = usuario.id_usuario WHERE cliente.id_usuario = @idUsuario;";


            NpgsqlParameter paramId = new NpgsqlParameter("@idUsuario", idUsuario); // parametro para la consulta
            List<NpgsqlParameter> lstParameter = new List<NpgsqlParameter>() { paramId }; // lista de paraetros
            DataTable tabla = GetQuery(SQL, lstParameter); // ejecuta la consulta y devuelve una tabla con los resultados

            if (tabla.Rows.Count < 1) return new Cliente();

            DataRow row = tabla.Rows[0];// obtiene la primera fila de la tabla (solo hay una fila si la consulta es correcta)

            // crea un cliente con los datos obtenidos de la fila
            Cliente client = new Cliente // estamos creando un nuevo objeto de esta Cliente
            {
                IdCliente = (int)row["id_cliente"], // se asivgvna el valor de la columna idcliente de la fila, a la propiedad IdCliente.
                                                    // como en la base de datos es un número, lo convertimos con (int) para que se muestre bien
                Estado = (string)row["estado"],
                Codigop = (int)row["codigo_p"],
                Municipio = (string)row["municipio"],
                Colonia = (string)row["colonia"],
                Calle = (string)row["calle"],
                IdUsuario = (int)row["id_usuario"],

                Usuario = new Usuario // se crea un nuevo objeto de tipo Usuario
                {
                    Nombre = (string)row["nombre"],
                    Apellidos = (string)row["apellidos"],
                    Telefono = (string)row["telefono"],
                }
            };
           // aqui se asignan los valores que se obtienen de la consulta SQL 
            //  con las propiedades correspondientes de la clase  Cliente
            // se tiene toda la informacion del cliente y su usuario lista para ser utilizad en la vista INFO

            return client;
        }

        public void EditarDatosPersonales(Cliente cliente)
        { //metodo para editar los datos del cliente, pero solo la parte del usuario
            const string sqlUsuario = @" UPDATE usuario  SET nombre = :nombre, apellidos = :apellidos, telefono = :telefono 
            WHERE id_usuario = :idUsuario;";
            //se crean los parametros de la parte de usuario
            NpgsqlParameter paramNombre = new NpgsqlParameter(":nombre", cliente.Usuario.Nombre);
            NpgsqlParameter paramApellidos = new NpgsqlParameter(":apellidos", cliente.Usuario.Apellidos);
            NpgsqlParameter paramTelefono = new NpgsqlParameter(":telefono", cliente.Usuario.Telefono);
            NpgsqlParameter paramIdUsuario = new NpgsqlParameter(":idUsuario", cliente.IdUsuario);
            //se crea la lista y se pasan los parametros
            List<NpgsqlParameter> lstParamsUsuario = new List<NpgsqlParameter>
            {
                paramNombre, paramApellidos, paramTelefono, paramIdUsuario
            };
            //se ejecuta la consulta
            GetQuery(sqlUsuario, lstParamsUsuario); 
        }
        public void EditarDireccion(Cliente cliente)
        {//metodo para editar los datos del cliente, ahora con los atributos de cliente

            const string sqlCliente = @" UPDATE cliente SET estado = :estado, codigo_p = :codigoP, municipio = :municipio, colonia = :colonia, calle = :calle 
        WHERE id_usuario = :idUsuario;";
            //se crean los parametros de la parte de cliente
            NpgsqlParameter paramEstado = new NpgsqlParameter(":estado", cliente.Estado);
            NpgsqlParameter paramCodigoP = new NpgsqlParameter(":codigoP", cliente.Codigop);
            NpgsqlParameter paramMunicipio = new NpgsqlParameter(":municipio", cliente.Municipio);
            NpgsqlParameter paramColonia = new NpgsqlParameter(":colonia", cliente.Colonia);
            NpgsqlParameter paramCalle = new NpgsqlParameter(":calle", cliente.Calle);
            NpgsqlParameter paramIdUsuario = new NpgsqlParameter(":idUsuario", cliente.IdUsuario);
            //se crea la lista y se pasan los parametros
            List<NpgsqlParameter> lstParamsCliente = new List<NpgsqlParameter>
            {
                paramEstado, paramCodigoP, paramMunicipio, paramColonia, paramCalle, paramIdUsuario
            };
            //se ejecuta la consulta
            GetQuery(sqlCliente, lstParamsCliente);
        }




        public void DeleteCliente(int id)
        {
            //metodo para eliminar al cliente
            const string sql = "DELETE FROM cliente WHERE id_cliente = :id;";
            //se crea el parametro y se pasa a la lista
            NpgsqlParameter paramId = new NpgsqlParameter(":id", id);
            List<NpgsqlParameter> lstParam = new List<NpgsqlParameter>() { paramId };
            //se ejecuta la consulta
            GetQuery(sql, lstParam);
        }


        public Cliente GetClienteId(int id)
        // este obtiene un cliente de la base de datos por su idCliente
        // la consulta selecciona todos los datos del cliente con el idcleint
        // si se encuentra un cliente con ese id, se asignan sus datos a un objeto Cliente y se devuelve
        {
            const string SQL = "SELECT * FROM cliente WHERE id_cliente = :id;";
            NpgsqlParameter paramId = new NpgsqlParameter(":id", id);
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>() { paramId };
            DataTable tabla = GetQuery(SQL, lstParams);

            // si no se encuentra ningun cliente, se devuelve un objeto Cliente vacío
            if (tabla.Rows.Count < 1) return new Cliente();

            // si se encuentra un cliente, se asignan sus datos a un nuevo objeto Cliente
            foreach (DataRow row in tabla.Rows)
            {
                Cliente cliente = new Cliente
                {
                    IdCliente = (int)row["id_cliente"],
                    Estado = (string)row["estado"],
                    Codigop = (int)row["codigo_p"],
                    Municipio = (string)row["municipio"],
                    Colonia = (string)row["colonia"],
                    Calle = (string)row["calle"],
                    IdUsuario = (int)row["id_usuario"]
                };

                // se devuelve el cliente con los datos asignados
                return cliente;
            }

            // si no se encuentra ningun cliente, se devuelve un objeto Cliente vacío
            return new Cliente();
        }


        public List<Mascota> GetMascotasByCliente(int id)
        // obtiene todas las mascotas de un cliente la consulta selecciona todas las mascotas que tienen el idCliente dado de la sesion
        // se crea una lista de Mascota y se llena con los datos obtenidos
        {
            const string sql = "SELECT * FROM mascota WHERE id_cliente = :id;";
            // se crea un nuevo parametro y la lista
            NpgsqlParameter paramId = new NpgsqlParameter(":id", id);
            List<NpgsqlParameter> lstParams = new List<NpgsqlParameter>() { paramId };
            DataTable tabla = GetQuery(sql, lstParams); // ejecuta la consulta y devuelve una tabla con los resultados

            // se crea una lista vacía de objetos Mascota para almacenar 
            List<Mascota> mascotas = new List<Mascota>();

            // se itera sobre las filas de la tabla de resultados y se asignan los datos de cada fila a un nuevo objeto Mascota
            foreach (DataRow fila in tabla.Rows)
            {
                mascotas.Add(new Mascota
                {
                    IdMascota = (int)fila["id_mascota"],
                    Nombre = (string)fila["nombre"],
                    Tamaño = (string)fila["tamano"],
                    Edad = (int)fila["edad"],
                    IdRaza = (int)fila["id_raza"],
                    IdCliente = (int)fila["id_cliente"]
                });
            }

            // se devuelve la lista con todas las mascotas del cliente
            return mascotas;
        }
       


    }
}