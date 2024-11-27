using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.Classes
{
    public class Customer : IdentityUser
    {
        public string fullname { get; set; }
        public string address { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
