using Exercise.Rules;
using Moq;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxLineLengthTests
    {
        [TestMethod]
        public void Execute_BreakRule_ReturnsOneIssue()
        {
            var maxNumberOfLines = 1;
            var ruleParamConfig = SetUpRuleConfig(maxNumberOfLines);
            var file = SetupFile(numberOfLines: 2);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            var expectedContent = "Number of lines in file is " + file.FileContent.Length +
                                  " which is greater than max specified: " + maxNumberOfLines;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
            Assert.AreEqual(2, result[0].Line);
            Assert.AreEqual(1, result[0].Column);
        }

        [TestMethod]
        public void Execute_FileIsEmpty_ReturnsNoIssue()
        {
            var maxNumberOfLines = 1;
            var ruleParamConfig = SetUpRuleConfig(maxNumberOfLines);
            var file = SetupFile(numberOfLines: 0);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Execute_DontBreakRule_ReturnsNoIssue()
        {
            var ruleParamConfig = SetUpRuleConfig(maxNumberOfLines: 1);
            var file = SetupFile(numberOfLines: 1);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        private static IRuleConfig SetUpRuleConfig(int maxNumberOfLines)
        {
            var ruleConfig = new Mock<IRuleConfig>();
            ruleConfig.Setup(p => p.RuleParam).Returns(maxNumberOfLines);

            return ruleConfig.Object;
        }

        private static IFile SetupFile(int numberOfLines)
        {
            var lines = new string[numberOfLines];
            Array.Fill(lines, "");
            var file = new Mock<IFile>();
            file.Setup(p => p.FileContent).Returns(lines);

            return file.Object;
        }
    }
}