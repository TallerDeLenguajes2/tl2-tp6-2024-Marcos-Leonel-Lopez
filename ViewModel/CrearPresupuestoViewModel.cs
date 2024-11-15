public class CrearPresupuestoViewModel
{
    private Presupuesto presupuesto;
    private List<Cliente> clientes;

    public CrearPresupuestoViewModel(List<Cliente> clientes)
    {
        this.presupuesto = new Presupuesto();
        this.clientes = clientes;
    }
    public Presupuesto Presupuesto { get => presupuesto; set => presupuesto = value; }
    public List<Cliente> Clientes { get => clientes;}

}