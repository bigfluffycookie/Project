using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

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
        public string AnalyzeAndGetResult()
        {
            var configProvider = new ConfigProviderJson();
            var configuration = configProvider.GetConfiguration();
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var rulesProvider = new AvailableRulesProvider();
            var rules = rulesProvider.GetAvailableRules();

            var result = Analyzer.Analyze(file, rules, configuration);

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
