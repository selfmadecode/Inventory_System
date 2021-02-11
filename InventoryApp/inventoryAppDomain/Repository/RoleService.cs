using System;
using System.Collections.Generic;
using System.Linq;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace inventoryAppDomain.Repository
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            }
            private set 
            { 
                _roleManager = value; 
            }
        }
        
        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                return _userManager ?? new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
            }
            private set 
            { 
                _userManager = value; 
            }
        }
        
        public IdentityRole Create(string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = RoleManager.Create(role);
            
            return result.Succeeded ? role : throw new Exception(result.Errors.ToString());
        }

        public List<string> GetAllRoles()
        {
            return RoleManager.Roles.Select(x => x.Name).ToList();
        }

        public IdentityRole FindByRoleName(string roleName)
        {
            var role = RoleManager.FindByName(roleName);
            return role;
        }

        public List<string> GetRolesByUser(string userId)
        {
            return UserManager.GetRoles(userId).ToList();
        }
    }
}