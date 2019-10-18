using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Bd.GithubAnalyzer.Data.Tests
{
	public static class HttpTestHelper
	{
		// The name of the protected method we are going to be mocking
		public const string SendAsyncMethod = "SendAsync";

		public static Mock<HttpMessageHandler> GetMockHttpMessageHandler(HttpResponseMessage response)
		{
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(SendAsyncMethod, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			return mockHttpMessageHandler;
		}
	}
}
