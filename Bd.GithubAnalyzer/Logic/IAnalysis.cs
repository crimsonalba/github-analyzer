namespace Bd.GithubAnalyzer.Logic
{
	/// <summary>
	/// Represents an analytic unit, that can be displayed on the front end
	/// </summary>
	public interface IAnalysis
	{
		/// <summary>
		/// Name of the Analysis
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Result of the Analysis
		/// </summary>
		string Result { get; }
	}
}