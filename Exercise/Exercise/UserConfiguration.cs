using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Exercise
{
    /* JSON STYLE
    {
        "fileToAnalyze": "<filePath>",
        "rules":{
            "<ruleID>" : [<ruleParam>]
        }
    }*/

    [Serializable]
    public class UserConfiguration
    {
        [JsonProperty("fileToAnalyze")]
        public string fileToAnalyze;
        [JsonProperty("rules")]
        public Dictionary<string, int[]> rules;
    }
}