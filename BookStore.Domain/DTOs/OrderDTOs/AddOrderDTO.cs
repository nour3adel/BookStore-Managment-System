namespace BookStore.Domain.DTOs.OrderDTOs
{
    public class AddOrderDTO
    {
        public string cust_id { get; set; }
        public IEnumerable<AddDetailsDTO> books { get; set; } = new List<AddDetailsDTO>();
    }
}
