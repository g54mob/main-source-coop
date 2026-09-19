using UnityEngine;

namespace Obi
{
	public class ObiHeightFieldHandle : ObiResourceHandle<TerrainData>
	{
		public ObiHeightFieldHandle(TerrainData data, int index = -1)
			: base(index)
		{
			owner = data;
		}
	}
}
