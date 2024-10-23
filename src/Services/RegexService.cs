using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace jsunmap.Services
{
    internal static class RegexService
    {
        private static readonly Dictionary<string, Regex> regexGeneralDictionary;
        private static readonly Dictionary<string, Regex> regexPIIDictionary;
        private static readonly Dictionary<string, Regex> regexSurfaceDictionary;
        static RegexService()
        {
            regexGeneralDictionary = new Dictionary<string, Regex>()
            {
                { "graphql-query", new Regex("gql`([\\s\\S]*?)`",RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline) },
                { "uuid", new Regex("\b[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "uuid-v4", new Regex("\b[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[89abAB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "base64", new Regex("(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "simple-password", new Regex("(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "api-token-key:" , new Regex("(api|key|token)[\\\"\\'\\s]*[:=][\\\"\\'\\s]*(\\w+)[\\\"\\'\\s]*",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "auth-info" , new Regex("(username|password|passwd|credential)[\\\"\\'\\s]*[:=][\\\"\\'\\s]*(\\w+)[\\\"\\'\\s]*",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "private-key" , new Regex("(secret|private)[\\\"\\'\\s]*[:=][\\\"\\'\\s]*(\\w+)[\\\"\\'\\s]*",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "database-config" , new Regex("(database|db)[\\\"\\'\\s]*[:=][\\\"\\'\\s]*(\\w+)[\\\"\\'\\s]*",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "env-data" , new Regex("(process\\.env|process\\.env\\.[a-zA-Z_]+)",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "logs:" , new Regex("(console\\.log|console\\.error)[\\s]*\\([\\s\\S]*?\\);?",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
            };

            regexSurfaceDictionary = new Dictionary<string, Regex>()
            {
                { "url", new Regex("(https?://\\S+)",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "ftp(s)", new Regex("\\bftp[s]?:\\/\\/[-A-Z0-9+&@#\\/%?=~_|!:,.;]*[-A-Z0-9+&@#\\/%=~_|]\\b", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "ip-v4", new Regex("\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "ip-v6", new Regex("\b(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) }
            };

            regexPIIDictionary = new Dictionary<string, Regex>()
            {
                { "email:" , new Regex("\\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Z|a-z]{2,}\\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "credit-card:" , new Regex("\\b\\d{4}[ -]?\\d{4}[ -]?\\d{4}[ -]?\\d{4}\\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
                { "phone-default:", new Regex("\\b(?:\\+\\d{1,2}\\s?)?\\(?\\d{3}\\)?[-.\\s]?\\d{3}[-.\\s]?\\d{4}\\b",RegexOptions.Compiled | RegexOptions.IgnoreCase) },
            };

        }

        internal static Dictionary<string, List<string>> ExtractAllGeneralSensitiveData(string Content)
        {
            return InternalExtractDataFromRegexDictionary(regexGeneralDictionary, Content);
        }


        internal static Dictionary<string, List<string>> ExtractAllSurfaceData(string Content)
        {
            return InternalExtractDataFromRegexDictionary(regexSurfaceDictionary, Content);
        }


        internal static Dictionary<string, List<string>> ExtractAllPÌIData(string Content)
        {
            return InternalExtractDataFromRegexDictionary(regexPIIDictionary, Content);
        }

        private static Dictionary<string, List<string>> InternalExtractDataFromRegexDictionary(Dictionary<string, Regex> RegexDictionary, string Content)
        {
            Dictionary<string, List<string>> outputResult = new Dictionary<string, List<string>>();

            foreach (var regexItem in RegexDictionary)
            {
                MatchCollection matches = regexItem.Value.Matches(Content);
                if (matches.Count <= 0) continue;


                outputResult.Add(regexItem.Key, new List<string>(matches
                    .Where(x => !string.IsNullOrEmpty(x.Groups[1].Value.Trim()))
                    .Select(x =>$"{x.Groups[1].Value.Trim()} (position {x.Groups[1].Index})" )));
            }

            return outputResult;
        }
    }
}
