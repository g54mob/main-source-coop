using System.Collections.Generic;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.SpecialTagsProcessor.Configurations
{
	[CreateAssetMenu(fileName = "DialogueSpecialTagsConfiguration_Default", menuName = "Configurations/DialoguesModule/DialogueSpecialTagsConfiguration")]
	public class DialogueSpecialTagsConfiguration : ScriptableObject
	{
		[SerializeField]
		private string _optionalChunkSeparator = "optional";

		[SerializeField]
		private string _mandatoryChunkSeparator = "mandatory";

		[SerializeField]
		private SerializableDictionary<string, TagReplacement> _specialTags;

		public string OptionalChunkSeparator => _optionalChunkSeparator;

		public string MandatoryChunkSeparator => _mandatoryChunkSeparator;

		public List<string> GetAllTags()
		{
			return new List<string>(_specialTags.Keys);
		}

		public TagReplacement GetReplacementForSpecialTag(string tag)
		{
			return _specialTags[tag];
		}
	}
}
