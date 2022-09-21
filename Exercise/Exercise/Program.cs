using Exercise.Rules;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
namespace Exercise
{
    public class Program
    {
        public static List<string> Main(string ruleConfigFile)
        {
            DisplayWelcomeText();
            var ruleFileContent = System.IO.File.ReadAllText(ruleConfigFile);
            var ruleConfig = JsonConvert.DeserializeObject<SerializedRuleConfig>(ruleFileContent);
            if (ruleConfig == null)
            {
                throw new Exception("Rule Configeration could not be Serialized, Exiting Analyzer");
            }
            var filePath = ruleConfig.fileToAnalyze;
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("File to analyze does not exist: " + filePath);
            }
            File file = new File(filePath, System.IO.File.ReadAllLines(filePath));

            var ruleParameterConfig = InitializeRuleParameterConfig(ruleConfig);
            var rules = GetRules();
            var result = Analyzer.Analyze(file, rules, ruleParameterConfig);
            var print = CreateReadableIssues(result);
            return print;
        }

        private static List<IRule> GetRules()
        {
            var rules = new List<IRule>();
            var maxLineLengthRule = new MaxLineLengthRule();
            rules.Add(maxLineLengthRule);
            var maxPathLengthRule = new MaxFilePathLengthRule();
            rules.Add(maxPathLengthRule);
            var todoRule = new TodoRule();
            rules.Add(todoRule);

            return rules;
        }

        private static RuleParameterConfig InitializeRuleParameterConfig(SerializedRuleConfig ruleConfig)
        {
            var ruleParameterConfig = new RuleParameterConfig();
            foreach (var rule in ruleConfig.rules)
            {
                ruleParameterConfig.AddRuleParam(rule.id, rule.paramaters);
            }
            return ruleParameterConfig;
        }

        private static List<string> CreateReadableIssues(List<Issue> issues)
        {
            var res = new List<string>();
            foreach (Issue issue in issues)
            {
                var print = "";
                print += "Line: " + issue.Line + ", ";
                print += "Column: " + issue.Column + ", ";
                print += "'" + issue.Text + "'";
                res.Add(print);
            }
            return res;
        }

        private static void DisplayWelcomeText()
        {
            Console.WriteLine("Welcome to the Analyzer");
        }
    }
}