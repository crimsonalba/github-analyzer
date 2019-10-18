using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data;
using Bd.GithubAnalyzer.Logic.Analyzers;

namespace Bd.GithubAnalyzer.Logic
{
	public class GithubAnalyzer : IGithubAnalyzer
	{
		private readonly IGithubRepository GithubRepository;

		public GithubAnalyzer(IGithubRepository githubRepository)
		{
			GithubRepository = githubRepository;
		}

		public async Task<IEnumerable<IAnalysis>> GetAnalytics(string organizationId)
		{
			var allPrs = await GithubRepository.GetAllPullsForOrganization(organizationId);

			if (allPrs == null || !allPrs.Any())
			{
				return null;
			}

			var totalCount = new TotalPrCountAnalyzer($" Total PR for organization: {allPrs.Count()}");

			return new[] { totalCount };
		}
	}
}
