using System.ComponentModel.DataAnnotations.Schema;

namespace MockPruebaTecnica.Models
{
	[Table("Productos")]
	public class Producto
    {
        [Column("id_producto")]
        public int Id { get; set; }

        [Column("codigo_barras")]
        public string CodigoBarras { get; set; }

        [Column("nombre_producto")]
        public string NombreProducto { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("categoria")]
        public string Categoria { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }
    }
}
