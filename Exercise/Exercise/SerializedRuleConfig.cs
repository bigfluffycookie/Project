using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    [System.Serializable]
    public class SerializedRuleConfig
    {
        public string fileToAnalyze;
        public Rule[] rules;
    }

    public class Rule
    {
        public string id;
        public string paramaters;
    }
}
