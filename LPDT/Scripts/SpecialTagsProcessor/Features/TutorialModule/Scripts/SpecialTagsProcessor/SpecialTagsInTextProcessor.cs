using System.Collections.Generic;
using System.Text.RegularExpressions;
using Features.TutorialModule.Scripts.SpecialTagsProcessor.API;
using Features.TutorialModule.Scripts.SpecialTagsProcessor.Configurations;

namespace Features.TutorialModule.Scripts.SpecialTagsProcessor
{
	public class SpecialTagsInTextProcessor : ISpecialTagsInTextProcessor
	{
		private readonly DialogueSpecialTagsConfiguration _dialogueSpecialTagsConfiguration;

		public SpecialTagsInTextProcessor(DialogueSpecialTagsConfiguration dialogueSpecialTagsConfiguration)
		{
			_dialogueSpecialTagsConfiguration = dialogueSpecialTagsConfiguration;
		}

		public string ProcessSpecialTags(string text)
		{
			foreach (string allTag in _dialogueSpecialTagsConfiguration.GetAllTags())
			{
				string oldValue = "<" + allTag + ">";
				string oldValue2 = "</" + allTag + ">";
				TagReplacement replacementForSpecialTag = _dialogueSpecialTagsConfiguration.GetReplacementForSpecialTag(allTag);
				List<string> list = replacementForSpecialTag.Tags.ConvertAll<string>(ConvertToOpenTag);
				List<string> list2 = replacementForSpecialTag.Tags.ConvertAll<string>(ConvertToCloseTag);
				list2.Reverse();
				string newValue = string.Concat(list);
				string newValue2 = string.Concat(list2);
				text = text.Replace(oldValue, newValue);
				text = text.Replace(oldValue2, newValue2);
			}
			return text;
		}

		private string ConvertToCloseTag(string tagString)
		{
			return "</" + ExtractTagName(tagString) + ">";
		}

		private static string ExtractTagName(string tagString)
		{
			Match match = new Regex("^([a-zA-Z0-9]+)").Match(tagString);
			if (!match.Success)
			{
				return null;
			}
			return match.Groups[1].Value;
		}

		private string ConvertToOpenTag(string input)
		{
			return "<" + input + ">";
		}
	}
}
