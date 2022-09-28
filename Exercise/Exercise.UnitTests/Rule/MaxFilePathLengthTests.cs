using Exercise.Rules;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxFilePathLengthTests
    {
        [TestMethod]
        [DataRow("12", 1)]
        [DataRow("123", 2)]
        public void Execute_BreakRule_ReturnsOneIssue(string path, int max)
        {
            var ruleParamConfig = SetUpRuleConfig(max);
            var file = new File(path, Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();

            var result = rule.Execute(file, ruleParamConfig);
            var expectedContent = "File path length is too large: " + path.Length.ToString() +
                                  " which is greater than max specified: " + max.ToString();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
            Assert.AreEqual(0, result[0].Line);
            Assert.AreEqual(0, result[0].Column);
        }

        [TestMethod]
        [DataRow("12", 3)]
        [DataRow("123", 5)]
        public void Execute_DontBreakRule_ReturnsNoIssue(string path, int max)
        {
            var ruleParamConfig = SetUpRuleConfig(max);
            var file = new File(path, Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        public RuleParameterConfig SetUpRuleConfig(int maxPathLength) 
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxPathLength", maxPathLength);
            return ruleParamConfig;
        }
    }
}