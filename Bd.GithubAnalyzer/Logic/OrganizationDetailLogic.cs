using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Models.Github;
using Bd.GithubAnalyzer.Repository;
using Bd.GithubAnalyzer.Repository.Models;
using Microsoft.Extensions.Logging;

namespace Bd.GithubAnalyzer.Logic
{
	public class OrganizationDetailLogic : IOrganizationDetailLogic
	{
		private readonly IGithubService GithubService;
		private readonly ILogger<OrganizationDetailLogic> Logger;

		public OrganizationDetailLogic(IGithubService githubService, ILogger<OrganizationDetailLogic> logger)
		{
			GithubService = githubService;
			Logger = logger;
		}

		public async Task<OrganizationDetail> GetOrganizationDetail(string organizationId)
		{
			var org = await GithubService.GetOrganization(organizationId);
			
			// Org was invalid, so just return null
			if (org == null)
			{
				return null;
			}

			var repos = await GithubService.GetRepositories(organizationId);

			var organizationRepos = new List<OrganizationRepository>();
			var organizationPulls = new List<OrganizationPullRequest>();

			foreach (var r in repos)
			{
				var allRawPulls = new List<PullRequest>();
				var rawPulls = default(IEnumerable<PullRequest>);

				// lets get _all_ PRs
				var page = 1;

				// while we still get results, keep getting them
				while (true)
				{
					rawPulls = await GithubService.GetPullsFromRepository(r, "all", page++);
					if (rawPulls == null || !rawPulls.Any())
					{
						break;
					}

					allRawPulls.AddRange(rawPulls);
				}

				var repoPulls = allRawPulls.Select(rp => new OrganizationPullRequest()
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
