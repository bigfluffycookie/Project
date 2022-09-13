namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        public void Analyze_HasNoLines_ReturnsEmptyList()
        {
            var lines = new string[0];
            var result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Hello :)")]
        public void Analyze_HasOneLine_NoTodo_ReturnsEmptyList(string content)
        {
            var lines = new string[] { content };
            var result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", 1, 1, "TODO: Hello")]
        [DataRow("12345TODO: Hello", 1, 6, "TODO: Hello")]
        public void Analyze_HasOneLine_WithTodo_ReturnsListWithOneLine(string content, int expectedLine,
                                                                       int expectedColumn, string expectedContent)
        {
            var lines = new string[] { content };
            var result = Analyzer.Analyze(lines);
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
        public void Analyze_HasTwoLines_NoTodo_ReturnsEmptyList(string firstLineContent, string secondLineContent)
        {
            var lines = new string[] { firstLineContent, secondLineContent };
            var result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", "", 1, 1, "TODO: Hello")]
        [DataRow("", "TODO: Hello", 2, 1, "TODO: Hello")]
        public void Analyze_HasTwoLines_OneTodo_ReturnsListWithOneLine(string firstLineContent, string secondLineContent, int expectedLine,
                                                                       int expectedColumn, string expectedContent)
        {
            var lines = new string[] { firstLineContent, secondLineContent };
            var result = Analyzer.Analyze(lines);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedLine, result[0].Line);
            Assert.AreEqual(expectedColumn, result[0].Column);
            Assert.AreEqual(expectedContent, result[0].Text);
        }

        [TestMethod]
        public void Analyze_HasTwoLines_TwoTodo_ReturnsListWithTwoLines()
        {
            var lines = new string[] { "TODO", "TODO" };
            var result = Analyzer.Analyze(lines);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TODO", result[0].Text);
        }
    }
}