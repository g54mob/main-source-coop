using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.SpecialTagsProcessor.Configurations
{
	[Serializable]
	public class TagReplacement
	{
		[SerializeField]
		private List<string> _tags;

		public List<string> Tags => _tags;
	}
}
