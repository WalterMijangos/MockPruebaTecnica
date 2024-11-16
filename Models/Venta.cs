using System.ComponentModel.DataAnnotations.Schema;

namespace MockPruebaTecnica.Models
{
    [Table("Ventas")]
    public class Venta
    {
        [Column("id_venta")]
        public int VentaId { get; set; }

        [Column("fecha_venta")]
        public DateTime Fecha { get; set; }

        [Column("id_cliente")]
        public int? ClienteId { get; set; }

        [Column("id_producto")]
        public int? ProductoId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("total_venta")]
        public decimal TotalVenta { get; set; }

        public Cliente? Cliente { get; set; } // Relacion de uno a uno

        public Producto? Producto { get; set; } // Relacion de uno a uno

    }
}
