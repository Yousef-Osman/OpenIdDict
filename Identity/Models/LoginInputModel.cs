using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class LoginInputModel
{

    [Display(Name = nameof(Username))]
    [Required(ErrorMessage = "Required")]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = nameof(Password))]
    [Required(ErrorMessage = "Required")]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }

    public string ClientUrl { get; set; }
}