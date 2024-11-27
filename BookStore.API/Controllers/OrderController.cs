using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Orders")]

    public class OrderController : AppControllerBase
    {
        private readonly IOrderServices _ordersServices;

        public OrderController(IOrderServices ordersServices)
        {
            _ordersServices = ordersServices;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "select all Orders ", Description = "example:  http:/localhost/api/orders")]
        [SwaggerResponse(200, "return all books", typeof(IEnumerable<DisplayAllOrdersDTO>))]

        public async Task<IActionResult> getall()
        {
            var result = await _ordersServices.GetAllOrders();
            return NewResult(result);
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "can search on order by order id ", Description = "example:  http:/localhost/api/order")]
        [SwaggerResponse(200, "return order data", typeof(DisplayOrderByIdDTO))]
        [SwaggerResponse(404, "if no order founded")]
        //[SwaggerIgnore]
        public async Task<IActionResult> getbyid(int id)
        {
            var result = await _ordersServices.GetOrderByID(id);
            return NewResult(result);
        }
        [HttpPost]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "add new order")]
        [SwaggerResponse(201, "if order created succcesfully")]
        [SwaggerResponse(400, "ifinvalid order data")]
        public async Task<IActionResult> AddNewBook(AddOrderDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                var result = await _ordersServices.AddNewOrder(bookdto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "edit order data")]
        [SwaggerResponse(204, "if order updated succcesfully")]
        [SwaggerResponse(400, "ifinvalid order data")]
        public async Task<IActionResult> EditBook(int id, EditOrderDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                var result = await _ordersServices.UpdateOrder(id, bookdto);
                return NewResult(result);
            }

            return BadRequest(ModelState);

        }

        [HttpDelete]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "delete order from datatbase")]
        [SwaggerResponse(200, "if order deleted succcesfully")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _ordersServices.DeleteOrder(id);
            return NewResult(result);
        }
    }
}
