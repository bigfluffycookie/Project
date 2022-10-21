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
            var id = "id";

            var ruleConfig = CreateRuleConfigWithId(id);
            var configuration = CreateConfigurationWithRuleConfigs(new List<IRuleConfig>{ruleConfig});

            var rule = CreateRuleWithId(id);
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
            var id = "id";
            var ruleConfig = CreateRuleConfigWithId(id);
            var configuration = CreateConfigurationWithRuleConfigs(new List<IRuleConfig> { ruleConfig });

            var rule = CreateRuleWithId(id);
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

            var idOne = "1";
            var ruleConfigOne = CreateRuleConfigWithId(idOne);
            var ruleOne = CreateRuleWithId(idOne);
            ruleOne.Setup(p => p.Execute(file, ruleConfigOne)).Returns(new List<IIssue>());

            var issue = Mock.Of<IIssue>();
            var idTwo = "2";
            var ruleConfigTwo = CreateRuleConfigWithId(idTwo);
            var ruleTwo = CreateRuleWithId(idTwo);
            ruleTwo.Setup(p => p.Execute(file, ruleConfigTwo)).Returns(new List<IIssue> { issue });

            var configuration = CreateConfigurationWithRuleConfigs(new List<IRuleConfig>{ruleConfigOne, ruleConfigTwo});

            var result = Analyzer.Analyze(file,
                                          new List<IRule> { ruleOne.Object, ruleTwo.Object },
                                          configuration);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(issue, result[0]);
        }

        private static Mock<IRule> CreateRuleWithId(string id)
        {
            var rule = new Mock<IRule>();
            rule.Setup(p => p.RuleId).Returns(id);

            return rule;
        }

        private static IRuleConfig CreateRuleConfigWithId(string id)
        {
            var ruleConfig = new Mock<IRuleConfig>();
            ruleConfig.Setup(p => p.RuleId).Returns(id);

            return ruleConfig.Object;
        }

        private static IConfiguration CreateConfigurationWithRuleConfigs(IEnumerable<IRuleConfig> ruleConfigs)
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(p => p.Rules).Returns(ruleConfigs);

            return configuration.Object;
        }
    }
}