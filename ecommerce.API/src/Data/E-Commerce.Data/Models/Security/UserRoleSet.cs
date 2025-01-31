using E_Commerce.Data.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Data.Models.Security
{
    public class UserRoleSet : IdentityUserRole<int>, IBaseEntity
    {
        #region Properties
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Navigatoin properties

        public UserSet User { get; set; }
        public RoleSet Role { get; set; }


        #endregion
    }
}
