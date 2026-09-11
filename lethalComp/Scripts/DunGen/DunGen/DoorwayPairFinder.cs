using System;
using System.Collections.Generic;
using System.Linq;
using DunGen.Graph;
using UnityEngine;

namespace DunGen
{
	public sealed class DoorwayPairFinder
	{
		public static readonly List<TileConnectionRule> CustomConnectionRules = new List<TileConnectionRule>();

		public RandomStream RandomStream;

		public List<GameObjectChance> TileWeights;

		public TileProxy PreviousTile;

		public bool IsOnMainPath;

		public float NormalizedDepth;

		public TilePlacementParameters PlacementParameters;

		public bool? AllowRotation;

		public Vector3 UpVector;

		public TileMatchDelegate IsTileAllowedPredicate;

		public GetTileTemplateDelegate GetTileTemplateDelegate;

		public DungeonFlow DungeonFlow;

		public DungeonProxy DungeonProxy;

		private Vector3? currentPathDirection;

		private bool shouldStraightenNextConnection;

		private List<GameObjectChance> tileOrder;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ResetStatics()
		{
			CustomConnectionRules.Clear();
		}

		private static int CompareConnectionRules(TileConnectionRule a, TileConnectionRule b)
		{
			return b.Priority.CompareTo(a.Priority);
		}

		public static void SortCustomConnectionRules()
		{
			CustomConnectionRules.Sort(CompareConnectionRules);
		}

		public Queue<DoorwayPair> GetDoorwayPairs(int? maxCount)
		{
			tileOrder = CalculateOrderedListOfTiles();
			shouldStraightenNextConnection = CalculateShouldStraightenNextConnection();
			if (shouldStraightenNextConnection)
			{
				currentPathDirection = CalculateCurrentPathDirection();
			}
			if (!currentPathDirection.HasValue)
			{
				shouldStraightenNextConnection = false;
			}
			List<DoorwayPair> list = ((PreviousTile != null) ? GetPotentialDoorwayPairsForNonFirstTile().ToList() : GetPotentialDoorwayPairsForFirstTile().ToList());
			int num = list.Count;
			if (maxCount.HasValue)
			{
				num = Math.Min(num, maxCount.Value);
			}
			return new Queue<DoorwayPair>(OrderDoorwayPairs(list).Take(num));
		}

		private bool CalculateShouldStraightenNextConnection()
		{
			PathStraighteningSettings pathStraighteningSettings = null;
			if (PlacementParameters.Archetype != null)
			{
				pathStraighteningSettings = PlacementParameters.Archetype.StraighteningSettings;
			}
			else if (PlacementParameters.Node != null)
			{
				pathStraighteningSettings = PlacementParameters.Node.StraighteningSettings;
				pathStraighteningSettings.CanStraightenMainPath = true;
				pathStraighteningSettings.CanStraightenBranchPaths = false;
			}
			if (pathStraighteningSettings == null)
			{
				return false;
			}
			pathStraighteningSettings = PathStraighteningSettings.GetFinalValues(pathStraighteningSettings, DungeonFlow.GlobalStraighteningSettings);
			if (IsOnMainPath && !pathStraighteningSettings.CanStraightenMainPath)
			{
				return false;
			}
			if (!IsOnMainPath && !pathStraighteningSettings.CanStraightenBranchPaths)
			{
				return false;
			}
			return RandomStream.NextDouble() < (double)pathStraighteningSettings.StraightenChance;
		}

		private Vector3? CalculateCurrentPathDirection()
		{
			if (PreviousTile == null || !shouldStraightenNextConnection)
			{
				return null;
			}
			if (IsOnMainPath)
			{
				float num = PreviousTile.Placement.PathDepth;
				foreach (DoorwayProxy usedDoorway in PreviousTile.UsedDoorways)
				{
					if ((float)usedDoorway.ConnectedDoorway.TileProxy.Placement.PathDepth < num)
					{
						return -usedDoorway.Forward;
					}
				}
			}
			else
			{
				if (PreviousTile.Placement.IsOnMainPath)
				{
					return null;
				}
				float num2 = PreviousTile.Placement.BranchDepth;
				foreach (DoorwayProxy usedDoorway2 in PreviousTile.UsedDoorways)
				{
					TileProxy tileProxy = usedDoorway2.ConnectedDoorway.TileProxy;
					if (tileProxy.Placement.IsOnMainPath || (float)tileProxy.Placement.BranchDepth < num2)
					{
						return -usedDoorway2.Forward;
					}
				}
			}
			return null;
		}

		private IEnumerable<DoorwayPair> OrderDoorwayPairs(List<DoorwayPair> potentialPairs)
		{
			potentialPairs.Sort(delegate(DoorwayPair a, DoorwayPair b)
			{
				int num = b.TileWeight.CompareTo(a.TileWeight);
				return (num == 0) ? b.DoorwayWeight.CompareTo(a.DoorwayWeight) : num;
			});
			return potentialPairs;
		}

		private List<GameObjectChance> CalculateOrderedListOfTiles()
		{
			List<GameObjectChance> list = new List<GameObjectChance>(TileWeights.Count);
			GameObjectChanceTable gameObjectChanceTable = new GameObjectChanceTable();
			gameObjectChanceTable.Weights.AddRange(TileWeights);
			while (gameObjectChanceTable.Weights.Any((GameObjectChance x) => x.Value != null && x.GetWeight(IsOnMainPath, NormalizedDepth) > 0f))
			{
				list.Add(gameObjectChanceTable.GetRandom(RandomStream, IsOnMainPath, NormalizedDepth, null, allowImmediateRepeats: true, removeFromTable: true));
			}
			return list;
		}

		private IEnumerable<DoorwayPair> GetPotentialDoorwayPairsForNonFirstTile()
		{
			foreach (DoorwayProxy previousDoor in PreviousTile.UnusedDoorways)
			{
				if (previousDoor.IsDisabled)
				{
					continue;
				}
				IEnumerable<DoorwayProxy> source = PreviousTile.UnusedDoorways.Intersect(PreviousTile.Exits);
				PreviousTile.UnusedDoorways.ToArray();
				if (source.Any() && !source.Contains(previousDoor))
				{
					continue;
				}
				foreach (GameObjectChance tileWeight in TileWeights)
				{
					if (!tileOrder.Contains(tileWeight))
					{
						continue;
					}
					TileProxy nextTile = GetTileTemplateDelegate(tileWeight.Value);
					float weight = tileOrder.Count - tileOrder.IndexOf(tileWeight);
					if (IsTileAllowedPredicate != null && !IsTileAllowedPredicate(PreviousTile, nextTile, ref weight))
					{
						continue;
					}
					foreach (DoorwayProxy doorway in nextTile.Doorways)
					{
						if ((!nextTile.Entrances.Any() || nextTile.Entrances.Contains(doorway)) && (nextTile == null || nextTile.Exits.Count != 1 || !nextTile.Exits.Contains(doorway)))
						{
							float weight2 = 0f;
							if (IsValidDoorwayPairing(previousDoor, doorway, PreviousTile, nextTile, ref weight2))
							{
								yield return new DoorwayPair(PreviousTile, previousDoor, nextTile, doorway, tileWeight.TileSet, weight, weight2);
							}
						}
					}
				}
			}
		}

		private IEnumerable<DoorwayPair> GetPotentialDoorwayPairsForFirstTile()
		{
			foreach (GameObjectChance tileWeight in TileWeights)
			{
				if (!tileOrder.Contains(tileWeight))
				{
					continue;
				}
				TileProxy nextTile = GetTileTemplateDelegate(tileWeight.Value);
				float weight = tileWeight.GetWeight(IsOnMainPath, NormalizedDepth) * (float)RandomStream.NextDouble();
				if (IsTileAllowedPredicate != null && !IsTileAllowedPredicate(PreviousTile, nextTile, ref weight))
				{
					continue;
				}
				foreach (DoorwayProxy doorway in nextTile.Doorways)
				{
					ProposedConnection connection = new ProposedConnection(DungeonProxy, null, nextTile, null, doorway);
					float doorwayWeight = CalculateConnectionWeight(connection);
					yield return new DoorwayPair(null, null, nextTile, doorway, tileWeight.TileSet, weight, doorwayWeight);
				}
			}
		}

		private bool IsValidDoorwayPairing(DoorwayProxy previousDoorway, DoorwayProxy nextDoorway, TileProxy previousTile, TileProxy nextTile, ref float weight)
		{
			ProposedConnection connection = new ProposedConnection(DungeonProxy, previousTile, nextTile, previousDoorway, nextDoorway);
			if (!DungeonFlow.CanDoorwaysConnect(connection))
			{
				return false;
			}
			Vector3? vector = null;
			bool flag = (AllowRotation.HasValue && !AllowRotation.Value) || (nextTile != null && !nextTile.PrefabTile.AllowRotation);
			if (Vector3.Angle(previousDoorway.Forward, UpVector) < 1f)
			{
				vector = -UpVector;
			}
			else if (Vector3.Angle(previousDoorway.Forward, -UpVector) < 1f)
			{
				vector = UpVector;
			}
			else if (flag)
			{
				vector = -previousDoorway.Forward;
			}
			if (vector.HasValue && Vector3.Angle(vector.Value, nextDoorway.Forward) > 1f)
			{
				return false;
			}
			weight = CalculateConnectionWeight(connection);
			return weight > 0f;
		}

		private float CalculateConnectionWeight(ProposedConnection connection)
		{
			float result = (float)RandomStream.NextDouble();
			if (shouldStraightenNextConnection && currentPathDirection.HasValue && connection.PreviousDoorway != null && Vector3.Dot(currentPathDirection.Value, connection.PreviousDoorway.Forward) < 0.99f)
			{
				result = 0f;
			}
			return result;
		}
	}
}
