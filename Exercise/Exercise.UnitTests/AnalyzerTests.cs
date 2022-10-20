using Castle.Core.Configuration;
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
                                          It.IsAny<IConfiguration>());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_OneRule_HasOneIssue_ReturnsOneIssue()
        {
            var file = Mock.Of<IFile>();
            var issue = Mock.Of<IIssue>();
            var rule = new Mock<IRule>();
            var configuration = Mock.Of<IConfiguration>();
            var ruleConfig = Mock.Of<IRuleConfig>();
            rule.Setup(p => p.Execute(file, ruleConfig)).Returns(new List<IIssue> { issue });

            var result = Analyzer.Analyze(file,
                                          new List<IRule> { rule.Object },
                                          configuration);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(issue, result[0]);
        }

        [TestMethod]
        public void Analyze_OneRule_HasNoIssues_ReturnsEmptyList()
        {
            var file = Mock.Of<IFile>();
            var configuration = Mock.Of<IConfiguration>();
            var ruleConfig = Mock.Of<IRuleConfig>();
            var rule = new Mock<IRule>();
            rule.Setup(p => p.Execute(file, ruleConfig)).Returns(new List<IIssue>());

            var result = Analyzer.Analyze(file,
                                          new List<IRule> { rule.Object },
                                          configuration);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_TwoRules_OneRuleHasIssue_ReturnsOneIssue()
        {
            var file = Mock.Of<IFile>();
            var configuration = Mock.Of<IConfiguration>();
            var ruleConfig = Mock.Of<IRuleConfig>();
            var ruleOne = new Mock<IRule>();
            ruleOne.Setup(p => p.Execute(file, ruleConfig)).Returns(new List<IIssue>());

            var issue = Mock.Of<IIssue>();
            var ruleTwo = new Mock<IRule>();
            ruleTwo.Setup(p => p.Execute(file, ruleConfig)).Returns(new List<IIssue> { issue });

            var result = Analyzer.Analyze(file,
                                          new List<IRule> { ruleOne.Object, ruleTwo.Object },
                                          configuration);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(issue, result[0]);
        }
    }
}