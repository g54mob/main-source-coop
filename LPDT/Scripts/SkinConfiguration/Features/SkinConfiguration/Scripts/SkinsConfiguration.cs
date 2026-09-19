using System.Collections.Generic;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	[CreateAssetMenu(fileName = "SkinsConfiguration_Default", menuName = "Configurations/SkinsModule/SkinsConfiguration")]
	public class SkinsConfiguration : ScriptableObject
	{
		public List<SkinType> StartedPossibleSkinTypes;

		public SerializableDictionary<SkinType, PlayerSkin> PlayerSkins;
	}
}
