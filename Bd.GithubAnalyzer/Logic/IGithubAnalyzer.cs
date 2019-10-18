using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Logic
{
	public interface IGithubAnalyzer
	{
		Task<IEnumerable<IAnalysis>> GetAnalytics(string organizationId);
	}
}
