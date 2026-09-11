using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen
{
	[Serializable]
	[CreateAssetMenu(fileName = "New Archetype", menuName = "DunGen/Dungeon Archetype", order = 700)]
	public sealed class DungeonArchetype : ScriptableObject, ISerializationCallbackReceiver
	{
		[Obsolete("StraightenChance is deprecated. Use StraighteningSettings instead")]
		public float StraightenChance;

		public static int CurrentFileVersion = 1;

		public List<TileSet> TileSets = new List<TileSet>();

		public List<TileSet> BranchStartTileSets = new List<TileSet>();

		public BranchCapType BranchStartType;

		public List<TileSet> BranchCapTileSets = new List<TileSet>();

		public BranchCapType BranchCapType = BranchCapType.AsWellAs;

		public IntRange BranchingDepth = new IntRange(2, 4);

		public IntRange BranchCount = new IntRange(0, 2);

		public PathStraighteningSettings StraighteningSettings = new PathStraighteningSettings();

		public bool Unique;

		[SerializeField]
		private int fileVersion;

		public bool GetHasValidBranchStartTiles()
		{
			if (BranchStartTileSets.Count == 0)
			{
				return false;
			}
			foreach (TileSet branchStartTileSet in BranchStartTileSets)
			{
				if (branchStartTileSet.TileWeights.Weights.Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool GetHasValidBranchCapTiles()
		{
			if (BranchCapTileSets.Count == 0)
			{
				return false;
			}
			foreach (TileSet branchCapTileSet in BranchCapTileSets)
			{
				if (branchCapTileSet.TileWeights.Weights.Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			if (!(this == null) && fileVersion < 1)
			{
				if (StraighteningSettings == null)
				{
					StraighteningSettings = new PathStraighteningSettings();
				}
				if (StraightenChance > 0f)
				{
					StraighteningSettings.StraightenChance = Mathf.Clamp01(StraightenChance);
					StraighteningSettings.OverrideStraightenChance = true;
				}
				fileVersion = 1;
			}
		}
	}
}
