using System;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	public abstract class BaseObjectsOutput : OutputGenerator
	{
		public enum BiomeBlend
		{
			Sharp = 0,
			Random = 1,
			Scale = 2,
			Pure = 3
		}

		public string name = "(Empty)";

		public GameObject[] prefabs = new GameObject[1];

		public bool guiMultiprefab;

		public bool guiProperties;

		public BiomeBlend biomeBlend = BiomeBlend.Random;

		public bool objHeight = true;

		public bool relativeHeight = true;

		public bool useRotation = true;

		public bool takeTerrainNormal;

		public bool rotateYonly;

		public bool regardPrefabRotation;

		public bool useScale = true;

		public bool scaleYonly;

		public bool regardPrefabScale;
	}
}
