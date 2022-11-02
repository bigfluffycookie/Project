using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Exercise.Rules;

namespace Exercise
{
    internal interface IAnalyzeManager
    {
       string AnalyzeAndGetResult();
    }

    [Export(typeof(IAnalyzeManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class AnalyzeManager : IAnalyzeManager
    {
        private IEnumerable<IRule> availableRules { get; }

        [ImportingConstructor]
        public AnalyzeManager([Import] IAvailableRulesProvider availableRulesProvider)
        {
            availableRules = availableRulesProvider.AvailableRules;
        }

        public string AnalyzeAndGetResult()
        {
            var configProvider = new ConfigProviderJson();
            var configuration = configProvider.GetConfiguration();
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var result = Analyzer.Analyze(file, availableRules, configuration);

            var formattedResult = FormatResult(result);

            return formattedResult;
        }

        private static string FormatResult(List<IIssue> issues)
        {
            var formattedResult = issues.Count == 0 ? "No Issues" : "";
            foreach (var issue in issues)
            {
                formattedResult += "Line: " + issue.Line + ", ";
                formattedResult += "Column: " + issue.Column + ", ";
                formattedResult += "'" + issue.Text + "'";
                formattedResult += "\n";
            }

            return formattedResult;
        }
    }
}
