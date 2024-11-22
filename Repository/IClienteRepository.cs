namespace IClienteRepo
{
    public interface IClienteRepository{
        List<Cliente> GetAll();
        Cliente GetById(int idCliente);
        Cliente Create(Cliente nuevoCliente);
        bool Delete(int idCliente);
        Cliente Update(Cliente cliente, int idCliente);
        
    }    
}