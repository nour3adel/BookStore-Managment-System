using BookStore.Application.Bases;
using BookStore.Domain.DTOs.AccountDTOs;
using BookStore.Domain.DTOs.CustomerDTOs;

namespace BookStore.Application.Features
{
    public interface IAccountServices
    {
        public Task<Response<JwtAuthResult>> Login(LoginDTO logindata);
        public Task<Response<string>> ChangePassword(ChangePasswordDTO pass);
        public Task<Response<string>> logout();
    }
}
