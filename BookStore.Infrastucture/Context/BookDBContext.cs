using BookStore.Domain.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookStore.Infrastructure.Context
{
    public class BookDBContext : IdentityDbContext
    {
        public BookDBContext() : base() { }


        public BookDBContext(DbContextOptions<BookDBContext> options)
            : base(options) { }


        public virtual DbSet<catlog> Catlogs { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<OrderDetails>().HasKey("order_id", "book_id");
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "customer", NormalizedName = "CUSTOMER" }
                );
        }
    }
}
