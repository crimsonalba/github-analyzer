using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Models.Github
{
	public class OrganizationPullRequest
	{
		public string Name { get; set; }
		public string State { get; set; }
		public bool IsOpen => State == "open";
		public DateTime CreatedDate { get; set; }
		public DateTime LastUpdatedDate { get; set; }
		public DateTime? MergedDate { get; set; }
		public string Creator { get; set; }
	}
}
