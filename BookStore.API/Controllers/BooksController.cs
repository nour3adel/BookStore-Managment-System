using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.BooksDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Books")]

    public class BooksController : AppControllerBase
    {
        #region Fields

        private readonly IBooksServices _booksServices;

        #endregion

        #region Constructor
        public BooksController(IBooksServices booksServices)
        {
            _booksServices = booksServices;
        }
        #endregion

        #region Get All Books

        [HttpGet]
        [SwaggerOperation(Summary = "احصل على جميع الكتب ", Description = "example:  http:/localhost/api/books")]
        [SwaggerResponse(200, "return all books", typeof(IEnumerable<DisplayBookDTO>))]

        public async Task<IActionResult> getall()
        {
            var result = await _booksServices.GetAllBooks();
            return NewResult(result);
        }
        #endregion

        #region Get Book By ID

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "يمكن البحث عن الكتاب عن طريق معرف الكتاب ", Description = "example:  http:/localhost/api/books/{id}")]
        [SwaggerResponse(200, "return book data", typeof(DisplayBookDTO))]
        [SwaggerResponse(404, "if no book founded")]
        //[SwaggerIgnore]
        public async Task<IActionResult> getbyid(int id)
        {
            var result = await _booksServices.GetByID(id);
            return NewResult(result);
        }
        #endregion

        #region Add New Book

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "أضف كتاب جديد")]
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
        #endregion

        #region Edit Book

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "تعديل بيانات الكتاب")]
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
        #endregion

        #region Delete Book

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "حذف الكتاب من قاعدة البيانات")]
        [SwaggerResponse(200, "if book deleted succcesfully")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _booksServices.Deletebook(id);
            return NewResult(result);
        }
        #endregion
    }
}