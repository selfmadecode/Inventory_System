using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;

namespace inventoryAppDomain.Services
{
    public interface IProfileService
    {
        void EditProfile(ApplicationUser user, Pharmacist pharmacist = null, StoreManager storeManager = null);
        List<ApplicationUser> GetAllUsers();

        Task<ApplicationUser> ChangeUserRole(Tuple<String, String> updateUserRoleViewModel);

        Task RemoveUser(string userId);
    }
}