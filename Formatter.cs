using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HICMigration
{
    public static class Formatter
	{
        private static Regex h1 = new Regex(@"^==.+?==\n?", RegexOptions.Compiled | RegexOptions.Multiline);
		private static Regex h2 = new Regex(@"^===.+?===\n?", RegexOptions.Compiled | RegexOptions.Multiline);
		private static Regex h3 = new Regex(@"^====.+?====\n?", RegexOptions.Compiled | RegexOptions.Multiline);
		private static Regex h4 = new Regex(@"^=====.+?=====\n?", RegexOptions.Compiled | RegexOptions.Multiline);

        private static void Format_h(Regex hRegex, string tag, int len, ref StringBuilder sb)
        {
            string h;
            int end = 0;
            Match match = hRegex.Match(sb.ToString());
            while (match.Success)
			{
				if (!NoWiki.IsNoWikied(match.Index, out end))
				{
                    match = hRegex.Match(sb.ToString(), end);
                    sb.Remove(match.Index, match.Length);
                    h = match.Value.Substring(len, match.Value.Length - (len * 2) - (match.Value.EndsWith("\n") ? 1 : 0));      
                    sb.Insert(match.Index, $"<{tag}>{h}</{tag}>");
                }

				NoWiki.Compute(sb.ToString());
				match = hRegex.Match(sb.ToString(), end);
			}
        }

        public static string Format(string raw)
        {
            StringBuilder sb = new StringBuilder(raw);

            sb.Replace("\r", "");
			if (!sb.ToString().EndsWith("\n")) sb.Append("\n"); // Very important to make Regular Expressions work!
            
            NoWiki.Compute(sb.ToString());

            Format_h(h4, "h4", 5, ref sb);
            Format_h(h3, "h3", 4, ref sb);
            Format_h(h2, "h2", 3, ref sb);
            Format_h(h1, "h1", 2, ref sb);

            return sb.ToString();
        }

    }

    public static class NoWiki
    {
        private static List<int> noWikiBegin = new List<int>();
        private static List<int> noWikiEnd = new List<int>();

		private static Regex noWiki = new Regex(@"\<nowiki\>(.|\n|\r)+?\<\/nowiki\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		public static void Compute(string text)
		{
			Match match;
			noWikiBegin.Clear();
			noWikiEnd.Clear();

			match = noWiki.Match(text);
			while (match.Success)
			{
				noWikiBegin.Add(match.Index);
				noWikiEnd.Add(match.Index + match.Length);
				match = noWiki.Match(text, match.Index + match.Length);
			}
		}

		public static bool IsNoWikied(int index, out int end)
		{
			for (int i = 0; i < noWikiBegin.Count; i++)
			{
				if (index > noWikiBegin[i] && index < noWikiEnd[i])
				{
					end = noWikiEnd[i];
					return true;
				}
			}
			end = 0;
			return false;
		}



    }
}
