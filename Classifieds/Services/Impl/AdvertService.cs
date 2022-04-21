using Classifieds.Domain.Models;
using Classifieds.Repository;


namespace Classifieds.Service.Impl
{
    /// <summary>
    /// Advert Service
    /// </summary>
    public class AdvertService : IAdvertService
    {
        readonly
        private IAdvertRepo advertRepo;

        public AdvertService(IAdvertRepo advertRepo) 
        {
            this.advertRepo = advertRepo;
        }
        public async Task<IEnumerable<Advert>> FindAll()
        {
            return await advertRepo.FindAll();
        }
    }
}
