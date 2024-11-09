using Microsoft.Data.Sqlite;
using ProductRepo;

// private int idPresupuesto;  => auto incremental
// private string nombreDestinatario; => viene de afuera
// private List<PresupuestosDetalle> detalle; =>    private Producto producto;  =>   private int idProducto;
//                                                  private int cantidad;           private string descripcion;
//                                                                                  private int precio;


namespace PresupuestoRepo
{
    //public class PresupuestoRepository : IRepository<Presupuesto>
    public class PresupuestoRepository
    {
        private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
        private int obtenerId(Presupuesto presupuesto)
        {
            var idBuscado = -999;
            string query = "SELECT idPresupuesto FROM Presupuestos WHERE NombreDestinatario = @nomb";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@nomb", presupuesto.NombreDestinatario));
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idBuscado = Convert.ToInt32(reader["idPresupuesto"]);
                    }
                };
                connection.Close();
            }
            return idBuscado;
        }
        private void auxSetId(Presupuesto presupuesto)
        {
            int idCorrespondiente = this.obtenerId(presupuesto);
            if (idCorrespondiente != -999) presupuesto.setId(idCorrespondiente);
        }
        public List<Presupuesto> GetAll()
        {
            List<Presupuesto> presupuestos = new List<Presupuesto>();
            string queryPresupuestos = "SELECT idPresupuesto, NombreDestinatario FROM Presupuestos";
            string queryDetalles = @"
                            SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                            FROM PresupuestosDetalle pd
                            INNER JOIN Productos p USING(idProducto)
                            WHERE pd.idPresupuesto = @idPresupuesto";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(queryPresupuestos, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPresupuesto = reader.GetInt32(0);
                            string nombreDestinatario = reader.GetString(1);

                            // Crear lista de detalles para cada presupuesto usando una nueva conexión
                            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();
                            using (var detalleConnection = new SqliteConnection(cadenaConexion))
                            {
                                detalleConnection.Open();
                                using (SqliteCommand detalleCommand = new SqliteCommand(queryDetalles, detalleConnection))
                                {
                                    detalleCommand.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                                    using (SqliteDataReader detalleReader = detalleCommand.ExecuteReader())
                                    {
                                        while (detalleReader.Read())
                                        {
                                            int idProducto = detalleReader.GetInt32(0);
                                            int cantidad = detalleReader.GetInt32(1);
                                            string descripcion = detalleReader.GetString(2);
                                            int precio = detalleReader.GetInt32(3);

                                            Producto producto = new Producto(idProducto, descripcion, precio);
                                            PresupuestosDetalle detalle = new PresupuestosDetalle(producto, cantidad);
                                            detalles.Add(detalle);
                                        }
                                    }
                                }
                            }
                            presupuestos.Add(new Presupuesto(idPresupuesto, nombreDestinatario, detalles));
                        }
                    }
                }
                connection.Close();
            }
            return presupuestos;
        }
        public Presupuesto GetById(int id)
        {
            Presupuesto presupuesto = null;
            string queryPresupuestos = "SELECT idPresupuesto, NombreDestinatario FROM Presupuestos WHERE idPresupuesto = @id";
            string queryDetalles = @"
                            SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                            FROM PresupuestosDetalle pd
                            INNER JOIN Productos p USING(idProducto)
                            WHERE pd.idPresupuesto = @idPresupuesto";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(queryPresupuestos, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", id));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Solo si encontramos un presupuesto
                        {
                            int idPresupuesto = reader.GetInt32(0);
                            string nombreDestinatario = reader.GetString(1);

                            // Cerrar el lector antes de ejecutar la consulta de detalles
                            reader.Close();

                            // Obtener los detalles
                            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();
                            using (SqliteCommand detalleCommand = new SqliteCommand(queryDetalles, connection))
                            {
                                detalleCommand.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

                                using (SqliteDataReader detalleReader = detalleCommand.ExecuteReader())
                                {
                                    while (detalleReader.Read())
                                    {
                                        int idProducto = detalleReader.GetInt32(0);
                                        int cantidad = detalleReader.GetInt32(1);
                                        string descripcion = detalleReader.GetString(2);
                                        int precio = detalleReader.GetInt32(3);

                                        Producto producto = new Producto(idProducto, descripcion, precio);
                                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto, cantidad);
                                        detalles.Add(detalle);
                                    }
                                }
                            }

                            presupuesto = new Presupuesto(idPresupuesto, nombreDestinatario, detalles);
                        }
                    }
                }
                connection.Close();
            }
            return presupuesto;
        }
        public Presupuesto Create(Presupuesto obj)
        {
            string queryInsertPresupuesto = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @fechaCreacion); SELECT last_insert_rowid();";
            //string queryInsertDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad);";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                // Insertar el presupuesto
                using (var command = new SqliteCommand(queryInsertPresupuesto, connection))
                {
                    command.Parameters.AddWithValue("@nombreDestinatario", obj.NombreDestinatario);
                    command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                    obj.setId(Convert.ToInt32(command.ExecuteScalar()));
                    // command.ExecuteNonQuery();
                }
                // Insertar los detalles del presupuesto
                connection.Close();
            }
            return obj;
        }
        public bool Remove(int id)
        {
            string queryDeletePresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            string queryDeleteDetalles = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();


                // Eliminar detalles primero
                using (var commandDetalles = new SqliteCommand(queryDeleteDetalles, connection))
                {
                    commandDetalles.Parameters.AddWithValue("@id", id);
                    commandDetalles.ExecuteNonQuery();
                }

                // Luego eliminar el presupuesto
                using (var commandPresupuesto = new SqliteCommand(queryDeletePresupuesto, connection))
                {
                    commandPresupuesto.Parameters.AddWithValue("@id", id);
                    commandPresupuesto.ExecuteNonQuery();
                    return true;
                }
                connection.Close();
            }
            return false;
        }
        public Presupuesto Update(PresupuestosDetalle pd, int id)
        {
            ProductoRepository _prodRepo = new ProductoRepository(); // instancia para buscar producto

            Presupuesto nuevoPresupuesto = new Presupuesto();
            var prodBuscado = _prodRepo.GetById(pd.Producto.IdProducto);

            if (prodBuscado != null)
            {
                // Primero, verificar si ya existe el par (idPresupuesto, idProducto)
                string queryCheckExistence = "SELECT COUNT(*) FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
                bool existe = false;

                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(queryCheckExistence, connection))
                    {
                        command.Parameters.AddWithValue("@idPresupuesto", id);
                        command.Parameters.AddWithValue("@idProducto", pd.Producto.IdProducto);

                        var count = Convert.ToInt32(command.ExecuteScalar());
                        existe = count > 0; // Si existe al menos una tupla, entonces existe = true
                    }
                    connection.Close();
                }

                // Si existe, se hace un UPDATE, si no, se hace un INSERT
                if (existe)
                {
                    string queryUpdateDetalle = "UPDATE PresupuestosDetalle SET Cantidad = @cantidad WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
                    using (var connection = new SqliteConnection(cadenaConexion))
                    {
                        connection.Open();
                        using (var command = new SqliteCommand(queryUpdateDetalle, connection))
                        {
                            command.Parameters.AddWithValue("@idPresupuesto", id);
                            command.Parameters.AddWithValue("@idProducto", pd.Producto.IdProducto);
                            command.Parameters.AddWithValue("@cantidad", pd.Cantidad);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                else
                {
                    string queryInsertDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
                    using (var connection = new SqliteConnection(cadenaConexion))
                    {
                        connection.Open();
                        using (var command = new SqliteCommand(queryInsertDetalle, connection))
                        {
                            command.Parameters.AddWithValue("@idPresupuesto", id);
                            command.Parameters.AddWithValue("@idProducto", pd.Producto.IdProducto);
                            command.Parameters.AddWithValue("@cantidad", pd.Cantidad);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                nuevoPresupuesto = this.GetById(id);
            }
            return nuevoPresupuesto;
        }
    }
}



// Para tu primera entrega, te proponemos que crees un programa que permita emular el registro y almacenamiento de usuarios en una base de datos. Hazlo utilizando el concepto de funciones, diccionarios, bucles y condicionales.
// Objetivos
// Practicar el concepto de funciones.
// Desarrollar la parte lógica para el registro de usuarios.
// Requisitos
// Diccionarios (guardado de datos)
// Input (solicitud de datos)
// Variables
// If (chequeo de datos)
// While (iteración para el programa, sea para agregar, loguear o mostrar)
// For (recorrer datos y para búsqueda)
// Print
// Funciones separadas para registro, almacenamiento y muestra
// Recomendaciones
// El formato de registro es: Nombre de usuario y Contraseña.
// Utilizar una función para almacenar la información y otra función para mostrar la información.
// Utilizar un diccionario para almacenar dicha información, con el par usuario-contraseña (clave-valor).
// Utilizar otra función para el login de usuarios, comprobando que la contraseña coincida con el usuario.
// Formato
// El proyecto debe compartirse utilizando Colab bajo el nombre “ArmaTuLogin+Apellido“, por ejemplo “ArmaTuLogin+Fernandez“