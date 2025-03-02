using System.ComponentModel.DataAnnotations;

namespace CoinDesk.Models
{
    public class Currency
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}