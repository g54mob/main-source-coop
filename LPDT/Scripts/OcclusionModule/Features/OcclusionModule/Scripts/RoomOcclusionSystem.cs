using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.GameUpdaterModule;
using Features.LevelModule.Scripts.RoomVariations;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.OcclusionModule.Scripts
{
	public class RoomOcclusionSystem : IInitializable, IDisposable
	{
		private readonly struct LineOfSightLatch
		{
			public readonly float Timestamp;

			public readonly Vector3 Position;

			public readonly Vector3 Forward;

			public LineOfSightLatch(float timestamp, Vector3 position, Vector3 forward)
			{
				Timestamp = timestamp;
				Position = position;
				Forward = forward;
			}
		}

		private readonly struct RoomHop
		{
			public readonly OcclusionRoom Room;

			public readonly int Hop;

			public RoomHop(OcclusionRoom room, int hop)
			{
				Room = room;
				Hop = hop;
			}
		}

		private const float OPENING_PLANE_DISTANCE = 1f;

		private const float OPENING_EDGE_MARGIN = 0.5f;

		private const float GROUND_PROBE_DISTANCE = 5f;

		private const int GROUND_PROBE_HIT_LIMIT = 16;

		private const int LOS_SAMPLE_STRATA = 5;

		private const float LINE_OF_SIGHT_GRACE = 1f;

		private const float LINE_OF_SIGHT_LATCH_RADIUS = 0.5f;

		private const float LINE_OF_SIGHT_LATCH_COS = 0.985f;

		private static readonly float[] _losOriginOffsets = new float[3] { 0f, -1f, 1f };

		private readonly IGameUpdater _gameUpdater;

		private readonly CameraModel _cameraModel;

		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		private readonly OcclusionSettings _occlusionSettings;

		private readonly HashSet<OcclusionRoom> _visibleRooms = new HashSet<OcclusionRoom>();

		private readonly Dictionary<OcclusionRoom, List<OcclusionPortal>> _adjacency = new Dictionary<OcclusionRoom, List<OcclusionPortal>>();

		private readonly Dictionary<OcclusionRoom, Bounds> _roomBounds = new Dictionary<OcclusionRoom, Bounds>();

		private readonly Dictionary<OcclusionRoom, float> _lastVisibleTime = new Dictionary<OcclusionRoom, float>();

		private readonly Dictionary<OcclusionPortal, LineOfSightLatch> _lineOfSightLatches = new Dictionary<OcclusionPortal, LineOfSightLatch>();

		private readonly HashSet<MeshRenderer> _managedRenderers = new HashSet<MeshRenderer>();

		private readonly List<OcclusionRoom> _containingRooms = new List<OcclusionRoom>();

		private readonly Queue<RoomHop> _frontier = new Queue<RoomHop>();

		private readonly Vector3[] _portalCornerBuffer = new Vector3[4];

		private readonly RaycastHit[] _groundProbeHits = new RaycastHit[16];

		private readonly List<Vector3> _viewSpacePolygon = new List<Vector3>(8);

		private readonly List<Vector3> _nearClippedPolygon = new List<Vector3>(8);

		private OcclusionRoom[] _rooms;

		private OcclusionPortal[] _portals;

		private OcclusionRoom _currentRoom;

		private RoomOcclusionSceneMarker _activeMarker;

		private bool _isWired;

		public RoomOcclusionSystem(IGameUpdater gameUpdater, CameraModel cameraModel, SpawnedRoomsModel spawnedRoomsModel, OcclusionSettings occlusionSettings)
		{
			_gameUpdater = gameUpdater;
			_cameraModel = cameraModel;
			_spawnedRoomsModel = spawnedRoomsModel;
			_occlusionSettings = occlusionSettings;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += Tick;
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= Tick;
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
			RestoreAllRooms();
		}

		private void Tick()
		{
			if (IsWiringStale())
			{
				Unwire();
			}
			if (!_isWired)
			{
				TryWire();
				if (!_isWired)
				{
					return;
				}
			}
			ApplyMarkerSettings();
			if (_occlusionSettings.CullingMode == OcclusionCullingMode.Disabled)
			{
				RestoreAllRooms();
			}
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (_isWired && !(camera == null) && !(camera != _cameraModel.CameraObject) && _occlusionSettings.CullingMode != OcclusionCullingMode.Disabled && !IsWiringStale())
			{
				UpdateVisibleSet(camera);
				ApplyVisibility();
			}
		}

		private void UpdateVisibleSet(Camera camera)
		{
			Vector3 position = camera.transform.position;
			_currentRoom = FindContainingRoom(position);
			if (_currentRoom == null)
			{
				OcclusionRoom occlusionRoom = FindNearestRoom(position);
				if (occlusionRoom != null && WithinGate(occlusionRoom, position))
				{
					_currentRoom = occlusionRoom;
				}
			}
			_visibleRooms.Clear();
			if (!(_currentRoom == null))
			{
				_visibleRooms.Add(_currentRoom);
				if (_occlusionSettings.CullingMode == OcclusionCullingMode.Portal)
				{
					TraversePortals(_currentRoom, new Rect(0f, 0f, Screen.width, Screen.height), 0, null, camera, position, hasCrossedShutDoor: false);
				}
				else
				{
					TraverseGraph(position);
				}
			}
		}

		private void ApplyVisibility()
		{
			float time = Time.time;
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (!(occlusionRoom == null))
				{
					bool num = _visibleRooms.Contains(occlusionRoom);
					if (num)
					{
						_lastVisibleTime[occlusionRoom] = time;
					}
					float value;
					bool visible = num || (_lastVisibleTime.TryGetValue(occlusionRoom, out value) && time - value < _occlusionSettings.VisibilityHideDelay);
					occlusionRoom.SetVisible(visible);
				}
			}
			if (_portals == null)
			{
				return;
			}
			OcclusionPortal[] portals = _portals;
			foreach (OcclusionPortal occlusionPortal in portals)
			{
				if (!(occlusionPortal == null))
				{
					bool frameVisible = (occlusionPortal.RoomA != null && occlusionPortal.RoomA.IsVisible) || (occlusionPortal.RoomB != null && occlusionPortal.RoomB.IsVisible);
					occlusionPortal.SetFrameVisible(frameVisible);
				}
			}
		}

		private bool IsWiringStale()
		{
			if (!_isWired)
			{
				return false;
			}
			if (_activeMarker == null)
			{
				return true;
			}
			if (_rooms == null || _rooms.Length == 0)
			{
				return true;
			}
			OcclusionRoom[] rooms = _rooms;
			for (int i = 0; i < rooms.Length; i++)
			{
				if (rooms[i] == null)
				{
					return true;
				}
			}
			if (_portals != null)
			{
				OcclusionPortal[] portals = _portals;
				foreach (OcclusionPortal occlusionPortal in portals)
				{
					if (occlusionPortal == null)
					{
						return true;
					}
					if (IsDestroyed(occlusionPortal.RoomA) || IsDestroyed(occlusionPortal.RoomB))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsDestroyed(OcclusionRoom room)
		{
			if ((object)room != null)
			{
				return room == null;
			}
			return false;
		}

		private void TryWire()
		{
			RoomOcclusionSceneMarker activeMarker = OcclusionRegistry.ActiveMarker;
			if (activeMarker == null || (_spawnedRoomsModel.RoomSpawnTasksCount.HasValue && !_spawnedRoomsModel.IsAllTasksCompleted) || OcclusionRegistry.RoomCount == 0)
			{
				return;
			}
			_activeMarker = activeMarker;
			ApplyMarkerSettings();
			_rooms = OcclusionRegistry.GetRooms();
			_portals = OcclusionRegistry.GetPortals();
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (!(occlusionRoom == null))
				{
					occlusionRoom.CollectRenderers();
				}
			}
			HingeJoint[] hinges = UnityEngine.Object.FindObjectsByType<HingeJoint>(FindObjectsSortMode.None);
			if (_portals != null)
			{
				OcclusionPortal[] portals = _portals;
				foreach (OcclusionPortal occlusionPortal in portals)
				{
					if (!(occlusionPortal == null))
					{
						occlusionPortal.ResolveRooms(_rooms);
						occlusionPortal.ResolveDoor(hinges);
						occlusionPortal.CollectFrameRenderers();
					}
				}
			}
			ExcludeManagedRenderersFromRooms();
			BuildRoomBounds();
			BuildAdjacency();
			ReportWiringHealth();
			_currentRoom = null;
			_isWired = true;
		}

		private void ReportWiringHealth()
		{
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (!(occlusionRoom == null) && occlusionRoom.Renderers != null && occlusionRoom.Renderers.Length == 0)
				{
					Debug.LogError("[Occlusion] '" + occlusionRoom.name + "' wired with 0 managed renderers " + $"({occlusionRoom.AuthoredVisibleCount} authored visible) — it can never be shown again.", occlusionRoom);
				}
			}
			if (_portals == null)
			{
				return;
			}
			OcclusionPortal[] portals = _portals;
			foreach (OcclusionPortal occlusionPortal in portals)
			{
				if (!(occlusionPortal == null) && (IsDestroyed(occlusionPortal.RoomA) || IsDestroyed(occlusionPortal.RoomB)))
				{
					Debug.LogError("[Occlusion] portal '" + occlusionPortal.name + "' points at a destroyed room — the room behind it never renders.", occlusionPortal);
				}
			}
		}

		private void ApplyMarkerSettings()
		{
			_activeMarker.ApplyTo(_occlusionSettings);
		}

		private void Unwire()
		{
			RestoreAllRooms();
			_isWired = false;
			_currentRoom = null;
			_activeMarker = null;
			_adjacency.Clear();
			_roomBounds.Clear();
			_lastVisibleTime.Clear();
			_lineOfSightLatches.Clear();
			_rooms = null;
			_portals = null;
		}

		private void RestoreAllRooms()
		{
			if (_rooms != null)
			{
				OcclusionRoom[] rooms = _rooms;
				foreach (OcclusionRoom occlusionRoom in rooms)
				{
					if (occlusionRoom != null)
					{
						occlusionRoom.SetVisible(isVisible: true);
					}
				}
			}
			if (_portals == null)
			{
				return;
			}
			OcclusionPortal[] portals = _portals;
			foreach (OcclusionPortal occlusionPortal in portals)
			{
				if (occlusionPortal != null)
				{
					occlusionPortal.SetFrameVisible(isVisible: true);
				}
			}
		}

		private void BuildAdjacency()
		{
			_adjacency.Clear();
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (occlusionRoom != null && !_adjacency.ContainsKey(occlusionRoom))
				{
					_adjacency[occlusionRoom] = new List<OcclusionPortal>();
				}
			}
			if (_portals == null)
			{
				return;
			}
			OcclusionPortal[] portals = _portals;
			foreach (OcclusionPortal occlusionPortal in portals)
			{
				if (!(occlusionPortal == null))
				{
					if (occlusionPortal.RoomA != null && _adjacency.TryGetValue(occlusionPortal.RoomA, out var value))
					{
						value.Add(occlusionPortal);
					}
					if (occlusionPortal.RoomB != null && _adjacency.TryGetValue(occlusionPortal.RoomB, out var value2))
					{
						value2.Add(occlusionPortal);
					}
				}
			}
		}

		private void ExcludeManagedRenderersFromRooms()
		{
			_managedRenderers.Clear();
			if (_portals != null)
			{
				OcclusionPortal[] portals = _portals;
				foreach (OcclusionPortal occlusionPortal in portals)
				{
					if (occlusionPortal == null || occlusionPortal.FrameRenderers == null)
					{
						continue;
					}
					MeshRenderer[] frameRenderers = occlusionPortal.FrameRenderers;
					foreach (MeshRenderer meshRenderer in frameRenderers)
					{
						if (meshRenderer != null)
						{
							_managedRenderers.Add(meshRenderer);
						}
					}
				}
			}
			if (_managedRenderers.Count == 0)
			{
				return;
			}
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (occlusionRoom != null)
				{
					occlusionRoom.ExcludeRenderers(_managedRenderers);
				}
			}
		}

		private void BuildRoomBounds()
		{
			_roomBounds.Clear();
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (occlusionRoom != null && occlusionRoom.HasBounds)
				{
					_roomBounds[occlusionRoom] = occlusionRoom.RendererBounds;
				}
			}
		}

		private OcclusionRoom FindContainingRoom(Vector3 viewpoint)
		{
			_containingRooms.Clear();
			OcclusionRoom baseRoom = null;
			float num = float.MaxValue;
			foreach (KeyValuePair<OcclusionRoom, Bounds> roomBound in _roomBounds)
			{
				OcclusionRoom key = roomBound.Key;
				if (OcclusionRoomQuery.IsPointInsideRoom(key, roomBound.Value, viewpoint))
				{
					_containingRooms.Add(key);
					float sqrMagnitude = (key.transform.position - viewpoint).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						baseRoom = key;
					}
				}
			}
			if (_containingRooms.Count > 1)
			{
				OcclusionRoom occlusionRoom = FindRoomUnderfoot(viewpoint);
				if (occlusionRoom != null && _containingRooms.Contains(occlusionRoom))
				{
					baseRoom = occlusionRoom;
				}
			}
			return RefineByPortalSide(baseRoom, viewpoint);
		}

		private OcclusionRoom FindRoomUnderfoot(Vector3 viewpoint)
		{
			int num = Physics.RaycastNonAlloc(viewpoint, Vector3.down, _groundProbeHits, 5f, _occlusionSettings.OcclusionBlockerMask, QueryTriggerInteraction.Ignore);
			OcclusionRoom result = null;
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _groundProbeHits[i];
				if (!(raycastHit.distance >= num2))
				{
					OcclusionRoom componentInParent = raycastHit.collider.GetComponentInParent<OcclusionRoom>();
					if (!(componentInParent == null))
					{
						result = componentInParent;
						num2 = raycastHit.distance;
					}
				}
			}
			return result;
		}

		private OcclusionRoom RefineByPortalSide(OcclusionRoom baseRoom, Vector3 viewpoint)
		{
			if (_portals == null || baseRoom == null)
			{
				return baseRoom;
			}
			OcclusionPortal occlusionPortal = null;
			float num = _occlusionSettings.PeekDistance * _occlusionSettings.PeekDistance;
			OcclusionPortal[] portals = _portals;
			foreach (OcclusionPortal occlusionPortal2 in portals)
			{
				if (!(occlusionPortal2 == null) && occlusionPortal2.IsOpen(_occlusionSettings.DoorOpenThreshold))
				{
					float sqrMagnitude = (occlusionPortal2.PortalCenter - viewpoint).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						occlusionPortal = occlusionPortal2;
					}
				}
			}
			if (occlusionPortal == null || (occlusionPortal.RoomA != baseRoom && occlusionPortal.RoomB != baseRoom))
			{
				return baseRoom;
			}
			OcclusionRoom occlusionRoom = ((Vector3.Dot(viewpoint - occlusionPortal.PortalCenter, occlusionPortal.transform.forward) >= 0f) ? occlusionPortal.RoomB : occlusionPortal.RoomA);
			if (occlusionRoom == null || occlusionRoom == baseRoom)
			{
				return baseRoom;
			}
			if (_roomBounds.TryGetValue(occlusionRoom, out var value) && value.Contains(viewpoint))
			{
				return occlusionRoom;
			}
			return baseRoom;
		}

		private OcclusionRoom FindNearestRoom(Vector3 viewpoint)
		{
			OcclusionRoom result = null;
			float num = float.MaxValue;
			OcclusionRoom[] rooms = _rooms;
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (!(occlusionRoom == null))
				{
					Bounds value;
					float num2 = (_roomBounds.TryGetValue(occlusionRoom, out value) ? value.SqrDistance(viewpoint) : (occlusionRoom.transform.position - viewpoint).sqrMagnitude);
					if (num2 < num)
					{
						num = num2;
						result = occlusionRoom;
					}
				}
			}
			return result;
		}

		private void TraverseGraph(Vector3 viewpoint)
		{
			_frontier.Clear();
			_frontier.Enqueue(new RoomHop(_currentRoom, 0));
			while (_frontier.Count > 0)
			{
				RoomHop roomHop = _frontier.Dequeue();
				if (roomHop.Hop >= _occlusionSettings.MaxDepth || !_adjacency.TryGetValue(roomHop.Room, out var value))
				{
					continue;
				}
				foreach (OcclusionPortal item in value)
				{
					if (!(item == null) && (item.IsOpen(_occlusionSettings.DoorOpenThreshold) || ViewpointNear(item, viewpoint)))
					{
						OcclusionRoom other = item.GetOther(roomHop.Room);
						if (!(other == null) && !_visibleRooms.Contains(other) && WithinGate(other, viewpoint))
						{
							_visibleRooms.Add(other);
							_frontier.Enqueue(new RoomHop(other, roomHop.Hop + 1));
						}
					}
				}
			}
		}

		private void TraversePortals(OcclusionRoom room, Rect clip, int depth, OcclusionPortal cameFrom, Camera camera, Vector3 viewpoint, bool hasCrossedShutDoor)
		{
			if (depth >= _occlusionSettings.MaxDepth || !_adjacency.TryGetValue(room, out var value))
			{
				return;
			}
			foreach (OcclusionPortal item in value)
			{
				if (item == null || item == cameFrom)
				{
					continue;
				}
				bool flag = item.IsOpen(_occlusionSettings.DoorOpenThreshold);
				if (!flag && (hasCrossedShutDoor || !WithinClosedDoorRange(item, viewpoint)))
				{
					continue;
				}
				OcclusionRoom other = item.GetOther(room);
				if (other == null || !WithinGate(other, viewpoint))
				{
					continue;
				}
				Rect rect;
				if (IsViewpointInOpening(item, viewpoint))
				{
					_visibleRooms.Add(other);
					TraversePortals(other, clip, depth + 1, item, camera, viewpoint, hasCrossedShutDoor || !flag);
				}
				else if (TryGetPortalRect(item, camera, out rect))
				{
					Rect clip2 = Intersect(clip, rect);
					if (!(clip2.width <= 0f) && !(clip2.height <= 0f) && HasLineOfSight(item, camera))
					{
						_visibleRooms.Add(other);
						TraversePortals(other, clip2, depth + 1, item, camera, viewpoint, hasCrossedShutDoor || !flag);
					}
				}
			}
		}

		private bool TryGetPortalRect(OcclusionPortal portal, Camera camera, out Rect rect)
		{
			portal.GetPortalCorners(_portalCornerBuffer);
			Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
			_viewSpacePolygon.Clear();
			for (int i = 0; i < 4; i++)
			{
				_viewSpacePolygon.Add(worldToCameraMatrix.MultiplyPoint3x4(_portalCornerBuffer[i]));
			}
			float num = 0f - Mathf.Min(camera.nearClipPlane, 0.02f);
			_nearClippedPolygon.Clear();
			for (int j = 0; j < _viewSpacePolygon.Count; j++)
			{
				Vector3 vector = _viewSpacePolygon[j];
				Vector3 b = _viewSpacePolygon[(j + 1) % _viewSpacePolygon.Count];
				bool num2 = vector.z <= num;
				bool flag = b.z <= num;
				if (num2)
				{
					_nearClippedPolygon.Add(vector);
				}
				if (num2 != flag)
				{
					float t = (num - vector.z) / (b.z - vector.z);
					_nearClippedPolygon.Add(Vector3.Lerp(vector, b, t));
				}
			}
			if (_nearClippedPolygon.Count == 0)
			{
				rect = default(Rect);
				return false;
			}
			Matrix4x4 projectionMatrix = camera.projectionMatrix;
			float num3 = float.MaxValue;
			float num4 = float.MaxValue;
			float num5 = float.MinValue;
			float num6 = float.MinValue;
			for (int k = 0; k < _nearClippedPolygon.Count; k++)
			{
				Vector3 vector2 = _nearClippedPolygon[k];
				Vector4 vector3 = projectionMatrix * new Vector4(vector2.x, vector2.y, vector2.z, 1f);
				if (!(vector3.w <= 0f))
				{
					float num7 = (vector3.x / vector3.w * 0.5f + 0.5f) * (float)Screen.width;
					float num8 = (vector3.y / vector3.w * 0.5f + 0.5f) * (float)Screen.height;
					if (num7 < num3)
					{
						num3 = num7;
					}
					if (num7 > num5)
					{
						num5 = num7;
					}
					if (num8 < num4)
					{
						num4 = num8;
					}
					if (num8 > num6)
					{
						num6 = num8;
					}
				}
			}
			if (num3 > num5)
			{
				rect = default(Rect);
				return false;
			}
			if (num5 < 0f || num3 > (float)Screen.width || num6 < 0f || num4 > (float)Screen.height)
			{
				rect = default(Rect);
				return false;
			}
			rect = Rect.MinMaxRect(num3, num4, num5, num6);
			return true;
		}

		private static Rect Intersect(Rect a, Rect b)
		{
			float num = Mathf.Max(a.xMin, b.xMin);
			float num2 = Mathf.Max(a.yMin, b.yMin);
			float b2 = Mathf.Min(a.xMax, b.xMax);
			float b3 = Mathf.Min(a.yMax, b.yMax);
			return Rect.MinMaxRect(num, num2, Mathf.Max(num, b2), Mathf.Max(num2, b3));
		}

		private bool WithinGate(OcclusionRoom room, Vector3 viewpoint)
		{
			if (_occlusionSettings.MaxRenderDistance <= 0f)
			{
				return true;
			}
			float num = _occlusionSettings.MaxRenderDistance * _occlusionSettings.MaxRenderDistance;
			if (_roomBounds.TryGetValue(room, out var value))
			{
				return value.SqrDistance(viewpoint) <= num;
			}
			return (room.transform.position - viewpoint).sqrMagnitude <= num;
		}

		private bool HasLineOfSight(OcclusionPortal portal, Camera camera)
		{
			Transform transform = camera.transform;
			if (SampleLineOfSight(portal, camera))
			{
				_lineOfSightLatches[portal] = new LineOfSightLatch(Time.time, transform.position, transform.forward);
				return true;
			}
			if (!_lineOfSightLatches.TryGetValue(portal, out var value))
			{
				return false;
			}
			if (Time.time - value.Timestamp < 1f)
			{
				return true;
			}
			if ((transform.position - value.Position).sqrMagnitude <= 0.25f)
			{
				return Vector3.Dot(transform.forward, value.Forward) >= 0.985f;
			}
			return false;
		}

		private bool SampleLineOfSight(OcclusionPortal portal, Camera camera)
		{
			Vector3 portalCenter = portal.PortalCenter;
			portal.GetPortalCorners(_portalCornerBuffer);
			Vector3 vector = (_portalCornerBuffer[1] - _portalCornerBuffer[0]) * 0.5f;
			Vector3 vector2 = (_portalCornerBuffer[3] - _portalCornerBuffer[0]) * 0.5f;
			Vector3 right = camera.transform.right;
			float[] losOriginOffsets = _losOriginOffsets;
			foreach (float num in losOriginOffsets)
			{
				Vector3 vector3 = camera.transform.position + right * num;
				for (int j = 0; j < 5; j++)
				{
					float num2 = StratifiedFraction(j);
					for (int k = 0; k < 5; k++)
					{
						Vector3 to = portalCenter + vector * StratifiedFraction(k) + vector2 * num2;
						if (IsRayClear(vector3, to))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static float StratifiedFraction(int stratum)
		{
			return -1f + ((float)stratum + UnityEngine.Random.value) * 0.4f;
		}

		private bool IsRayClear(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			float num = magnitude - 0.1f;
			if (num <= 0f)
			{
				return true;
			}
			return !Physics.Raycast(from, vector / magnitude, num, _occlusionSettings.OcclusionBlockerMask, QueryTriggerInteraction.Ignore);
		}

		private bool IsViewpointInOpening(OcclusionPortal portal, Vector3 viewpoint)
		{
			portal.GetPortalCorners(_portalCornerBuffer);
			Vector3 portalCenter = portal.PortalCenter;
			Vector3 vector = _portalCornerBuffer[1] - _portalCornerBuffer[0];
			Vector3 vector2 = _portalCornerBuffer[3] - _portalCornerBuffer[0];
			Vector3 lhs = viewpoint - portalCenter;
			if (Mathf.Abs(Vector3.Dot(lhs, portal.transform.forward)) > 1f)
			{
				return false;
			}
			float num = Mathf.Abs(Vector3.Dot(lhs, vector.normalized));
			float num2 = Mathf.Abs(Vector3.Dot(lhs, vector2.normalized));
			if (num <= vector.magnitude * 0.5f + 0.5f)
			{
				return num2 <= vector2.magnitude * 0.5f + 0.5f;
			}
			return false;
		}

		private bool WithinClosedDoorRange(OcclusionPortal portal, Vector3 viewpoint)
		{
			float closedDoorRenderDistance = _occlusionSettings.ClosedDoorRenderDistance;
			if (closedDoorRenderDistance <= 0f)
			{
				return false;
			}
			return (portal.PortalCenter - viewpoint).sqrMagnitude <= closedDoorRenderDistance * closedDoorRenderDistance;
		}

		private bool ViewpointNear(OcclusionPortal portal, Vector3 viewpoint)
		{
			if (_occlusionSettings.PeekDistance <= 0f)
			{
				return false;
			}
			return (portal.PortalCenter - viewpoint).sqrMagnitude <= _occlusionSettings.PeekDistance * _occlusionSettings.PeekDistance;
		}
	}
}
