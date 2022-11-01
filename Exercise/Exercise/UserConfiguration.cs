using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Exercise
{
    /* JSON STYLE
    {
        "rules":{
            "<ruleID>" : [<ruleParam>]
        }
    }*/

    [Serializable]
    public class UserConfiguration
    {
        [JsonProperty("rules")]
        public Dictionary<string, int[]> rules;
    }
}