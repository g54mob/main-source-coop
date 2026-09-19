using Features.TipsModule.Scripts.Data;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TipsModule.Scripts.Display
{
	[CreateAssetMenu(fileName = "TipsConfiguration_Default", menuName = "Configurations/TipsModule/TipsConfiguration")]
	public class TipsConfiguration : ScriptableObject
	{
		public SerializableDictionary<TipType, TipConfigEntry> TipConfigurationData = new SerializableDictionary<TipType, TipConfigEntry>();

		public bool TryGetDisplay(TipType tipType, out TipDisplayDataBase display)
		{
			display = null;
			if (!TipConfigurationData.TryGetValue(tipType, out var value) || value?.Display == null)
			{
				return false;
			}
			display = value.Display;
			return true;
		}
	}
}
