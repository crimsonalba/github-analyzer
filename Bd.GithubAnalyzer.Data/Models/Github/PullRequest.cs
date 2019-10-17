using System;

namespace Bd.GithubAnalyzer.Data.Models.Github
{
	public class PullRequest
	{
		public string url { get; set; }
		public int id { get; set; }
		public string node_id { get; set; }
		public string html_url { get; set; }
		public string diff_url { get; set; }
		public string patch_url { get; set; }
		public string issue_url { get; set; }
		public int number { get; set; }
		public string state { get; set; }
		public bool locked { get; set; }
		public string title { get; set; }
		public RepositoryUser user { get; set; }
		public string body { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public DateTime? closed_at { get; set; }
		public DateTime? merged_at { get; set; }
		public string merge_commit_sha { get; set; }
		public object assignee { get; set; }
		public object[] assignees { get; set; }
		public object[] requested_reviewers { get; set; }
		public object[] requested_teams { get; set; }
		public object[] labels { get; set; }
		public object milestone { get; set; }
		public string commits_url { get; set; }
		public string review_comments_url { get; set; }
		public string review_comment_url { get; set; }
		public string comments_url { get; set; }
		public string statuses_url { get; set; }
		public PullCommit head { get; set; }
		public PullCommit _base { get; set; }
		public Links _links { get; set; }
		public string author_association { get; set; }
	}
}


