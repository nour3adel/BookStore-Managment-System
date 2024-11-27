using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.DTOs.CustomerDTOs
{
    public class EditCutomerDTO
    {
        [Required]
        public string id { get; set; }
        public string fullname { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }

        public string address { get; set; }
        [Required]
        public string phonenumber { get; set; }
    }
}
