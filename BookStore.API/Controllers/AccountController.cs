using BookStore.API.Base;
using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AccountDTOs;
using BookStore.Domain.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Accounts")]

    public class AccountController : AppControllerBase
    {
        #region Fields

        private readonly IAccountServices _services;

        #endregion

        #region Constructor
        public AccountController(IAccountServices services)
        {
            _services = services;
        }

        #endregion

        #region Login

        [HttpPost("Login")]
        [SwaggerOperation(Summary = "تسجيل الدخول والحصول على رمز الوصول ", Description = "<h3> Example:  https://localhost/api/Accounts/login </h3>")]
        [SwaggerResponse(200, "Login Successfuly", typeof(Response<JwtAuthResult>))]

        public async Task<IActionResult> Login(LoginDTO logindata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.Login(logindata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region Change Password

        [Authorize]
        [HttpPost("changepassword")]
        [SwaggerOperation(Summary = "تغيير كلمة المرور", Description = "<h3> Example:  https://localhost/api/Accounts/changepassword </h3>")]
        [SwaggerResponse(200, "Password Changed Successfully", typeof(Response<string>))]

        public async Task<IActionResult> changepassword(ChangePasswordDTO changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.ChangePassword(changePassword);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        #endregion

        #region Logout

        [Authorize]
        [HttpGet("Logout")]
        [SwaggerOperation(Summary = "تسجيل الخروج", Description = "<h3> Example:  https:/slocalhost/api/Accounts/Logout </h3>")]
        [SwaggerResponse(200, "Logged Out Successfully", typeof(Response<string>))]

        public async Task<IActionResult> logout()
        {
            var result = await _services.logout();
            return NewResult(result);
        }

        #endregion
    }
}
