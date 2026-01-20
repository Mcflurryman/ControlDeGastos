using System.ComponentModel.DataAnnotations;

namespace ControlDeGastos.Models;
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }
}
