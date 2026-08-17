using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration
{
	internal static class WorldGenerationTagUtility
	{
		public static void ApplyTagSafe(GameObject go, string tag)
		{
			if (go == null || string.IsNullOrEmpty(tag))
			{
				return;
			}
			try
			{
				go.tag = tag;
			}
			catch (UnityException)
			{
				EvilLogger.LogError("[WorldGenerationTagUtility] Tag '" + tag + "' is not defined in TagManager. GO '" + go.name + "' left untagged.", "ApplyTagSafe", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerationTagUtility.cs", 17);
			}
		}
	}
}
