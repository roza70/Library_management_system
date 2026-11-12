
using Microsoft.AspNetCore.Identity;

namespace BookGarden.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;

        public string StreetAddress { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
