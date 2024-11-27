using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AccountDTOs;
using BookStore.Domain.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Accounts")]

    public class AccountController : AppControllerBase
    {
        private readonly IAccountServices _services;
        public AccountController(IAccountServices services)
        {
            _services = services;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO logindata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.Login(logindata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> changepassword(ChangePasswordDTO changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.ChangePassword(changePassword);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> logout()
        {
            var result = await _services.logout();
            return NewResult(result);
        }
    }
}
