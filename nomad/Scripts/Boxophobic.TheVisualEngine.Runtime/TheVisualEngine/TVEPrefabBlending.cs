using System;
using Boxophobic.StyledGUI;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEPrefabBlending
	{
		[StyledEnum("NULL", "Off 0 Auto_Terrain_Blending 1", 0, 0)]
		public int blendMode;
	}
}
