using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Customers")]

    public class CustomerController : AppControllerBase
    {
        #region Fields

        private readonly ICustomerServices _services;

        #endregion

        #region Constructors
        public CustomerController(ICustomerServices services)
        {
            _services = services;
        }

        #endregion

        #region Register New Customer

        [HttpPost("Register")]
        [SwaggerOperation(Summary = "تسجيل عميل جديد", Description = "example:  http:/localhost/api/Customers/register")]
        [SwaggerResponse(200, "Register Successfully ", typeof(string))]
        public async Task<IActionResult> Register(RegisterCustomerDTO registerdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.RegisterUser(registerdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        #endregion

        #region Edit Customer

        [HttpPut("EditProfile")]
        [SwaggerOperation(Summary = "تعديل بيانات العميل", Description = "example:  http:/localhost/api/Customers/EditProfile")]
        [SwaggerResponse(200, "Updated Successfully ", typeof(string))]
        public async Task<IActionResult> EditProfile(EditCutomerDTO editdata)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.EditUser(editdata);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region Get All Customers

        [HttpGet("GetAllCustomers")]
        [SwaggerOperation(Summary = "احصل على جميع العملاء", Description = "example:  http:/localhost/api/Customers/GetAllCustomers")]
        [SwaggerResponse(200, "return customer List", typeof(IEnumerable<SelectCustomerDTO>))]
        public async Task<IActionResult> GetAll()
        {
            if (ModelState.IsValid)
            {
                var result = await _services.GetAllUsers();
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        #endregion

        #region Get Customer By ID

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "احصل على العميل عن طريق معرفه", Description = "example:  http:/localhost/api/Customers/{id}")]
        [SwaggerResponse(200, "return customer data", typeof(SelectCustomerDTO))]
        public async Task<IActionResult> GetCustomerByID(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.GetCustomerByID(id);
                return NewResult(result);
            }
            return BadRequest(ModelState);

        }
        #endregion
    }
}
