using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise.Rules;

namespace Exercise
{
    public interface IInput
    {
        string GetPathForFileToAnalyze();
        List<IRule> GetRulesToExecute(List<IRule> availableRules);

        IRuleParameterConfig GetRuleParmParameterConfig(List<IRule> rules);
    }
}
