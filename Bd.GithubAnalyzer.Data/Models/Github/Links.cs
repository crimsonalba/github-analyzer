namespace Bd.GithubAnalyzer.Data.Models.Github
{
	public class Links
	{
		public SelfLink self { get; set; }
		public HtmlLink html { get; set; }
		public IssueLink issue { get; set; }
		public CommentsLink comments { get; set; }
		public ReviewCommentsLink review_comments { get; set; }
		public ReviewCommentLink review_comment { get; set; }
		public CommitsLink commits { get; set; }
		public StatusesLink statuses { get; set; }
	}
}
