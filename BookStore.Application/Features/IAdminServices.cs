using BookStore.Application.Bases;
using BookStore.Domain.DTOs.AdminDTOs;

namespace BookStore.Application.Features
{
    public interface IAdminServices
    {
        public Task<Response<string>> RegisterAdmin(RegisterAdminDTO admin);
    }
}
