using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadConnectionManager : MonoBehaviour
	{
		[Header("Connection Settings")]
		[SerializeField]
		private float connectionDistance = 5f;

		[SerializeField]
		private bool enableDebugVisualization = true;

		[SerializeField]
		private Color connectionPointColor = Color.green;

		[SerializeField]
		private Color connectedColor = Color.blue;

		[Header("Debug")]
		[SerializeField]
		private EvilRoadsManager roadsManager;

		private List<RoadConnectionData> activeConnections = new List<RoadConnectionData>();

		private Dictionary<EvilRoad, List<RoadConnectionPoint>> roadConnectionPoints = new Dictionary<EvilRoad, List<RoadConnectionPoint>>();

		public float ConnectionDistance => connectionDistance;

		public void RegisterRoad(EvilRoad road)
		{
			if (!(road == null))
			{
				road.SetConnectionManager(this);
				List<RoadConnectionPoint> value = CreateConnectionPointsForRoad(road);
				roadConnectionPoints[road] = value;
			}
		}

		public void UnregisterRoad(EvilRoad road)
		{
			if (!(road == null))
			{
				DisconnectAllConnectionsForRoad(road);
				roadConnectionPoints.Remove(road);
			}
		}

		private void RemoveRoadFromConnectionManager(EvilRoad road)
		{
			if (!(road == null) && roadConnectionPoints.ContainsKey(road))
			{
				roadConnectionPoints.Remove(road);
			}
		}

		private List<RoadConnectionPoint> CreateConnectionPointsForRoad(EvilRoad road)
		{
			List<RoadConnectionPoint> list = new List<RoadConnectionPoint>();
			Spline spline = road.GetSpline();
			if (spline == null || spline.Count == 0)
			{
				return list;
			}
			BezierKnot bezierKnot = spline[0];
			Vector3 normalized = ((Vector3)bezierKnot.TangentOut).normalized;
			if (normalized == Vector3.zero && spline.Count > 1)
			{
				normalized = ((Vector3)(spline[1].Position - bezierKnot.Position)).normalized;
			}
			RoadConnectionPoint item = new RoadConnectionPoint(bezierKnot.Position, normalized, road, isStartPoint: true, 0);
			list.Add(item);
			BezierKnot bezierKnot2 = spline[spline.Count - 1];
			Vector3 vector = -((Vector3)bezierKnot2.TangentIn).normalized;
			if (vector == Vector3.zero && spline.Count > 1)
			{
				vector = ((Vector3)(bezierKnot2.Position - spline[spline.Count - 2].Position)).normalized;
			}
			RoadConnectionPoint item2 = new RoadConnectionPoint(bezierKnot2.Position, vector, road, isStartPoint: false, spline.Count - 1);
			list.Add(item2);
			return list;
		}

		private bool CanConnect(RoadConnectionPoint point1, RoadConnectionPoint point2)
		{
			if (point1.DistanceTo(point2) > connectionDistance)
			{
				return false;
			}
			if (!point1.IsCompatibleWith(point2))
			{
				return false;
			}
			if (AreRoadsConnected(point1.OwnerRoad, point2.OwnerRoad))
			{
				return false;
			}
			return true;
		}

		private void CreateConnection(RoadConnectionPoint point1, RoadConnectionPoint point2)
		{
			RoadConnectionData roadConnectionData = new RoadConnectionData(point1, point2);
			Dictionary<string, List<GameObject>> placedRoadObjects = point1.OwnerRoad.GetPlacedRoadObjects();
			Dictionary<string, List<GameObject>> placedRoadObjects2 = point2.OwnerRoad.GetPlacedRoadObjects();
			if (placedRoadObjects != null)
			{
				roadConnectionData.StoreOriginalRoadObjects(point1.OwnerRoad, placedRoadObjects);
			}
			if (placedRoadObjects2 != null)
			{
				roadConnectionData.StoreOriginalRoadObjects(point2.OwnerRoad, placedRoadObjects2);
			}
			EvilRoad evilRoad = CreateMergedRoad(roadConnectionData);
			if (evilRoad != null)
			{
				roadConnectionData.SetConnectedRoad(evilRoad);
				roadConnectionData.TransferRoadObjectsToMergedRoad();
				point1.OwnerRoad.ClearPlacedRoadObjects();
				point2.OwnerRoad.ClearPlacedRoadObjects();
				activeConnections.Add(roadConnectionData);
				point1.OwnerRoad.SetActive(active: false);
				point2.OwnerRoad.SetActive(active: false);
				RemoveRoadFromConnectionManager(point1.OwnerRoad);
				RemoveRoadFromConnectionManager(point2.OwnerRoad);
				RegisterRoad(evilRoad);
			}
		}

		private EvilRoad CreateMergedRoad(RoadConnectionData connectionData)
		{
			EvilRoadsManager evilRoadsManager = UnityEngine.Object.FindObjectOfType<EvilRoadsManager>();
			if (evilRoadsManager == null)
			{
				EvilLogger.LogError("Cannot create merged road: DivisionRoadsManager not found", "CreateMergedRoad", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadConnectionManager.cs", 199);
				return null;
			}
			Spline spline = RoadSplineMerger.MergeSplines(connectionData);
			if (spline == null)
			{
				EvilLogger.LogError("Failed to merge splines", "CreateMergedRoad", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadConnectionManager.cs", 207);
				return null;
			}
			EvilRoad ownerRoad = connectionData.ConnectionPoint1.OwnerRoad;
			EvilRoad ownerRoad2 = connectionData.ConnectionPoint2.OwnerRoad;
			Terrain terrain = ownerRoad.GetAssociatedTerrain() ?? ownerRoad2.GetAssociatedTerrain();
			EvilRoad evilRoad = new GameObject("MergedRoad_" + ownerRoad.name + "_" + ownerRoad2.name).AddComponent<EvilRoad>();
			evilRoad.transform.SetParent(evilRoadsManager.transform);
			evilRoad.Setup(ownerRoad.GetRoadConfig(), evilRoadsManager);
			evilRoad.CreateFromSpline(spline, terrain);
			evilRoad.SetConnectionManager(this);
			return evilRoad;
		}

		private bool AreRoadsConnected(EvilRoad road1, EvilRoad road2)
		{
			foreach (RoadConnectionData activeConnection in activeConnections)
			{
				if (activeConnection.InvolvesRoad(road1) && activeConnection.InvolvesRoad(road2))
				{
					return true;
				}
			}
			return false;
		}

		public void DisconnectAllConnectionsForRoad(EvilRoad road)
		{
			if (road == null || activeConnections == null || activeConnections.Count == 0)
			{
				return;
			}
			List<RoadConnectionData> list = new List<RoadConnectionData>(activeConnections);
			List<RoadConnectionData> list2 = new List<RoadConnectionData>();
			foreach (RoadConnectionData item in list)
			{
				if (item != null && item.InvolvesRoad(road))
				{
					list2.Add(item);
				}
			}
			foreach (RoadConnectionData item2 in list2)
			{
				activeConnections.Remove(item2);
			}
			foreach (RoadConnectionData item3 in list2)
			{
				if (item3 != null)
				{
					DisconnectRoads(item3);
				}
			}
		}

		public void DisconnectRoads(RoadConnectionData connection)
		{
			if (connection == null || !connection.IsActive)
			{
				return;
			}
			connection.RestoreOriginalRoadObjects();
			if (connection.ConnectedRoad != null && connection.ConnectedRoad.gameObject != null)
			{
				UnregisterRoad(connection.ConnectedRoad);
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(connection.ConnectedRoad.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(connection.ConnectedRoad.gameObject);
				}
			}
			foreach (EvilRoad originalRoad in connection.OriginalRoads)
			{
				if (originalRoad != null && originalRoad.gameObject != null)
				{
					try
					{
						originalRoad.SetActive(active: true);
						originalRoad.UpdatePlacedRoadObjectsFromChildren();
						RegisterRoad(originalRoad);
					}
					catch (Exception)
					{
					}
				}
			}
			connection.Disconnect();
		}

		public List<RoadConnectionData> GetActiveConnections()
		{
			return new List<RoadConnectionData>(activeConnections);
		}

		public void SetConnectionDistance(float distance)
		{
			connectionDistance = Mathf.Max(0.1f, distance);
		}

		public void UpdateConnectionPointsForRoad(EvilRoad road)
		{
			if (!(road == null))
			{
				if (roadConnectionPoints.ContainsKey(road))
				{
					roadConnectionPoints[road].Clear();
				}
				List<RoadConnectionPoint> value = CreateConnectionPointsForRoad(road);
				roadConnectionPoints[road] = value;
			}
		}

		public void ConnectSpecificRoad(EvilRoad road)
		{
			if (road == null || !roadConnectionPoints.ContainsKey(road))
			{
				return;
			}
			List<RoadConnectionPoint> list = roadConnectionPoints[road];
			bool flag = false;
			foreach (KeyValuePair<EvilRoad, List<RoadConnectionPoint>> roadConnectionPoint in roadConnectionPoints)
			{
				EvilRoad key = roadConnectionPoint.Key;
				List<RoadConnectionPoint> value = roadConnectionPoint.Value;
				if (key == road || !key.IsActive() || AreRoadsConnected(road, key))
				{
					continue;
				}
				foreach (RoadConnectionPoint item in list)
				{
					foreach (RoadConnectionPoint item2 in value)
					{
						if (CanConnect(item, item2))
						{
							CreateConnection(item, item2);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}

		private void DisconnectAllRoadsButton()
		{
			if (activeConnections == null || activeConnections.Count == 0)
			{
				return;
			}
			List<RoadConnectionData> list = new List<RoadConnectionData>(activeConnections);
			_ = list.Count;
			activeConnections.Clear();
			foreach (RoadConnectionData item in list)
			{
				if (item != null)
				{
					DisconnectRoads(item);
				}
			}
		}

		private void Start()
		{
			if (roadsManager == null)
			{
				roadsManager = UnityEngine.Object.FindObjectOfType<EvilRoadsManager>();
			}
		}

		private void OnDrawGizmos()
		{
			if (!enableDebugVisualization)
			{
				return;
			}
			foreach (KeyValuePair<EvilRoad, List<RoadConnectionPoint>> roadConnectionPoint in roadConnectionPoints)
			{
				foreach (RoadConnectionPoint item in roadConnectionPoint.Value)
				{
					Gizmos.color = connectionPointColor;
					Gizmos.DrawWireSphere(item.Position, 0.5f);
					Gizmos.DrawRay(item.Position, item.Direction * 2f);
				}
			}
			Gizmos.color = connectedColor;
			foreach (RoadConnectionData activeConnection in activeConnections)
			{
				if (activeConnection.IsActive)
				{
					Gizmos.DrawLine(activeConnection.ConnectionPoint1.Position, activeConnection.ConnectionPoint2.Position);
					Gizmos.DrawWireSphere(activeConnection.ConnectionPosition, 1f);
				}
			}
			if (roadConnectionPoints.Count <= 0)
			{
				return;
			}
			Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
			foreach (KeyValuePair<EvilRoad, List<RoadConnectionPoint>> roadConnectionPoint2 in roadConnectionPoints)
			{
				foreach (RoadConnectionPoint item2 in roadConnectionPoint2.Value)
				{
					Gizmos.DrawWireSphere(item2.Position, connectionDistance);
				}
			}
		}
	}
}
