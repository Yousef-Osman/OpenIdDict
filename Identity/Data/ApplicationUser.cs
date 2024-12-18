using Microsoft.AspNetCore.Identity;

namespace Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameFr { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
