using BookStore.Application.Bases;
using BookStore.Domain.DTOs.AuthorDTOs;

namespace BookStore.Application.Features
{
    public interface IAuthorServices
    {
        public Task<Response<IEnumerable<DisplayAllAuthorsDTO>>> GetAllauthors();
        public Task<Response<DisplayAuthorByIdDTO>> GetAuthorByID(int id);
        public Task<Response<string>> AddNewAuthor(AddNewAuthorDTO addBookDTO);
        public Task<Response<string>> UpdateAuthor(int id, EditAuthorDTO editBookDTO);
        public Task<Response<string>> DeleteAuthor(int id);
    }
}
