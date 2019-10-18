using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Data.Models.Github;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Bd.GithubAnalyzer.Data.Tests.GithubServiceTests
{
	[TestFixture]
	public class GithubServiceTests_GetAllPullsFromRepository
	{
		private const string FakeBaseUrl = "http://api.mock.fake";

		[Test]
		public async Task Success_MultiplePages()
		{
			var expectedUrl1 = FakeBaseUrl + "/repos/ramda/ramda-lens/pulls?state=all";
			var expectedUrl2 = FakeBaseUrl + "/repos/ramda/ramda-lens/pulls?state=all&page=2";

			var repository = new Repository()
			{
				name = "repo1",
				pulls_url = FakeBaseUrl + "/repos/ramda/ramda-lens/pulls{/number}",
			};

			var prResponse1 = new PullRequest()
			{
				id = 1
			};
			var prResponse2 = new PullRequest()
			{
				id = 2
			};

			var httpResponse1 = new HttpResponseMessage(HttpStatusCode.OK);
			httpResponse1.Headers.Add("Link", $"<{expectedUrl2}>; rel=\"next\"");

			var httpResponse2 = new HttpResponseMessage(HttpStatusCode.OK);

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(HttpTestHelper.SendAsyncMethod, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri == new Uri(expectedUrl1)), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(httpResponse1);

			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(HttpTestHelper.SendAsyncMethod, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri == new Uri(expectedUrl2)), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(httpResponse2);

			var httpClient = new HttpClient(mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri(FakeBaseUrl),
			};

			var service = new Mock<GithubService>(MockBehavior.Strict, httpClient)
			{
				CallBase = true
			};

			// On first execution
			service.Setup(uu => uu.DeserializeResult<PullRequest[]>(httpResponse1))
				.ReturnsAsync(new[] { prResponse1 });

			// On second execution
			service.Setup(uu => uu.DeserializeResult<PullRequest[]>(httpResponse2))
				.ReturnsAsync(new[] { prResponse2 });

			var result = await service.Object.GetAllPullsFromRepository(repository);

			service.VerifyAll();

			// Verify the first URL was called
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(expectedUrl1)
				),
				ItExpr.IsAny<CancellationToken>()
			);

			// Verify the second URL was called
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(expectedUrl2)
				),
				ItExpr.IsAny<CancellationToken>()
			);

			Assert.IsTrue(result.Count() == 2);
			Assert.IsTrue(result.First().id == 1);
			Assert.IsTrue(result.Last().id == 2);
		}

		[Test]
		public async Task GetOrganization_NotSuccess()
		{
			var organizationId = "myorg";
			var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
			var mockHttpMessageHandler = HttpTestHelper.GetMockHttpMessageHandler(httpResponse);

			var httpClient = new HttpClient(mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri(FakeBaseUrl),
			};

			var service = new Mock<GithubService>(MockBehavior.Strict, httpClient)
			{
				CallBase = true
			};

			var result = await service.Object.GetRepositories(organizationId);

			service.VerifyAll();
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(FakeBaseUrl + "/orgs/myorg/repos")
				),
				ItExpr.IsAny<CancellationToken>()
			);

			Assert.IsNull(result);
		}
	}
}
