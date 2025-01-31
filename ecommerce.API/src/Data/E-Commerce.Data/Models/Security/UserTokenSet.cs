using E_Commerce.Data.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Data.Models.Security
{
    public class UserTokenSet : IdentityUserToken<int>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
