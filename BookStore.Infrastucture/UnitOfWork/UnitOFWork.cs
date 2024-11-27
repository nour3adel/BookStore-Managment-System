using BookStore.Domain.Classes;
using BookStore.Infrastructure.Common;
using BookStore.Infrastructure.Context;

namespace BookStore.Infrastructure.UnitOfWork
{
    public class UnitOFWork
    {
        BookDBContext db;
        GenericRepository<Book> booksRepository;
        GenericRepository<catlog> catlogsRepository;
        GenericRepository<Author> authorssRepository;
        GenericRepository<Order> orderRepository;
        GenericRepository<OrderDetails> orderDEtailsRepository;
        public UnitOFWork(BookDBContext db)
        {
            this.db = db;

        }
        public GenericRepository<Book> BooksRepository
        {
            get
            {
                if (booksRepository == null)
                {
                    booksRepository = new GenericRepository<Book>(db);

                }
                return booksRepository;
            }
        }
        public GenericRepository<catlog> CatlogRepository
        {
            get
            {
                if (catlogsRepository == null)
                {
                    catlogsRepository = new GenericRepository<catlog>(db);

                }
                return catlogsRepository;
            }
        }
        public GenericRepository<Author> AuthorRepository
        {
            get
            {
                if (authorssRepository == null)
                {
                    authorssRepository = new GenericRepository<Author>(db);

                }
                return authorssRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(db);

                }
                return orderRepository;
            }
        }
        public GenericRepository<OrderDetails> OrderDetailsRepository
        {
            get
            {
                if (orderDEtailsRepository == null)
                {
                    orderDEtailsRepository = new GenericRepository<OrderDetails>(db);

                }
                return orderDEtailsRepository;
            }
        }
        public async Task savechanges()
        {
            db.SaveChangesAsync();
        }
    }
}
