using System.Threading.Tasks;
using Bd.GithubAnalyzer.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bd.GithubAnalyzer.Controllers
{
	[Route("github")]
	public class GithubController : Controller
	{
		private readonly ILogger<HomeController> Logger;
		private readonly IOrganizationDetailLogic OrganizationDetailLogic;
		private readonly IGithubAnalyzer GithubAnalyzer;

		public GithubController(ILogger<HomeController> logger, IOrganizationDetailLogic organizationDetailLogc, IGithubAnalyzer githubAnalyzer)
		{
			Logger = logger;
			OrganizationDetailLogic = organizationDetailLogc;
			GithubAnalyzer = githubAnalyzer;
		}

		[Route("organization/{organizationId}")]
		public async Task<IActionResult> Organization([FromRoute] string organizationId)
		{
			var details = await OrganizationDetailLogic.GetOrganizationDetail(organizationId);

			if (details == null)
			{
				return RedirectToAction("NoResults", new { organizationId });
			}

			return View(details);
		}

		[Route("organization/{organizationId}/analytics")]
		public async Task<IActionResult> Analytics([FromRoute] string organizationId)
		{
			var analysis = await GithubAnalyzer.GetAnalytics(organizationId);

			if (analysis == null)
			{
				return RedirectToAction("NoResults", new { organizationId });
			}

			return View(analysis);
		}

		[Route("noresults/{organizationId}")]
		public IActionResult NoResults([FromRoute] string organizationId)
		{
			return View((object)organizationId);
		}
	}
}
