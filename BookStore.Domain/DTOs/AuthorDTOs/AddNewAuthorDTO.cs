namespace BookStore.Domain.DTOs.AuthorDTOs
{
    public class AddNewAuthorDTO
    {
        public string name { get; set; }
        public string bio { get; set; }
        public int age { get; set; }
        public IEnumerable<AddAuthorBooksDTO> books { get; set; } = new List<AddAuthorBooksDTO>();
    }
}
