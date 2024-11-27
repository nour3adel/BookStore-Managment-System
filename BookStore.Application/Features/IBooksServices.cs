using BookStore.Application.Bases;
using BookStore.Domain.DTOs.BooksDTOs;

namespace BookStore.Application.Features
{
    public interface IBooksServices
    {
        public Task<Response<IEnumerable<DisplayBookDTO>>> GetAllBooks();
        public Task<Response<DisplayBookDTO>> GetByID(int id);
        public Task<Response<string>> AddNewBook(AddBookDTO addBookDTO);
        public Task<Response<string>> Updatebook(int id, EditBookDTO editBookDTO);
        public Task<Response<string>> Deletebook(int id);

    }
}