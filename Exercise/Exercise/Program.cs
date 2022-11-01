﻿using Exercise.Rules;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exercise
{
    public static class Program
    {
        public static string ProgramSetUp(string[] args)
        {
            var rules = GetAvailableRules();
            var configProvider = GetConfigProvider(args);
            var configuration = configProvider.GetConfiguration();
            var filePath = configuration.FileToAnalyze;
            var fileContent = GetFileContent(filePath);
            var file = new File(filePath, fileContent);

            var result = Analyzer.Analyze(file, rules, configuration);

            var formattedResultReadyResult = FormatResult(result);

            return formattedResultReadyResult;
        }

        private static IConfigProvider GetConfigProvider(string[] args)
        {
            var inputValidator = new InputValidator();

            if (!inputValidator.FileExists(args[0]))
            {
                throw new Exception("Json File at path: " + args[0] + " was not found.");
            }

            var inputFromJson = new ConfigProviderJson(args[0]);

            return inputFromJson;
        }

        private static string[] GetFileContent(string filePath)
        {
            var fileContent = Array.Empty<string>();

            try
            {
                fileContent = System.IO.File.ReadAllLines(filePath);
            }
            catch (IOException e)
            {
                Console.WriteLine("File could not be read with error message: " + e.Message);
                Console.Write("Press any key to close App");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return fileContent;
        }

        private static List<IRule> GetAvailableRules()
        {
            var rules = new List<IRule>()
            {
                                new MaxLineLengthRule(),
                                new MaxFilePathLengthRule(),
                                new TodoRule()
            };

            return rules;
        }

        private static string FormatResult(List<IIssue> issues)
        {
            var formattedResult = issues.Count == 0 ? "No Issues" : "";
            foreach (var issue in issues)
            {
                formattedResult += "Line: " + issue.Line + ", ";
                formattedResult += "Column: " + issue.Column + ", ";
                formattedResult += "'" + issue.Text + "'";
                formattedResult += "\n";
            }

            return formattedResult;
        }
    }
}