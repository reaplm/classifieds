using Microsoft.EntityFrameworkCore;
using Classifieds.Domain.Models;

namespace Classifieds.Repository.Impl
{
    public class AdvertRepo : IAdvertRepo
    {
        readonly
        private ApplicationContext context;

        public AdvertRepo(ApplicationContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Fetch all adverts
        /// </summary>
        /// <returns>Task<List<Advert>></returns>
        public async Task<List<Advert>> FindAll()
        {
           return await context.Adverts
                .Include(x => x.Detail).ToListAsync();

            
        }
    }
}
