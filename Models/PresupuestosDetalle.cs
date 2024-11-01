public class PresupuestosDetalle
{
    private Product producto;
    private int cantidad;


    public PresupuestosDetalle(){

    }
    public PresupuestosDetalle(Product producto,int cantidad){
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Product Product { get => producto;}
    public int Cantidad { get => cantidad;}



}