using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.DTOs.AdminDTOs
{
    public class RegisterAdminDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string email { get; set; }
        [Required]
        public string phonenumber { get; set; }
    }
}
