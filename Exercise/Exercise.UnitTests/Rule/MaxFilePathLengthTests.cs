﻿using Exercise.Rules;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxFilePathLengthTests
    {
        [TestMethod]
        [DataRow("12", 1)]
        public void Execute_BreakRule_ReturnsOneIssue(string path, int max)
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxPathLength", max);
            var file = new File(path, Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            var expectedContent = "File path length is too large: " + path.Length.ToString() +
                                  " which is greater than max specified: " + max.ToString();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        public void Execute_DontBreakRule_ReturnsNoIssue()
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxPathLength", 2);
            var file = new File("ab", Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            Assert.AreEqual(0, result.Count);
        }
    }
}