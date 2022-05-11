using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Domain.Models;
using Moq;
using Xunit;
using System.Net.Http.Json;
using System.Threading;
using Classifieds.Service.Impl;
using Moq.Protected;
using Newtonsoft.Json;

namespace UnitTestProject.Service
{
	/// <summary>
	/// Test AdvertService
	/// </summary>
	public class TestAdvertService
	{
		public TestAdvertService()
		{

		}
		//Test FindAll Status OK
		[Fact]
		public void TestFindAll()
		{

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
				true,
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
                {
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(JsonConvert.SerializeObject(GetAdverts()))
					
                })
				.Verifiable();
			//Otherwise you get base address exception
			var client = new HttpClient(mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri("http://test.com")
			};
			
			var advertService = new AdvertService(client);

			var task = advertService.FindAll();
			var adverts = task.Result;

			Assert.NotNull(adverts);
			Assert.Equal(7, adverts.Count());
		}
		//Test FindAll Status 500
		[Fact]
		public void TestFindAllStatus500()
		{

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
				true,
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.InternalServerError

				})
				.Verifiable();
			//Otherwise you get base address exception
			var client = new HttpClient(mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri("http://test.com")
			};
		
			var advertService = new AdvertService(client);
			var task = advertService.FindAll();
			Assert.NotNull(task.Exception.InnerException);
			Assert.ThrowsAsync<HttpRequestException>(() => advertService.FindAll());
		}
		private IEnumerable<Advert> GetAdverts()
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
