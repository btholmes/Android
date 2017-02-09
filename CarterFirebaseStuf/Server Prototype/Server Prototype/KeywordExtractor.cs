using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

public class KeywordExtractor
{
	/// <summary>
	/// Takes the text from a set of webpages and returns keywords based on each websites weight and the keyword's placement in the sites.
	/// </summary>
	/// <param name="websites">A dictionary containing resolvable URIs and their weights.</param>
	/// <returns>A dictionary containing potential search terms and their weighted relevance.</returns>
	internal static Dictionary<string, double> ExtractKeywords(Dictionary<string, double> websites, LinkedList<string> usedSearchTerms)
	{
		System.Net.ServicePointManager.ServerCertificateValidationCallback +=
			(se, cert, chain, sslerror) =>
			{
				return true;
			};//Accept every certificate. Not terribly secure, would need to be more elaborate in full version.

		var keywords = new Dictionary<string, double>();
		foreach (var site in websites)
		{
			if(site.Value != 0)//Anything with a weight of 0 doesn't even need to be passed this far.
				AnalyzeSite(site.Key, site.Value, ref keywords);
		}

		foreach(var usedTerm in usedSearchTerms)
		{
			if (keywords.ContainsKey(usedTerm))
				keywords.Remove(usedTerm);
		}

		return keywords;
	}

	/// <summary>
	/// Performs keyword analysis on a single site.
	/// </summary>
	/// <param name="url">The resolvable url of the site to be analyzed.</param>
	/// <param name="weight">The weight given to the website. Higher values indicate higher priority.</param>
	/// <param name="keywords">The current list of keywords. Will be modified to include new keywords and updated weights for old keywords.</param>
	internal static void AnalyzeSite(string url, double weight, ref Dictionary<string, double> keywords)
	{
		using (WebClient client = new WebClient())
		{
			string siteContents = "";
			try
			{
				siteContents = client.DownloadString(url);
			}
			catch(WebException e)
			{
				Console.WriteLine($"Error {e} while accessing {url}");
				return;
			}
			siteContents = GetPlainTextFromHtml(siteContents).ToLower();
			
			//split contents on delimiters, then only accept uncommon words.
			var words = siteContents.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries).Distinct().Where(word => !IgnoredWords.Contains(word));
			foreach (var word in words)
			{
				if (!keywords.ContainsKey(word))
				{
					keywords[word] = 0;
				}
				keywords[word] += weight;
			}
		}
	}

	/// <summary>
	/// Function taken from https://consultrikin.wordpress.com/2013/02/21/c-get-plain-text-from-html-string/
	/// Removes HTML elements from HTML code, leaving us with raw text.
	/// </summary>
	/// <param name="htmlString">The HTML to be filtered</param>
	/// <returns>htmlString with all HTML elements removed.</returns>
	internal static string GetPlainTextFromHtml(string htmlString)
	{
		if (htmlString == null || htmlString == string.Empty)
			return string.Empty;

		string htmlTagPattern = "<.*?>";
		var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		htmlString = regexCss.Replace(htmlString, string.Empty);
		htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
		htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
		htmlString = htmlString.Replace("&nbsp;", string.Empty);

		return htmlString;
	}

	private static HashSet<string> ignoredWords = null;
	/// <summary>
	/// A HashSet of ignored words, generated from the file specefied by "IgnoredWordsTxtPath" in App.config
	/// </summary>
	static HashSet<string> IgnoredWords
	{
		get
		{
			if(ignoredWords == null)
			{
				ignoredWords = LoadUniqueStringsFromTxt(ConfigurationManager.AppSettings["IgnoredWordsTxtPath"]);
			}
			return ignoredWords;
		}
	}

	/// <summary>
	/// Reads the given file and separates it into unique lines.
	/// If any duplicates are found they will be logged to the console.
	/// </summary>
	/// <param name="txtLocation">The location of the file to be parsed.</param>
	/// <returns>A HashSet containing the lines of the file. Will not contain duplicates.</returns>
	protected static HashSet<string> LoadUniqueStringsFromTxt(string txtLocation)
	{
		var linesFromFile = new HashSet<string>();

		using (System.IO.StreamReader file = new System.IO.StreamReader(txtLocation))
		{
			string line;
			while ((line = file.ReadLine()) != null)//while we still have lines to read
			{
				if (line.Length > 0)
				{
					if(!linesFromFile.Add(line.ToLower()))
					{
						Console.WriteLine($"Duplicate entry \'{line}\' found in {txtLocation}");
					}
				}
			}
		}
		return linesFromFile;
	}

	static char[] Delimiters = new char[]
	{
	' ',
	',',
	';',
	':',
	'.',
	'!',
	'?',
	'&',
	'\n',
	'\r',
	'\t',
	'\"',
	'\'',
	'(',
	')',
	'[',
	']',
	'<',
	'>',
	'/',
	'\\'
	};
}