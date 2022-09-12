namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void Analyze_WithNoLines_ReturnsEmptyList()
        {
            string[] lines = new string[0];
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_WithOneLineNoTodo_ReturnsEmptyList()
        {
            string[] lines = new string[] { "" };
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_WithOneLineWithTodo_ReturnsListWithOneLine()
        {
            string[] lines = new string[] { "TODO" };
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Analyze_WithTwoLinesNoTodo_ReturnsEmptyList()
        {
            string[] lines = new string[] { "", "" };
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Analyze_WithTwoLinesOneTodo_ReturnsListWithOneLine()
        {
            string[] lines = new string[] { "TODO", "" };
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Analyze_WithTwoLinesTwoTodo_ReturnsListWithTwoLines()
        {
            string[] lines = new string[] { "TODO", "TODO" };
            List<string> result = Analyzer.Analyze(lines);
            Assert.AreEqual(2, result.Count);
        }
    }
}