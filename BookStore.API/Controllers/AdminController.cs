using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AdminDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Admins")]

    public class AdminController : AppControllerBase
    {
        private readonly IAdminServices _services;
        public AdminController(IAdminServices services)
        {
            _services = services;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterAdminDTO registerdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.RegisterAdmin(registerdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }

    }
}
