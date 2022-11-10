using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Exercise.Rules;

namespace Exercise
{
    internal interface IAvailableRulesProvider
    {
        ImmutableArray<IRule> AvailableRules { get; }
    }

    [Export(typeof(IAvailableRulesProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AvailableRulesProvider : IAvailableRulesProvider
    {
        public ImmutableArray<IRule> AvailableRules { get; }

        [ImportingConstructor]
        public AvailableRulesProvider([ImportMany] IEnumerable<IRule> rules)
        {
            AvailableRules = rules.ToImmutableArray();
        }
    }
}
