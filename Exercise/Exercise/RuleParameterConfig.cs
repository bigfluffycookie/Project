using System.Collections.Generic;
using System;
namespace Exercise
{
    public class RuleParameterConfig
    {
        private readonly Dictionary<string, string> ruleIdToParam;
        public RuleParameterConfig()
        {
            ruleIdToParam = new Dictionary<string, string>();
        }

        public void AddRuleParam(string key, string value)
        {
            if (ruleIdToParam.ContainsKey(key)) { return; }
            ruleIdToParam.Add(key, value);
        }

        public bool HasRule(string key)
        {
            return ruleIdToParam.ContainsKey(key);
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
