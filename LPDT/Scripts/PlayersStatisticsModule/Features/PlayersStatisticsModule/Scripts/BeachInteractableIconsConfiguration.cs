using System.Collections.Generic;
using Features.BeachInteractableCommonModule.Scripts;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts
{
	[CreateAssetMenu(fileName = "BeachInteractableIconsConfiguration_Default", menuName = "Configurations/PlayersStatistics/BeachInteractableIconsConfiguration")]
	public class BeachInteractableIconsConfiguration : ScriptableObject
	{
		[SerializeField]
		private List<BeachInteractableIconEntry> _entries = new List<BeachInteractableIconEntry>();

		public bool TryGetIcon(BeachInteractableType beachInteractableType, out Sprite icon)
		{
			icon = null;
			for (int i = 0; i < _entries.Count; i++)
			{
				if (_entries[i].BeachInteractableType == beachInteractableType)
				{
					icon = _entries[i].Icon;
					return true;
				}
			}
			return false;
		}
	}
}
