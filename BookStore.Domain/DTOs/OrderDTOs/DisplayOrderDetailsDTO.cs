namespace BookStore.Domain.DTOs.OrderDTOs
{
    public class DisplayOrderDetailsDTO
    {
        public string BookName { get; set; }
        public int quantity { get; set; }
        public decimal unitprice { get; set; }

    }
}
