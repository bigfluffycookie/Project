namespace Exercise
{
    public class RuleParameterConfig : IRuleParameterConfig
    {
        private readonly Dictionary<string, string> ruleIdToParam = new();

        public void AddRuleParam(string key, string value)
        {
            if (ruleIdToParam.ContainsKey(key))
            {
                return;
            }

            ruleIdToParam.Add(key, value);
        }

        public string GetRuleParam(string key)
        {
            if (!ruleIdToParam.TryGetValue(key, out var value))
            {
                throw new Exception("No Parameter found that matches given id: " + key);
            }

            return value;
        }
    }
}
