using System.Diagnostics;
using Bd.GithubAnalyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bd.GithubAnalyzer.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> Logger;

		public HomeController(ILogger<HomeController> logger)
		{
			Logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index([FromForm] string organization)
		{
			return RedirectToAction("Organization", "Github", new { organizationId = organization });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
