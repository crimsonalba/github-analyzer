using Bd.GithubAnalyzer.Models.Github;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Logic
{
	public interface IOrganizationDetailLogic
	{
		/// <summary>
		/// Retrieves details about a Github organization
		/// </summary>
		/// <param name="organization">The name of the organization</param>
		/// <returns><see cref="OrganizationDetail"/></returns>
		Task<OrganizationDetail> GetOrganizationDetail(string organization);
	}
}