using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bd.GithubAnalyzer.Logic.Analyzers
{
	public class TotalPrCountAnalyzer : IAnalysis
	{
		public TotalPrCountAnalyzer(string result)
		{
			Result = result;
		}

		public string Name => "Total PRs";

		public string Result { get; }
	}
}
