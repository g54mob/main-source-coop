using System.Collections.Generic;
using UnityEngine;

namespace Features.DebugArtSelectorModule.Scripts
{
	[CreateAssetMenu(fileName = "ArtListConfiguration_Default", menuName = "Configurations/DebugArtSelectorModule/ArtListConfiguration")]
	public class ArtListConfiguration : ScriptableObject
	{
		public List<ArtConfiguration> ArtConfigurations;
	}
}
