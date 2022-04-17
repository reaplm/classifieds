using Classifieds.Domain.Models;

namespace Classifieds.Repository
{
    public interface IAdvertRepo
    {
        Task<List<Advert>> FindAll();
    }
}
