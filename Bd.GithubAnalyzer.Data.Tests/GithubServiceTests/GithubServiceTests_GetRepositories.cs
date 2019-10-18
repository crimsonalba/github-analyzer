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
	public class GithubServiceTests_GetRepositories
	{
		private const string FakeBaseUrl = "http://api.mock.fake";

		[Test]
		public async Task GetOrganization_Success()
		{
			var organizationId = "myorg";
			var repositories = new[] {
				new Repository()
				{
					name = "repo1"
				},
				new Repository()
				{
					name = "repo2"
				}
			}; 

			var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
			var mockHttpMessageHandler = HttpTestHelper.GetMockHttpMessageHandler(httpResponse);

			var httpClient = new HttpClient(mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri(FakeBaseUrl),
			};

			var service = new Mock<GithubService>(MockBehavior.Strict, httpClient)
			{
				CallBase = true
			};

			service.Setup(uu => uu.DeserializeResult<Repository[]>(httpResponse))
				.ReturnsAsync(repositories);

			var result = await service.Object.GetRepositories(organizationId);

			service.VerifyAll();
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(FakeBaseUrl + "/orgs/myorg/repos")
				),
				ItExpr.IsAny<CancellationToken>()
			);

			Assert.AreSame(result, repositories);
			Assert.IsTrue(result.Count() == 2);
			Assert.IsTrue(result.First().name == "repo1");
			Assert.IsTrue(result.Last().name == "repo2");
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
