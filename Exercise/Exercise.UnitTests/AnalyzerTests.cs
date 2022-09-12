namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void Analyze_WithNoLines_ReturnsEmptyList()
        {
            string[] lines = new string[0];
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Hello :)")]
        public void Analyze_WithOneLine_NoTodo_ReturnsEmptyList(string content)
        {
            string[] lines = new string[] { content };
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", 1)]
        [DataRow("12345TODO: Hello", 6)]
        public void Analyze_WithOneLine_WithTodo_ReturnsListWithOneLine(string content, int column)
        {
            string[] lines = new string[] { content };
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result[0].column, column);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("Hello :)", "")]
        [DataRow("", "Hello :)")]
        [DataRow("Hello :)", "Hello :)")]
        public void Analyze_WithTwoLines_NoTodo_ReturnsEmptyList(string firstLine, string secondLine)
        {
            string[] lines = new string[] { firstLine, secondLine };
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("TODO: Hello", "", 1)]
        [DataRow("", "TODO: Hello", 2)]
        public void Analyze_WithTwoLines_OneTodo_ReturnsListWithOneLine(string firstLine, string secondLine, int line)
        {
            string[] lines = new string[] { firstLine, secondLine };
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(line, result[0].line);
        }

        [TestMethod]
        public void Analyze_WithTwoLines_TwoTodo_ReturnsListWithTwoLines()
        {
            string[] lines = new string[] { "TODO", "TODO" };
            List<Issue> result = Analyzer.Analyze(lines);
            Assert.AreEqual(2, result.Count);
        }
    }
}