
using System.ComponentModel.DataAnnotations;
namespace memoryGameAPI.Models
{
    public class User
    {
        [Required]
        [MinLength(2,ErrorMessage ="name must be minimum 2 characters")]
        [MaxLength(10, ErrorMessage = "name must be maximum 10 characters")]
        public string Name { get; set; }
        [Required]
        [Range(18,120,ErrorMessage ="age must be between 18 and 120")]
        public int Age { get; set; }
        public string PartnerUserName { get; set; }
        public int Score { get; set; }
    }
}