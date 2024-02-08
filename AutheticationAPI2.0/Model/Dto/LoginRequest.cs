using System.ComponentModel.DataAnnotations;

namespace AutheticationAPI2._0.Model.Dto
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
