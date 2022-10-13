namespace Exercise
{
    /* JSON STYLE
    {
        "fileToAnalyze": "<filePath>",
        "rules":{
            "<ruleID>" : "<ruleParamater>"
        }
    }*/

    [Serializable]
    public class UserConfiguration
    {
        public string fileToAnalyze;
        public Dictionary<string,string> rules;
    }
}