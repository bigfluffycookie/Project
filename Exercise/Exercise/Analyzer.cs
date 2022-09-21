using Exercise.Rules;
using System;
using System.Collections.Generic;
namespace Exercise
{
    public class Analyzer
    {
        public static List<Issue> Analyze(File file, List<IRule> rules, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            foreach (var rule in rules)
            {
                if (!rule.ShouldExecute(ruleParameterConfig)) { continue; }
                var resultFromRule = rule.Execute(file, ruleParameterConfig);
                result.AddRange(resultFromRule);
            }
            return result;
        }
    }
}