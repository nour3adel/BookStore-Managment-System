using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.DTOs.CustomerDTOs
{
    public class RegisterCustomerDTO
    {
        public string fullname { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "invalid pasword")]
        public string password { get; set; }
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string email { get; set; }

        public string address { get; set; }
        [Required]
        public string phonenumber { get; set; }
    }
}
