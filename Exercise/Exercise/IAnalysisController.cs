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

        private readonly IConfigProvider configProvider;

        [ImportingConstructor]
        public AnalysisController(ILogger logger, IAvailableRulesProvider availableRulesProvider, IConfigProvider configProvider)
        {
            this.logger = logger;
            availableRules = availableRulesProvider.AvailableRules;
            this.configProvider = configProvider;
        }

        public void AnalyzeAndGetResult()
        {
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var configuration = configProvider.GetConfiguration();
            LogStartMessage(filePath, configuration.ConfigurationPath);

            var result = Analyzer.Analyze(file, availableRules, configuration);

            FormatAndLogResult(result);
        }

        private void LogStartMessage(string fileToAnalyzePath, string ruleConfigPath)
        {
            logger.Log("----------------------------------------------------------------------------\n");
            logger.Log($"Analyzing File:  {fileToAnalyzePath} \n");
            logger.Log($"Using rule configuration from path: {ruleConfigPath} \n");
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
