using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.BooksDTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Books")]

    public class BooksController : AppControllerBase
    {
        private readonly IBooksServices _booksServices;

        public BooksController(IBooksServices booksServices)
        {
            _booksServices = booksServices;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "select all books ", Description = "example:  http:/localhost/api/books")]
        [SwaggerResponse(200, "return all books", typeof(IEnumerable<DisplayBookDTO>))]

        public async Task<IActionResult> getall()
        {
            var result = await _booksServices.GetAllBooks();
            return NewResult(result);
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "can earch on book by book id ", Description = "example:  http:/localhost/api/books")]
        [SwaggerResponse(200, "return book data", typeof(DisplayBookDTO))]
        [SwaggerResponse(404, "if no book founded")]
        //[SwaggerIgnore]
        public async Task<IActionResult> getbyid(int id)
        {
            var result = await _booksServices.GetByID(id);
            return NewResult(result);
        }
        [HttpPost]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "add new book")]
        [SwaggerResponse(201, "if book created succcesfully")]
        [SwaggerResponse(400, "ifinvalid book data")]
        public async Task<IActionResult> AddNewBook(AddBookDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                var result = await _booksServices.AddNewBook(bookdto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "edit book data")]
        [SwaggerResponse(204, "if book updated succcesfully")]
        [SwaggerResponse(400, "ifinvalid book data")]
        public async Task<IActionResult> EditBook(int id, EditBookDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                var result = await _booksServices.Updatebook(id, bookdto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }

        [HttpDelete]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "delete book from datatbase")]
        [SwaggerResponse(200, "if book deleted succcesfully")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _booksServices.Deletebook(id);
            return NewResult(result);
        }
    }
}