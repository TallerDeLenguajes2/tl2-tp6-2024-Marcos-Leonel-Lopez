using System.Text.Json.Serialization;

public class Presupuesto
{
    private int idPresupuesto;
    private Cliente destinatario;
    private List<PresupuestosDetalle> detalle;
    public Presupuesto()
    {
    }
    [JsonConstructor]
    public Presupuesto(Cliente destinatario)
    {
        this.destinatario = destinatario;
        this.detalle = new List<PresupuestosDetalle>();
    }
    public Presupuesto(int id, Cliente destinatario, List<PresupuestosDetalle> detalles)
    {
        this.idPresupuesto = id;
        this.destinatario = destinatario;
        this.detalle = detalles;
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value;}
    public Cliente Destinatario { get => destinatario; set => destinatario = value;}
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