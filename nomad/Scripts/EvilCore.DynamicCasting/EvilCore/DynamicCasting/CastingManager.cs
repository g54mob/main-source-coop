using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.DynamicCasting
{
	[DefaultExecutionOrder(150)]
	public class CastingManager : MonoBehaviour, ICastingManager, IInitialize
	{
		private class MeshHitDistanceComparer : IComparer<MeshRayHit>
		{
			public static readonly MeshHitDistanceComparer Instance = new MeshHitDistanceComparer();

			public int Compare(MeshRayHit x, MeshRayHit y)
			{
				return x.Distance.CompareTo(y.Distance);
			}
		}

		private class RaycastHandle : IRaycastHandle
		{
			private CastRequest _request;

			public int Id { get; }

			public bool IsValid { get; private set; } = true;

			public bool IsEnabled { get; set; } = true;

			public CastRequest Request => _request;

			public Action<CastResult> Callback { get; private set; }

			public RaycastHit[] HitBuffer { get; private set; }

			public Collider[] ColliderBuffer { get; private set; }

			public MeshRayHit[] MeshHitBuffer { get; private set; }

			public CastResult LastResult { get; set; }

			public RaycastHandle(int id, CastRequest request, Action<CastResult> callback)
			{
				Id = id;
				_request = request;
				Callback = callback;
				int num = ((request.MaxHits > 0) ? request.MaxHits : 8);
				int num2 = ((request.MaxHits > 0) ? request.MaxHits : 16);
				HitBuffer = new RaycastHit[num];
				ColliderBuffer = new Collider[num2];
				if (request.Type == CastType.MeshRay)
				{
					MeshHitBuffer = new MeshRayHit[num];
				}
			}

			public void UpdateRequest(CastRequest newRequest)
			{
				_request = newRequest;
				if (newRequest.MaxHits > HitBuffer.Length)
				{
					HitBuffer = new RaycastHit[newRequest.MaxHits];
				}
				if (newRequest.MaxHits > ColliderBuffer.Length)
				{
					ColliderBuffer = new Collider[newRequest.MaxHits];
				}
				if (newRequest.Type == CastType.MeshRay && (MeshHitBuffer == null || newRequest.MaxHits > MeshHitBuffer.Length))
				{
					MeshHitBuffer = new MeshRayHit[(newRequest.MaxHits > 0) ? newRequest.MaxHits : 8];
				}
			}

			public void Invalidate()
			{
				IsValid = false;
				Callback = null;
			}
		}

		[Header("Debug")]
		[SerializeField]
		private bool debugVisualization;

		[SerializeField]
		private Color debugHitColor = Color.green;

		[SerializeField]
		private Color debugMissColor = Color.red;

		[SerializeField]
		private float debugHitSphereRadius = 0.05f;

		private readonly Dictionary<int, RaycastHandle> _handles = new Dictionary<int, RaycastHandle>();

		private readonly List<RaycastHandle> _fixedUpdateHandles = new List<RaycastHandle>();

		private readonly List<RaycastHandle> _updateHandles = new List<RaycastHandle>();

		private readonly List<RaycastHandle> _lateUpdateHandles = new List<RaycastHandle>();

		private readonly List<RaycastHandle> _manualHandles = new List<RaycastHandle>();

		private readonly HashSet<MeshFilter> _registeredMeshTargets = new HashSet<MeshFilter>();

		private readonly Dictionary<int, MeshDataCache> _meshDataCache = new Dictionary<int, MeshDataCache>();

		private readonly List<MeshFilter> _meshFilterBuffer = new List<MeshFilter>(64);

		private readonly List<int> _meshCacheKeysToRemove = new List<int>();

		private Collider[] _tempColliderBuffer = new Collider[64];

		private const int MESH_CACHE_LIFETIME_FRAMES = 300;

		private int _lastCacheCleanupFrame;

		private int _nextHandleId = 1;

		[field: SerializeField]
		public bool IsInitialized { get; set; }

		public int ActiveCount => _handles.Count;

		public int RegisteredMeshCount => _registeredMeshTargets.Count;

		private void Awake()
		{
			Init();
		}

		public void Init()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
			}
		}

		private void FixedUpdate()
		{
			if (IsInitialized)
			{
				ProcessHandles(_fixedUpdateHandles);
			}
		}

		private void Update()
		{
			if (IsInitialized)
			{
				ProcessHandles(_updateHandles);
			}
		}

		private void LateUpdate()
		{
			if (IsInitialized)
			{
				Physics.SyncTransforms();
				ProcessHandles(_lateUpdateHandles);
			}
		}

		private void OnDestroy()
		{
			UnregisterAll();
		}

		public IRaycastHandle Register(CastRequest request, Action<CastResult> onResult)
		{
			RaycastHandle raycastHandle = new RaycastHandle(_nextHandleId++, request, onResult);
			_handles[raycastHandle.Id] = raycastHandle;
			AddToUpdateList(raycastHandle);
			return raycastHandle;
		}

		public void Unregister(IRaycastHandle handle)
		{
			if (handle != null && _handles.TryGetValue(handle.Id, out var value))
			{
				RemoveFromUpdateList(value);
				value.Invalidate();
				_handles.Remove(handle.Id);
			}
		}

		public void UnregisterAll()
		{
			foreach (RaycastHandle value in _handles.Values)
			{
				value.Invalidate();
			}
			_handles.Clear();
			_fixedUpdateHandles.Clear();
			_updateHandles.Clear();
			_lateUpdateHandles.Clear();
			_manualHandles.Clear();
		}

		public void RegisterMeshTarget(MeshFilter meshFilter)
		{
			if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				_registeredMeshTargets.Add(meshFilter);
			}
		}

		public void UnregisterMeshTarget(MeshFilter meshFilter)
		{
			if (meshFilter != null)
			{
				_registeredMeshTargets.Remove(meshFilter);
				int instanceID = meshFilter.GetInstanceID();
				if (_meshDataCache.ContainsKey(instanceID))
				{
					_meshDataCache[instanceID].Clear();
					_meshDataCache.Remove(instanceID);
				}
			}
		}

		public void ClearMeshTargets()
		{
			_registeredMeshTargets.Clear();
			foreach (MeshDataCache value in _meshDataCache.Values)
			{
				value.Clear();
			}
			_meshDataCache.Clear();
		}

		public void SetEnabled(IRaycastHandle handle, bool enabled)
		{
			if (handle != null && _handles.TryGetValue(handle.Id, out var value))
			{
				value.IsEnabled = enabled;
			}
		}

		public void UpdateRequest(IRaycastHandle handle, CastRequest newRequest)
		{
			if (handle != null && _handles.TryGetValue(handle.Id, out var value))
			{
				UpdateMode updateMode = value.Request.UpdateMode;
				value.UpdateRequest(newRequest);
				if (updateMode != newRequest.UpdateMode)
				{
					RemoveFromUpdateList(value);
					AddToUpdateList(value);
				}
			}
		}

		public CastResult CastImmediate(CastRequest request)
		{
			int num = ((request.MaxHits > 0) ? request.MaxHits : 8);
			RaycastHit[] hitBuffer = new RaycastHit[num];
			Collider[] colliderBuffer = new Collider[(request.MaxHits > 0) ? request.MaxHits : 16];
			MeshRayHit[] meshHitBuffer = ((request.Type == CastType.MeshRay) ? new MeshRayHit[num] : null);
			return ExecuteCast(request, hitBuffer, colliderBuffer, meshHitBuffer);
		}

		public void ProcessAll()
		{
			ProcessHandles(_manualHandles);
		}

		public void Process(IRaycastHandle handle)
		{
			if (handle != null && _handles.TryGetValue(handle.Id, out var value) && value.IsValid && value.IsEnabled && !(value.Request.Origin == null))
			{
				CastResult obj = ExecuteCast(value.Request, value.HitBuffer, value.ColliderBuffer, value.MeshHitBuffer);
				value.Callback?.Invoke(obj);
			}
		}

		private void ProcessHandles(List<RaycastHandle> handles)
		{
			if (Time.frameCount - _lastCacheCleanupFrame > 60)
			{
				CleanMeshCache();
				_lastCacheCleanupFrame = Time.frameCount;
			}
			for (int i = 0; i < handles.Count; i++)
			{
				RaycastHandle raycastHandle = handles[i];
				if (raycastHandle.IsValid && raycastHandle.IsEnabled && !(raycastHandle.Request.Origin == null))
				{
					CastResult obj = (raycastHandle.LastResult = ExecuteCast(raycastHandle.Request, raycastHandle.HitBuffer, raycastHandle.ColliderBuffer, raycastHandle.MeshHitBuffer));
					raycastHandle.Callback?.Invoke(obj);
				}
			}
		}

		private CastResult ExecuteCast(CastRequest request, RaycastHit[] hitBuffer, Collider[] colliderBuffer, MeshRayHit[] meshHitBuffer = null)
		{
			Vector3 origin = GetOrigin(request);
			Vector3 direction = GetDirection(request);
			CastResult result = new CastResult
			{
				Type = request.Type,
				Hits = hitBuffer,
				Colliders = colliderBuffer,
				MeshHits = meshHitBuffer
			};
			switch (request.Type)
			{
			case CastType.Ray:
				ExecuteRayCast(origin, direction, request, ref result);
				break;
			case CastType.Sphere:
				ExecuteSphereCast(origin, direction, request, ref result);
				break;
			case CastType.Box:
				ExecuteBoxCast(origin, direction, request, ref result);
				break;
			case CastType.Capsule:
				ExecuteCapsuleCast(origin, direction, request, ref result);
				break;
			case CastType.OverlapSphere:
				ExecuteOverlapSphere(origin, request, ref result);
				break;
			case CastType.OverlapBox:
				ExecuteOverlapBox(origin, request, ref result);
				break;
			case CastType.MeshRay:
				ExecuteMeshRayCast(origin, direction, request, ref result);
				break;
			}
			if (request.ComponentFilter != null && result.DidHit && request.Type != CastType.MeshRay)
			{
				FilterByComponent(request, ref result);
			}
			return result;
		}

		private void FilterByComponent(CastRequest request, ref CastResult result)
		{
			if (request.Type == CastType.OverlapSphere || request.Type == CastType.OverlapBox)
			{
				FilterCollidersByComponent(request, ref result);
			}
			else
			{
				FilterHitsByComponent(request, ref result);
			}
		}

		private void FilterHitsByComponent(CastRequest request, ref CastResult result)
		{
			int num = 0;
			for (int i = 0; i < result.HitCount; i++)
			{
				RaycastHit raycastHit = result.Hits[i];
				if (!(raycastHit.collider == null) && (request.searchInParent ? (raycastHit.collider.GetComponentInParent(request.ComponentFilter) != null) : (raycastHit.collider.GetComponent(request.ComponentFilter) != null)))
				{
					if (num != i)
					{
						result.Hits[num] = raycastHit;
					}
					num++;
				}
			}
			for (int j = num; j < result.HitCount; j++)
			{
				result.Hits[j] = default(RaycastHit);
			}
			result.HitCount = num;
			result.DidHit = num > 0;
		}

		private void FilterCollidersByComponent(CastRequest request, ref CastResult result)
		{
			int num = 0;
			for (int i = 0; i < result.HitCount; i++)
			{
				Collider collider = result.Colliders[i];
				if (!(collider == null) && (request.searchInParent ? (collider.GetComponentInParent(request.ComponentFilter) != null) : (collider.GetComponent(request.ComponentFilter) != null)))
				{
					if (num != i)
					{
						result.Colliders[num] = collider;
					}
					num++;
				}
			}
			for (int j = num; j < result.HitCount; j++)
			{
				result.Colliders[j] = null;
			}
			result.HitCount = num;
			result.DidHit = num > 0;
		}

		private Vector3 GetOrigin(CastRequest request)
		{
			if (request.Origin != null)
			{
				return request.Origin.TransformPoint(request.Offset);
			}
			return request.Offset;
		}

		private Vector3 GetDirection(CastRequest request)
		{
			if (request.UseTransformForward && request.Origin != null)
			{
				return request.Origin.forward;
			}
			if (!(request.Direction.sqrMagnitude > 0f))
			{
				return Vector3.forward;
			}
			return request.Direction.normalized;
		}

		private void ExecuteRayCast(Vector3 origin, Vector3 direction, CastRequest request, ref CastResult result)
		{
			result.HitCount = Physics.RaycastNonAlloc(origin, direction, result.Hits, request.Distance, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteSphereCast(Vector3 origin, Vector3 direction, CastRequest request, ref CastResult result)
		{
			result.HitCount = Physics.SphereCastNonAlloc(origin, request.Radius, direction, result.Hits, request.Distance, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteBoxCast(Vector3 origin, Vector3 direction, CastRequest request, ref CastResult result)
		{
			Quaternion orientation = ((request.Origin != null) ? request.Origin.rotation : Quaternion.identity);
			result.HitCount = Physics.BoxCastNonAlloc(origin, request.HalfExtents, direction, result.Hits, orientation, request.Distance, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteCapsuleCast(Vector3 origin, Vector3 direction, CastRequest request, ref CastResult result)
		{
			Vector3 vector = ((request.Origin != null) ? request.Origin.up : Vector3.up);
			float num = request.CapsuleHeight / 2f - request.Radius;
			Vector3 point = origin + vector * num;
			Vector3 point2 = origin - vector * num;
			result.HitCount = Physics.CapsuleCastNonAlloc(point, point2, request.Radius, direction, result.Hits, request.Distance, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteOverlapSphere(Vector3 origin, CastRequest request, ref CastResult result)
		{
			result.HitCount = Physics.OverlapSphereNonAlloc(origin, request.Radius, result.Colliders, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteOverlapBox(Vector3 origin, CastRequest request, ref CastResult result)
		{
			Quaternion orientation = ((request.Origin != null) ? request.Origin.rotation : Quaternion.identity);
			result.HitCount = Physics.OverlapBoxNonAlloc(origin, request.HalfExtents, result.Colliders, orientation, request.LayerMask, request.TriggerInteraction);
			result.DidHit = result.HitCount > 0;
		}

		private void ExecuteMeshRayCast(Vector3 origin, Vector3 direction, CastRequest request, ref CastResult result)
		{
			result.HitCount = 0;
			result.DidHit = false;
			if (result.MeshHits == null)
			{
				return;
			}
			_meshFilterBuffer.Clear();
			FindMeshTargets(origin, request, _meshFilterBuffer);
			if (_meshFilterBuffer.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = result.MeshHits.Length;
			foreach (MeshFilter item in _meshFilterBuffer)
			{
				if (num >= num2)
				{
					break;
				}
				if (item == null || item.sharedMesh == null || (request.ComponentFilter != null && !(request.searchInParent ? (item.GetComponentInParent(request.ComponentFilter) != null) : (item.GetComponent(request.ComponentFilter) != null))))
				{
					continue;
				}
				MeshDataCache orCreateMeshCache = GetOrCreateMeshCache(item);
				Transform transform = item.transform;
				Vector3 vector = transform.InverseTransformPoint(origin);
				Vector3 normalized = transform.InverseTransformDirection(direction).normalized;
				if (!MeshRayUtility.RayIntersectsBounds(vector, normalized, orCreateMeshCache.LocalBounds))
				{
					continue;
				}
				float num3 = 3.4028235E+38f;
				int num4 = -1;
				float u = 0f;
				float v = 0f;
				for (int i = 0; i < orCreateMeshCache.TriangleCount; i++)
				{
					orCreateMeshCache.GetTriangleVertices(i, out var v2, out var v3, out var v4);
					if (MeshRayUtility.RayTriangleIntersection(vector, normalized, v2, v3, v4, out var t, out var u2, out var v5))
					{
						Vector3 position = vector + normalized * t;
						Vector3 b = transform.TransformPoint(position);
						if (!(Vector3.Distance(origin, b) > request.Distance) && t < num3)
						{
							num3 = t;
							num4 = i;
							u = u2;
							v = v5;
						}
					}
				}
				if (num4 >= 0)
				{
					orCreateMeshCache.GetTriangleVertices(num4, out var _, out var _, out var _);
					orCreateMeshCache.GetTriangleNormals(num4, out var n, out var n2, out var n3);
					orCreateMeshCache.GetTriangleUVs(num4, out var uv, out var uv2, out var uv3);
					Vector3 position2 = vector + normalized * num3;
					Vector3 vector2 = transform.TransformPoint(position2);
					Vector3 direction2 = MeshRayUtility.BarycentricInterpolate(n, n2, n3, u, v);
					Vector3 normalized2 = transform.TransformDirection(direction2).normalized;
					Vector2 textureCoord = MeshRayUtility.BarycentricInterpolate(uv, uv2, uv3, u, v);
					result.MeshHits[num] = new MeshRayHit
					{
						Point = vector2,
						Normal = normalized2,
						Distance = Vector3.Distance(origin, vector2),
						TriangleIndex = num4,
						BarycentricCoord = MeshRayUtility.GetBarycentricCoord(u, v),
						TextureCoord = textureCoord,
						HitTransform = transform,
						MeshFilter = item,
						Renderer = item.GetComponent<MeshRenderer>()
					};
					num++;
				}
			}
			result.HitCount = num;
			result.DidHit = num > 0;
			if (num > 1)
			{
				Array.Sort(result.MeshHits, 0, num, MeshHitDistanceComparer.Instance);
			}
		}

		private void FindMeshTargets(Vector3 origin, CastRequest request, List<MeshFilter> output)
		{
			if (request.UseRegisteredMeshes)
			{
				foreach (MeshFilter registeredMeshTarget in _registeredMeshTargets)
				{
					if (!(registeredMeshTarget == null) && !(registeredMeshTarget.sharedMesh == null) && !(Vector3.Distance(origin, registeredMeshTarget.transform.position) > request.Distance + registeredMeshTarget.sharedMesh.bounds.extents.magnitude))
					{
						output.Add(registeredMeshTarget);
					}
				}
			}
			if ((int)request.LayerMask == 0)
			{
				return;
			}
			int num = Physics.OverlapSphereNonAlloc(origin, request.Distance, _tempColliderBuffer, request.LayerMask);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _tempColliderBuffer[i];
				if (collider == null)
				{
					continue;
				}
				MeshFilter component = collider.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh != null && !output.Contains(component))
				{
					output.Add(component);
				}
				if (!(collider.transform.parent != null))
				{
					continue;
				}
				Transform parent = collider.transform.parent;
				component = parent.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh != null && !output.Contains(component))
				{
					output.Add(component);
				}
				MeshFilter[] componentsInChildren = parent.GetComponentsInChildren<MeshFilter>();
				foreach (MeshFilter meshFilter in componentsInChildren)
				{
					if (meshFilter.sharedMesh != null && !output.Contains(meshFilter))
					{
						output.Add(meshFilter);
					}
				}
			}
		}

		private MeshDataCache GetOrCreateMeshCache(MeshFilter meshFilter)
		{
			int instanceID = meshFilter.GetInstanceID();
			if (!_meshDataCache.TryGetValue(instanceID, out var value))
			{
				value = new MeshDataCache();
				_meshDataCache[instanceID] = value;
			}
			value.Refresh(meshFilter.sharedMesh);
			return value;
		}

		private void CleanMeshCache()
		{
			_meshCacheKeysToRemove.Clear();
			int frameCount = Time.frameCount;
			foreach (KeyValuePair<int, MeshDataCache> item in _meshDataCache)
			{
				if (frameCount - item.Value.LastFrameAccessed > 300)
				{
					_meshCacheKeysToRemove.Add(item.Key);
				}
			}
			foreach (int item2 in _meshCacheKeysToRemove)
			{
				_meshDataCache[item2].Clear();
				_meshDataCache.Remove(item2);
			}
		}

		private void AddToUpdateList(RaycastHandle handle)
		{
			GetUpdateList(handle.Request.UpdateMode).Add(handle);
		}

		private void RemoveFromUpdateList(RaycastHandle handle)
		{
			GetUpdateList(handle.Request.UpdateMode).Remove(handle);
		}

		private List<RaycastHandle> GetUpdateList(UpdateMode mode)
		{
			return mode switch
			{
				UpdateMode.FixedUpdate => _fixedUpdateHandles, 
				UpdateMode.Update => _updateHandles, 
				UpdateMode.LateUpdate => _lateUpdateHandles, 
				UpdateMode.Manual => _manualHandles, 
				_ => _updateHandles, 
			};
		}
	}
}
