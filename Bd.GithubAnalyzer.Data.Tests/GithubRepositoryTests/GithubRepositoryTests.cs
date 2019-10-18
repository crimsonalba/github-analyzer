using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data.Models.Github;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Bd.GithubAnalyzer.Data.Tests.GithubRepositoryTests
{
	[TestFixture]
	public class GithubRepositoryTests
	{
		private Mock<IOptions<MemoryCacheOptions>> MockMemoryCacheOptions;

		[SetUp]
		public void Setup()
		{
			MockMemoryCacheOptions = new Mock<IOptions<MemoryCacheOptions>>(MockBehavior.Strict);
			MockMemoryCacheOptions.Setup(uu => uu.Value)
				.Returns(new MemoryCacheOptions());
		}

		[TearDown]
		public void Teardown()
		{
			MockMemoryCacheOptions.VerifyAll();
		}

		[Test]
		public async Task GetOrganization_Success()
		{
			var memoryCache = new MemoryCache(MockMemoryCacheOptions.Object);
			var organizationId = "myorg";

			var serviceResult = new Organization()
			{
				login = organizationId
			};

			var mockService = new Mock<IGithubService>(MockBehavior.Strict);
			mockService.Setup(uu => uu.GetOrganization(organizationId))
				.ReturnsAsync(serviceResult);

			var repo = new GithubRepository(memoryCache, mockService.Object);

			var result = await repo.GetOrganization(organizationId);

			Assert.AreEqual(serviceResult, result);

			// Now call again, and the cache should return the existing item
			result = await repo.GetOrganization(organizationId);
			Assert.AreEqual(serviceResult, result);

			// would throw exception if called twice
			mockService.VerifyAll();
		}

		[Test]
		public async Task GetRepositories_Success()
		{
			var memoryCache = new MemoryCache(MockMemoryCacheOptions.Object);
			var organizationId = "myorg";

			var repo1 = new Repository()
			{
				id = 1
			};

			var mockService = new Mock<IGithubService>(MockBehavior.Strict);
			mockService.Setup(uu => uu.GetRepositories(organizationId))
				.ReturnsAsync(new[] { repo1 });

			var repo = new GithubRepository(memoryCache, mockService.Object);

			var result = await repo.GetRepositories(organizationId);

			Assert.IsTrue(result.Count() == 1);

			// Now call again, and the cache should return the existing item
			result = await repo.GetRepositories(organizationId);
			Assert.IsTrue(result.Count() == 1);

			// would throw exception if called twice
			mockService.VerifyAll();
		}

		[Test]
		public async Task GetAllPullsForRepository_Success()
		{
			var memoryCache = new MemoryCache(MockMemoryCacheOptions.Object);
			var repo1 = new Repository()
			{
				id = 1,
				full_name = "repo1"
			};

			var pr1 = new PullRequest()
			{
				id = 10
			};

			var mockService = new Mock<IGithubService>(MockBehavior.Strict);
			mockService.Setup(uu => uu.GetAllPullsFromRepository(repo1, "all"))
				.ReturnsAsync(new[] { pr1 });

			var repo = new GithubRepository(memoryCache, mockService.Object);

			var result = await repo.GetAllPullsForRepository(repo1, "all");

			Assert.IsTrue(result.Count() == 1);

			// Now call again, and the cache should return the existing item
			result = await repo.GetAllPullsForRepository(repo1, "all");
			Assert.IsTrue(result.Count() == 1);

			// would throw exception if called twice
			mockService.VerifyAll();
		}

		[Test]
		public async Task GetAllPullsForOrganization_Success()
		{
			var memoryCache = new MemoryCache(MockMemoryCacheOptions.Object);
			var organizationId = "myorg";

			var repo1 = new Repository()
			{
				id = 1,
				full_name = "repo1"
			};

			var pr1 = new PullRequest()
			{
				id = 10
			};

			var mockService = new Mock<IGithubService>(MockBehavior.Strict);
			mockService.Setup(uu => uu.GetRepositories(organizationId))
				.ReturnsAsync(new[] { repo1 });

			mockService.Setup(uu => uu.GetAllPullsFromRepository(repo1, "all"))
				.ReturnsAsync(new[] { pr1 });

			var repo = new GithubRepository(memoryCache, mockService.Object);

			var result = await repo.GetAllPullsForOrganization(organizationId, "all");

			Assert.IsTrue(result.Count() == 1);

			// Now call again, and the cache should return the existing item
			result = await repo.GetAllPullsForOrganization(organizationId, "all");
			Assert.IsTrue(result.Count() == 1);

			// would throw exception if called twice
			mockService.VerifyAll();
		}
	}
}