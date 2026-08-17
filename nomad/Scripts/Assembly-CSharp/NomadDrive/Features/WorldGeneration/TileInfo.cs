using System;
using System.Collections.Generic;
using MapMagic.Products;
using MapMagic.Terrains;
using NomadDrive.Features.EvilRoads;
using NomadDrive.Features.WorldGeneration.POISpawning;
using UnityEngine.Serialization;

namespace NomadDrive.Features.WorldGeneration
{
	[Serializable]
	public class TileInfo
	{
		public TerrainTile tile;

		public TileData Data;

		public bool isRoadGenerated;

		[FormerlySerializedAs("Road")]
		public EvilRoad road;

		public List<EvilRoad> branchRoads = new List<EvilRoad>();

		public List<Poi> spawnedPois = new List<Poi>();

		public float roadStartPointX;

		public float roadEndPointX;

		public bool hasRoadStartPoint;

		public bool hasRoadEndPoint;

		public bool isRoadSimplified;

		public float distanceToCamera;
	}
}
