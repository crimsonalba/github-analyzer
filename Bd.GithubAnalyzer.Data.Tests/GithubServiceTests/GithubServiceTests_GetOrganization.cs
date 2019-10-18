using System;
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
	public class GithubRepositoryTests
	{
		private const string FakeBaseUrl = "http://api.mock.fake";

		[Test]
		public async Task GetOrganization_Success()
		{ 
			var organizationId = "myorg";
			var organization = new Organization()
			{
				login = organizationId
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

			service.Setup(uu => uu.DeserializeResult<Organization>(httpResponse))
				.ReturnsAsync(organization);

			var result = await service.Object.GetOrganization(organizationId);

			service.VerifyAll();
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(FakeBaseUrl + "/orgs/myorg")
				),
				ItExpr.IsAny<CancellationToken>()
			);

			Assert.AreSame(result, organization);
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

			var result = await service.Object.GetOrganization(organizationId);

			service.VerifyAll();
			mockHttpMessageHandler
				.Protected()
				.Verify(HttpTestHelper.SendAsyncMethod, Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri == new Uri(FakeBaseUrl + "/orgs/myorg")
				),
				ItExpr.IsAny<CancellationToken>()
			);

			Assert.IsNull(result);
		}
	}
}