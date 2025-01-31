using E_Commerce.Data.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Data.Models.Security
{
    public class UserClaimSet : IdentityUserClaim<int>, IBaseEntity
    {
        #region Properties

        public bool IsDeleted { get; set; }
        #endregion]
    }
}
