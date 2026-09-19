using System.Collections.Generic;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	[CreateAssetMenu(fileName = "RumIconsConfiguration_Default", menuName = "Configurations/Rum/RumIconsConfiguration")]
	public class RumIconsConfiguration : ScriptableObject
	{
		[SerializeField]
		private List<RumIconEntry> _entries = new List<RumIconEntry>();

		public Sprite GetIcon(RumType rumType)
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				if (_entries[i].RumType == rumType)
				{
					return _entries[i].Icon;
				}
			}
			return null;
		}
	}
}
