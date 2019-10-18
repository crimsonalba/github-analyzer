using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data;
using Bd.GithubAnalyzer.Data.Models.Github;
using Bd.GithubAnalyzer.Models.Github;
using Microsoft.Extensions.Logging;

namespace Bd.GithubAnalyzer.Logic
{
	public class OrganizationDetailLogic : IOrganizationDetailLogic
	{
		private readonly IGithubRepository GithubRepository;
		private readonly ILogger<OrganizationDetailLogic> Logger;

		public OrganizationDetailLogic(IGithubRepository githubRepository, ILogger<OrganizationDetailLogic> logger)
		{
			GithubRepository = githubRepository;
			Logger = logger;
		}

		public async Task<OrganizationDetail> GetOrganizationDetail(string organizationId)
		{
			var org = await GithubRepository.GetOrganization(organizationId);
			
			// Org was invalid, so just return null
			if (org == null)
			{
				Logger.LogDebug($"No Github Organization was found for: {organizationId}");
				return null;
			}

			var repos = await GithubRepository.GetRepositories(organizationId);

			var organizationRepos = new List<OrganizationRepository>();
			var organizationPulls = new List<OrganizationPullRequest>();

			foreach (var r in repos)
			{
				var rawPulls = default(IEnumerable<PullRequest>);

				rawPulls = await GithubRepository.GetAllPullsForRepository(r, "all");
				if (rawPulls == null || !rawPulls.Any())
				{
					continue;
				}

				var repoPulls = rawPulls.Select(rp => new OrganizationPullRequest()
				{
					Name = rp.title,
					State = rp.state,
					Creator = rp.user.login,
					CreatedDate = rp.created_at,
					MergedDate = rp.merged_at,
					LastUpdatedDate = rp.updated_at,
				});

				organizationPulls.AddRange(repoPulls);

				organizationRepos.Add(new OrganizationRepository()
				{
					Name = r.full_name,
					Description = r.description,
					GithubUrl = r.html_url,
					LastUpdated = r.updated_at,
					OpenIssuesCount = r.open_issues_count,
					ForksCount = r.forks_count,
					Pulls = repoPulls
				});
			}

			return new OrganizationDetail()
			{
				Name = org.login,
				GithubUrl = org.html_url,
				Repositories = organizationRepos,
				Pulls = organizationPulls,
			};
		}
	}
}
