using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration
{
	internal static class WorldGenerationLayerUtility
	{
		public static void ApplyLayerSafe(GameObject go, string layerName)
		{
			if (!(go == null) && !string.IsNullOrEmpty(layerName))
			{
				int num = LayerMask.NameToLayer(layerName);
				if (num < 0)
				{
					EvilLogger.LogError($"[WorldGenerationLayerUtility] Layer '{layerName}' is not defined in TagManager. GO '{go.name}' left on layer {go.layer}.", "ApplyLayerSafe", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerationLayerUtility.cs", 14);
				}
				else
				{
					go.layer = num;
				}
			}
		}
	}
}
