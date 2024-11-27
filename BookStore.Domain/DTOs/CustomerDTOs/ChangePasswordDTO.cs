using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.DTOs.CustomerDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string id { get; set; }
        [Required]

        public string oldpassword { get; set; }
        [Required]

        public string newpassword { get; set; }
        [Required]
        [Compare("newpassword", ErrorMessage = "password not match")]
        public string confirm_password { get; set; }
    }
}
