using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace inventoryAppDomain.Repository
{
    public class ProfileService : IProfileService
    {
        public IRoleService RoleService { get; }
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ProfileService(IRoleService roleService)
        {
            RoleService = roleService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public ProfileService(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public void EditProfile(ApplicationUser user, Pharmacist pharmacist = null, StoreManager storeManager = null)
        {
            if (pharmacist != null)
            {
                if (!_dbContext.Pharmacists.Any(pharmacist1 => pharmacist1.UserName.Equals(pharmacist.UserName)))
                {
                    pharmacist.ApplicationUser = user;
                    pharmacist.ApplicationUserId = user.Id;
                    _dbContext.Pharmacists.Add(pharmacist);
                }
                else throw new Exception("Username Already Exists");
            }

            if (storeManager != null)
            {
                if (!_dbContext.StoreManagers.Any(storeManager1 =>
                    storeManager1.UserName.Equals(storeManager.UserName)))
                {
                    storeManager.ApplicationUser = user;
                    storeManager.ApplicationUserId = user.Id;
                    _dbContext.StoreManagers.Add(storeManager);
                }
                else throw new Exception("Username Already Exists");
            }

            _dbContext.SaveChanges();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return UserManager.Users.Where(user => user.Email != "Admin@Admin.com").ToList();
        }

        public async Task<ApplicationUser> ChangeUserRole(MockViewModel updateUserRoleViewModel)
        {
            var user = await ValidateUser(updateUserRoleViewModel.UserId);
            await RoleService.ChangeUserRole(user.Id, updateUserRoleViewModel.UpdatedUserRole);
            
            //create the appropriate profile for him
            
            return user;
        }

        public async Task RemoveUser(string userId)
        {
            //delete corresponding profile pharmacist or store manager

            var user = await ValidateUser(userId);
            var userRole = RoleService.GetRolesByUser(user.Id).FirstOrDefault();

            if (userRole != null && userRole.Equals("Pharmacists"))
            {
                var pharmacist =
                    _dbContext.Pharmacists.FirstOrDefault(pharmacist1 => pharmacist1.ApplicationUserId.Equals(user.Id));
                
                _dbContext.Pharmacists.Remove(pharmacist ?? throw new Exception("Pharmacist Not Found"));
            }
            else
            {
                var storeManager =
                    _dbContext.StoreManagers.FirstOrDefault(manager => manager.ApplicationUserId.Equals(user.Id));

                _dbContext.StoreManagers.Remove(storeManager?? throw new Exception("Store Manager Not Found"));
            }

            await _dbContext.SaveChangesAsync();
            await RoleService.RemoveUserFromRole(user.Id);
            await UserManager.DeleteAsync(user);
        }

        private async Task<ApplicationUser> ValidateUser(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not Found");
            }

            return user;
        }
    }

    public class MockViewModel
    {
        public string UserId { get; set; }
        public string UpdatedUserRole { get; set; }
    }
}