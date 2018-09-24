using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using K.SEOAnalyser.Web.Enums;

namespace K.SEOAnalyser.Web.Utils
{
    public static class CommonFunctions
    {
        const string REGEX_MATCH_SINGLE_URL = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        const string REGEX_MATCH_WORD = @"[^\d\W]+";
        const string REGEX_MATCH_METATAG = @"<meta name=""(.+?)"" content=""(.+?)"">";
        const string REGEX_MATCH_URL = @"\b(?:https?://|www\.)\S+\b";
        
        public static bool IsValidUrl(string value)
        {
            Match m = Regex.Match(value, REGEX_MATCH_SINGLE_URL);
            return m.Success;
        }

        public static InputValueType ValidateValueType(string value)
        {
            if (IsValidUrl(value))
                return InputValueType.URL;

            return InputValueType.TEXT;
        }

        public static bool IsUrlResponses(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 15000;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<string> GetPageWordsFromUrl(string url)
        {
            WebClient client = new WebClient();
            string downloadString = await client.DownloadStringTaskAsync(url);
            if (!string.IsNullOrWhiteSpace(downloadString))
                downloadString = downloadString.Trim();

            return downloadString;
        }

        public static List<string> ExtractOnlyWords(string contents)
        {
            Regex wordsRegex = new Regex(REGEX_MATCH_WORD);
            MatchCollection matchCollection = wordsRegex.Matches(contents);
            List<string> words = matchCollection.Select(s => s.Value.Trim()).ToList();

            return string.Join(" ", words).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string GetStringInMetaTag(string contents)
        {
            Regex metaTag = new Regex(REGEX_MATCH_METATAG);
            MatchCollection matchCollection = metaTag.Matches(contents);

            return string.Join(" ", matchCollection.Select(s => s.Groups[2].Value).ToArray());
        }

        public static List<string> ExtractUrlFromString(string contents)
        {
            List<string> urls = new List<string>();

            if (!string.IsNullOrWhiteSpace(contents))
            {
                contents = contents.Replace("\">", " ");

                Regex urlTag = new Regex(REGEX_MATCH_URL);
                MatchCollection matchCollection = urlTag.Matches(contents);

                if (matchCollection != null)
                {
                    urls = matchCollection.Select(s => s.Groups[0].Value).ToList();
                }
            }
            
            return urls;
        }
    }
}
