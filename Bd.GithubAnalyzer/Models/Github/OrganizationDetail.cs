using System.Collections.Generic;

namespace Bd.GithubAnalyzer.Models.Github
{
	public class OrganizationDetail
	{
		public string Name { get; set; }
		public string GithubUrl { get; set; }
		public IEnumerable<OrganizationRepository> Repositories { get; set; }
		public IEnumerable<OrganizationPullRequest> Pulls { get; set; }
	}
}
