using Microsoft.Data.Sqlite;

namespace ClienteRepo
{
    public class ClienteRepository
    {
        private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
        public List<Cliente> GetAll()
        {
            List<Cliente> result = new List<Cliente>();
            string query = "SELECT idCliente, Nombre, Email, COALESCE(Telefono, '-') as Telefono FROM Clientes";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Cliente(
                            Convert.ToInt32(reader["idCliente"]),
                            reader["Nombre"].ToString(),
                            reader["Email"].ToString(),
                            reader["Telefono"].ToString()
                        ));
                    }
                }
                connection.Close();
            }
            return result;
        }
public Cliente GetById(int idCliente)
{
    Cliente result = null; // Usamos null para indicar que no se encontró el cliente
    string query = "SELECT idCliente, Nombre, Email, COALESCE(Telefono, '-') as Telefono FROM Clientes WHERE idCliente = @id";
    using (var connection = new SqliteConnection(cadenaConexion))
    {
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.Add(new SqliteParameter("@id", idCliente));
        connection.Open();
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.Read()) // Verifica si hay al menos una fila en el resultado
            {
                result = new Cliente(); // Inicializamos el cliente si hay datos
                result.setId(reader.GetInt32(0)); // Asumiendo que idCliente es la primera columna
                result.NombreCliente = reader["Nombre"].ToString();
                result.Email = reader["Email"].ToString();
                result.Telefono = reader["Telefono"].ToString();
            }
        }
        connection.Close();
    }
    return result; // Devolverá null si no se encontró ningún registro
}

        public Cliente Create(Cliente nuevoCliente)
        {
            Cliente cliente = null;
            string query = "INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@nomb, @email, @tel); SELECT last_insert_rowid();";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomb", nuevoCliente.NombreCliente);
                    command.Parameters.AddWithValue("@email", nuevoCliente.Email);
                    // Si Teléfono es null, pasar DBNull.Value a la base de datos
                    command.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(nuevoCliente.Telefono) ? DBNull.Value : nuevoCliente.Telefono);
                    cliente = nuevoCliente;
                    cliente.setId(Convert.ToInt32(command.ExecuteScalar()));
                }
                connection.Close();
            }
            return cliente;
        }

        public bool Remove(int idCliente)
        {
            string query = "DELETE FROM Clientes WHERE idCliente = @id";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", idCliente));
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
        }
        public Cliente Update(Cliente cliente, int idCliente)
        {
            string query = "UPDATE Clientes SET Nombre = @nomb, Email = @email, Telefono = @tel WHERE idCliente = @id";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@nomb", cliente.NombreCliente));
                    command.Parameters.Add(new SqliteParameter("@email", cliente.Email));
                    command.Parameters.Add(new SqliteParameter("@tel", cliente.Telefono));
                    command.Parameters.Add(new SqliteParameter("@id", idCliente));
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    if (rowsAffected > 0)
                    {
                        cliente.setId(idCliente);
                        return cliente;
                    }
                }
                return null;
            }
        }
        public bool Delete(int idCliente)
        {
            string query = "DELETE FROM Clientes WHERE idCliente = @id";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", idCliente));
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
        }
    }
}