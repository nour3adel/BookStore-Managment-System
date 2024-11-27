using BookStore.Application.Bases;
using BookStore.Domain.DTOs.OrderDTOs;

namespace BookStore.Application.Features
{
    public interface IOrderServices
    {
        public Task<Response<IEnumerable<DisplayAllOrdersDTO>>> GetAllOrders();
        public Task<Response<DisplayOrderByIdDTO>> GetOrderByID(int id);
        public Task<Response<string>> AddNewOrder(AddOrderDTO addBookDTO);
        public Task<Response<string>> UpdateOrder(int id, EditOrderDTO editBookDTO);
        public Task<Response<string>> DeleteOrder(int id);
    }
}
