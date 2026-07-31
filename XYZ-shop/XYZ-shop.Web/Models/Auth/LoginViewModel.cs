using System.ComponentModel.DataAnnotations;

namespace XYZ_shop.Web.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
