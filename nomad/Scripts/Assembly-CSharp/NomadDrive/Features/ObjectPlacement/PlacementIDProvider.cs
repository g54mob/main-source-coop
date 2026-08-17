using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class PlacementIDProvider
	{
		public static string GetPlacementID(GameObject obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			if (obj.TryGetComponent<LootItemDefinition>(out var component))
			{
				return component.DisplayName;
			}
			string text = obj.name;
			if (text.EndsWith("(Clone)"))
			{
				text = text.Substring(0, text.Length - 7).Trim();
			}
			return text;
		}
	}
}
