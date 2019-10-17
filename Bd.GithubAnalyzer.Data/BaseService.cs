using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Data
{
	public abstract class BaseService
	{
		public bool IsSuccess(HttpResponseMessage httpResponse)
		{
			return httpResponse.IsSuccessStatusCode;
		}

		public async Task<TResult> DeserializeResult<TResult>(HttpResponseMessage httpResponse)
		{
			var jsonStream =  await httpResponse.Content.ReadAsStreamAsync();
			return await JsonSerializer.DeserializeAsync<TResult>(jsonStream);
		}
	}
}
