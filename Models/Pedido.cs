using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarenVision.Models
{
    /// <summary>
    /// Entidad que representa un pedido realizado por un cliente
    /// </summary>
    [Table("pedidos")]
    public class Pedido
    {
        /// <summary>
        /// ID único del pedido
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ID del cliente que realizó el pedido
        /// </summary>
        [Required]
        [Column("cliente_id")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Referencia al cliente
        /// </summary>
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; } = null!;

        /// <summary>
        /// Fecha del pedido
        /// </summary>
        [Column("fecha_pedido")]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        /// <summary>
        /// Total del pedido
        /// </summary>
        [Required]
        [Column("total")]
        public decimal Total { get; set; }

        /// <summary>
        /// Estado del pedido (Pendiente, Procesando, Completado, Cancelado)
        /// </summary>
        [Column("estado")]
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";

        /// <summary>
        /// Observaciones adicionales del pedido
        /// </summary>
        [Column("observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Detalles del pedido (productos incluidos)
        /// </summary>
        public virtual ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}