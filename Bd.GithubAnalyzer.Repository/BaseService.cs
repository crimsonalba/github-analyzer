using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Repository
{
	public abstract class BaseService
	{
		public bool IsSuccess(HttpResponseMessage httpResponse)
		{
			return httpResponse.IsSuccessStatusCode;
		}

		public async Task<TResult> DeserializeResult<TResult>(HttpResponseMessage httpResponse)
		{
			var jsonResult = await httpResponse.Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<TResult>(jsonResult);
		}
	}
}
