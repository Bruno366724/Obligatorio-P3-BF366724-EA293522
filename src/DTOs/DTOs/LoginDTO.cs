using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs;

public class LoginDTO
{
    [Required]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
}
