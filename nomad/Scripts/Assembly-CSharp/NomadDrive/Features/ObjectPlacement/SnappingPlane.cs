using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Objectives;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.ObjectPlacement
{
	[RequireComponent(typeof(NetworkedTransform))]
	public class SnappingPlane : NetworkBehaviour
	{
		private const float ENTER_POSITION_EPSILON = 0.001f;

		private const float STAY_POSITION_EPSILON = 0.005f;

		[Tooltip("Surface mode only: slerp rate for continuous rotation re-alignment while sweeping across the surface.")]
		[SerializeField]
		private float surfaceAlignSpeed = 20f;

		[Header("Grid Settings")]
		[SerializeField]
		private float _gridSize = 0.5f;

		[Header("Audio")]
		[Tooltip("Played on all clients when an object snaps onto this plane.")]
		[SerializeField]
		private SoundID snapSound;

		[HideInInspector]
		[SerializeField]
		private List<string> _allowedItemIDs = new List<string>();

		[SerializeField]
		private SyncList<uint> _placedEntityNetworkIDs = new SyncList<uint>();

		[Inject]
		protected INetworkManager networkManager;

		[Inject]
		private INetworkedAudioManager networkAudioRelay;

		public bool IsLateJoinCompleted;

		private MeshRenderer _meshRenderer;

		private Material _material;

		private Collider[] _colliders;

		private bool _parentHasDynamicRigidbody;

		private HeldItem _ownerHeldItem;

		private Coroutine _objectAlignRotationCoroutine;

		private MeshRenderer[] _currentRenderers;

		private Vector3 _surfaceBaseForward;

		private Vector3 _lastSurfaceNormal;

		private bool _recaptureSurfaceForward;

		private static readonly int Position;

		private static readonly int Radius;

		private static readonly int CenterPosition;

		private static readonly int PlaneNormal;

		private static readonly int PlaneTangent;

		private static readonly int PlaneBitangent;

		private static readonly int GridSize;

		private static readonly int PlaneOrigin;

		private const float ShaderRadiusMultiplier = 2.5f;

		public UnityEvent<GameObject, ObjectPlacementData> OnObjectEnter { get; } = new UnityEvent<GameObject, ObjectPlacementData>();

		public UnityEvent<GameObject, ObjectPlacementData, bool> OnObjectStay { get; } = new UnityEvent<GameObject, ObjectPlacementData, bool>();

		public UnityEvent<GameObject> OnObjectExit { get; } = new UnityEvent<GameObject>();

		public UnityEvent<GameObject> OnObjectPlaced { get; } = new UnityEvent<GameObject>();

		public UnityEvent<GameObject> OnObjectFallen { get; } = new UnityEvent<GameObject>();

		public UnityEvent<GameObject> OnObjectRemoved { get; } = new UnityEvent<GameObject>();

		public bool IsEnabled { get; private set; } = true;

		public bool IsAnyObjectPlaced => _placedEntityNetworkIDs.Count > 0;

		public uint SingleSlotPlacedEntityNetworkID
		{
			get
			{
				if (_placedEntityNetworkIDs.Count <= 0)
				{
					return 0u;
				}
				return _placedEntityNetworkIDs[0];
			}
		}

		public byte SnappingPlaneIndex { get; set; }

		public bool IsOnDynamicRigidbodyParent => _parentHasDynamicRigidbody;

		public bool SnapsToCenter
		{
			get
			{
				if (SnapOnCenter)
				{
					return !AlignToSurfaceNormal;
				}
				return false;
			}
		}

		[Header("Placement Behavior")]
		[Tooltip("If true, objects stick to this plane. If false, objects align and then become free.")]
		[field: SerializeField]
		public bool MakeObjectsStatic { get; set; }

		[Tooltip("If true, objects snap to the center of this plane. If false, objects follow the hit point.")]
		[field: SerializeField]
		public bool SnapOnCenter { get; set; }

		[Tooltip("Opt-in surface mode: objects align to the raycast hit normal at the hover point (curved/sloped colliders such as a vehicle exterior). Off = legacy flat-plane behavior (transform.up).")]
		[field: SerializeField]
		public bool AlignToSurfaceNormal { get; set; }

		[Tooltip("If true, the placement shader indicator is not shown for this plane.")]
		[field: SerializeField]
		public bool IsPlacementShaderDeactivated { get; set; }

		[Tooltip("If true, only one object can be placed on this plane at a time.")]
		[field: SerializeField]
		public bool IsSingleUseSlot { get; set; }

		[Header("Placement Filtering")]
		[Tooltip("If true, accepts ALL objects regardless of type.")]
		[field: SerializeField]
		public bool AcceptAll { get; set; }

		public List<string> AllowedItemIDs => _allowedItemIDs;

		public SyncList<uint> PlacedEntityNetworkIDs => _placedEntityNetworkIDs;

		public ISnappingPlaneShaderGroup ShaderGroup { get; set; }

		protected virtual void Awake()
		{
			base.gameObject.InjectGameObject();
			_meshRenderer = GetComponent<MeshRenderer>();
			_material = ((_meshRenderer != null) ? _meshRenderer.material : null);
			if (_meshRenderer != null)
			{
				_meshRenderer.enabled = false;
			}
			_colliders = GetComponents<Collider>();
			Rigidbody componentInParent = GetComponentInParent<Rigidbody>();
			_parentHasDynamicRigidbody = componentInParent != null && !componentInParent.isKinematic;
			_ownerHeldItem = GetComponentInParent<HeldItem>();
			InitializePlacementShaderDefaults();
		}

		private void InitializePlacementShaderDefaults()
		{
			if (!(_material == null))
			{
				_material.SetFloat(Radius, 3f);
				_material.SetFloat(GridSize, _gridSize);
			}
		}

		private float CalculateRadiusFromBounds(MeshRenderer[] renderers, float perObjectMultiplier = 1f)
		{
			if (!TryGetCombinedRendererBounds(renderers, out var combined))
			{
				return 1f * perObjectMultiplier;
			}
			float x = combined.extents.x;
			float z = combined.extents.z;
			return Mathf.Sqrt(x * x + z * z) * 2.5f * perObjectMultiplier;
		}

		protected virtual void OnEnable()
		{
			if (_meshRenderer != null)
			{
				_meshRenderer.enabled = false;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			IsLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (_meshRenderer != null)
			{
				_meshRenderer.enabled = false;
			}
			SyncList<uint> placedEntityNetworkIDs = _placedEntityNetworkIDs;
			placedEntityNetworkIDs.OnChange = (Action<SyncList<uint>.Operation, int, uint>)Delegate.Combine(placedEntityNetworkIDs.OnChange, new Action<SyncList<uint>.Operation, int, uint>(OnPlacedEntityNetworkIDsChanged));
			if (!base.isServer)
			{
				SetForLateJoiner();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			SyncList<uint> placedEntityNetworkIDs = _placedEntityNetworkIDs;
			placedEntityNetworkIDs.OnChange = (Action<SyncList<uint>.Operation, int, uint>)Delegate.Remove(placedEntityNetworkIDs.OnChange, new Action<SyncList<uint>.Operation, int, uint>(OnPlacedEntityNetworkIDsChanged));
		}

		protected virtual void SetForLateJoiner()
		{
			IsLateJoinCompleted = true;
			if (_placedEntityNetworkIDs.Count > 0)
			{
				ResolveAndCachePlacedEntitiesAsync().Forget();
			}
		}

		private async UniTaskVoid ResolveAndCachePlacedEntitiesAsync()
		{
			foreach (uint placedEntityNetworkID in _placedEntityNetworkIDs)
			{
				(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(placedEntityNetworkID, 30, 100, this.GetCancellationTokenOnDestroy());
				if (tuple.Item1)
				{
					_ = tuple.Item2 != null;
				}
			}
		}

		public void Enable()
		{
			IsEnabled = true;
			SetCollidersEnabled(enabled: true);
		}

		public void Disable()
		{
			IsEnabled = false;
			SetCollidersEnabled(enabled: false);
		}

		private void SetCollidersEnabled(bool enabled)
		{
			if (_colliders == null)
			{
				return;
			}
			Collider[] colliders = _colliders;
			foreach (Collider collider in colliders)
			{
				if (collider != null)
				{
					collider.enabled = enabled;
				}
			}
		}

		public void ActivatePlacementShader(Vector3 centerPosition, float radius)
		{
			if (!IsPlacementShaderDeactivated && !(_meshRenderer == null) && !(_material == null))
			{
				_meshRenderer.enabled = true;
				_material.SetVector(Position, centerPosition);
				_material.SetVector(CenterPosition, centerPosition);
				_material.SetVector(PlaneNormal, base.transform.up);
				_material.SetVector(PlaneTangent, base.transform.right);
				_material.SetVector(PlaneBitangent, base.transform.forward);
				_material.SetVector(PlaneOrigin, (ShaderGroup != null) ? ShaderGroup.SharedOrigin : base.transform.position);
				_material.SetFloat(Radius, Mathf.Max(radius, 0.5f));
				_material.SetFloat(GridSize, _gridSize);
				ShaderGroup?.OnMemberShaderActivated(this, centerPosition, radius);
			}
		}

		public void ActivatePlacementShaderForGroup(Vector3 centerPosition, float radius, Vector3 sharedOrigin)
		{
			if (!IsPlacementShaderDeactivated && !(_meshRenderer == null) && !(_material == null))
			{
				_meshRenderer.enabled = true;
				_material.SetVector(Position, centerPosition);
				_material.SetVector(CenterPosition, centerPosition);
				_material.SetVector(PlaneNormal, base.transform.up);
				_material.SetVector(PlaneTangent, base.transform.right);
				_material.SetVector(PlaneBitangent, base.transform.forward);
				_material.SetVector(PlaneOrigin, sharedOrigin);
				_material.SetFloat(Radius, Mathf.Max(radius, 0.5f));
				_material.SetFloat(GridSize, _gridSize);
			}
		}

		public void UpdateShaderForGroup(Vector3 centerPosition, Vector3 sharedOrigin)
		{
			if (!(_meshRenderer == null) && !(_material == null))
			{
				_material.SetVector(Position, centerPosition);
				_material.SetVector(CenterPosition, centerPosition);
				_material.SetVector(PlaneNormal, base.transform.up);
				_material.SetVector(PlaneTangent, base.transform.right);
				_material.SetVector(PlaneBitangent, base.transform.forward);
				_material.SetVector(PlaneOrigin, sharedOrigin);
			}
		}

		public void ActivatePlacementShader(Vector3 centerPosition, MeshRenderer[] renderers, float radiusMultiplier = 1f)
		{
			float radius = CalculateRadiusFromBounds(renderers, radiusMultiplier);
			ActivatePlacementShader(centerPosition, radius);
		}

		public void DeactivatePlacementShader()
		{
			if (!(_meshRenderer == null))
			{
				bool num = _meshRenderer.enabled;
				_meshRenderer.enabled = false;
				if (num)
				{
					ShaderGroup?.OnMemberShaderDeactivated(this);
				}
			}
		}

		private static bool TryGetCombinedRendererBounds(MeshRenderer[] renderers, out Bounds combined)
		{
			combined = default(Bounds);
			bool flag = false;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (!(meshRenderer == null) && meshRenderer.enabled && !(meshRenderer.bounds.size == Vector3.zero))
				{
					if (!flag)
					{
						combined = meshRenderer.bounds;
						flag = true;
					}
					else
					{
						combined.Encapsulate(meshRenderer.bounds);
					}
				}
			}
			return flag;
		}

		private Vector3 CalculatePivotOnCenter(Vector3 pivotPosition, Vector3 normal)
		{
			float num = PlacementGeometry.CalculateDynamicNormalOffset(_currentRenderers, pivotPosition, normal);
			return base.transform.position + normal * num;
		}

		private void SetOnObjectEnterTransform(Transform objectTransform, Vector3 pointOnPlane, Vector3 normal, Vector3 positionOffset, Quaternion configRotation)
		{
			if (SnapsToCenter)
			{
				objectTransform.position = CalculatePivotOnCenter(objectTransform.position, normal);
			}
			else
			{
				float num = PlacementGeometry.CalculateDynamicNormalOffset(_currentRenderers, objectTransform.position, normal);
				Vector3 vector = pointOnPlane + normal * (num + 0.001f);
				Vector3 vector2 = PlacementGeometry.CalculateTangentialOffset(_currentRenderers, objectTransform.position, normal);
				objectTransform.position = vector - vector2;
			}
			if (_objectAlignRotationCoroutine != null)
			{
				StopCoroutine(_objectAlignRotationCoroutine);
				_objectAlignRotationCoroutine = null;
			}
			if (AlignToSurfaceNormal)
			{
				_recaptureSurfaceForward = true;
				return;
			}
			Vector3 surfacePoint = (SnapsToCenter ? base.transform.position : pointOnPlane);
			_objectAlignRotationCoroutine = StartCoroutine(AlignObjectRotationCo(objectTransform, normal, surfacePoint, positionOffset, configRotation));
		}

		private void UpdateObjectPosition(Transform objectTransform, Vector3 pointOnPlane, Vector3 normal, Vector3 positionOffset)
		{
			if (!SnapsToCenter)
			{
				Vector3 b = PlacementGeometry.CalculateTargetPosition(_currentRenderers, objectTransform.position, pointOnPlane, normal, positionOffset, objectTransform);
				b += normal * 0.005f;
				objectTransform.position = Vector3.Lerp(objectTransform.position, b, Time.deltaTime * 20f);
			}
		}

		private void UpdateSurfaceAlignedRotation(Transform objectTransform, ObjectPlacementData data)
		{
			Quaternion quaternion = Quaternion.Euler(data.ConfigRotation);
			if (_recaptureSurfaceForward)
			{
				Quaternion quaternion2 = objectTransform.rotation * Quaternion.Inverse(quaternion);
				_surfaceBaseForward = Vector3.ProjectOnPlane(quaternion2 * Vector3.forward, data.Normal);
				if (_surfaceBaseForward.sqrMagnitude < 0.001f)
				{
					_surfaceBaseForward = Vector3.ProjectOnPlane(quaternion2 * Vector3.up, data.Normal);
				}
				_surfaceBaseForward.Normalize();
				_lastSurfaceNormal = data.Normal;
				_recaptureSurfaceForward = false;
			}
			else if (_lastSurfaceNormal != data.Normal)
			{
				_surfaceBaseForward = Quaternion.FromToRotation(_lastSurfaceNormal, data.Normal) * _surfaceBaseForward;
				_surfaceBaseForward = Vector3.ProjectOnPlane(_surfaceBaseForward, data.Normal).normalized;
				_lastSurfaceNormal = data.Normal;
			}
			Quaternion b = CalculatePlaneAlignedRotation(data.Normal, _surfaceBaseForward) * quaternion;
			objectTransform.rotation = Quaternion.Slerp(objectTransform.rotation, b, Time.deltaTime * surfaceAlignSpeed);
		}

		private IEnumerator AlignObjectRotationCo(Transform objectTransform, Vector3 normal, Vector3 surfacePoint, Vector3 positionOffset, Quaternion configRotation)
		{
			Quaternion quaternion = objectTransform.rotation * Quaternion.Inverse(configRotation);
			Vector3 objectForward = Vector3.ProjectOnPlane(quaternion * Vector3.forward, normal);
			if (objectForward.sqrMagnitude < 0.001f)
			{
				objectForward = Vector3.ProjectOnPlane(quaternion * Vector3.up, normal);
			}
			Quaternion targetRotation = CalculatePlaneAlignedRotation(normal, objectForward) * configRotation;
			while (Quaternion.Angle(objectTransform.rotation, targetRotation) > 0.1f)
			{
				objectTransform.rotation = Quaternion.Slerp(objectTransform.rotation, targetRotation, Time.deltaTime * 20f);
				objectTransform.position = (SnapsToCenter ? CalculatePivotOnCenter(objectTransform.position, normal) : PlacementGeometry.CalculateTargetPosition(_currentRenderers, objectTransform.position, surfacePoint, normal, positionOffset, objectTransform));
				yield return null;
			}
			objectTransform.rotation = targetRotation;
			objectTransform.position = (SnapsToCenter ? CalculatePivotOnCenter(objectTransform.position, normal) : PlacementGeometry.CalculateTargetPosition(_currentRenderers, objectTransform.position, surfacePoint, normal, positionOffset, objectTransform));
		}

		private Quaternion CalculatePlaneAlignedRotation(Vector3 normal)
		{
			Vector3 rhs = -base.transform.right;
			return Quaternion.LookRotation(Vector3.Cross(normal, rhs), base.transform.up);
		}

		private Quaternion CalculatePlaneAlignedRotation(Vector3 normal, Vector3 objectForward)
		{
			Vector3 vector = Vector3.ProjectOnPlane(objectForward, normal);
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.ProjectOnPlane(Vector3.forward, normal);
			}
			return Quaternion.LookRotation(vector.normalized, normal);
		}

		public void EnterObject(GameObject grabbableObject, ObjectPlacementData data)
		{
			_currentRenderers = grabbableObject.GetComponentsInChildren<MeshRenderer>();
			SetOnObjectEnterTransform(grabbableObject.transform, data.PointOnPlane, data.Normal, data.PositionOffset, Quaternion.Euler(data.ConfigRotation));
			if (!IsPlacementShaderDeactivated)
			{
				Vector3 centerPosition = grabbableObject.transform.TransformPoint(data.BoundsCenterLocalOffset);
				float radiusMultiplier = 1f;
				if (grabbableObject.TryGetComponent<HeldItem>(out var component))
				{
					radiusMultiplier = component.PlacementRadiusMultiplier;
				}
				ActivatePlacementShader(centerPosition, _currentRenderers, radiusMultiplier);
			}
			else if (_meshRenderer != null)
			{
				_meshRenderer.enabled = false;
			}
			OnObjectEnter.Invoke(grabbableObject, data);
		}

		public void StayObject(GameObject grabbableObject, ObjectPlacementData data, bool rotatingSignal)
		{
			if (!IsPlacementShaderDeactivated && _material != null)
			{
				Vector3 vector = grabbableObject.transform.TransformPoint(data.BoundsCenterLocalOffset);
				_material.SetVector(Position, vector);
				_material.SetVector(CenterPosition, vector);
				_material.SetVector(PlaneNormal, base.transform.up);
				_material.SetVector(PlaneTangent, base.transform.right);
				_material.SetVector(PlaneBitangent, base.transform.forward);
				_material.SetVector(PlaneOrigin, (ShaderGroup != null) ? ShaderGroup.SharedOrigin : base.transform.position);
				ShaderGroup?.OnMemberShaderUpdated(this, vector);
			}
			if (rotatingSignal && _objectAlignRotationCoroutine != null)
			{
				StopCoroutine(_objectAlignRotationCoroutine);
				_objectAlignRotationCoroutine = null;
			}
			if (AlignToSurfaceNormal)
			{
				if (rotatingSignal)
				{
					_recaptureSurfaceForward = true;
				}
				else
				{
					UpdateSurfaceAlignedRotation(grabbableObject.transform, data);
					UpdateObjectPosition(grabbableObject.transform, data.PointOnPlane, data.Normal, data.PositionOffset);
				}
			}
			else if (!rotatingSignal)
			{
				UpdateObjectPosition(grabbableObject.transform, data.PointOnPlane, data.Normal, data.PositionOffset);
			}
			else if (SnapsToCenter)
			{
				grabbableObject.transform.position = CalculatePivotOnCenter(grabbableObject.transform.position, data.Normal);
			}
			OnObjectStay.Invoke(grabbableObject, data, rotatingSignal);
		}

		public void ExitObject(GameObject grabbableObject)
		{
			DeactivatePlacementShader();
			_currentRenderers = null;
			_recaptureSurfaceForward = false;
			if (_objectAlignRotationCoroutine != null)
			{
				StopCoroutine(_objectAlignRotationCoroutine);
				_objectAlignRotationCoroutine = null;
			}
			OnObjectExit.Invoke(grabbableObject);
		}

		public void PlaceObject(GameObject grabbableObject)
		{
			DeactivatePlacementShader();
			if (grabbableObject.TryGetComponent<IPlaceable>(out var component))
			{
				if (grabbableObject.TryGetComponent<NetworkIdentity>(out var component2))
				{
					component.SetPlacementNetworkData(base.netId, SnappingPlaneIndex);
					AddPlacedEntityNetworkID(component2.netId);
					ComputeLocalPlacementOffset(grabbableObject.transform, out var localPositionOffset, out var localRotationOffsetEuler);
					CmdParentPlacedItem(component2, localPositionOffset, localRotationOffsetEuler);
				}
				component.OnPlacedActions();
			}
			OnObjectPlaced.Invoke(grabbableObject);
			ObjectivesEventBus.Raise(ObjectiveSignal.FurniturePlacedOnVehicle, grabbableObject);
		}

		public void FallObject(GameObject grabbableObject)
		{
			DeactivatePlacementShader();
			if (grabbableObject.TryGetComponent<IPlaceable>(out var component))
			{
				component.OnFallenActions();
			}
			OnObjectFallen.Invoke(grabbableObject);
		}

		private bool ShouldIsolatePlacedItem()
		{
			if (!_parentHasDynamicRigidbody)
			{
				if (_ownerHeldItem != null)
				{
					return _ownerHeldItem.IsVehicleColliderIsolated;
				}
				return false;
			}
			return true;
		}

		public void RemoveObject(GameObject grabbableObject)
		{
			if (grabbableObject.TryGetComponent<HeldItem>(out var component) && component.IsVehicleColliderIsolated)
			{
				component.SetVehicleIsolationRecursive(isolated: false);
			}
			if (MakeObjectsStatic && grabbableObject.TryGetComponent<IPlaceable>(out var component2))
			{
				component2.SetPlacementNetworkData(0u, 0);
				uint networkID = grabbableObject.GetComponent<NetworkIdentity>().netId;
				RemovePlacedEntityNetworkID(networkID);
				component2.OnRemovedActions();
			}
			OnObjectRemoved.Invoke(grabbableObject);
		}

		public void DetachForEquip(GameObject grabbableObject)
		{
			if (!(grabbableObject == null))
			{
				if (grabbableObject.TryGetComponent<IPlaceable>(out var component))
				{
					component.SetPlacementNetworkData(0u, 0);
				}
				if (grabbableObject.TryGetComponent<NetworkIdentity>(out var component2))
				{
					RemovePlacedEntityNetworkID(component2.netId);
				}
			}
		}

		public bool IsPlacementAllowed(GameObject targetObject)
		{
			if (AcceptAll)
			{
				return true;
			}
			if (_allowedItemIDs == null || _allowedItemIDs.Count == 0)
			{
				return true;
			}
			string placementID = PlacementIDProvider.GetPlacementID(targetObject);
			if (string.IsNullOrEmpty(placementID))
			{
				return true;
			}
			return _allowedItemIDs.Contains(placementID);
		}

		public bool SnapObjectExternal(HeldItem heldItem, Vector3 hitPoint, Vector3 hitNormal)
		{
			if (heldItem == null)
			{
				return false;
			}
			if (!heldItem.TryGetComponent<IPlaceable>(out var component))
			{
				return false;
			}
			if (!IsPlacementAllowed(heldItem.gameObject))
			{
				return false;
			}
			if (!IsEnabled)
			{
				return false;
			}
			if (IsSingleUseSlot && IsAnyObjectPlaced)
			{
				return false;
			}
			Quaternion rotation = CalculatePlaneAlignedRotation(hitNormal, heldItem.transform.forward) * Quaternion.Euler(component.DefaultPlacingRotation);
			heldItem.transform.rotation = rotation;
			MeshRenderer[] componentsInChildren = heldItem.GetComponentsInChildren<MeshRenderer>();
			Vector3 position;
			if (SnapsToCenter)
			{
				float num = PlacementGeometry.CalculateDynamicNormalOffset(componentsInChildren, heldItem.transform.position, hitNormal);
				position = base.transform.position + hitNormal * num;
			}
			else
			{
				position = PlacementGeometry.CalculateTargetPosition(componentsInChildren, heldItem.transform.position, hitPoint, hitNormal);
			}
			heldItem.transform.position = position;
			ComputeLocalPlacementOffset(heldItem.transform, out var localPositionOffset, out var localRotationOffsetEuler);
			NetworkIdentity component2 = heldItem.GetComponent<NetworkIdentity>();
			CmdSnapObjectExternal(component2, localPositionOffset, localRotationOffsetEuler);
			return true;
		}

		private void CleanupPreviousPlacement(IPlaceable placeable, uint objectNetId)
		{
			PlacementNetworkData placementNetworkData = placeable.PlacementNetworkData;
			if (placementNetworkData.ParentNetworkID != 0 && (placementNetworkData.ParentNetworkID != base.netId || placementNetworkData.SnappingPlaneIndex != SnappingPlaneIndex) && networkManager.TryGetNetworkObjectById(placementNetworkData.ParentNetworkID, out var networkObject) && networkObject.TryGetComponent<NetworkIdentity>(out var component) && component.TryGetComponent<ISnappingPlaneContainer>(out var component2))
			{
				SnappingPlane snappingPlaneByIndex = component2.GetSnappingPlaneByIndex(placementNetworkData.SnappingPlaneIndex);
				if (snappingPlaneByIndex != null && snappingPlaneByIndex._placedEntityNetworkIDs.Contains(objectNetId))
				{
					snappingPlaneByIndex._placedEntityNetworkIDs.Remove(objectNetId);
				}
			}
		}

		private void ComputeLocalPlacementOffset(Transform itemTransform, out Vector3 localPositionOffset, out Vector3 localRotationOffsetEuler)
		{
			NetworkedTransform networkedTransform = ResolveParentNetworkedTransform();
			if (networkedTransform == null)
			{
				localPositionOffset = itemTransform.position;
				localRotationOffsetEuler = itemTransform.rotation.eulerAngles;
			}
			else
			{
				Transform transform = networkedTransform.transform;
				localPositionOffset = Quaternion.Inverse(transform.rotation) * (itemTransform.position - transform.position);
				localRotationOffsetEuler = (Quaternion.Inverse(transform.rotation) * itemTransform.rotation).eulerAngles;
			}
		}

		[Server]
		private void ServerParentItemToPlane(NetworkedTransform itemNT, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::ServerParentItemToPlane(EvilCore.Networking.Parenting.NetworkedTransform,UnityEngine.Vector3,UnityEngine.Vector3)' called when server was not active");
			}
			else if (!(itemNT == null))
			{
				NetworkedTransform networkedTransform = ResolveParentNetworkedTransform();
				if (!(networkedTransform == null))
				{
					NetworkedTransformParentingConfig parentingConfig = new NetworkedTransformParentingConfig
					{
						PositionOffset = localPositionOffset,
						RotationOffset = localRotationOffsetEuler,
						KeepPositionAxes = false,
						KeepRotationAxes = false
					};
					itemNT.ServerSetParent(networkedTransform, parentingConfig, networkedTransform.NetworkedTransformIndex);
				}
			}
		}

		public NetworkedTransform ResolveParentNetworkedTransform()
		{
			if (!TryGetComponent<NetworkedTransform>(out var component))
			{
				return GetComponentInParent<NetworkedTransform>();
			}
			return component;
		}

		[Server]
		public void ServerRestorePlacement(HeldItem item, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::ServerRestorePlacement(NomadDrive.Features.Interaction.HeldItem,UnityEngine.Vector3,UnityEngine.Vector3)' called when server was not active");
			}
			else
			{
				if (item == null)
				{
					return;
				}
				uint num = item.netId;
				if (num == 0)
				{
					return;
				}
				if (!_placedEntityNetworkIDs.Contains(num))
				{
					_placedEntityNetworkIDs.Add(num);
				}
				if (item.TryGetComponent<NetworkedTransform>(out var component))
				{
					NetworkedTransform networkedTransform = ResolveParentNetworkedTransform();
					if (networkedTransform != null)
					{
						NetworkedTransformParentingConfig parentingConfig = new NetworkedTransformParentingConfig
						{
							PositionOffset = localPositionOffset,
							RotationOffset = localRotationOffsetEuler,
							KeepPositionAxes = false,
							KeepRotationAxes = false
						};
						component.ServerSetParent(networkedTransform, parentingConfig, networkedTransform.NetworkedTransformIndex);
					}
				}
				item.SetInteractionAvailability(newValue: true);
				item.SetRigidCollidersTriggered(newValue: false);
				item.PlacementNetworkData = new PlacementNetworkData
				{
					ParentNetworkID = base.netId,
					SnappingPlaneIndex = SnappingPlaneIndex
				};
				if (ShouldIsolatePlacedItem())
				{
					item.SetVehicleIsolationRecursive(isolated: true);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdParentPlacedItem(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			writer.WriteVector3(localPositionOffset);
			writer.WriteVector3(localRotationOffsetEuler);
			SendCommandInternal("System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdParentPlacedItem(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3)", -1420175060, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSnapObjectExternal(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			writer.WriteVector3(localPositionOffset);
			writer.WriteVector3(localRotationOffsetEuler);
			SendCommandInternal("System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdSnapObjectExternal(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", -140009129, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSnap(HeldItem heldItem)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::ServerSnap(NomadDrive.Features.Interaction.HeldItem)' called when server was not active");
			}
			else if (!(heldItem == null))
			{
				_ = AlignToSurfaceNormal;
				Vector3 position = base.transform.position;
				Vector3 up = base.transform.up;
				Quaternion quaternion = CalculatePlaneAlignedRotation(up, heldItem.transform.forward) * Quaternion.Euler(heldItem.DefaultPlacingRotation);
				heldItem.transform.rotation = quaternion;
				MeshRenderer[] componentsInChildren = heldItem.GetComponentsInChildren<MeshRenderer>();
				Vector3 targetPosition;
				if (SnapsToCenter)
				{
					float num = PlacementGeometry.CalculateDynamicNormalOffset(componentsInChildren, heldItem.transform.position, up);
					targetPosition = base.transform.position + up * num;
				}
				else
				{
					targetPosition = PlacementGeometry.CalculateTargetPosition(componentsInChildren, heldItem.transform.position, position, up);
				}
				ServerSnap(heldItem, targetPosition, quaternion);
			}
		}

		[Server]
		public void ServerSnap(HeldItem heldItem, Vector3 targetPosition, Quaternion targetRotation)
		{
			NetworkIdentity component;
			IPlaceable component2;
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::ServerSnap(NomadDrive.Features.Interaction.HeldItem,UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
			}
			else if (heldItem == null)
			{
				EvilLogger.LogError("[SnappingPlane] ServerSnap: heldItem is null", "ServerSnap", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlane.cs", 1064);
			}
			else if (!heldItem.TryGetComponent<NetworkIdentity>(out component))
			{
				EvilLogger.LogError("[SnappingPlane] ServerSnap: NetworkIdentity missing on heldItem", "ServerSnap", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlane.cs", 1070);
			}
			else if (!heldItem.TryGetComponent<IPlaceable>(out component2))
			{
				EvilLogger.LogError("[SnappingPlane] ServerSnap: IPlaceable missing on heldItem", "ServerSnap", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlane.cs", 1076);
			}
			else if (IsPlacementAllowed(heldItem.gameObject) && IsEnabled && (!IsSingleUseSlot || !IsAnyObjectPlaced))
			{
				CleanupPreviousPlacement(component2, component.netId);
				heldItem.transform.SetPositionAndRotation(targetPosition, targetRotation);
				ComputeLocalPlacementOffset(heldItem.transform, out var localPositionOffset, out var localRotationOffsetEuler);
				if (heldItem.TryGetComponent<NetworkedTransform>(out var component3))
				{
					ServerParentItemToPlane(component3, localPositionOffset, localRotationOffsetEuler);
				}
				heldItem.SetInteractionAvailability(newValue: true);
				heldItem.SetRigidCollidersTriggered(newValue: false);
				if (ShouldIsolatePlacedItem())
				{
					heldItem.SetVehicleIsolationRecursive(isolated: true);
				}
				component2.SetPlacementNetworkData(base.netId, SnappingPlaneIndex);
				bool flag = false;
				if (!_placedEntityNetworkIDs.Contains(component.netId))
				{
					_placedEntityNetworkIDs.Add(component.netId);
					flag = true;
				}
				component2.OnPlacedActions();
				OnObjectPlaced.Invoke(heldItem.gameObject);
				if (flag)
				{
					PlaySnapSoundNetworked();
				}
				RpcSyncSnapObjectExternal(component, localPositionOffset, localRotationOffsetEuler);
			}
		}

		[ClientRpc]
		private void RpcSyncSnapObjectExternal(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			writer.WriteVector3(localPositionOffset);
			writer.WriteVector3(localRotationOffsetEuler);
			SendRPCInternal("System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::RpcSyncSnapObjectExternal(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3)", -1129620470, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void AddPlacedEntityNetworkID(uint networkID)
		{
			if (!IsSingleUseSlot || !IsAnyObjectPlaced)
			{
				CmdAddPlacedEntityNetworkID(networkID);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdAddPlacedEntityNetworkID(uint networkID, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(networkID);
			SendCommandInternal("System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdAddPlacedEntityNetworkID(System.UInt32,Mirror.NetworkConnectionToClient)", 1586585542, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void RemovePlacedEntityNetworkID(uint networkID)
		{
			if (IsAnyObjectPlaced)
			{
				CmdRemovePlacedEntityNetworkID(networkID);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdRemovePlacedEntityNetworkID(uint networkID, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(networkID);
			SendCommandInternal("System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdRemovePlacedEntityNetworkID(System.UInt32,Mirror.NetworkConnectionToClient)", 235697359, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void PlaySnapSoundNetworked()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::PlaySnapSoundNetworked()' called when server was not active");
			}
			else if (snapSound.IsValid())
			{
				networkAudioRelay?.PlayOneShot(snapSound, base.transform.position);
			}
		}

		private void OnPlacedEntityNetworkIDsChanged(SyncList<uint>.Operation op, int index, uint item)
		{
			_ = IsLateJoinCompleted;
		}

		public async UniTask<GameObject[]> GetPlacedEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			List<GameObject> entities = new List<GameObject>();
			foreach (uint placedEntityNetworkID in _placedEntityNetworkIDs)
			{
				(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(placedEntityNetworkID, 30, 100, cancellationToken);
				if (tuple.Item1 && tuple.Item2 != null)
				{
					entities.Add(tuple.Item2);
				}
			}
			return entities.ToArray();
		}

		private static async UniTask ResolveSnappedInteractablesRecursiveAsync(IEnumerable<uint> networkIDs, List<Interactable> results, HashSet<uint> visited, INetworkManager networkManager, CancellationToken cancellationToken)
		{
			foreach (uint networkID in networkIDs)
			{
				if (!visited.Add(networkID))
				{
					continue;
				}
				(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(networkID, 30, 100, cancellationToken);
				if (tuple.Item1 && !(tuple.Item2 == null))
				{
					Interactable component = tuple.Item2.GetComponent<Interactable>();
					if (component != null)
					{
						results.Add(component);
					}
					SnappingPlane[] componentsInChildren = tuple.Item2.GetComponentsInChildren<SnappingPlane>();
					SnappingPlane[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						await ResolveSnappedInteractablesRecursiveAsync(array[i].PlacedEntityNetworkIDs, results, visited, networkManager, cancellationToken);
					}
				}
			}
		}

		public void ActivateSnappedObjectsPhysics()
		{
			ActivateSnappedObjectsPhysicsAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid ActivateSnappedObjectsPhysicsAsync(CancellationToken cancellationToken)
		{
			List<Interactable> interactables = new List<Interactable>();
			await ResolveSnappedInteractablesRecursiveAsync(_placedEntityNetworkIDs, interactables, new HashSet<uint>(), networkManager, cancellationToken);
			foreach (Interactable item in interactables)
			{
				item.SetRigidCollidersEnabled(newValue: true);
			}
		}

		public void DeactivateSnappedObjectsPhysics()
		{
			DeactivateSnappedObjectsPhysicsAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid DeactivateSnappedObjectsPhysicsAsync(CancellationToken cancellationToken)
		{
			List<Interactable> interactables = new List<Interactable>();
			await ResolveSnappedInteractablesRecursiveAsync(_placedEntityNetworkIDs, interactables, new HashSet<uint>(), networkManager, cancellationToken);
			foreach (Interactable item in interactables)
			{
				item.SetRigidCollidersEnabled(newValue: false);
			}
		}

		public void ActivateSnappedObjectsInteraction()
		{
			ActivateSnappedObjectsInteractionAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid ActivateSnappedObjectsInteractionAsync(CancellationToken cancellationToken)
		{
			List<Interactable> interactables = new List<Interactable>();
			await ResolveSnappedInteractablesRecursiveAsync(_placedEntityNetworkIDs, interactables, new HashSet<uint>(), networkManager, cancellationToken);
			foreach (Interactable item in interactables)
			{
				item.SetInteractionAvailability(newValue: true);
			}
		}

		public void DeactivateSnappedObjectsInteraction()
		{
			DeactivateSnappedObjectsInteractionAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid DeactivateSnappedObjectsInteractionAsync(CancellationToken cancellationToken)
		{
			List<Interactable> interactables = new List<Interactable>();
			await ResolveSnappedInteractablesRecursiveAsync(_placedEntityNetworkIDs, interactables, new HashSet<uint>(), networkManager, cancellationToken);
			foreach (Interactable item in interactables)
			{
				item.SetInteractionAvailability(newValue: false);
			}
		}

		public void GetPlacedObjectRigidColliders(List<Collider> results)
		{
			foreach (uint placedEntityNetworkID in _placedEntityNetworkIDs)
			{
				if (networkManager.TryGetNetworkObjectById(placedEntityNetworkID, out var networkObject) && networkObject.TryGetComponent<Interactable>(out var component))
				{
					Collider[] rigidColliders = component.GetRigidColliders();
					if (rigidColliders != null)
					{
						results.AddRange(rigidColliders);
					}
				}
			}
		}

		public SnappingPlane()
		{
			InitSyncObject(_placedEntityNetworkIDs);
		}

		static SnappingPlane()
		{
			Position = Shader.PropertyToID("_Position");
			Radius = Shader.PropertyToID("_Radius");
			CenterPosition = Shader.PropertyToID("_CenterPosition");
			PlaneNormal = Shader.PropertyToID("_PlaneNormal");
			PlaneTangent = Shader.PropertyToID("_PlaneTangent");
			PlaneBitangent = Shader.PropertyToID("_PlaneBitangent");
			GridSize = Shader.PropertyToID("_GridSize");
			PlaneOrigin = Shader.PropertyToID("_PlaneOrigin");
			RemoteProcedureCalls.RegisterCommand(typeof(SnappingPlane), "System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdParentPlacedItem(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_CmdParentPlacedItem__NetworkIdentity__Vector3__Vector3, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(SnappingPlane), "System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdSnapObjectExternal(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdSnapObjectExternal__NetworkIdentity__Vector3__Vector3__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(SnappingPlane), "System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdAddPlacedEntityNetworkID(System.UInt32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdAddPlacedEntityNetworkID__UInt32__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(SnappingPlane), "System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::CmdRemovePlacedEntityNetworkID(System.UInt32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRemovePlacedEntityNetworkID__UInt32__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(SnappingPlane), "System.Void NomadDrive.Features.ObjectPlacement.SnappingPlane::RpcSyncSnapObjectExternal(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_RpcSyncSnapObjectExternal__NetworkIdentity__Vector3__Vector3);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdParentPlacedItem__NetworkIdentity__Vector3__Vector3(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			if (!(itemIdentity == null) && itemIdentity.TryGetComponent<NetworkedTransform>(out var component))
			{
				ServerParentItemToPlane(component, localPositionOffset, localRotationOffsetEuler);
				if (ShouldIsolatePlacedItem() && itemIdentity.TryGetComponent<HeldItem>(out var component2))
				{
					component2.SetVehicleIsolationRecursive(isolated: true);
				}
			}
		}

		protected static void InvokeUserCode_CmdParentPlacedItem__NetworkIdentity__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdParentPlacedItem called on client.");
			}
			else
			{
				((SnappingPlane)obj).UserCode_CmdParentPlacedItem__NetworkIdentity__Vector3__Vector3(reader.ReadNetworkIdentity(), reader.ReadVector3(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdSnapObjectExternal__NetworkIdentity__Vector3__Vector3__NetworkConnectionToClient(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler, NetworkConnectionToClient sender)
		{
			if (itemIdentity == null)
			{
				EvilLogger.LogError("[SnappingPlane] CmdSnapObjectExternal: itemIdentity is null", "CmdSnapObjectExternal", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlane.cs", 989);
				return;
			}
			if (!itemIdentity.TryGetComponent<HeldItem>(out var component))
			{
				EvilLogger.LogError("[SnappingPlane] CmdSnapObjectExternal: HeldItem component missing", "CmdSnapObjectExternal", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlane.cs", 995);
				return;
			}
			NetworkedTransform networkedTransform = ResolveParentNetworkedTransform();
			Vector3 targetPosition;
			Quaternion targetRotation;
			if (networkedTransform != null)
			{
				Transform transform = networkedTransform.transform;
				targetPosition = transform.position + transform.rotation * localPositionOffset;
				targetRotation = transform.rotation * Quaternion.Euler(localRotationOffsetEuler);
			}
			else
			{
				targetPosition = localPositionOffset;
				targetRotation = Quaternion.Euler(localRotationOffsetEuler);
			}
			ServerSnap(component, targetPosition, targetRotation);
		}

		protected static void InvokeUserCode_CmdSnapObjectExternal__NetworkIdentity__Vector3__Vector3__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSnapObjectExternal called on client.");
			}
			else
			{
				((SnappingPlane)obj).UserCode_CmdSnapObjectExternal__NetworkIdentity__Vector3__Vector3__NetworkConnectionToClient(reader.ReadNetworkIdentity(), reader.ReadVector3(), reader.ReadVector3(), senderConnection);
			}
		}

		protected void UserCode_RpcSyncSnapObjectExternal__NetworkIdentity__Vector3__Vector3(NetworkIdentity itemIdentity, Vector3 localPositionOffset, Vector3 localRotationOffsetEuler)
		{
			if (itemIdentity == null)
			{
				return;
			}
			if (itemIdentity.TryGetComponent<HeldItem>(out var component))
			{
				component.StopPlacementStream();
			}
			if (itemIdentity.TryGetComponent<NetworkedTransform>(out var component2))
			{
				NetworkedTransform networkedTransform = ResolveParentNetworkedTransform();
				if (!(networkedTransform == null))
				{
					Transform transform = networkedTransform.transform;
					Vector3 position = transform.position + transform.rotation * localPositionOffset;
					Quaternion rotation = transform.rotation * Quaternion.Euler(localRotationOffsetEuler);
					itemIdentity.transform.SetPositionAndRotation(position, rotation);
					NetworkedTransformParentingConfig config = new NetworkedTransformParentingConfig
					{
						PositionOffset = localPositionOffset,
						RotationOffset = localRotationOffsetEuler,
						KeepPositionAxes = false,
						KeepRotationAxes = false
					};
					component2.ClientApplyParentingImmediate(networkedTransform, config, networkedTransform.NetworkedTransformIndex);
				}
			}
		}

		protected static void InvokeUserCode_RpcSyncSnapObjectExternal__NetworkIdentity__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSyncSnapObjectExternal called on server.");
			}
			else
			{
				((SnappingPlane)obj).UserCode_RpcSyncSnapObjectExternal__NetworkIdentity__Vector3__Vector3(reader.ReadNetworkIdentity(), reader.ReadVector3(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdAddPlacedEntityNetworkID__UInt32__NetworkConnectionToClient(uint networkID, NetworkConnectionToClient sender)
		{
			if (networkManager.TryGetNetworkObjectById(networkID, out var networkObject) && networkObject.TryGetComponent<IPlaceable>(out var _) && IsPlacementAllowed(networkObject) && (!IsSingleUseSlot || !IsAnyObjectPlaced) && !_placedEntityNetworkIDs.Contains(networkID))
			{
				_placedEntityNetworkIDs.Add(networkID);
				PlaySnapSoundNetworked();
			}
		}

		protected static void InvokeUserCode_CmdAddPlacedEntityNetworkID__UInt32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAddPlacedEntityNetworkID called on client.");
			}
			else
			{
				((SnappingPlane)obj).UserCode_CmdAddPlacedEntityNetworkID__UInt32__NetworkConnectionToClient(reader.ReadVarUInt(), senderConnection);
			}
		}

		protected void UserCode_CmdRemovePlacedEntityNetworkID__UInt32__NetworkConnectionToClient(uint networkID, NetworkConnectionToClient sender)
		{
			if (_placedEntityNetworkIDs.Contains(networkID))
			{
				_placedEntityNetworkIDs.Remove(networkID);
			}
		}

		protected static void InvokeUserCode_CmdRemovePlacedEntityNetworkID__UInt32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRemovePlacedEntityNetworkID called on client.");
			}
			else
			{
				((SnappingPlane)obj).UserCode_CmdRemovePlacedEntityNetworkID__UInt32__NetworkConnectionToClient(reader.ReadVarUInt(), senderConnection);
			}
		}
	}
}
