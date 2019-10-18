using System.Collections.Generic;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data.Models.Github;

namespace Bd.GithubAnalyzer.Data
{
	public interface IGithubService
	{
		/// <summary>
		/// Retrieves an <see cref="Organization"/> from Github
		/// </summary>
		/// <param name="organization">The organization name</param>
		/// <returns>The organization data</returns>
		Task<Organization> GetOrganization(string organization);

		/// <summary>
		/// Retrieves all <see cref="Models.Repository"/> for an organization
		/// </summary>
		/// <param name="organization">The organization name</param>
		/// <returns>The repository data</returns>
		Task<IEnumerable<Repository>> GetRepositories(string organization);

		/// <summary>
		/// Retrieves all <see cref="PullRequest"/>s for a repository
		/// automatically iterating through all pages
		/// </summary>
		/// <param name="repository">The <see cref="Repository"/> to find Pull REquests for </param>
		/// <param name="state"> The state of the PR to filter by. Values are "all" (default), "open" or "closed"</param>
		/// <returns>The pull request data</returns>
		Task<IEnumerable<PullRequest>> GetAllPullsFromRepository(Repository repository, string state = "all");
	}
}
