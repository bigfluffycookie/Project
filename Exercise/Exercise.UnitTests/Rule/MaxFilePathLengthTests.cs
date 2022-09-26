using Exercise.Rules;
using Moq;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxFilePathLengthTests
    {
        [TestMethod]
        [DataRow("12", "1")]
        [DataRow("123", "2")]
        public void Execute_BreakRule_ReturnsOneIssue(string path, string max)
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxPathLength", max);
            var file = new File(path, Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            var expectedContent = "File path length is too large: " + path.Length.ToString() +
                                  " which is greater than max specified: " + max;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        [DataRow("12", "3")]
        [DataRow("123", "5")]
        public void Execute_DontBreakRule_ReturnsNoIssue(string path, string max)
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxPathLength", max);
            var file = new File(path, Array.Empty<string>());
            var rule = new MaxFilePathLengthRule();
            var result = rule.Execute(file, ruleParamConfig);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldExecute_No_ReturnsFalse()
        {
            Mock<RuleParameterConfig> ruleParamConfig = new Mock<RuleParameterConfig>();
            ruleParamConfig.Setup(p => p.HasRule(It.IsAny<string>())).Returns(false);
            var rule = new MaxFilePathLengthRule();
            var result = rule.ShouldExecute(ruleParamConfig.Object);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldExecute_Yes_ReturnsTrue()
        {
            Mock<RuleParameterConfig> ruleParamConfig = new Mock<RuleParameterConfig>();
            ruleParamConfig.Setup(p => p.HasRule(It.IsAny<string>())).Returns(true);
            var rule = new MaxFilePathLengthRule();
            var result = rule.ShouldExecute(ruleParamConfig.Object);
            Assert.IsTrue(result);
        }
    }
}