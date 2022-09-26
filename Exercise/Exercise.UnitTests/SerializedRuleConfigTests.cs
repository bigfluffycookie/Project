using Newtonsoft.Json;

namespace Exercise.UnitTests
{
    [TestClass]
    public class SerializedRuleConfigTests
    {
        [TestMethod]
        public void DeserializeObject_IncorrectJson_ThrowsException()
        {
            string text = "{\r\n  \'e\': \'a'\r\n\r\n";
            Assert.ThrowsException<Newtonsoft.Json.JsonSerializationException>(
                    () => JsonConvert.DeserializeObject<SerializedRuleConfig>(text));
        }

        [TestMethod]
        public void DeserializeObject_CorrectJson_DoesNotThrowException()
        {
            string text = "\r\n{\r\n  \'fileToAnalyze\': \'some_path\',\r\n  \'rules\': []\r\n}";
            var ruleConfig = JsonConvert.DeserializeObject<SerializedRuleConfig>(text);
            Assert.IsNotNull(ruleConfig);
        }

        [TestMethod]
        public void DeserializeObject_CorrectJson_OneRule_HasExpectedValues()
        {
            string text = "\r\n{\r\n  \'fileToAnalyze\': \'some_path\',\r\n" +
                          "\'rules\':" +
                          "[{\r\n      \"id\": \"pathLength\",\r\n      " +
                          "\"paramaters\": \"5\"\r\n    }]\r\n}";
            var ruleConfig = JsonConvert.DeserializeObject<SerializedRuleConfig>(text);
            Assert.IsNotNull(ruleConfig);
            Assert.AreEqual("some_path", ruleConfig.fileToAnalyze);
            Assert.AreEqual(1, ruleConfig.rules.Length);
            Assert.AreEqual("pathLength", ruleConfig.rules[0].id);
            Assert.AreEqual("5", ruleConfig.rules[0].paramaters);
        }
    }
}