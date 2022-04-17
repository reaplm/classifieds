using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Domain.Models;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTestProject.Repository
{
	public class TestAdvertRepository
	{

        private ApplicationContext appContext;
        private DbSet<Advert> mockSet;

        public TestAdvertRepository()
        {
            InitContext();
            mockSet = appContext.Set<Advert>();
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void FindAll()
        {
            var repo = new AdvertRepo(appContext);
            Task<List<Advert>> task = repo.FindAll();
            List<Advert> result = task.Result;

            Assert.Equal(7, result.Count());
            Assert.NotNull(result.ElementAt(0).Detail);
            Assert.NotNull(result.ElementAt(1).Detail);
            Assert.NotNull(result.ElementAt(2).Detail);
        }
        /// <summary>
        /// Initialize
        /// </summary>
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");

            var context = new ApplicationContext(builder.Options);



            List<AdvertDetail> advertDetails = new List<AdvertDetail>
            {
                new AdvertDetail{ID=1,Title="room for rent", Body="A LARGE ROOM- can be shared by 2 people", Email="my@email.com",Location="Gaborone"},
                new AdvertDetail{ID=2,Title="2011 BMW120i", Body="2011 bmw120i Manual gear 150000km 80k", Email="my@email.com", Location="Mogoditshane"},
                new AdvertDetail{ID=3,Title="Tyres, Mag Wheels", Body="Your Professional Tyre Fitment Centre", Email="my@email.com",  Location="Gaborone"},
                new AdvertDetail{ID=4,Title="GOLF POLO GTI MODEL 2013", Body="Full serviced car.Aircon sound system.Price 130000 negotiable", Email="my@email.com", Location="Gaborone"},
                new AdvertDetail{ID=5,Title="Handheld Car Vacuum Cleaners", Body="Fine Living Handheld Vacuum Cleaner", Email="my@email.com", Location="Lobatse"},
                new AdvertDetail{ID=6,Title="3 bedroom bhc house for Rent", Body="3 bedroom bhc house available in Gaborone ", Email="my@email.com", Location="Mogoditshane"},
                new AdvertDetail{ID =7,Title="Samsung J1 Ace For Sale",Body="3month Samsung J1 Ace For Sale. P650 ", Email="my@email.com", Location="Mochudi" } 
            };
            List<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1, SubmittedDate= new DateTime(2019,1,3), Detail=advertDetails[0]},
                new Advert{ID=2, SubmittedDate= new DateTime(2018,10,5), Detail=advertDetails[1]},
                new Advert{ID=3, SubmittedDate= new DateTime(2018,12,20), Detail=advertDetails[2]},
                new Advert{ID=4, SubmittedDate= new DateTime(2019,3,11), Detail=advertDetails[3]},
                new Advert{ID=5, SubmittedDate= new DateTime(2018,10,15), Detail=advertDetails[4]},
                new Advert{ID=6, SubmittedDate= new DateTime(2019,2,28), Detail=advertDetails[5]},
                new Advert{ID=7, SubmittedDate= new DateTime(2018,7,3), Detail=advertDetails[6]}
            };

            context.AddRange(adverts);
            context.AddRange(advertDetails);
            int changed = context.SaveChanges();
            appContext = context;

        }
    }
}
