using BookStore.API.Base;
using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AdminDTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Admins")]

    public class AdminController : AppControllerBase
    {
        #region Fields

        private readonly IAdminServices _services;

        #endregion

        #region Constructors
        public AdminController(IAdminServices services)
        {
            _services = services;
        }

        #endregion

        #region Admin Register

        [HttpPost("Register")]
        [SwaggerOperation(Summary = "تسجيل مشرف جديد ", Description = "<h3> Example:  https://localhost/api/Admins/Register</h3>")]
        [SwaggerResponse(200, "Register Successfuly", typeof(Response<string>))]
        public async Task<IActionResult> Register(RegisterAdminDTO registerdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.RegisterAdmin(registerdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        #endregion

    }
}
