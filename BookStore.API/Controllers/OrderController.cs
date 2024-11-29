using BookStore.API.Base;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Orders")]

    public class OrderController : AppControllerBase
    {
        #region Fields

        private readonly IOrderServices _ordersServices;

        #endregion

        #region Constructors

        public OrderController(IOrderServices ordersServices)
        {
            _ordersServices = ordersServices;
        }

        #endregion

        #region Get All Orders

        [HttpGet]
        [SwaggerOperation(Summary = "احصل على جميع الطلبات ", Description = "example:  http:/localhost/api/orders")]
        [SwaggerResponse(200, "return all orders", typeof(IEnumerable<DisplayAllOrdersDTO>))]

        public async Task<IActionResult> getall()
        {
            var result = await _ordersServices.GetAllOrders();
            return NewResult(result);
        }

        #endregion

        #region Get Order By ID

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "يمكن البحث عن الطلب حسب معرف الطلب ", Description = "example:  http:/localhost/api/order/{id}")]
        [SwaggerResponse(200, "return order data", typeof(DisplayOrderByIdDTO))]
        [SwaggerResponse(404, "if no order founded")]
        //[SwaggerIgnore]
        public async Task<IActionResult> getbyid(int id)
        {
            var result = await _ordersServices.GetOrderByID(id);
            return NewResult(result);
        }
        #endregion

        #region Add New Book

        [HttpPost]
        //[Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "أضف طلب جديد")]
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

        #endregion

        #region Edit Book

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "تعديل بيانات الطلب")]
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

        #endregion

        #region Delete Book

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "حذف الطلب من قاعدة البيانات")]
        [SwaggerResponse(200, "if order deleted succcesfully")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _ordersServices.DeleteOrder(id);
            return NewResult(result);
        }

        #endregion
    }
}
