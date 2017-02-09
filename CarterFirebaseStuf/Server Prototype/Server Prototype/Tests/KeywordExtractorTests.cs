using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KeywordExtractorTests
{
	[TestClass]
	public class KeywordExtractorTests
	{
		[TestMethod]
		public void FunctionName_Preconditions_Result()
		{
			Assert.AreEqual(1, 1);
			
		}

		[TestMethod]
		public void GetPlainTextFromHtml_NullString_ReturnsBlankString()
		{
			var result = KeywordExtractor.GetPlainTextFromHtml(null);
			Assert.AreEqual(string.Empty, result, "Incorrect string returned.");
		}

		[TestMethod]
		public void GetPlainTextFromHtml_BlankString_ReturnsBlankString()
		{
			var result = KeywordExtractor.GetPlainTextFromHtml("");
			Assert.AreEqual(string.Empty, result, "Incorrect string returned.");
		}

		[TestMethod]
		public void GetPlainTextFromHtml_NonHtmlStringContainsHtmlCharacters_ReturnsSameString()
		{
			var testString = "How about a nice game of chess? 5 is > 3";
			var result = KeywordExtractor.GetPlainTextFromHtml(testString);
			Assert.AreEqual(testString, result, "String should not be modified.");
		}

		[TestMethod]
		public void GetPlainTextFromHtml_HtmlStringContainsHtmlCharacters_RemovesHtml()
		{
			var testString = "<body><h1/>How about a <i>nice</i> game of <b>chess</b>? 5 is > 3</body>";
			var testStringWithoutHtml = "How about a nice game of chess? 5 is > 3";
			var result = KeywordExtractor.GetPlainTextFromHtml(testString);
			Assert.AreEqual(testStringWithoutHtml, result, "String should not be modified.");
		}

		[TestMethod]
		public void AnalyzeSite_NonExistentWebPage_PerformsNoChanges()
		{
			var keywords = new Dictionary<string, double>();
			KeywordExtractor.AnalyzeSite("https://notarealwebsite.com/AndrewDorn", 1, ref keywords);
			Assert.AreEqual(0, keywords.Count, "Unexpected element in keywords.");
		}
	}
}
