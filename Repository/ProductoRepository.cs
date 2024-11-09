using Microsoft.Data.Sqlite;

namespace ProductRepo
{
    //public class ProductoRepository : IRepository<Producto>
    public class ProductoRepository
    {
        private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
        private int obtenerId(Producto prod)
        {
            var idBuscado = -999;
            string query = "SELECT idProducto FROM Productos WHERE Descripcion = @desc AND Precio = @precio";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@desc", prod.Descripcion));
                command.Parameters.Add(new SqliteParameter("@precio", prod.Precio));
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idBuscado = Convert.ToInt32(reader["idProducto"]);
                    }
                };
                connection.Close();
            }
            return idBuscado;
        }
        private void auxSetId(Producto producto)
        {
            int idCorrespondiente = this.obtenerId(producto);
            if (idCorrespondiente != -999) producto.setId(idCorrespondiente);
        }
        public List<Producto> GetAll()
        {
            List<Producto> productos = new List<Producto>();
            string query = "SELECT * FROM Productos";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"])));
                    }
                };
                connection.Close();
            }
            return productos;
        }
        public Producto GetById(int id)
        {
            Producto producto = null;
            string query = "SELECT * FROM Productos WHERE idProducto = @id";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@id", id));
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                    }
                };
                connection.Close();
            }
            return producto;
        }
        public Producto Create(Producto newProduct)
        {
            Producto producto = newProduct;
            string query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @precio);";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@desc", newProduct.Descripcion));
                    command.Parameters.Add(new SqliteParameter("@precio", newProduct.Precio));
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            this.auxSetId(producto);
            return producto;
        }
        public Producto Update(Producto prod, int id)
        {
            Producto producto = this.GetById(id);
            if (producto == null) return producto;
            string query = "UPDATE Productos SET Descripcion = @desc, Precio = @precio WHERE idProducto = @id;";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@desc", prod.Descripcion));
                    command.Parameters.Add(new SqliteParameter("@precio", prod.Precio));
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            //---
            producto = this.GetById(id);
            this.auxSetId(producto);
            //---
            return producto;
        }
        public bool Delete(int id)
        {
            Producto producto = this.GetById(id);
            if (producto == null) return false;
            string deleteProductoQuery = "DELETE FROM Productos WHERE idProducto = @id;";
            string deletePresupuestosDetalleQuery = "DELETE FROM PresupuestosDetalle WHERE idProducto = @id;";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(deletePresupuestosDetalleQuery, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    command.ExecuteNonQuery();
                }

                using (SqliteCommand command = new SqliteCommand(deleteProductoQuery, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
