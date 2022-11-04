using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Exercise.Rules;
using static Microsoft.ServiceHub.Framework.ServiceJsonRpcDescriptor;

namespace Exercise
{
    internal interface IAnalysisController
    {
       void AnalyzeAndGetResult(string fileToAnalyzePath);
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

        public void AnalyzeAndGetResult(string fileToAnalyzePath)
        {
            var file = new File(fileToAnalyzePath);

            var configuration = configProvider.GetConfiguration();
            LogStartMessage(filePath, configuration.ConfigurationPath);

            var result = Analyzer.Analyze(file, availableRules, configuration);

            FormatAndLogResult(result);
        }

        private void LogStartMessage(string fileToAnalyzePath, string ruleConfigPath)
        {
            logger.LogMessageSeperator();
            logger.LogWithNewLine($"Analyzing File:  {fileToAnalyzePath}");
            logger.LogWithNewLine($"Using rule configuration from path: {ruleConfigPath}");
        }

        private void FormatAndLogResult(List<IIssue> issues)
        {
            if (issues.Count == 0)
            {
                logger.LogWithNewLine("No Issues found");
                return;
            }

            foreach (var issue in issues)
            {
                var formattedResult = @$"Line:  {issue.Line} , 
                                        Column: {issue.Column} ,
                                        '{issue.Text}'";
                logger.LogWithNewLine(formattedResult);
            }
        }
    }
}
