namespace Exercise
{
    public class RuleParameterConfig : IRuleParameterConfig
    {
        private readonly Dictionary<string, int> ruleIdToParam = new();

        public void AddRuleParam(string key, int value)
        {
            if (ruleIdToParam.ContainsKey(key))
            {
                return;
            }

            ruleIdToParam.Add(key, value);
        }

        public int GetRuleParam(string key)
        {
            if (!ruleIdToParam.TryGetValue(key, out var value))
            {
                throw new Exception("No Parameter found that matches given id: " + key);
            }

            return value;
        }
    }
}
