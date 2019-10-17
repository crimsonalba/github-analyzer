using Bd.GithubAnalyzer.Data.Models;
using Bd.GithubAnalyzer.Data.Models.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Data
{
	public class GithubService : BaseService, IGithubService
	{
		private readonly Regex LinkHeaderParse = new Regex(@"^<(.*)>; rel=""(.*)""");
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

		public async Task<IEnumerable<Repository>> GetRepositories(string organziation)
		{
			var result = await HttpClient.GetAsync($"/orgs/{organziation}/repos");

			if (!IsSuccess(result))
			{
				return null;
			}

			return await DeserializeResult<Repository[]>(result);
		}

		public async Task<IEnumerable<PullRequest>> GetAllPullsFromRepository(Repository repository, string state = "all")
		{
			// Github recommends you dont parse their URLs, but we need to remove the {/number}, from the URL
			var pullUrl = repository.pulls_url.Substring(0, repository.pulls_url.IndexOf("{/"));

			// if we have state, lets add it as query param
			if (!string.IsNullOrWhiteSpace(state))
			{
				pullUrl = $"{pullUrl}?state={state}";
			}

			var result = await HttpClient.GetAsync(pullUrl);

			if (!IsSuccess(result))
			{
				return null;
			}

			var allPullRequests = new List<PullRequest>();
			var prs = await DeserializeResult<PullRequest[]>(result);

			if (prs != null)
			{
				allPullRequests.AddRange(prs);
			}

			var nextUrl = GetNextLinkFromHeaders(result);
			while (nextUrl != null)
			{
				// get the next result
				result = await HttpClient.GetAsync(nextUrl);
				if (!IsSuccess(result))
				{
					break;
				}

				// deserialize the data
				prs = await DeserializeResult<PullRequest[]>(result);
				if (prs != null)
				{
					allPullRequests.AddRange(prs);
				}

				nextUrl = GetNextLinkFromHeaders(result);
			}

			return allPullRequests;
		}

		private string GetNextLinkFromHeaders(HttpResponseMessage result)
		{
			// If there were multiple pages, we should have a Link header
			if (result.Headers.TryGetValues("Link", out var linkHeaders))
			{
				var headerValue = linkHeaders.FirstOrDefault();
				if (!string.IsNullOrWhiteSpace(headerValue))
				{
					// has form of <url>; rel="next/last/etc", etc
					// split on the comma
					var linkHeaderUrls = headerValue.Split(',').Select(l => l.Trim());
					foreach (var link in linkHeaderUrls)
					{
						var match = LinkHeaderParse.Match(link);
						if (match.Success)
						{
							// 0 index is the full string match,
							// 1 index is the url
							// 2 index is the rel type ("next", "last", etc)
							var url = match.Groups[1].Value;
							var rel = match.Groups[2].Value;

							if (rel == "next")
							{
								return url;
							}
						}
					}
				}
			}

			return null;
		}
	}
}
