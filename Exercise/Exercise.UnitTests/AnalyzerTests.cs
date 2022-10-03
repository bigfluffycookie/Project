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
        public void Analyze_OneRule_HasOneIssue_ReturnsOneIssue()
        {
            var file = new Mock<IFile>();
            var rule = new Mock<IRule>();
            var issue = new Mock<IIssue>();
            var ruleParameterConfig = new Mock<IRuleParameterConfig>();
            rule.Setup(p => p.Execute(file.Object, ruleParameterConfig.Object)).Returns(new List<IIssue> { issue.Object });

            var result = Analyzer.Analyze(file.Object,
                                          new List<IRule> { rule.Object },
                                          ruleParameterConfig.Object);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Analyze_OneRule_HasNoIssues_ReturnsEmptyList()
        {
            var file = new Mock<IFile>();
            var rule = new Mock<IRule>();
            var ruleParameterConfig = new Mock<IRuleParameterConfig>();
            rule.Setup(p => p.Execute(file.Object, ruleParameterConfig.Object)).Returns(new List<IIssue>());

            var result = Analyzer.Analyze(file.Object,
                                          new List<IRule> { rule.Object },
                                          ruleParameterConfig.Object);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_TwoRules_OneRuleHasIssue_ReturnsOneIssue()
        {
            var file = new Mock<IFile>();
            var ruleOne = new Mock<IRule>();
            var ruleParameterConfig = new Mock<IRuleParameterConfig>();
            ruleOne.Setup(p => p.Execute(file.Object, ruleParameterConfig.Object)).Returns(new List<IIssue>());

            var issue = new Mock<IIssue>();
            var ruleTwo = new Mock<IRule>();
            ruleTwo.Setup(p => p.Execute(file.Object, ruleParameterConfig.Object)).Returns(new List<IIssue> { issue.Object });

            var result = Analyzer.Analyze(file.Object,
                                          new List<IRule> { ruleOne.Object, ruleTwo.Object },
                                          ruleParameterConfig.Object);

            Assert.AreEqual(1, result.Count);
        }
    }
}