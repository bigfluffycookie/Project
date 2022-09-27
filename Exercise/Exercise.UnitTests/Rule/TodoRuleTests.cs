using Exercise.Rules;

namespace Exercise.UnitTests.Rule
{
    [TestClass]
    public class TodoRuleTests
    {
        [TestMethod]
        public void Execute_HasNoLines_ReturnsEmptyList()
        {
            var file = new File("", Array.Empty<string>());
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Hello :)")]
        public void Execute_HasOneLine_NoTodo_ReturnsEmptyList(string content)
        {
            var lines = new string[] { content };
            var file = new File("", lines);
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", 1, 1, "TODO: Hello")]
        [DataRow("12345TODO: Hello", 1, 6, "TODO: Hello")]
        public void Execute_HasOneLine_WithTodo_ReturnsListWithOneLine(string content, int expectedLine,
                                                                       int expectedColumn, string expectedContent)
        {
            var lines = new string[] { content };
            var file = new File("", lines);
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedLine, result[0].Line);
            Assert.AreEqual(expectedColumn, result[0].Column);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("Hello :)", "")]
        [DataRow("", "Hello :)")]
        [DataRow("Hello :)", "Hello :)")]
        public void Execute_HasTwoLines_NoTodo_ReturnsEmptyList(string firstLineContent, string secondLineContent)
        {
            var lines = new string[] { firstLineContent, secondLineContent };
            var file = new File("", lines);
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", "", 1, 1, "TODO: Hello")]
        [DataRow("", "TODO: Hello", 2, 1, "TODO: Hello")]
        public void Execute_HasTwoLines_OneTodo_ReturnsListWithOneLine(string firstLineContent, string secondLineContent, int expectedLine,
                                                                       int expectedColumn, string expectedContent)
        {
            var lines = new string[] { firstLineContent, secondLineContent };
            var file = new File("", lines);
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedLine, result[0].Line);
            Assert.AreEqual(expectedColumn, result[0].Column);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        public void Execute_HasTwoLines_TwoTodo_ReturnsListWithTwoLines()
        {
            var lines = new string[] { "TODO", "TODO" };
            var file = new File("", lines);
            var rule = new TodoRule();

            var result = rule.Execute(file, new RuleParameterConfig());

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TODO", result[0].Text);
        }
    }
}