namespace BookStore.Domain.DTOs.AuthorDTOs
{
    public class EditAuthorDTO
    {
        public string name { get; set; }
        public string bio { get; set; }
        public int age { get; set; }

        public virtual List<EditAuthorBooksDTO> Books { get; set; } = new List<EditAuthorBooksDTO>();
    }
}
