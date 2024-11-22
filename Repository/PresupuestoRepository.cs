using Microsoft.Data.Sqlite;
using ProductRepo;
using IPresupuestoRepo;

namespace PresupuestoRepo
{
    //public class PresupuestoRepository : IRepository<Presupuesto>
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
        public List<Presupuesto> GetAll()
        {
            List<Presupuesto> presupuestos = new List<Presupuesto>();
            string queryPresupuestos = "SELECT idPresupuesto, idCliente FROM Presupuestos";
            string queryDetalles = @"
                            SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                            FROM PresupuestosDetalle pd
                            INNER JOIN Productos p USING(idProducto)
                            WHERE pd.idPresupuesto = @idPresupuesto";
            string queryCliente = "SELECT idCliente, Nombre, Email, Telefono FROM Clientes WHERE idCliente = @idCliente";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(queryPresupuestos, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            int idCliente = Convert.ToInt32(reader["idCliente"]);

                            // Recuperar el Cliente asociado
                            Cliente cliente = null;
                            using (var clienteCommand = new SqliteCommand(queryCliente, connection))
                            {
                                clienteCommand.Parameters.AddWithValue("@idCliente", idCliente);
                                using (SqliteDataReader clienteReader = clienteCommand.ExecuteReader())
                                {
                                    if (clienteReader.Read())
                                    {
                                        int clienteId = clienteReader.GetInt32(0);
                                        string nombre = clienteReader.GetString(1);
                                        string email = clienteReader.GetString(2);
                                        string telefono = clienteReader.IsDBNull(3) ? null : clienteReader.GetString(3);

                                        cliente = new Cliente(clienteId, nombre, email, telefono);
                                    }
                                }
                            }

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
                            presupuestos.Add(new Presupuesto(idPresupuesto, cliente, detalles));
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
            string queryPresupuestos = "SELECT idPresupuesto, idCliente FROM Presupuestos WHERE idPresupuesto = @id";
            string queryDetalles = @"
                            SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                            FROM PresupuestosDetalle pd
                            INNER JOIN Productos p USING(idProducto)
                            WHERE pd.idPresupuesto = @idPresupuesto";
            string queryCliente = "SELECT idCliente, Nombre, Email, Telefono FROM Clientes WHERE idCliente = @idCliente";

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
                            int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                            int idCliente = Convert.ToInt32(reader["idCliente"]);
                            reader.Close();

                            // Recuperar el Cliente asociado
                            Cliente cliente = null;
                            using (var clienteCommand = new SqliteCommand(queryCliente, connection))
                            {
                                clienteCommand.Parameters.AddWithValue("@idCliente", idCliente);
                                using (SqliteDataReader clienteReader = clienteCommand.ExecuteReader())
                                {
                                    if (clienteReader.Read())
                                    {
                                        int clienteId = clienteReader.GetInt32(0);
                                        string nombre = clienteReader.GetString(1);
                                        string email = clienteReader.GetString(2);
                                        string telefono = clienteReader.IsDBNull(3) ? null : clienteReader.GetString(3);

                                        cliente = new Cliente(clienteId, nombre, email, telefono);
                                    }
                                    clienteReader.Close();
                                }
                            }

                            // Cerrar el lector antes de ejecutar la consulta de detalles

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

                            presupuesto = new Presupuesto(idPresupuesto, cliente, detalles);
                        }
                    }
                }
                connection.Close();
            }
            return presupuesto;
        }
        public Presupuesto Create(Presupuesto nuevoPresupuesto)
        {
            string queryInsertPresupuesto = "INSERT INTO Presupuestos (idCliente, FechaCreacion) VALUES (@idCliente, @fechaCreacion); SELECT last_insert_rowid();";
            //string queryInsertDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad);";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                // Insertar el presupuesto
                using (var command = new SqliteCommand(queryInsertPresupuesto, connection))
                {
                    command.Parameters.AddWithValue("@idCliente", nuevoPresupuesto.Destinatario.IdCliente);
                    command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                    nuevoPresupuesto.setId(Convert.ToInt32(command.ExecuteScalar()));
                    // command.ExecuteNonQuery();
                }
                // Insertar los detalles del presupuesto
                connection.Close();
            }
            return nuevoPresupuesto;
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
        public Presupuesto AddProductoEnPresupuesto(int idPresupuesto, int idProd, int cantidad)
        {
            ProductoRepository _prodRepo = new ProductoRepository(); // instancia para buscar producto

            Presupuesto nuevoPresupuesto = new Presupuesto();
            var prodBuscado = _prodRepo.GetById(idProd);

            if (prodBuscado != null)
            {
                //         // Primero, verificar si ya existe el par (idPresupuesto, idProducto)
                string queryCheckExistence = "SELECT COUNT(*) FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
                bool existe = false;

                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(queryCheckExistence, connection))
                    {
                        command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                        command.Parameters.AddWithValue("@idProducto", idProd);

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
                            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                            command.Parameters.AddWithValue("@idProducto", idProd);
                            command.Parameters.AddWithValue("@cantidad", cantidad);
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
                            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                            command.Parameters.AddWithValue("@idProducto", idProd);
                            command.Parameters.AddWithValue("@cantidad", cantidad);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                nuevoPresupuesto = this.GetById(idPresupuesto);
            }
            return nuevoPresupuesto;
        }
        public bool RemoveProducto(int idPresupuesto, int idProducto)
        {
            string queryDeleteDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryDeleteDetalle, connection))
                {
                    command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                    command.Parameters.AddWithValue("@idProducto", idProducto);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // True si se eliminó el producto
                }
            }
        }
        public bool UpdateCantidadEnDetalle(int idPresupuesto, int idProducto, int nuevaCantidad)
        {
            string queryUpdateCantidad = "UPDATE PresupuestosDetalle SET Cantidad = @nuevaCantidad WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryUpdateCantidad, connection))
                {
                    command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                    command.Parameters.AddWithValue("@idProducto", idProducto);
                    command.Parameters.AddWithValue("@nuevaCantidad", nuevaCantidad);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // True si se modificó la cantidad
                }
            }
        }

    }
}