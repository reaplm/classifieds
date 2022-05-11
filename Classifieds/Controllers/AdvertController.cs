using Classifieds.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdvertController : ControllerBase
	{
		private IAdvertRepo _advertRepo;

		public AdvertController(IAdvertRepo advertRepo)
		{
			_advertRepo = advertRepo;
		}

		[HttpGet]
		public async Task<IActionResult> FindAll()
		{
            try
            {
				return Ok(await _advertRepo.FindAll());
			}
			catch (Exception)
            {
				return StatusCode(StatusCodes.Status500InternalServerError,
					"Error retrieving data from the database");
            }
			
		}
	}
}
