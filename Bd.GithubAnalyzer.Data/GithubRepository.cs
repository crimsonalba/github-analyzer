using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data.Models;
using Bd.GithubAnalyzer.Data.Models.Github;
using Microsoft.Extensions.Caching.Memory;

namespace Bd.GithubAnalyzer.Data
{

	/// <summary>
	///		A repository layer on top of the <see cref="IGithubService"/>
	///		that caches responses from the service for further use
	/// </summary>
	public class GithubRepository : IGithubRepository
	{
		private readonly int SlidingCacheExpirationMinutes = 30;
		private readonly IMemoryCache MemoryCache;
		private readonly IGithubService GithubService;

		public GithubRepository(IMemoryCache memoryCache, IGithubService githubService)
		{
			MemoryCache = memoryCache;
			GithubService = githubService;
		}

		public async Task<Organization> GetOrganization(string organization)
		{
			var cacheKey = $"GetOrganization-{nameof(organization)}:{organization}";

			return await MemoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
			{
				var result = await GithubService.GetOrganization(organization);
				SetCacheLimit(entry, result);
				return result;
			});
		}

		public async Task<IEnumerable<Repository>> GetRepositories(string organization)
		{
			var cacheKey = $"GetRepositories-{nameof(organization)}:{organization}";

			return await MemoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
			{
				var result = await GithubService.GetRepositories(organization);
				SetCacheLimit(entry, result);
				return result;
			});
		}

		public async Task<IEnumerable<PullRequest>> GetAllPullsForRepository(Repository repository, string state = "all")
		{
			var cacheKey = $"GetAllPullsForRepository-{nameof(repository)}:{repository.full_name}-{nameof(state)}:{state}";

			return await MemoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
			{
				var result = await GithubService.GetAllPullsFromRepository(repository, state);
				SetCacheLimit(entry, result);
				return result;
			});
		}

		public async Task<IEnumerable<PullRequest>> GetAllPullsForOrganization(string organization, string state = "all")
		{
			var cacheKey = $"GetAllPullsForOrganization-{nameof(organization)}:{organization}-{nameof(state)}:{state}";
			return await MemoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
			{
				var result = new List<PullRequest>();

				var repositories = await GetRepositories(organization);
				if (repositories != null)
				{
					foreach (var repo in repositories)
					{
						var repoPrs = await GetAllPullsForRepository(repo);
						if (repoPrs != null)
						{
							result.AddRange(repoPrs);
						}
					}
				}

				// if we dont have PRs, just set our result to null, so we wont cache anything
				result = result.Count == 0 ? null : result; 

				SetCacheLimit(entry, result);
				return result;
			});
		}

		private void SetCacheLimit(ICacheEntry cacheEntry, object result)
		{
			if (result == null)
			{
				// if the result is null, lets not _actually_ cache it.
				cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(1));
				return;
			}

			cacheEntry.SetSlidingExpiration(TimeSpan.FromMinutes(SlidingCacheExpirationMinutes));
		}
	}
}
