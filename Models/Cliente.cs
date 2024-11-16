using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockPruebaTecnica.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Column("id_cliente")]
        public int ClienteID { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido")]
        public string Apellido { get; set; }

        [Column("correo_electronico")]
        public string Correo { get; set;}

        public ICollection<Venta> Ventas { get; set; } // Relacion de uno a muchos

    }
}
