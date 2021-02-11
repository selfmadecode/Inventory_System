using System.Web;
using System.Web.Mvc;
using inventoryAppDomain.IdentityEntities;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppWebUi.Controllers
{
    public class PharmacistController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _dbContext;

        public PharmacistController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        
        public PharmacistController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }        // GET
    }
}