using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.Classes;
using BookStore.Domain.DTOs.AuthorDTOs;
using BookStore.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Application.Implementations
{
    public class AuthorService : ResponseHandler, IAuthorServices
    {
        private readonly UnitOFWork _unit;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorService(UnitOFWork _unit, UserManager<IdentityUser> userManager)
        {
            this._unit = _unit;
            _userManager = userManager;
        }
        public async Task<Response<string>> AddNewAuthor(AddNewAuthorDTO addauthDTO)
        {

            // Validate cat_id for each book
            foreach (var bookDTO in addauthDTO.books)
            {
                if (bookDTO.cat_id.HasValue)
                {
                    var catalog = await _unit.CatlogRepository.GetByIdAsync(bookDTO.cat_id.Value);
                    if (catalog == null)
                    {
                        return BadRequest<string>($"Catalog with ID {bookDTO.cat_id} not found.");
                    }
                }
            }

            // Map DTO to Author entity
            Author author = new Author
            {
                name = addauthDTO.name,
                bio = addauthDTO.bio,
                age = addauthDTO.age,
                Books = addauthDTO.books.Select(bookDTO => new Book
                {
                    title = bookDTO.title,
                    price = bookDTO.price,
                    stock = bookDTO.stock,
                    publishdate = bookDTO.publishdate,
                    cat_id = bookDTO.cat_id
                }).ToList()
            };

            // Add the author to the database
            await _unit.AuthorRepository.AddAsync(author);
            await _unit.savechanges();

            return Created("New Author Created Successfully");
        }

        public async Task<Response<string>> DeleteAuthor(int id)
        {
            // Retrieve the book by ID
            var existingAuthor = await _unit.AuthorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound<string>($"No order found with ID = {id}");
            }
            var books = existingAuthor.Books.ToList();
            foreach (var book in books)
            {
                // Perform any specific logic if needed
                existingAuthor.Books.Remove(book);
            }

            // Alternatively, clear the collection directly if removing all books
            // existingAuthor.Books.Clear();

            // Remove the author
            await _unit.AuthorRepository.DeleteAsync(existingAuthor);
            await _unit.savechanges();

            return Success("author deleted successfully.");
        }

        public async Task<Response<IEnumerable<DisplayAllAuthorsDTO>>> GetAllauthors()
        {
            var authors = await _unit.AuthorRepository.selectall();
            if (authors == null || !authors.Any())
            {
                return NotFound<IEnumerable<DisplayAllAuthorsDTO>>("No Author found.");
            }

            var authorsResult = authors.Select(author => new DisplayAllAuthorsDTO
            {
                id = author.id,
                name = author.name,
                bio = author.bio,
                numberOfBooks = author.Books.Count,
                age = author.age,


            }).ToList();


            return Success<IEnumerable<DisplayAllAuthorsDTO>>(authorsResult);
        }

        public async Task<Response<DisplayAuthorByIdDTO>> GetAuthorByID(int id)
        {
            var author = await _unit.AuthorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound<DisplayAuthorByIdDTO>("No Author found.");
            }

            var authorsResult = new DisplayAuthorByIdDTO
            {
                name = author.name,
                bio = author.bio,
                numberOfBooks = author.Books.Count,
                age = author.age,
                AuthorBooks = author.Books.Select(book => new DisplayAuthorBooksDTO
                {
                    id = book.id,
                    title = book.title,
                    price = book.price,
                    stock = book.stock,
                    publishdate = book.publishdate,
                    CatalogName = book.Catlog.name

                }).ToList()
            };



            return Success(authorsResult);
        }

        public async Task<Response<string>> UpdateAuthor(int id, EditAuthorDTO editAuthorDTO)
        {
            if (editAuthorDTO == null)
            {
                return BadRequest<string>("Invalid author data.");
            }

            // Retrieve the existing author
            var existingAuthor = await _unit.AuthorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound<string>($"No author found with ID = {id}");
            }

            // Update author details


            existingAuthor.name = editAuthorDTO.name;
            existingAuthor.bio = editAuthorDTO.bio;
            existingAuthor.age = editAuthorDTO.age;
            existingAuthor.age = editAuthorDTO.age;

            // Clear existing books
            existingAuthor.Books.Clear();

            // Update books associated with the author
            foreach (var bookDTO in editAuthorDTO.Books)
            {
                // Check if the associated catalog (cat_id) exists
                if (bookDTO.cat_id.HasValue)
                {
                    var catalog = await _unit.CatlogRepository.GetByIdAsync(bookDTO.cat_id.Value);
                    if (catalog == null)
                    {
                        return NotFound<string>($"No catalog found with ID = {bookDTO.cat_id}");
                    }
                }

                // Add new book details to the author's books
                existingAuthor.Books.Add(new Book
                {
                    id = bookDTO.id,
                    title = bookDTO.title,
                    price = bookDTO.price,
                    stock = bookDTO.stock,
                    publishdate = bookDTO.publishdate,
                    cat_id = bookDTO.cat_id
                });
            }


            // Save changes
            await _unit.AuthorRepository.UpdateAsync(existingAuthor);
            await _unit.savechanges();

            return Updated<string>("Author updated successfully.");
        }
    }
}

