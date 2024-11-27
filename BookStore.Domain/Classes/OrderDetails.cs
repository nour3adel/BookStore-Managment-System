using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Classes
{
    public class OrderDetails
    {
        [ForeignKey("order")]
        public int order_id { get; set; }
        [ForeignKey("book")]
        public int book_id { get; set; }

        public int quentity { get; set; }
        [Column(TypeName = "money")]
        public decimal unitprice { get; set; }


        public virtual Order order { get; set; }
        public virtual Book book { get; set; }
    }
}
