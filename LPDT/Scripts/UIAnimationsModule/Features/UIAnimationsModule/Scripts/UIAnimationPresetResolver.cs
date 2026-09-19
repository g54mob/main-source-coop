using System.Collections.Generic;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	public class UIAnimationPresetResolver : IUIAnimationPresetResolver
	{
		private readonly Dictionary<UIAnimationKey, UIAnimationPreset> _byKey;

		public UIAnimationPresetResolver(UIAnimationsConfig config)
		{
			_byKey = new Dictionary<UIAnimationKey, UIAnimationPreset>();
			for (int i = 0; i < config.Entries.Count; i++)
			{
				UIAnimationsConfig.Entry entry = config.Entries[i];
				if (entry.Preset == null)
				{
					Debug.LogWarning($"[UIAnimations] Entry with key {entry.Key} has null preset; skipped.");
				}
				else if (_byKey.ContainsKey(entry.Key))
				{
					Debug.LogWarning($"[UIAnimations] Duplicate entry for key {entry.Key}; first one is used.");
				}
				else
				{
					_byKey.Add(entry.Key, entry.Preset);
				}
			}
		}

		public UIAnimationPreset Get(UIAnimationKey key)
		{
			if (_byKey.TryGetValue(key, out var value))
			{
				return value;
			}
			Debug.LogError($"[UIAnimations] No preset configured for key {key}.");
			return null;
		}

		public bool TryGet(UIAnimationKey key, out UIAnimationPreset preset)
		{
			return _byKey.TryGetValue(key, out preset);
		}
	}
}
