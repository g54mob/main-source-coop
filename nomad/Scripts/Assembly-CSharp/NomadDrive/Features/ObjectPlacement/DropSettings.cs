using System;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[Serializable]
	public struct DropSettings
	{
		public LayerMask GroundLayers;

		public float MaxDistance;

		public float SurfaceOffset;

		public bool AlignToSurface;

		public float ForwardOffset;

		public static DropSettings Default => new DropSettings
		{
			GroundLayers = -1,
			MaxDistance = 50f,
			SurfaceOffset = 0.05f,
			AlignToSurface = true,
			ForwardOffset = 1.5f
		};
	}
}
