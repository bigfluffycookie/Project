using System;
using System.Collections.Generic;
using Exercise.Rules;

namespace Exercise
{
    internal interface IAvailableRulesProvider
    {
        IEnumerable<IRule> GetAvailableRules();
    }

    public class AvailableRulesProvider : IAvailableRulesProvider
    {
        public IEnumerable<IRule> GetAvailableRules() 
        {
            var rules = new List<IRule>()
            {
                new MaxLineLengthRule(),
                new MaxFilePathLengthRule(),
                new TodoRule()
            };

            return rules;
        }
    }
}
