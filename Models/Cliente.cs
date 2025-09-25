using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarenVision.Models
{
    /// <summary>
    /// Entidad que representa un cliente en la base de datos
    /// </summary>
    [Table("clientes")]
    public class Cliente
    {
        /// <summary>
        /// ID único del cliente
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ID de identificación del cliente (cédula, pasaporte, etc.)
        /// </summary>
        [Required]
        [Column("id_identificacion")]
        [MaxLength(50)]
        public string IdIdentificacion { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del cliente
        /// </summary>
        [Required]
        [Column("nombre_completo")]
        [MaxLength(255)]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Email del cliente (opcional)
        /// </summary>
        [Column("email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Teléfono del cliente (opcional)
        /// </summary>
        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Dirección del cliente (opcional)
        /// </summary>
        [Column("direccion")]
        public string? Direccion { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha de último pedido
        /// </summary>
        [Column("ultimo_pedido")]
        public DateTime? UltimoPedido { get; set; }
    }
}