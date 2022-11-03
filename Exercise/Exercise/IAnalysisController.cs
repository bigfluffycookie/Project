using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Exercise.Rules;

namespace Exercise
{
    internal interface IAnalysisController
    {
       void AnalyzeAndGetResult();
    }

    [Export(typeof(IAnalysisController))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class AnalysisController : IAnalysisController
    {
        private readonly ILogger logger;

        private readonly ImmutableArray<IRule> availableRules;

        [ImportingConstructor]
        public AnalysisController(ILogger logger, IAvailableRulesProvider availableRulesProvider)
        {
            this.logger = logger;
            availableRules = availableRulesProvider.AvailableRules;
        }

        public void AnalyzeAndGetResult()
        {
            var configProvider = new ConfigProviderJson();
            var configuration = configProvider.GetConfiguration();
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var result = Analyzer.Analyze(file, availableRules, configuration);

            FormatAndLogResult(result);
        }

        private void FormatAndLogResult(List<IIssue> issues)
        {
            var formattedResult = issues.Count == 0 ? "No Issues" : "";
            foreach (var issue in issues)
            {
                formattedResult += "Line: " + issue.Line + ", ";
                formattedResult += "Column: " + issue.Column + ", ";
                formattedResult += "'" + issue.Text + "'";
                formattedResult += "\n";
            }

            logger.Log(formattedResult);
        }
    }
}
