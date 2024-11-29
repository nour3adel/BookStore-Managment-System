using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AuthorDTOs;
using BookStore.Domain.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Authors")]

    public class AuthorController : AppControllerBase
    {
        #region Fields

        private readonly IAuthorServices _authorservices;

        #endregion

        #region Constructors
        public AuthorController(IAuthorServices authorservices)
        {
            _authorservices = authorservices;
        }

        #endregion

        #region Get All Authors 

        [HttpGet]
        [SwaggerOperation(Summary = "احصل على جميع المؤلفين ", Description = "example:  http:/localhost/api/Authors")]
        [SwaggerResponse(200, "Return all authors", typeof(IEnumerable<DisplayAllOrdersDTO>))]

        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await _authorservices.GetAllauthors();
            return NewResult(result);
        }

        #endregion

        #region Get By Id

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "يمكن البحث عن المؤلف عن طريق معرف المؤلف ", Description = "example:  http:/localhost/api/author/{id}")]
        [SwaggerResponse(200, "return author data", typeof(DisplayOrderByIdDTO))]
        [SwaggerResponse(404, "if no order founded")]
        //[SwaggerIgnore]
        public async Task<IActionResult> getbyid(int id)
        {
            var result = await _authorservices.GetAuthorByID(id);
            return NewResult(result);
        }
        #endregion

        #region Add New Author

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "إضافة مؤلف جديد", Description = "example:  http:/localhost/api/author")]
        [SwaggerResponse(201, "if author created succcesfully")]
        [SwaggerResponse(400, "if invalid author data")]
        public async Task<IActionResult> AddNewAuthor(AddNewAuthorDTO authordto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorservices.AddNewAuthor(authordto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }
        #endregion

        #region Edit Author

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "تعديل بيانات المؤلف", Description = "example:  http:/localhost/api/auther/{id}")]
        [SwaggerResponse(204, "if author updated succcesfully")]
        [SwaggerResponse(400, "ifinvalid author data")]
        public async Task<IActionResult> EditAuthor(int id, EditAuthorDTO authordto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorservices.UpdateAuthor(id, authordto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }
        #endregion

        #region Delete Author

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "حذف المؤلف من قاعدة البيانات", Description = "example: http:/ localhost / api / auther /{id}")]
        [SwaggerResponse(200, "if order author succcesfully")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _authorservices.DeleteAuthor(id);
            return NewResult(result);
        }
        #endregion
    }
}
