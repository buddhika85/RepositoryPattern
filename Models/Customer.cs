using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerGuid { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public bool IsPremium { get; set; }
    }
}