using Bd.GithubAnalyzer.Repository.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Repository
{
	public class GithubService : BaseService, IGithubService
	{
		private readonly HttpClient HttpClient;

		public GithubService(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		public async Task<Organization> GetOrganization(string organziation)
		{
			var result = await HttpClient.GetAsync($"/orgs/{organziation}");

			if (!IsSuccess(result))
			{
				return null;
			}

			return await DeserializeResult<Organization>(result);
		}

		public async Task<IEnumerable<Models.Repository>> GetRepositories(string organziation)
		{
			var result = await HttpClient.GetAsync($"/orgs/{organziation}/repos");

			if (!IsSuccess(result))
			{
				return null;
			}

			return await DeserializeResult<Models.Repository[]>(result);
		}

		public async Task<IEnumerable<PullRequest>> GetPullsFromRepository(Models.Repository repository, string state = "all", int page = 1)
		{
			// HttpClient already has BaseAddress, so lets strip it out
			var pullsRepoUrl = repository.pulls_url.Replace(HttpClient.BaseAddress.AbsoluteUri, string.Empty);

			// Github recommends you dont parse their URLs, but we need to remove the {/number}, from the URL
			pullsRepoUrl = pullsRepoUrl.Substring(0, pullsRepoUrl.IndexOf("{/"));

			// Lets add back in pagination options
			pullsRepoUrl = $"{pullsRepoUrl}?state={state}&page={page}";

			var result = await HttpClient.GetAsync(pullsRepoUrl);

			if (!IsSuccess(result))
			{
				return null;
			}

			return await DeserializeResult<PullRequest[]>(result);
		}
	}
}
