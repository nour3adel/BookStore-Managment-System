namespace BookStore.Domain.DTOs.AuthorDTOs
{
    public class DisplayAuthorByIdDTO
    {
        public string name { get; set; }
        public string bio { get; set; }
        public int numberOfBooks { get; set; }
        public int age { get; set; }

        public virtual List<DisplayAuthorBooksDTO> AuthorBooks { get; set; } = new List<DisplayAuthorBooksDTO>();
    }
}
