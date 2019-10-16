using System;
using System.Collections.Generic;

namespace Bd.GithubAnalyzer.Models.Github
{
	public class OrganizationRepository
	{
		public string Name { get; set; }
		public string GithubUrl { get; set; }
		public DateTime LastUpdated { get; set; }
		public string Description { get; set; }
		public int ForksCount { get; set; }
		public int OpenIssuesCount { get; set; }
		public IEnumerable<OrganizationPullRequest> Pulls { get; set; }
	}
}
