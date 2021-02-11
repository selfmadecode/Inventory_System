using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;

namespace inventoryAppDomain.Repository
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void EditProfile(ApplicationUser user, Pharmacist pharmacist = null, StoreManager storeManager = null)
        {
            if (pharmacist != null)
            {
                pharmacist.ApplicationUser = user;
                pharmacist.ApplicationUserId = user.Id;
                _dbContext.Pharmacists.Add(pharmacist);
            }

            if (storeManager != null)
            {
                storeManager.ApplicationUser = user;
                storeManager.ApplicationUserId = user.Id;
                _dbContext.StoreManagers.Add(storeManager);
            }

            _dbContext.SaveChanges();
        }
    }
}