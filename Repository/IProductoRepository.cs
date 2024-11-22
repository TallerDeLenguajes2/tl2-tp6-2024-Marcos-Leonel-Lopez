namespace IProductoRepo
{
    public interface IProductoRepository
    {
        List<Producto> GetAll();
        Producto GetById(int id);
        Producto Create(Producto newProduct);
        Producto Update(Producto prod, int id);
        bool Delete(int id);
    }
}