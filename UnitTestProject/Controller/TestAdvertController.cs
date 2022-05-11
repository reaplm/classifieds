using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Controllers;
using Classifieds.Domain.Models;
using Classifieds.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace UnitTestProject.Controller
{
    public class TestAdvertController
    {
		[Fact]
		public void TestFindAll()
		{

			var mockRepo = new Mock<IAdvertRepo>();
			mockRepo.Setup(x => x.FindAll()).Returns(Task.FromResult(GetAdverts()));

			var controller = new AdvertController(mockRepo.Object);
			var task = controller.FindAll();
			var okResult = task.Result as OkObjectResult;
			var adverts = okResult.Value as List<Advert>;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.NotNull(adverts);
			Assert.Equal(7, adverts.Count);
		}
		[Fact]
		public void TestFindAll_ExceptionThrown()
        {
			var mockRepo = new Mock<IAdvertRepo>();
			mockRepo.Setup(x => x.FindAll()).Throws<Exception>();

			var controller = new AdvertController(mockRepo.Object);
			var task = controller.FindAll();
			var exceptionResult = task.Result as ObjectResult;

			Assert.NotNull(exceptionResult);
			Assert.Equal(500, exceptionResult.StatusCode);
			Assert.Equal("Error retrieving data from the database", exceptionResult.Value);
			
		}
		private List<Advert> GetAdverts()
		{
			List<Advert> adverts = new List<Advert>
			{
				new Advert{ID=1, SubmittedDate= new DateTime(2019,1,3)},
				new Advert{ID=2, SubmittedDate= new DateTime(2018,10,5)},
				new Advert{ID=3, SubmittedDate= new DateTime(2018,12,20)},
				new Advert{ID=4, SubmittedDate= new DateTime(2019,3,11)},
				new Advert{ID=5, SubmittedDate= new DateTime(2018,10,15)},
				new Advert{ID=6, SubmittedDate= new DateTime(2019,2,28)},
				new Advert{ID=7, SubmittedDate= new DateTime(2018,7,3)}
			};

			return adverts;
		}
	}
}
