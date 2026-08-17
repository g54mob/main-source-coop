using System;
using MapMagic.Nodes.MatrixGenerators;
using UnityEngine;

namespace MapMagic.Core
{
	[Serializable]
	public class Globals
	{
		public enum MicroSplatApplyType
		{
			Textures = 0,
			Splats = 1,
			Both = 2
		}

		public float height = 250f;

		public HeightOutput200.Interpolation heightInterpolation;

		public int heightSplit = 129;

		public HeightOutput200.ApplyType heightMainApply = HeightOutput200.ApplyType.TextureToHeightmap;

		public HeightOutput200.ApplyType heightDraftApply;

		public int grassResDownscale = 1;

		public int grassResPerPatch = 16;

		public DetailScatterMode grassScatterMode = DetailScatterMode.InstanceCountMode;

		public int objectsNumPerFrame = 500;

		public int holesRes = 64;

		public string[] customControlTextureNames = new string[1] { "_ControlTexture1" };

		public UnityEngine.Object ctsProfile;

		public UnityEngine.Object microSplatPropData;

		public bool microSplatTerrainDescriptor;

		public MicroSplatApplyType microSplatApplyType;

		public bool useCustomControlTextures;

		public bool microSplatNormals;

		public UnityEngine.Object microSplatTexArrConfig;

		public UnityEngine.Object megaSplatTexList;

		public UnityEngine.Object vegetationPackage;

		public bool assignComponent;

		public UnityEngine.Object vegetationSystem;

		public bool vegetationSystemCopy = true;

		public string sourceMultiPackageVsProTagName;

		public Globals Clone()
		{
			return MemberwiseClone() as Globals;
		}
	}
}
