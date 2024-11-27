using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.Classes;
using BookStore.Domain.DTOs.BooksDTOs;
using BookStore.Infrastructure.UnitOfWork;

namespace BookStore.Application.Implementations
{
    public class BooksServices : ResponseHandler, IBooksServices
    {
        private readonly UnitOFWork _unit;

        public BooksServices(UnitOFWork _unit)
        {
            this._unit = _unit;
        }
        public async Task<Response<string>> AddNewBook(AddBookDTO addBookDTO)
        {
            if (addBookDTO == null)
            {
                return BadRequest<string>("Invalid book data.");
            }

            // Validate the catalog existence
            var catalog = await _unit.CatlogRepository.GetByIdAsync(addBookDTO.cat_id);
            if (catalog == null)
            {
                return NotFound<string>($"No catalog found with ID = {addBookDTO.cat_id}");
            }
            // Validate the author existence
            var author = await _unit.AuthorRepository.GetByIdAsync(addBookDTO.author_id);
            if (author == null)
            {
                return NotFound<string>($"No Author found with ID = {addBookDTO.author_id}");
            }
            var book = new Book
            {
                title = addBookDTO.title,
                stock = addBookDTO.stock,
                publishdate = addBookDTO.publishdate,
                price = addBookDTO.price,
                author_id = addBookDTO.author_id,
                cat_id = addBookDTO.cat_id,
            };

            await _unit.BooksRepository.AddAsync(book);
            await _unit.savechanges();
            return Created<string>("New book added successfully.");
        }


        public async Task<Response<string>> Deletebook(int id)
        {
            // Retrieve the book by ID
            var book = await _unit.BooksRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound<string>($"No book found with ID = {id}");
            }

            // Delete the book
            await _unit.BooksRepository.DeleteAsync(book);
            await _unit.savechanges();

            return Success<string>("Book deleted successfully.");
        }

        public async Task<Response<IEnumerable<DisplayBookDTO>>> GetAllBooks()
        {
            var books = await _unit.BooksRepository.selectall();
            if (books == null || !books.Any())
            {
                return NotFound<IEnumerable<DisplayBookDTO>>("No books found.");
            }

            var bookDtos = books.Select(book => new DisplayBookDTO
            {
                id = book.id,
                title = book.title,
                stock = book.stock,
                publishdate = book.publishdate,
                price = book.price,
                authorname = book.author.name,
                catalog = book.Catlog.name
            }).ToList();

            return Success<IEnumerable<DisplayBookDTO>>(bookDtos);
        }

        public async Task<Response<DisplayBookDTO>> GetByID(int id)
        {
            var book = await _unit.BooksRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound<DisplayBookDTO>($"No Book With id {id} was found.");
            }
            DisplayBookDTO displayBookDTO = new DisplayBookDTO
            {
                id = book.id,
                title = book.title,
                stock = book.stock,
                publishdate = book.publishdate,
                price = book.price,
                authorname = book.author.name,
                catalog = book.Catlog.name
            };
            return Success(displayBookDTO);

        }

        public async Task<Response<string>> Updatebook(int id, EditBookDTO editBookDTO)
        {
            if (editBookDTO == null)
            {
                return BadRequest<string>("Invalid book data.");
            }

            // Retrieve the existing book
            var existingBook = await _unit.BooksRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound<string>($"No book found with ID = {id}");
            }

            // Update book properties
            existingBook.title = editBookDTO.title;
            existingBook.stock = editBookDTO.stock;
            existingBook.publishdate = editBookDTO.publishdate;
            existingBook.price = editBookDTO.price;
            existingBook.author_id = editBookDTO.author_id;
            existingBook.cat_id = editBookDTO.cat_id;

            await _unit.BooksRepository.UpdateAsync(existingBook);
            await _unit.savechanges();

            return Success<string>("Book updated successfully.");
        }
    }
}
