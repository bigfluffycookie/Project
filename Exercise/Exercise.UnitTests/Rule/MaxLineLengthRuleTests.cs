using Exercise.Rules;

namespace Exercise.UnitTests.Rules
{
    [TestClass]
    public class MaxLineLengthTests
    {
        [TestMethod]
        public void Execute_BreakRule_ReturnsOneIssue()
        {
            int max = 1;
            var ruleParamConfig = SetUpRuleConfig(max);
            var file = SetUpFile(2);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);
            var expectedContent = "Number of lines in file is " + file.FileContent.Length.ToString() +
                                 " which is greater than max specified: " + max.ToString();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        public void Execute_FileIsEmpty_DontBreakRule_ReturnsNoIsse()
        {
            int max = 1;
            var ruleParamConfig = SetUpRuleConfig(max);
            var file = SetUpFile(0);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Execute_DontBreakRule_ReturnsNoIssue()
        {
            var ruleParamConfig = SetUpRuleConfig(1);
            var file = SetUpFile(1);
            var rule = new MaxLineLengthRule();

            var result = rule.Execute(file, ruleParamConfig);

            Assert.AreEqual(0, result.Count);
        }

        public RuleParameterConfig SetUpRuleConfig(int maxPathLength)
        {
            var ruleParamConfig = new RuleParameterConfig();
            ruleParamConfig.AddRuleParam("maxLineLength", maxPathLength);
            return ruleParamConfig;
        }

        public File SetUpFile(int nrOfLines)
        {
            var lines = new string[nrOfLines];
            Array.Fill(lines, "");
            var file = new File("", lines);
            return file;
        }
    }
}