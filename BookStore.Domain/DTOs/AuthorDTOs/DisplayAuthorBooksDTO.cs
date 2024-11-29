namespace BookStore.Domain.DTOs.AuthorDTOs
{
    public class DisplayAuthorBooksDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public DateOnly publishdate { get; set; }
        public string CatalogName { get; set; }

    }
}
