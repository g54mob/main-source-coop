using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	[CreateAssetMenu(fileName = "UIAnimationsConfig_Default", menuName = "Configurations/UIAnimations/UIAnimationsConfig")]
	public class UIAnimationsConfig : ScriptableObject
	{
		[Serializable]
		public struct Entry
		{
			[SerializeField]
			private UIAnimationKey _key;

			[SerializeField]
			private UIAnimationPreset _preset;

			public UIAnimationKey Key => _key;

			public UIAnimationPreset Preset => _preset;
		}

		[SerializeField]
		private Entry[] _entries = Array.Empty<Entry>();

		public IReadOnlyList<Entry> Entries => _entries;
	}
}
