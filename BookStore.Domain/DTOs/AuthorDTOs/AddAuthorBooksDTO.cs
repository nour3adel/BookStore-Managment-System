namespace BookStore.Domain.DTOs.AuthorDTOs
{
    public class AddAuthorBooksDTO
    {
        public int quantity { get; set; }
        public string? title { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public DateOnly publishdate { get; set; }
        public int? cat_id { get; set; }

    }
}
