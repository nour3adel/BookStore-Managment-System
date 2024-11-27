namespace BookStore.Domain.Classes
{
    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
        public string bio { get; set; }
        public int numberOfBooks { get; set; }
        public int age { get; set; }

        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
