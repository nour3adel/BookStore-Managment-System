using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Classes
{
    public class catlog
    {
        public int id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        public string desc { get; set; }
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
