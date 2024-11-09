using System.Text.Json.Serialization;

public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private List<PresupuestosDetalle> detalle;
    public Presupuesto()
    {
    }
    [JsonConstructor]
    public Presupuesto(string NombreDestinatario)
    {
        this.nombreDestinatario = NombreDestinatario;
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
            monto += (d.Cantidad * d.Producto.Precio);
        }
        return monto;
    }
    public double MontoPresupuestoConIVA()
    {
        double monto = 0;
        foreach (var d in detalle)
        {
            monto += (d.Cantidad * d.Producto.Precio);
        }
        return monto * (1 + Constantes.IVA);
    }

    public int CantidadProducts()
    {
        int sum = 0;
        foreach (var d in detalle)
        {
            sum += d.Cantidad;
        }
        return sum;
    }
        public void setId(int id){
        this.idPresupuesto = id;
    }
}