using System;

namespace RetroShop_Client.Model
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public int cantidadDisponible { get; set; }
        public string fechaFabricacion { get; set; }
        public Foto[] fotos { get; set; }
        public int idCategoria { get; set; }
        public int idUsuario { get; set; }
    }
}
