using BookStore.Application.Bases;
using BookStore.Domain.DTOs.CustomerDTOs;

namespace BookStore.Application.Features
{
    public interface ICustomerServices
    {
        public Task<Response<string>> RegisterUser(RegisterCustomerDTO user);
        public Task<Response<string>> EditUser(EditCutomerDTO user);
        public Task<Response<IEnumerable<SelectCustomerDTO>>> GetAllUsers();
        public Task<Response<SelectCustomerDTO>> GetCustomerByID(string id);

    }
}
