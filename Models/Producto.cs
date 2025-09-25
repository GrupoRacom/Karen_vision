using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarenVision.Models
{
    /// <summary>
    /// Entidad que representa un producto o servicio en la base de datos
    /// </summary>
    [Table("productos")]
    public class Producto
    {
        /// <summary>
        /// ID único del producto
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del producto o servicio
        /// </summary>
        [Required]
        [Column("nombre")]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto
        /// </summary>
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Precio del producto
        /// </summary>
        [Required]
        [Column("precio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        /// <summary>
        /// Stock disponible del producto
        /// </summary>
        [Column("stock")]
        public int Stock { get; set; }

        /// <summary>
        /// URL de la imagen del producto (opcional)
        /// </summary>
        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        /// <summary>
        /// Indica si el producto está activo/disponible
        /// </summary>
        [Column("activo")]
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}