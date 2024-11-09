using System.Text.Json.Serialization;

public class PresupuestosDetalle
{
    private Producto producto;
    private int cantidad;


    public PresupuestosDetalle(){

    }
    [JsonConstructor]
    public PresupuestosDetalle(Producto producto,int cantidad){
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Producto Producto { get => producto;}
    public int Cantidad { get => cantidad;}



}