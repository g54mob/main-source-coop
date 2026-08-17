using System;
using Den.Tools;
using MapMagic.Nodes;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.VegetationStudio
{
	[Serializable]
	[GeneratorMenu(name = "VS Pro Objs", section = 2, drawButtons = false, colorType = typeof(TransitionsList))]
	public class VSProObjectsOut : OutputGenerator, IInlet<TransitionsList>, IUnit
	{
		[Serializable]
		public class Layer
		{
			public string id;

			public string lastUsedName;

			public string lastUsedType;
		}

		public OutputLevel outputLevel = OutputLevel.Main;

		public PositioningSettings posSettings;

		public BiomeBlend biomeBlend = BiomeBlend.Random;

		public Layer[] layers = new Layer[1]
		{
			new Layer()
		};

		public const byte VS_MM_id = 15;

		public bool objHeight = true;

		public bool relativeHeight = true;

		public bool guiHeight;

		public bool useRotation = true;

		public bool takeTerrainNormal;

		public bool rotateYonly;

		public bool regardPrefabRotation;

		public bool guiRotation;

		public bool useScale = true;

		public bool scaleYonly;

		public bool regardPrefabScale;

		public bool guiScale;

		public override OutputLevel OutputLevel => outputLevel;

		public PositioningSettings CreatePosSettings()
		{
			PositioningSettings obj = new PositioningSettings
			{
				objHeight = objHeight,
				relativeHeight = relativeHeight,
				guiHeight = guiHeight,
				useRotation = useRotation,
				takeTerrainNormal = takeTerrainNormal,
				rotateYonly = rotateYonly,
				regardPrefabRotation = regardPrefabRotation,
				guiRotation = guiRotation,
				useScale = useScale,
				scaleYonly = scaleYonly,
				regardPrefabScale = regardPrefabScale,
				guiScale = guiScale
			};
			PositioningSettings result = obj;
			posSettings = obj;
			return result;
		}

		public override void Generate(TileData data, StopToken stop)
		{
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
