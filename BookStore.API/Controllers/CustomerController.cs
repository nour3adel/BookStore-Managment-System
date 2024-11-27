using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Customers")]

    public class CustomerController : AppControllerBase
    {
        private readonly ICustomerServices _services;
        public CustomerController(ICustomerServices services)
        {
            _services = services;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCustomerDTO registerdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.RegisterUser(registerdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile(EditCutomerDTO editdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.EditUser(editdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAll()
        {
            if (ModelState.IsValid)
            {
                var result = await _services.GetAllUsers();
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerByID(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.GetCustomerByID(id);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
    }
}
