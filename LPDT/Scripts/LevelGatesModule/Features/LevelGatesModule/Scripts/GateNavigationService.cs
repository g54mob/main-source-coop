using System;
using System.Collections.Generic;
using Features.NavigationModule.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Features.LevelGatesModule.Scripts
{
	public class GateNavigationService : IGateNavigationService
	{
		private static readonly IComparer<RaycastHit> DistanceComparer = Comparer<RaycastHit>.Create((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));

		private const int MAX_GATES_ON_PATH = 30;

		private const int MAX_PATH_CORNERS = 128;

		private const float MIN_PATH_SEGMENT_LENGTH = 0.0001f;

		private const string AGENT_TYPE_NAME = "Universal";

		private readonly LevelGatesModel _levelGatesModel;

		private readonly INavigationService _navigationService;

		private readonly LevelGateConfiguration _levelGateConfiguration;

		private readonly RaycastHit[] _gateHitBuffer = new RaycastHit[30];

		private readonly Vector3[] _pathCorners = new Vector3[128];

		private readonly NavMeshPath _gatePath = new NavMeshPath();

		private readonly NavMeshQueryFilter _queryFilter = new NavMeshQueryFilter
		{
			areaMask = -1,
			agentTypeID = GetAgentTypeIdByName("Universal")
		};

		public GateNavigationService(LevelGatesModel levelGatesModel, INavigationService navigationService, LevelGateConfiguration levelGateConfiguration)
		{
			_levelGatesModel = levelGatesModel;
			_navigationService = navigationService;
			_levelGateConfiguration = levelGateConfiguration;
		}

		private static int GetAgentTypeIdByName(string agentTypeName)
		{
			for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
			{
				NavMeshBuildSettings settingsByIndex = NavMesh.GetSettingsByIndex(i);
				if (NavMesh.GetSettingsNameFromID(settingsByIndex.agentTypeID) == agentTypeName)
				{
					return settingsByIndex.agentTypeID;
				}
			}
			return NavMesh.GetSettingsByIndex(0).agentTypeID;
		}

		public void ConstructPathToGate(Vector3 startingPoint, IGate gate, bool projectPoints, List<GatePathData> pathGates)
		{
			pathGates.Clear();
			Vector3 vector = startingPoint;
			Vector3 vector2 = gate.Position;
			if (projectPoints)
			{
				if (_navigationService.TryGetPointOnNavMeshProjected(vector, out var projectedPoint))
				{
					vector = projectedPoint;
				}
				if (_navigationService.TryGetPointOnNavMeshProjected(vector2, out var projectedPoint2))
				{
					vector2 = projectedPoint2;
				}
			}
			if (!NavMesh.CalculatePath(vector, vector2, _queryFilter, _gatePath))
			{
				return;
			}
			int cornersNonAlloc = _gatePath.GetCornersNonAlloc(_pathCorners);
			if (cornersNonAlloc != 0)
			{
				TryWritePathGate(startingPoint, _pathCorners[0], pathGates);
				for (int i = 0; i < cornersNonAlloc - 1; i++)
				{
					TryWritePathGate(_pathCorners[i], _pathCorners[i + 1], pathGates);
				}
				if (!PathContainsGate(pathGates, gate))
				{
					pathGates.Add(new GatePathData
					{
						PathGate = gate,
						IntersectionStart = startingPoint
					});
				}
			}
		}

		public bool TryGetClosestExitGate(Vector3 startingPoint, out IGate closestGate)
		{
			closestGate = null;
			float num = float.MaxValue;
			IReadOnlyList<IGate> exitGates = _levelGatesModel.ExitGates;
			for (int i = 0; i < exitGates.Count; i++)
			{
				IGate gate = exitGates[i];
				float sqrMagnitude = (gate.Position - startingPoint).sqrMagnitude;
				if (!(sqrMagnitude >= num))
				{
					num = sqrMagnitude;
					closestGate = gate;
				}
			}
			return closestGate != null;
		}

		private void TryWritePathGate(Vector3 startPoint, Vector3 endPoint, List<GatePathData> pathGates)
		{
			Vector3 vector = endPoint - startPoint;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.0001f)
			{
				return;
			}
			Vector3 direction = vector / magnitude;
			int num = Physics.RaycastNonAlloc(startPoint, direction, _gateHitBuffer, magnitude, _levelGateConfiguration.GateLayerMask);
			if (num <= 0)
			{
				return;
			}
			if (num > 1)
			{
				Array.Sort(_gateHitBuffer, 0, num, DistanceComparer);
			}
			for (int i = 0; i < num; i++)
			{
				if (_gateHitBuffer[i].collider.gameObject.TryGetComponent<IGate>(out var component) && component.GateGroup == GateGroup.Path && !PathContainsGate(pathGates, component))
				{
					pathGates.Add(new GatePathData
					{
						PathGate = component,
						IntersectionStart = startPoint
					});
				}
			}
		}

		public bool PathContainsGate(List<GatePathData> pathGates, IGate gate)
		{
			for (int i = 0; i < pathGates.Count; i++)
			{
				if (pathGates[i].PathGate == gate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
