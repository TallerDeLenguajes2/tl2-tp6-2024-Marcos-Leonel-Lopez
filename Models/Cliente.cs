using System.Text.Json.Serialization;
public class Cliente
{
    private int idCliente;
    private string nombreCliente;
    private string email;
    private string telefono;
    public Cliente(){
    }
    [JsonConstructor]
    public Cliente(string nombreCliente, string email, string telefono){
        this.nombreCliente = nombreCliente;
        this.email = email;
        this.telefono = telefono;
    }

    public Cliente(int idCliente, string nombreCliente, string email, string telefono){
        this.idCliente = idCliente;
        this.nombreCliente = nombreCliente;
        this.email = email;
        this.telefono = telefono;
    }
    public int IdCliente { get => idCliente; set => idCliente = value; }
    public string NombreCliente { get => nombreCliente; set => nombreCliente = value; }
    public string Email { get => email; set => email = value; }
    public string Telefono { get => telefono; set => telefono = value; }

    public void setId(int idCliente){
        this.idCliente = idCliente;
    }
}