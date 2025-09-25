using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarenVision.Models
{
    /// <summary>
    /// Entidad que representa el detalle de un pedido (productos incluidos con cantidad)
    /// </summary>
    [Table("detalles_pedido")]
    public class DetallePedido
    {
        /// <summary>
        /// ID único del detalle
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ID del pedido al que pertenece este detalle
        /// </summary>
        [Required]
        [Column("pedido_id")]
        public int PedidoId { get; set; }

        /// <summary>
        /// Referencia al pedido
        /// </summary>
        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; } = null!;

        /// <summary>
        /// ID del producto
        /// </summary>
        [Required]
        [Column("producto_id")]
        public int ProductoId { get; set; }

        /// <summary>
        /// Referencia al producto
        /// </summary>
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } = null!;

        /// <summary>
        /// Cantidad del producto en el pedido
        /// </summary>
        [Required]
        [Column("cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario del producto al momento del pedido
        /// </summary>
        [Required]
        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Subtotal del detalle (cantidad * precio_unitario)
        /// </summary>
        [Column("subtotal")]
        public decimal Subtotal { get; set; }
    }
}