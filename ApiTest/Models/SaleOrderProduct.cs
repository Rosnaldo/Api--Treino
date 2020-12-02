using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTest.Models
{
    public class SaleOrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public Guid ProductId { get; set; }

        [Required]
        [ForeignKey("SaleOrderId")]
        public SaleOrder SaleOrder { get; set; }
        public Guid SaleOrderId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
