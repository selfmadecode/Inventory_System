using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace inventoryAppDomain.Services
{
    public interface IRoleService
    {
        IdentityRole Create(string roleName);
        List<String> GetAllRoles();
    }
}