using BookStore.Domain.Enums;

namespace BookStore.Domain.DTOs.OrderDTOs
{
    public class EditOrderDTO
    {
        public DateOnly orderdate { get; set; }
        public OrderStatus status { get; set; }
        public string cust_id { get; set; }
        public virtual List<EditOrderDetails> OrderDetails { get; set; } = new List<EditOrderDetails>();

    }
}

