using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.Classes;
using BookStore.Domain.DTOs.OrderDTOs;
using BookStore.Domain.Enums;
using BookStore.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Application.Implementations
{
    public class OrderServices : ResponseHandler, IOrderServices
    {
        private readonly UnitOFWork _unit;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderServices(UnitOFWork _unit, UserManager<IdentityUser> userManager)
        {
            this._unit = _unit;
            _userManager = userManager;
        }

        public async Task<Response<string>> AddNewOrder(AddOrderDTO addorderDTO)
        {
            var user = await _userManager.FindByIdAsync(addorderDTO.cust_id);
            if (user == null)
            {
                return NotFound<string>($"No Customer with ID = {addorderDTO.cust_id} was Found");
            }
            Order order = new Order()
            {
                cust_id = addorderDTO.cust_id,
                orderdate = DateOnly.FromDateTime(DateTime.Now),
                status = OrderStatus.Created
            };
            decimal totalprice = 0;
            foreach (var item in addorderDTO.books)
            {
                Book book = await _unit.BooksRepository.GetByIdAsync(item.book_id);
                if (book == null)
                {
                    return BadRequest<string>($"Book with ID {item.book_id} not found.");
                }
                // Check if the stock is sufficient
                if (book.stock < item.quentity)
                {
                    return BadRequest<string>($"Insufficient stock for Book {book.title}. Requested: {item.quentity}, Available: {book.stock}.");
                }

                OrderDetails _details = new OrderDetails()
                {
                    order_id = order.id,
                    book_id = item.book_id,
                    quentity = item.quentity,
                    unitprice = book.price,
                };
                totalprice = totalprice + (book.price * item.quentity);
                order.OrderDetails.Add(_details);


                book.stock -= item.quentity;
                await _unit.BooksRepository.UpdateAsync(book);

            }
            order.totalprice = totalprice;
            await _unit.OrderRepository.AddAsync(order);
            await _unit.savechanges();
            return Created<string>("New Order Created Successfully");
        }
        public async Task<Response<string>> DeleteOrder(int id)
        {
            // Retrieve the book by ID
            var order = await _unit.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound<string>($"No order found with ID = {id}");
            }

            // Delete the book
            await _unit.OrderRepository.DeleteAsync(order);
            await _unit.savechanges();

            return Success<string>("Order deleted successfully.");
        }

        public async Task<Response<IEnumerable<DisplayAllOrdersDTO>>> GetAllOrders()
        {
            var orders = await _unit.OrderRepository.selectall();
            if (orders == null || !orders.Any())
            {
                return NotFound<IEnumerable<DisplayAllOrdersDTO>>("No order found.");
            }

            var orderResult = orders.Select(order => new DisplayAllOrdersDTO
            {
                id = order.id,
                CustomerName = order.customer.fullname,
                orderdate = order.orderdate,
                totalprice = order.totalprice,
                status = order.status,
                NumberofItems = order.OrderDetails.Count(),

            }).ToList();


            return Success<IEnumerable<DisplayAllOrdersDTO>>(orderResult);
        }

        public async Task<Response<DisplayOrderByIdDTO>> GetOrderByID(int id)
        {
            var order = await _unit.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound<DisplayOrderByIdDTO>($"No Order With id {id} was found.");
            }
            DisplayOrderByIdDTO displayOrderDTO = new DisplayOrderByIdDTO
            {
                id = order.id,
                orderdate = order.orderdate,
                totalprice = order.totalprice,
                status = order.status,
                CustomerName = order.customer.fullname,
                details = order.OrderDetails.Select(detail => new DisplayOrderDetailsDTO
                {
                    BookName = detail.book.title,
                    quantity = detail.quentity,
                    unitprice = detail.unitprice

                })
            };
            return Success(displayOrderDTO);
        }


        public async Task<Response<string>> UpdateOrder(int id, EditOrderDTO editOrder)
        {
            if (editOrder == null)
            {
                return BadRequest<string>("Invalid order data.");
            }

            // Retrieve the existing order
            var existingOrder = await _unit.OrderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound<string>($"No order found with ID = {id}");
            }
            // Check if the customer exists
            var customer = await _userManager.FindByIdAsync(editOrder.cust_id);
            if (customer == null)
            {
                return NotFound<string>($"No customer found with ID = {editOrder.cust_id}");
            }
            // Clear existing order details
            existingOrder.OrderDetails.Clear();


            decimal newTotalPrice = 0;

            // Update order details
            foreach (var detail in editOrder.OrderDetails)
            {
                var book = await _unit.BooksRepository.GetByIdAsync(detail.book_id);
                if (book == null)
                {
                    return NotFound<string>($"No book found with ID = {detail.book_id}");
                }

                // Add new order details
                OrderDetails n = new OrderDetails
                {
                    book_id = detail.book_id,
                    quentity = detail.quentity,
                    unitprice = detail.unitprice
                };
                existingOrder.OrderDetails.Add(n);

                // Calculate the total price for the order
                newTotalPrice += n.quentity * n.unitprice;
            }

            // Update order properties
            existingOrder.orderdate = editOrder.orderdate;
            existingOrder.status = editOrder.status;
            existingOrder.cust_id = editOrder.cust_id;
            existingOrder.totalprice = newTotalPrice;

            // Save changes
            await _unit.OrderRepository.UpdateAsync(existingOrder);
            await _unit.savechanges();

            return Updated<string>("Order updated successfully.");
        }

    }
}
