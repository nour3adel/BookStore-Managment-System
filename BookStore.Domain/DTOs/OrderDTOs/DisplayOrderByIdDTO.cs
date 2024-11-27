using BookStore.Domain.Enums;

namespace BookStore.Domain.DTOs.OrderDTOs
{
    public class DisplayOrderByIdDTO
    {
        public int id { get; set; }
        public DateOnly orderdate { get; set; }
        public decimal totalprice { get; set; }
        public OrderStatus status { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<DisplayOrderDetailsDTO> details { get; set; }
    }
}

