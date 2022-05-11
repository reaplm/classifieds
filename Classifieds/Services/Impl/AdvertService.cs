using Classifieds.Domain.Models;
using Classifieds.Repository;
using Newtonsoft.Json;

namespace Classifieds.Service.Impl
{
    /// <summary>
    /// Advert Service
    /// </summary>
    public class AdvertService : IAdvertService
    {
        readonly
        private HttpClient _httpClient;

        public AdvertService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Advert>> FindAll()
        {
            
            var response = await _httpClient.GetAsync("api/Advert");
            response.EnsureSuccessStatusCode();

            Task<String> jsonString = response.Content.ReadAsStringAsync();
            var adverts = JsonConvert.DeserializeObject<List<Advert>>(jsonString.Result);

            return adverts;
             
        }
    }
}
