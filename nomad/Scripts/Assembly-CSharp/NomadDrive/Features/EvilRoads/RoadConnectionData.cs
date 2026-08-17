using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[Serializable]
	public class RoadConnectionData
	{
		[SerializeField]
		private RoadConnectionPoint connectionPoint1;

		[SerializeField]
		private RoadConnectionPoint connectionPoint2;

		[SerializeField]
		private EvilRoad connectedRoad;

		[SerializeField]
		private List<EvilRoad> originalRoads;

		[SerializeField]
		private Vector3 connectionPosition;

		[SerializeField]
		private bool isActive;

		private Dictionary<EvilRoad, Dictionary<string, List<GameObject>>> originalRoadObjects;

		public RoadConnectionPoint ConnectionPoint1 => connectionPoint1;

		public RoadConnectionPoint ConnectionPoint2 => connectionPoint2;

		public EvilRoad ConnectedRoad => connectedRoad;

		public List<EvilRoad> OriginalRoads => originalRoads;

		public Vector3 ConnectionPosition => connectionPosition;

		public bool IsActive => isActive;

		public RoadConnectionData(RoadConnectionPoint point1, RoadConnectionPoint point2)
		{
			connectionPoint1 = point1;
			connectionPoint2 = point2;
			originalRoads = new List<EvilRoad> { point1.OwnerRoad, point2.OwnerRoad };
			connectionPosition = Vector3.Lerp(point1.Position, point2.Position, 0.5f);
			isActive = false;
			originalRoadObjects = new Dictionary<EvilRoad, Dictionary<string, List<GameObject>>>();
		}

		public void SetConnectedRoad(EvilRoad road)
		{
			connectedRoad = road;
			isActive = true;
		}

		public void StoreOriginalRoadObjects(EvilRoad road, Dictionary<string, List<GameObject>> roadObjects)
		{
			if (roadObjects == null)
			{
				return;
			}
			originalRoadObjects[road] = new Dictionary<string, List<GameObject>>();
			foreach (KeyValuePair<string, List<GameObject>> roadObject in roadObjects)
			{
				originalRoadObjects[road][roadObject.Key] = new List<GameObject>(roadObject.Value);
			}
		}

		public void TransferRoadObjectsToMergedRoad()
		{
			if (connectedRoad == null)
			{
				return;
			}
			foreach (KeyValuePair<EvilRoad, Dictionary<string, List<GameObject>>> originalRoadObject in originalRoadObjects)
			{
				foreach (KeyValuePair<string, List<GameObject>> item in originalRoadObject.Value)
				{
					foreach (GameObject item2 in item.Value)
					{
						if (item2 != null)
						{
							item2.transform.SetParent(connectedRoad.transform);
						}
					}
				}
			}
		}

		public void RestoreOriginalRoadObjects()
		{
			foreach (KeyValuePair<EvilRoad, Dictionary<string, List<GameObject>>> originalRoadObject in originalRoadObjects)
			{
				EvilRoad key = originalRoadObject.Key;
				if (key == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, List<GameObject>> item in originalRoadObject.Value)
				{
					foreach (GameObject item2 in item.Value)
					{
						if (item2 != null)
						{
							item2.transform.SetParent(key.transform);
						}
					}
				}
			}
		}

		public Dictionary<string, List<GameObject>> GetStoredRoadObjects(EvilRoad road)
		{
			if (originalRoadObjects.TryGetValue(road, out var value))
			{
				return value;
			}
			return null;
		}

		public bool InvolvesRoad(EvilRoad road)
		{
			if (!originalRoads.Contains(road))
			{
				return connectedRoad == road;
			}
			return true;
		}

		public void Disconnect()
		{
			RestoreOriginalRoadObjects();
			isActive = false;
			connectedRoad = null;
		}

		public RoadConnectionPoint GetConnectionPointForRoad(EvilRoad road)
		{
			if (connectionPoint1?.OwnerRoad == road)
			{
				return connectionPoint1;
			}
			if (connectionPoint2?.OwnerRoad == road)
			{
				return connectionPoint2;
			}
			return null;
		}

		public EvilRoad GetOtherRoad(EvilRoad road)
		{
			if (connectionPoint1?.OwnerRoad == road)
			{
				return connectionPoint2?.OwnerRoad;
			}
			if (connectionPoint2?.OwnerRoad == road)
			{
				return connectionPoint1?.OwnerRoad;
			}
			return null;
		}

		public float GetConnectionDistance()
		{
			return connectionPoint1.DistanceTo(connectionPoint2);
		}
	}
}
