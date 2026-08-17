using UnityEngine;

namespace GameKit.Utilities
{
	public static class Layers
	{
		public static int LayerMaskToLayerNumber(LayerMask mask)
		{
			return LayerValueToLayerNumber(mask.value);
		}

		public static int LayerValueToLayerNumber(int bitmask)
		{
			int num = ((bitmask <= 0) ? 31 : 0);
			while (bitmask > 1)
			{
				bitmask >>= 1;
				num++;
			}
			return num;
		}

		public static bool ContainsLayer(LayerMask layerMask, int layer)
		{
			return (int)layerMask == ((int)layerMask | (1 << layer));
		}
	}
}
