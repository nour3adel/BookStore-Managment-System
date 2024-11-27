using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.DTOs.BooksDTOs
{
    public class AddBookDTO
    {
        public string title { get; set; }

        [Range(20, 1000, ErrorMessage = "invalid price ,price must between 20 and 1000")]
        public decimal price { get; set; }
        [Range(1, 500, ErrorMessage = "inalid stock numer")]
        public int stock { get; set; }

        [Column(TypeName = "date")]
        public DateOnly publishdate { get; set; }

        public int cat_id { get; set; }
        public int author_id { get; set; }
    }
}
