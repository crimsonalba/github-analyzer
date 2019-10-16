using Bd.GithubAnalyzer.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Repository
{
	public interface IGithubService
	{
		/// <summary>
		/// Retrieves an <see cref="Organization"/> from Github
		/// </summary>
		/// <param name="organziation">The organization name</param>
		/// <returns>The organization data</returns>
		Task<Organization> GetOrganization(string organziation);

		Task<IEnumerable<Models.Repository>> GetRepositories(string organziation);

		Task<IEnumerable<PullRequest>> GetPullsFromRepository(Models.Repository repository, string state = "all", int page = 1);
	}
}
