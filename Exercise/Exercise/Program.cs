using System;
using System.IO;
using System.Collections.Generic;
using Exercise.Rules;

namespace Exercise
{
    public static class Program
    {
        public static string ProgramSetUp()
        {
            var rulesProvider = new AvailableRulesProvider();
            var rules = rulesProvider.GetAvailableRules();

            var configProvider = new ConfigProviderJson();
            var configuration = configProvider.GetConfiguration();
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var result = Analyzer.Analyze(file, rules, configuration);

            var formattedResultReadyResult = FormatResult(result);

            return formattedResultReadyResult;
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