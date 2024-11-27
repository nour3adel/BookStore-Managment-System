namespace BookStore.Domain.DTOs.BooksDTOs
{
    public class DisplayBookDTO
    {
        public int id { get; set; }

        public string title { get; set; }

        public decimal price { get; set; }
        public int stock { get; set; }

        public DateOnly publishdate { get; set; }

        public string authorname { get; set; }
        public string catalog { get; set; }
    }
}
