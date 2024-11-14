public class PresupuestoViewModel
{
    private Presupuesto presupuesto;
    private List<Cliente> clientes;

    public Presupuesto Presupuesto { get => presupuesto; set => presupuesto = value; }
    public List<Cliente> Clientes { get => clientes; set => clientes = value; }
}