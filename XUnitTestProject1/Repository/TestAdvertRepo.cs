using Classifieds.Domain.Data;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Repository
{

    public class TestAdvertRepo : IDisposable
    {
        private ApplicationContext appContext;
        private DbSet<Advert> mockSet;

        public TestAdvertRepo()
        {
            InitContext();
            mockSet = appContext.Set<Advert>();
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void FindByCategory()
        {
            var repo = new AdvertRepo(appContext);
            IEnumerable<Advert> adverts = repo.FindByCategory(6);

            Assert.Equal(3, adverts.Count());
        }
        /// <summary>
        /// Create an advert that has pictures
        /// </summary>
        [Fact]
        public void Create()
        {
            Advert advert = GetAdvert();

            AdvertRepo repo = new AdvertRepo(appContext);
            repo.Create(advert);
            repo.Save();

            IEnumerable<Advert> adverts = repo.FindAll();
            Advert ad = adverts.FirstOrDefault(x => x.ID == 8);



            Assert.Equal(8, adverts.Count());
            Assert.Equal("Black Toyota for sale", ad.Detail.Title);
            Assert.Equal(2, ad.Detail.AdPictures.Count());

        }
        /// <summary>
        /// Test for public int RemoveAllPictures(int id)
        /// </summary>
        [Fact]
        public void RemoveAllPictures()
        {
            var repo = new AdvertRepo(appContext);

            int changedRows = repo.RemoveAllPictures(7);

            Advert advert = repo.Find(7);
            int count = advert.Detail.AdPictures.Count();

            Assert.Equal(2, changedRows);
            Assert.Equal(0, count);

        }
        /// <summary>
        /// Test for public JObject DeleteFromStorage(string uuid)
        /// </summary>
        [Fact]
        public void DeleteFile()
        {
            var repo = new AdvertRepo(appContext);

            JObject result = repo.DeleteFromStorage("57ef964b-6fa4-44f9-b019-f1622ace0587");

            string uuid = result.Property("uuid").Value.ToString();
            string url = result.Property("url").Value.ToString(); 
            int status = (int)result.Property("status").Value;

            Assert.Equal(200, status);
            Assert.Equal("57ef964b-6fa4-44f9-b019-f1622ace0587", uuid );
            Assert.Equal("https://api.uploadcare.com/files/57ef964b-6fa4-44f9-b019-f1622ace0587/", url);
        }
        /// <summary>
        /// Test for public JObject DeleteFromStorage(string uuid)
        /// Test when bad request is returned from the server
        /// </summary>
        [Fact]
        public void DeleteFile_BadRequest()
        {
            var repo = new AdvertRepo(appContext);

            JObject result = repo.DeleteFromStorage("98b9adf1-7019-4236-9d06-0bc7cecdc9e3");

            int status = (int)result.Property("status").Value;

            Assert.Equal(400, status);
        }
        /// <summary>
        /// Test for public JObject DeleteFromStorage(List<string> uuidGroup)
        /// Test when response from the server is successful
        /// </summary>
        [Fact]
        public void DeleteBatch_Successs()
        {
            var repo = new AdvertRepo(appContext);
            List<string> files = new List<string>
            {
                "07fac068-1eb8-4b32-92a4-96bf40d9f9d8",
                "624f9274-f568-484b-8f13-aed558d742b1"
            };

            JObject result = repo.DeleteFromStorage(files);

            string status = result.Property("status").Value.ToString();
            JArray results = (JArray)result.Property("result").Value;

            Assert.Equal("ok", status);
            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count);
        }
        /// <summary>
        /// Test for public JObject DeleteFromStorage(List<string> uuidGroup)
        /// Test when wrong uuid values are supplied
        /// </summary>
        [Fact]
        public void DeleteBatch_ErrorResponse()
        {
            var repo = new AdvertRepo(appContext);
            List<string> files = new List<string>
            {
                "307cbed4-2f11-46ce-a3a4-f1d6fc5c268b",
                "fbc85a6f-baf8-4aad-866e-79c52e017b71"
            };

            JObject result = repo.DeleteFromStorage(files);

            string status = result.Property("status").Value.ToString();
            JObject problems = (JObject)result.Property("problems").Value;
            

            Assert.Equal("ok", status);
            Assert.Equal("Missing in the project", problems.Property(files[0]).Value);
            Assert.Equal("Missing in the project", problems.Property(files[1]).Value);
        }
        /// <summary>
        /// Test data
        /// </summary>
        /// <returns></returns>
        private Advert GetAdvert()
        {
            AdPicture picture1 = new AdPicture
            {
                ID = 1,
                Uuid = "0b83b507-8c11-4c0e-96d2-5fd773d525f7",
                CdnUrl = "https://ucarecdn.com/0b83b507-8c11-4c0e-96d2-5fd773d525f7/",
                Name = "about me sample 3.PNG",
                Size = 135083
            };
            AdPicture picture2 = new AdPicture
            {
                ID = 2,
                Uuid = "c1df9f17-61ad-450a-87f9-d846c312dae0",
                CdnUrl = "https://ucarecdn.com/c1df9f17-61ad-450a-87f9-d846c312dae0/",
                Name = "about me sample 4.PNG",
                Size = 146888
            };
            List<AdPicture> pictures = new List<AdPicture> { picture1, picture2 };

            AdvertDetail advertDetail = new AdvertDetail
            {
                ID = 8,
                Title = "Black Toyota for sale",
                Body = "Black 4x4 Toyota cruiser",
                Email = "pearl@email",
                GroupCdn = "GroupCdnValue",
                GroupCount = 2,
                GroupSize = 2048,
                GroupUuid = "GroupUuidValue",
                Location = "Gaborone",
                AdPictures = pictures
            };

            Advert advert = new Advert
            {
                ID = 8,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                CategoryID = 6,
                Detail = advertDetail
            };

            return advert;
        }
        [Fact]
        public void AdvertCountByStatus()
        {
            var repo = new AdvertRepo(appContext);

            var result = repo.AdvertCountByStatus();
            var expected = new List<CountPercentSummary>
            {
                new CountPercentSummary{Column="APPROVED", Count=2,Percent=28.57},
                new CountPercentSummary{Column="REJECTED", Count=1,Percent=14.29},
                new CountPercentSummary{Column="SUBMITTED", Count=4,Percent=57.14}
            };

            Console.WriteLine("Expected - " + expected[0].Column);
            Console.WriteLine("result - " + result[0].Column);

            Assert.Equal(expected[0].Column, result[0].Column);
            Assert.Equal(expected[1].Column, result[1].Column);
            Assert.Equal(expected[2].Column, result[2].Column);
            Assert.Equal(3, result.Count);

        }
        [Fact]
        public void AdvertCountByLocation()
        {
            var repo = new AdvertRepo(appContext);

            var result = repo.AdvertCountByLocation();
            var expected = new List<CountPercentSummary>
            {
                new CountPercentSummary{Column="Gaborone", Count=3,Percent=42.86},
                new CountPercentSummary{Column="Lobatse", Count=1,Percent=14.29},
                new CountPercentSummary{Column="Mochudi", Count=1,Percent=14.29},
                new CountPercentSummary{Column="Mogoditshane", Count=2,Percent=28.57}
            };

            Assert.Equal(expected[0].Column, result[0].Column);
            Assert.Equal(expected[1].Column, result[1].Column);
            Assert.Equal(expected[2].Column, result[2].Column);
            Assert.Equal(4, result.Count);

        }
        /// <summary>
        /// Initialize
        /// </summary>
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");

            var context = new ApplicationContext(builder.Options);

            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles",Type="HOME"},
                new Menu{ID=2, Name="gardening", Type="HOME"},
                new Menu{ID=3, Name="travel",Type="SIDEBAR"},
                new Menu{ID=4, Name="electronics",Type="SUBMENU"},
                new Menu{ID=5, Name="property",Type="HOME"},
                new Menu{ID=6, Name="cars",Type="SUBMENU",ParentID=1}
            };

            List<AdvertDetail> advertDetails = new List<AdvertDetail>
            {
                new AdvertDetail{ID=1,Title="room for rent", Body="A LARGE ROOM- can be shared by 2 people", Email="my@email.com",AdvertID=1, Location="Gaborone"},
                new AdvertDetail{ID=2,Title="2011 BMW120i", Body="2011 bmw120i Manual gear 150000km 80k", Email="my@email.com",AdvertID=2, Location="Mogoditshane"},
                new AdvertDetail{ID=3,Title="Tyres, Mag Wheels", Body="Your Professional Tyre Fitment Centre", Email="my@email.com", AdvertID=3, Location="Gaborone"},
                new AdvertDetail{ID=4,Title="GOLF POLO GTI MODEL 2013", Body="Full serviced car.Aircon sound system.Price 130000 negotiable", Email="my@email.com", AdvertID=4, Location="Gaborone"},
                new AdvertDetail{ID=5,Title="Handheld Car Vacuum Cleaners", Body="Fine Living Handheld Vacuum Cleaner", Email="my@email.com", AdvertID=5, Location="Lobatse"},
                new AdvertDetail{ID=6,Title="3 bedroom bhc house for Rent", Body="3 bedroom bhc house available in Gaborone ", Email="my@email.com", AdvertID=6, Location="Mogoditshane"},
                new AdvertDetail
                {
                    ID =7,Title="Samsung J1 Ace For Sale",Body="3month Samsung J1 Ace For Sale. P650 ", Email="my@email.com", AdvertID=7,
                    Location="Mochudi",
                    AdPictures = new List<AdPicture>
                    {
                        new AdPicture
                        {
                            ID = 1,
                            Uuid = "0b83b507-8c11-4c0e-96d2-5fd773d525f7",
                            CdnUrl = "https://ucarecdn.com/0b83b507-8c11-4c0e-96d2-5fd773d525f7/",
                            Name = "about me sample 3.PNG",
                            Size = 135083
                        },
                        new AdPicture
                        {
                            ID = 2,
                            Uuid = "c1df9f17-61ad-450a-87f9-d846c312dae0",
                            CdnUrl = "https://ucarecdn.com/c1df9f17-61ad-450a-87f9-d846c312dae0/",
                            Name = "about me sample 4.PNG",
                            Size = 146888
                        }
                    }
                }

            };

            List<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1, Status="SUBMITTED", SubmittedDate= new DateTime(2019,1,3), CategoryID=5},
                new Advert{ID=2, Status="REJECTED", SubmittedDate= new DateTime(2018,10,5), CategoryID=6},
                new Advert{ID=3, Status="APPROVED", SubmittedDate= new DateTime(2018,12,20), CategoryID=6},
                new Advert{ID=4, Status="SUBMITTED", SubmittedDate= new DateTime(2019,3,11), CategoryID=6},
                new Advert{ID=5, Status="APPROVED", SubmittedDate= new DateTime(2018,10,15), CategoryID=4},
                new Advert{ID=6, Status="SUBMITTED", SubmittedDate= new DateTime(2019,2,28), CategoryID=5},
                new Advert{ID=7, Status="SUBMITTED", SubmittedDate= new DateTime(2018,7,3), CategoryID=4, Detail=advertDetails[6]}
            };

            Advert Ad = GetAdvert();
            

            context.AddRange(menus);
            context.AddRange(adverts);
            context.AddRange(advertDetails);
            int changed = context.SaveChanges();
            appContext = context;

        }
        /// <summary>
        /// Setup next test
        /// </summary>
        public void Dispose()
        {
            appContext.Menus.RemoveRange(appContext.Menus);
            appContext.Adverts.RemoveRange(appContext.Adverts);
            appContext.AdvertDetails.RemoveRange(appContext.AdvertDetails);
            int changed = appContext.SaveChanges();
        }
    }
}
