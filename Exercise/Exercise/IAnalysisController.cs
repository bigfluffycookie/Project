using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

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

        private readonly IConfigProvider configProvider;

        [ImportingConstructor]
        public AnalysisController([Import] ILogger logger, [Import]IConfigProvider configProvider)
        {
            this.logger = logger;
            this.configProvider = configProvider;
        }

        public void AnalyzeAndGetResult()
        {
            var filePath = "C:\\Junk\\File.txt";
            var file = new File(filePath);

            var rulesProvider = new AvailableRulesProvider();
            var rules = rulesProvider.GetAvailableRules();

            var result = Analyzer.Analyze(file, rules, configProvider.GetConfiguration());

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
