using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Data.Models.Security
{
    public class UserSet : IdentityUser<int>
    {

        public ICollection<UserRoleSet> UserRoles { get; set; }

    }
}