using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Exercise.Rules;

namespace Exercise
{
    internal interface IAvailableRulesProvider
    {
        IEnumerable<IRule> AvailableRules { get; }
    }

    [Export(typeof(IAvailableRulesProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AvailableRulesProvider : IAvailableRulesProvider
    {
        public IEnumerable<IRule> AvailableRules { get; }

        [ImportingConstructor]
        public AvailableRulesProvider([ImportMany] IEnumerable<IRule> rules)
        {
            AvailableRules = rules;
        }
    }
}
