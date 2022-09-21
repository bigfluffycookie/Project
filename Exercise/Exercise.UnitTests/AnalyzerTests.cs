using Exercise.Rules;
using Moq;

namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        public void Analyze_NoRules_ReturnsEmptyList()
        {
            var result = Analyzer.Analyze(It.IsAny<File>(),
                                          new List<IRule>(),
                                          It.IsAny<RuleParameterConfig>());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("ContentOne", 0, 0)]
        [DataRow("ContentTwo", 1, 2)]
        public void Analyze_OneRule_HasOneIssue_ReturnsOneIssue(string content, int line, int column)
        {
            Mock<IRule> rule = new Mock<IRule>();
            var issue = new Issue(content, line, column);

            rule.Setup(p => p.Execute(It.IsAny<File>(),
                                      It.IsAny<RuleParameterConfig>())).Returns(new List<Issue>() { issue });

            var result = Analyzer.Analyze(It.IsAny<File>(),
                                          new List<IRule>() { rule.Object },
                                          It.IsAny<RuleParameterConfig>());
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(content, result[0].Text);
            Assert.AreEqual(line, result[0].Line);
            Assert.AreEqual(column, result[0].Column);
        }

        [TestMethod]
        public void Analyze_OneRule_HasNoIssues_ReturnsEmptyList()
        {
            Mock<IRule> rule = new Mock<IRule>();
            rule.Setup(p => p.Execute(It.IsAny<File>(),
                                      It.IsAny<RuleParameterConfig>())).Returns(new List<Issue>());
            var result = Analyzer.Analyze(It.IsAny<File>(),
                                          new List<IRule>() { rule.Object },
                                          It.IsAny<RuleParameterConfig>());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("ContentOne", 0, 0)]
        [DataRow("ContentTwo", 1, 2)]
        public void Analyze_TwoRules_OneRuleHasIssue_ReturnsOneIssue(string content, int line, int column)
        {
            Mock<IRule> ruleOne = new Mock<IRule>();
            ruleOne.Setup(p => p.Execute(It.IsAny<File>(),
                                         It.IsAny<RuleParameterConfig>())).Returns(new List<Issue>());

            var issue = new Issue(content, line, column);
            Mock<IRule> ruleTwo = new Mock<IRule>();
            ruleTwo.Setup(p => p.Execute(It.IsAny<File>(),
                                         It.IsAny<RuleParameterConfig>())).Returns(new List<Issue>() { issue });
            var result = Analyzer.Analyze(It.IsAny<File>(),
                                          new List<IRule>() { ruleOne.Object, ruleTwo.Object },
                                          It.IsAny<RuleParameterConfig>());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(content, result[0].Text);
            Assert.AreEqual(line, result[0].Line);
            Assert.AreEqual(column, result[0].Column);
        }
    }
}