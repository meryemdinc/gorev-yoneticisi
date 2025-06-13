using System.ComponentModel.DataAnnotations;

namespace GorevYoneticisiProjesi.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Parola en az 8 karakter olmalıdır.")]
        public string Password { get; set; }= string.Empty;
    }
}
