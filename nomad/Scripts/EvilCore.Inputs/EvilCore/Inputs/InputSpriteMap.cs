using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Inputs
{
	[CreateAssetMenu(menuName = "EvilCore/Inputs/Input Sprite Map", fileName = "InputSpriteMap")]
	public class InputSpriteMap : ScriptableObject
	{
		[Serializable]
		public class SpriteEntry
		{
			[Tooltip("Rewired element identifier name (e.g., 'E', 'Cross', 'A', 'Left Bumper')")]
			public string elementName;

			public Sprite sprite;
		}

		[SerializeField]
		private List<SpriteEntry> entries = new List<SpriteEntry>();

		private Dictionary<string, Sprite> _lookup;

		public Sprite GetSprite(string elementIdentifierName)
		{
			if (_lookup == null)
			{
				BuildLookup();
			}
			if (!_lookup.TryGetValue(elementIdentifierName, out var value))
			{
				return null;
			}
			return value;
		}

		private void BuildLookup()
		{
			_lookup = new Dictionary<string, Sprite>(entries.Count);
			foreach (SpriteEntry entry in entries)
			{
				if (!string.IsNullOrEmpty(entry.elementName))
				{
					_lookup[entry.elementName] = entry.sprite;
				}
			}
		}

		private void OnEnable()
		{
			_lookup = null;
		}
	}
}
