public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private List<PresupuestosDetalle> detalle;
    public Presupuesto()
    {

    }
    public Presupuesto(string destinatario)
    {
        this.nombreDestinatario = destinatario;
        this.detalle = new List<PresupuestosDetalle>();
    }
    public Presupuesto(int id, string destinatario, List<PresupuestosDetalle> detalles)
    {
        this.idPresupuesto = id;
        this.nombreDestinatario = destinatario;
        this.detalle = detalles;
    }

    public int IdPresupuesto { get => idPresupuesto; }
    public string NombreDestinatario { get => nombreDestinatario; }
    public List<PresupuestosDetalle> Detalle { get => detalle; }

    public double MontoPresupuesto()
    {
        double monto = 0;
        foreach (var d in detalle)
        {
            monto += (d.Cantidad * d.Product.Precio);
        }
        return monto;
    }
    public double MontoPresupuestoConIVA()
    {
        double monto = 0;
        foreach (var d in detalle)
        {
            monto += (d.Cantidad * d.Product.Precio);
        }
        return monto * (1 + Constantes.IVA);
    }

    public int CantidadProducts(){
        int sum = 0;
        foreach (var d in detalle)
        {
            sum += d.Cantidad;
        }
        return sum;
    }

}