public class AgregarProductoViewModel
{
    private Presupuesto presupuesto;
    private List<Producto> productos;

    public AgregarProductoViewModel(Presupuesto presupuesto, List<Producto> productos)
    {
        this.presupuesto = presupuesto;
        this.productos = productos;
    }

    public Presupuesto Presupuesto { get => presupuesto; set => presupuesto = value; }
    public List<Producto> Productos { get => productos; }
}