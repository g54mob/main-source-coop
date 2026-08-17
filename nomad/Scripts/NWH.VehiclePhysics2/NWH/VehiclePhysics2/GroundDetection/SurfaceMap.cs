using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.GroundDetection
{
	[Serializable]
	public class SurfaceMap
	{
		[Tooltip("    Name of the surface map. For display purposes only.")]
		public string name;

		public SurfacePreset surfacePreset;

		[Tooltip("    Objects with tags in this list will be recognized as this type of surface.")]
		public List<string> tags = new List<string>();

		[Tooltip("Indices of terrain textures that represent this type of surface. Starts with 0 with the first texture being in the top left corner under terrain settings - Paint Texture.")]
		public List<int> terrainTextureIndices = new List<int>();
	}
}
