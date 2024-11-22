namespace IPresupuestoRepo
{
    public interface IPresupuestoRepository
    {
        List<Presupuesto> GetAll();
        Presupuesto GetById(int id);
        Presupuesto Create(Presupuesto nuevoPresupuesto);
        bool Remove(int id);
        Presupuesto AddProductoEnPresupuesto(int idPresupuesto, int idProd, int cantidad);
        bool RemoveProducto(int idPresupuesto, int idProducto);
        bool UpdateCantidadEnDetalle(int idPresupuesto, int idProducto, int nuevaCantidad);
    }
}