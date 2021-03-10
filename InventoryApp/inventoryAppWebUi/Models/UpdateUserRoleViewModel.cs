using System;
using System.Collections.Generic;

namespace inventoryAppWebUi.Models
{
    public class UpdateUserRoleViewModel
    {
        public string UserId { get; set; }
        public string UpdatedUserRole { get; set; }
        
        public string Email { get; set; }
        public List<String> Roles { get; set; }
    }
}