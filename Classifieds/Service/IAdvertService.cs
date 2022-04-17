using Classifieds.Domain.Models;

namespace Classifieds.Service
{
    public interface IAdvertService {
        Task<IEnumerable<Advert>> FindAll();
    
    }
}
