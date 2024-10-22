using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIDemo161024.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}
