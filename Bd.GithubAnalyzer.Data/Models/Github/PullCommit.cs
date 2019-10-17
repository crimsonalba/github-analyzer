namespace Bd.GithubAnalyzer.Data.Models.Github
{
	public class PullCommit
	{
		public string label { get; set; }
		public string _ref { get; set; }
		public string sha { get; set; }
		public RepositoryUser user { get; set; }
		public Repository repo { get; set; }
	}

}
