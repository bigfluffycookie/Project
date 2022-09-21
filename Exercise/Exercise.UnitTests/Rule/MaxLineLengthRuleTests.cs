using Exercise.Rules;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxLineLengthTests
    {
        [TestMethod]
        public void Execute_BreakRule_ReturnsOneIssue()
        {
            var ruleParamConfig = new RuleParameterConfig();
            int max = 1;
            ruleParamConfig.AddRuleParam("maxLineLength", max);
            var file = new File("", new string[] { "", "" });
            var rule = new MaxLineLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            var expectedContent = "Number of lines in file is " + file.FileContent.Length.ToString() +
                                 " which is greater than max specified: " + max.ToString();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        public void Execute_DontBreakRule_ReturnsNoIssue()
        {
            var ruleParamConfig = new RuleParameterConfig();
            int max = 1;
            ruleParamConfig.AddRuleParam("maxLineLength", max);
            var file = new File("", new string[] { "" });
            var rule = new MaxFilePathLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            Assert.AreEqual(0, result.Count);
        }
    }
}