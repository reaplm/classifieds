using Classifieds.Domain.Models;
using Classifieds.Service.Impl;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Controllers
{
	public class AdvertController : Controller
	{
		private AdvertService _advertService;

		public AdvertController(AdvertService advertService)
		{
			_advertService = advertService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
