using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Logic;
using Bd.GithubAnalyzer.Models.Github;
using Bd.GithubAnalyzer.Repository;
using Bd.GithubAnalyzer.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bd.GithubAnalyzer.Controllers
{
	[Route("github")]
	public class GithubController : Controller
	{
		private readonly ILogger<HomeController> Logger;
		private readonly IOrganizationDetailLogic OrganizationDetailLogic;

		public GithubController(ILogger<HomeController> logger, IOrganizationDetailLogic organizationDetailLogc)
		{
			Logger = logger;
			OrganizationDetailLogic = organizationDetailLogc;
		}

		[Route("organization/{organizationId}")]
		public async Task<IActionResult> Organization([FromRoute] string organizationId)
		{
			var details = await OrganizationDetailLogic.GetOrganizationDetail(organizationId);

			if (details == null)
			{
				RedirectToAction("NoResults", new { organizationId });
			}

			return View(details);
		}

		[Route("noresults/{organizationId}")]
		public IActionResult NoResults([FromRoute] string organizationId)
		{
			return View((object)organizationId);
		}
	}
}
