using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bd.GithubAnalyzer.Models;
using Bd.GithubAnalyzer.Repository;

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
		public IActionResult Index([FromBody] string organization)
		{
			return RedirectToAction("Organization", "Github", new { organization });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
