using Exercise.Rules;
using Moq;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxFilePathLengthTests
    {
        [TestMethod]
        [DataRow("1", 0)]
        [DataRow("123", 2)]
        public void Execute_BreakRule_ReturnsOneIssue(string path, int maxPathLength)
        {
            var ruleParamConfig = SetUpRuleConfig(maxPathLength);
            var file = SetupFile(path);
            var rule = new MaxFilePathLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            var expectedContent = "File path length is too large: " + path.Length +
                                  " which is greater than max specified: " + maxPathLength;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
            Assert.AreEqual(0, result[0].Line);
            Assert.AreEqual(0, result[0].Column);
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow("123", 5)]
        public void Execute_DontBreakRule_ReturnsNoIssue(string path, int maxPathLength)
        {
            var ruleParamConfig = SetUpRuleConfig(maxPathLength);
            var file = SetupFile(path);
            var rule = new MaxFilePathLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        private static IRuleConfig SetUpRuleConfig(int maxPathLength)
        {
            var ruleConfig = new Mock<IRuleConfig>();
            ruleConfig.Setup(p => p.RuleParam).Returns(maxPathLength);

            return ruleConfig.Object;
        }

        private static IFile SetupFile(string path)
        {
            var file = new Mock<IFile>();
            file.Setup(p => p.FilePath).Returns(path);

            return file.Object;
        }
    }
}