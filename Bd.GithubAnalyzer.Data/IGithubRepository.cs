using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data.Models;
using Bd.GithubAnalyzer.Data.Models.Github;

namespace Bd.GithubAnalyzer.Data
{
	public interface IGithubRepository
	{
		/// <summary>
		/// Retrieves an <see cref="Organization"/> from Github
		/// </summary>
		/// <param name="organziation">The organization name</param>
		/// <returns>The organization data</returns>
		Task<Organization> GetOrganization(string organziation);

		/// <summary>
		/// Retrieves all <see cref="Models.Repository"/> for an organization
		/// </summary>
		/// <param name="organziation">The organization name</param>
		/// <returns>The repository data</returns>
		Task<IEnumerable<Repository>> GetRepositories(string organziation);

		/// <summary>
		/// Retrieves all <see cref="PullRequest"/>s for a repository
		/// automatically iterating through all pages
		/// </summary>
		/// <param name="repository">The <see cref="Repository"/> to find Pull Requests for </param>
		/// <param name="state"> The state of the PR to filter by. Values are "all" (default), "open" or "closed"</param>
		/// <returns>The pull request data</returns>
		Task<IEnumerable<PullRequest>> GetAllPullsForRepository(Repository repository, string state = "all");

		/// <summary>
		/// Retrieves all <see cref="PullRequest"/>s for an organization
		/// automatically iterating through all pages, and repositories
		/// </summary>
		/// <param name="organziation">The organization to find Pull Requests for </param>
		/// <param name="state"> The state of the PR to filter by. Values are "all" (default), "open" or "closed"</param>
		/// <returns>The pull request data</returns>
		Task<IEnumerable<PullRequest>> GetAllPullsForOrganization(string organziation, string state = "all");
	}
}
