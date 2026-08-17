using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.Vehicle.Testing
{
	[Serializable]
	public class SlotPartConfig
	{
		public bool enabled = true;

		public AssetReferenceGameObject prefab;

		[Range(0f, 100f)]
		public float condition = 100f;

		public SlotPartConfig()
		{
			enabled = true;
			condition = 100f;
		}
	}
}
