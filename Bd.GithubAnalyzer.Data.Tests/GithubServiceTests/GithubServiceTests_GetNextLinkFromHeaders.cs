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
	public class GithubServiceTests_GetNextLinkFromHeaders
	{
		private GithubService Service;

		[SetUp]
		public void Setup()
		{
			// We dont require an HttpClient for any of these tests
			Service = new GithubService(null);
		}

		[Test]
		public void NoLinkHeaders()
		{
			var response = new HttpResponseMessage();

			var result = Service.GetNextLinkFromHeaders(response);

			Assert.IsNull(result);
		}

		[Test]
		public void LinkHeaderIsEmpty()
		{
			var response = new HttpResponseMessage();
			response.Headers.Add("Link", string.Empty);

			var result = Service.GetNextLinkFromHeaders(response);

			Assert.IsNull(result);
		}

		[Test]
		public void CantParseLinkValue()
		{
			var response = new HttpResponseMessage();
			response.Headers.Add("Link", "garbage header value");

			var result = Service.GetNextLinkFromHeaders(response);

			Assert.IsNull(result);
		}

		[Test]
		public void NoNextLink()
		{
			var response = new HttpResponseMessage();
			response.Headers.Add("Link", "<someurl>; rel=\"nogoodrel\"");

			var result = Service.GetNextLinkFromHeaders(response);

			Assert.IsNull(result);
		}

		[Test]
		public void ParseNextLinkSuccess()
		{
			var response = new HttpResponseMessage();
			response.Headers.Add("Link", "<someurl>; rel=\"next\"");

			var result = Service.GetNextLinkFromHeaders(response);

			Assert.AreEqual(result, "someurl");
		}
	}
}
