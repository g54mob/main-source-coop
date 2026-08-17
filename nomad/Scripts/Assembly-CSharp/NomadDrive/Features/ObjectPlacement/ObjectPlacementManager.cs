using System;
using System.Collections.Generic;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.Audio;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking;
using EvilCore.Particles;
using Mirror;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.ObjectPlacement
{
	public class ObjectPlacementManager : MonoBehaviour, IObjectPlacementManager, IUniqueNetworkComponent, IPlayerComponent
	{
		public ObjectPlacementSetting objectPlacementSetting;

		public bool isSnappingModeActive;

		public Func<bool> OnTrySlotAttach;

		private IPlaceable _currentPlaceable;

		private Interactable _currentInteractable;

		private MeshRenderer[] _currentRenderers;

		private int _snappingPlaneLayerMask;

		private int _placementObstacleMask;

		private int _dropBlockingMask;

		private int _snappingObstacleMask;

		private bool _hasUserRotated;

		private Vector3 _currentBoundsCenterLocalOffset;

		private Vector3 _currentPositionOffset;

		private IPlayerService _playerService;

		private Transform _cameraTransform;

		private FirstPersonController _firstPersonController;

		private Collider _playerCapsuleCollider;

		private float _currentObjectDistanceToCamera;

		private InputActionPromptsPanel _inputActionPromptsPanel;

		private ICastingManager _castingManager;

		[Header("Drop Settings")]
		[SerializeField]
		private DropSettings _dropSettings = DropSettings.Default;

		[SerializeField]
		private DropSurfaceEffectConfig dropSurfaceEffectConfig;

		[SerializeField]
		private DropSurfaceSoundConfig dropSurfaceSoundConfig;

		[Header("Drop Indicator")]
		[SerializeField]
		private DropIndicatorSettings _dropIndicatorSettings;

		[Header("Target Collision")]
		[SerializeField]
		[Range(0.5f, 1f)]
		private float targetCollisionShrinkFactor = 0.85f;

		private bool _isTargetPositionColliding;

		private bool _isGroundDetected;

		private Vector3 _lastSnapHitNormal;

		private LayerMask _currentCollisionLayers;

		private static readonly Collider[] TargetOverlapBuffer = new Collider[32];

		[Header("Placement Performance")]
		[Tooltip("Seconds between the expensive placement overlap/drop-cast samples. The highlight and CanPlaceable flag refresh on this cadence; the actual release always re-validates fresh, so throttling can never cause an invalid placement.")]
		[SerializeField]
		private float placementSampleInterval = 0.05f;

		[Tooltip("Seconds between streamed placement-pose packets sent to other clients (~20Hz).")]
		[SerializeField]
		private float placementStreamInterval = 0.05f;

		private float _placementStreamTimer;

		private float _collisionSampleTimer;

		private float _dropSampleTimer;

		private bool _cachedFloatingColliding;

		private DropResult _cachedDropResult;

		private IRaycastHandle _dropCastHandle;

		private CastResult _lastDropCastResult;

		private DropIndicatorController _dropIndicator;

		private SnappingPlane _dropIndicatorSnappingPlane;

		private readonly List<Collider> _disabledPlacedObjectSnappingColliders = new List<Collider>();

		private readonly List<Collider> _disabledOwnSnappingColliders = new List<Collider>();

		private readonly List<PlacementCollisionForwarder> _snappedObjectForwarders = new List<PlacementCollisionForwarder>();

		private readonly List<Collider> _snappedCollidersSetToTrigger = new List<Collider>();

		private readonly List<Collider> _snappedPlayerIgnoredColliders = new List<Collider>();

		private readonly List<Interactable> _snappedInteractables = new List<Interactable>();

		private readonly HashSet<Collider> _liveOverlapExclusions = new HashSet<Collider>();

		private readonly HashSet<Collider> _dropSelfExclusions = new HashSet<Collider>();

		private readonly HashSet<uint> _compoundMemberNetIds = new HashSet<uint>();

		[Inject]
		private INetworkManager _networkManager;

		private INetworkedParticlesManager _networkedParticlesManager;

		private INetworkedAudioManager _networkAudioRelay;

		private static readonly RaycastHit[] SnappingPlaneHitBuffer = new RaycastHit[16];

		private static readonly RaycastHit[] SnappingObstacleBuffer = new RaycastHit[16];

		private Transform _playerCameraTransform;

		public int SetupPriority => 10;

		public UnityEvent<GameObject> OnPlacementEnter { get; set; } = new UnityEvent<GameObject>();

		public UnityEvent OnPlacementExit { get; set; } = new UnityEvent();

		public GameObject CurrentPlacementObject { get; private set; }

		public bool IsPlacementModeActive => CurrentPlacementObject != null;

		public float DefaultObjectDistanceToCamera { get; set; }

		public float SnappingAreaDetectionRayLength { get; set; } = 2.5f;

		public float PlacementShaderScale { get; set; }

		public SnappingPlane CurrentSnappingPlane { get; set; }

		public float ScrollSpeedMultiplier { get; set; } = 1f;

		public bool IsExternalSnapActive { get; set; }

		public bool IsActive { get; set; }

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				Init();
				return;
			}
			IsActive = false;
			base.enabled = false;
		}

		[Inject]
		private void Construct(IPlayerService playerService, InputActionPromptsPanel inputActionPromptsPanel, ICastingManager castingManager, INetworkedParticlesManager networkedParticlesManager, INetworkedAudioManager networkAudioRelay)
		{
			_playerService = playerService;
			_inputActionPromptsPanel = inputActionPromptsPanel;
			_castingManager = castingManager;
			_networkedParticlesManager = networkedParticlesManager;
			_networkAudioRelay = networkAudioRelay;
		}

		public void Init()
		{
			DefaultObjectDistanceToCamera = objectPlacementSetting.defaultObjectDistanceToCamera;
			_currentObjectDistanceToCamera = DefaultObjectDistanceToCamera;
			_snappingPlaneLayerMask = LayerMask.GetMask("SnappingPlane");
			_placementObstacleMask = LayerMask.GetMask("VehicleBody", "SnappedInteractableOnVehicle", "Interactable");
			_dropBlockingMask = LayerMask.GetMask("Default", "Interactable", "VehicleBody", "SnappedInteractableOnVehicle");
			_snappingObstacleMask = LayerMask.GetMask("Default", "NonInteractableCollision", "Interactable", "VehicleBody", "Restorable", "SnappedInteractableOnVehicle");
			_playerService.TryGetCameraTransform(out _cameraTransform);
			_playerService.TryGetFirstPersonController(out _firstPersonController);
			_playerCapsuleCollider = ((_firstPersonController != null) ? _firstPersonController.GetPlayerCapsuleCollider() : null);
			OnPlacementEnter.AddListener(OnPlacementEnterActions);
			OnPlacementExit.AddListener(OnObjectReleaseActions);
			Activate();
		}

		private void Update()
		{
			if (IsActive && IsPlacementModeActive)
			{
				HandleInputs();
			}
		}

		private void LateUpdate()
		{
			if (IsActive && IsPlacementModeActive)
			{
				CheckSnappingPlane();
				UpdateCurrentObjectTransform();
				CheckCollisions();
				UpdateDropIndicator();
				StreamPlacementPoseIfDue();
			}
		}

		private void StreamPlacementPoseIfDue()
		{
			if (_currentPlaceable is HeldItem heldItem)
			{
				_placementStreamTimer -= Time.deltaTime;
				if (!(_placementStreamTimer > 0f))
				{
					_placementStreamTimer = placementStreamInterval;
					Transform transform = CurrentPlacementObject.transform;
					heldItem.StreamPlacementPose(transform.position, transform.rotation);
				}
			}
		}

		private void CheckCollisions()
		{
			if (CurrentPlacementObject == null)
			{
				return;
			}
			if (IsExternalSnapActive)
			{
				_currentPlaceable.CanPlaceable = true;
				_currentInteractable.SetHighlight(ObjectHighlightType.CorrectPlacement);
				_dropIndicator?.Hide();
				return;
			}
			_collisionSampleTimer -= Time.deltaTime;
			if (_collisionSampleTimer <= 0f)
			{
				_collisionSampleTimer = placementSampleInterval;
				_cachedFloatingColliding = HasMatrixHiddenOverlapAtLivePose();
			}
			if (_currentPlaceable.IsCollidingAny() || _cachedFloatingColliding)
			{
				_currentPlaceable.CanPlaceable = false;
				_currentInteractable.SetHighlight(ObjectHighlightType.IncorrectPlacement);
				SetSnappedObjectsHighlight(ObjectHighlightType.IncorrectPlacement);
				_dropIndicator?.SetColliding(isColliding: true);
				return;
			}
			_dropIndicator?.SetColliding(_isTargetPositionColliding);
			if (!_isGroundDetected && !isSnappingModeActive)
			{
				_currentPlaceable.CanPlaceable = false;
				_currentInteractable.SetHighlight(ObjectHighlightType.IncorrectPlacement);
				SetSnappedObjectsHighlight(ObjectHighlightType.IncorrectPlacement);
				_dropIndicator?.Hide();
				return;
			}
			if (CurrentSnappingPlane != null && CurrentSnappingPlane.IsSingleUseSlot)
			{
				_currentInteractable.SetHighlight(ObjectHighlightType.CorrectPlacement);
				SetSnappedObjectsHighlight(ObjectHighlightType.CorrectPlacement);
			}
			else
			{
				_currentInteractable.SetHighlight(ObjectHighlightType.Neutral);
				SetSnappedObjectsHighlight(ObjectHighlightType.Neutral);
			}
			_currentPlaceable.CanPlaceable = !_isTargetPositionColliding;
			if (isSnappingModeActive)
			{
				_dropIndicator?.Hide();
			}
		}

		private void CheckSnappingPlane()
		{
			if (IsExternalSnapActive || !ValidateSnappingPrerequisites(out var placeable))
			{
				return;
			}
			Ray ray = CreateSnappingRay(SnappingAreaDetectionRayLength);
			int num = Physics.RaycastNonAlloc(ray.origin, ray.direction, SnappingPlaneHitBuffer, SnappingAreaDetectionRayLength, _snappingPlaneLayerMask);
			if (num == 0)
			{
				HandleNoSnappingPlaneHit();
				return;
			}
			SortSnappingHitsByDistance(num);
			SnappingPlane snappingPlane = null;
			RaycastHit hitInfo = default(RaycastHit);
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = SnappingPlaneHitBuffer[i];
				if (!IsSnappingPlaneOccluded(ray, raycastHit) && TryGetValidSnappingPlane(raycastHit, out var snappingPlane2))
				{
					snappingPlane = snappingPlane2;
					hitInfo = raycastHit;
					break;
				}
			}
			if (CurrentSnappingPlane != null && snappingPlane != CurrentSnappingPlane)
			{
				for (int j = 0; j < num; j++)
				{
					RaycastHit raycastHit2 = SnappingPlaneHitBuffer[j];
					if (!(raycastHit2.collider == null) && raycastHit2.collider.TryGetComponent<SnappingPlane>(out var component) && !(component != CurrentSnappingPlane))
					{
						if (!IsSnappingPlaneOccluded(ray, raycastHit2) && TryGetValidSnappingPlane(raycastHit2, out var _))
						{
							snappingPlane = CurrentSnappingPlane;
							hitInfo = raycastHit2;
						}
						break;
					}
				}
			}
			if (snappingPlane == null)
			{
				HandleNoSnappingPlaneHit();
				return;
			}
			_lastSnapHitNormal = hitInfo.normal;
			HandleSnappingPlaneInteraction(snappingPlane, hitInfo, placeable);
		}

		private static void SortSnappingHitsByDistance(int count)
		{
			for (int i = 1; i < count; i++)
			{
				RaycastHit raycastHit = SnappingPlaneHitBuffer[i];
				int num = i - 1;
				while (num >= 0 && SnappingPlaneHitBuffer[num].distance > raycastHit.distance)
				{
					SnappingPlaneHitBuffer[num + 1] = SnappingPlaneHitBuffer[num];
					num--;
				}
				SnappingPlaneHitBuffer[num + 1] = raycastHit;
			}
		}

		private bool ValidateSnappingPrerequisites(out IPlaceable placeable)
		{
			placeable = null;
			if (!IsPlacementModeActive)
			{
				return false;
			}
			if (!CurrentPlacementObject.TryGetComponent<IPlaceable>(out placeable))
			{
				EvilLogger.LogError("Missing IPlaceable component for snapping object!", "ValidateSnappingPrerequisites", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\ObjectPlacementManager.cs", 381);
				return false;
			}
			return true;
		}

		private Ray CreateSnappingRay(float snappingDistance)
		{
			return new Ray(_cameraTransform.position, _cameraTransform.TransformDirection(Vector3.forward) * snappingDistance);
		}

		private void HandleNoSnappingPlaneHit()
		{
			if (isSnappingModeActive && !(CurrentSnappingPlane == null))
			{
				CurrentSnappingPlane.ExitObject(CurrentPlacementObject);
				DisableSnappingMode();
			}
		}

		private bool IsSnappingPlaneOccluded(Ray ray, RaycastHit planeHit)
		{
			if (_snappingObstacleMask == 0)
			{
				return false;
			}
			float num = planeHit.distance - 0.02f;
			if (num <= 0f)
			{
				return false;
			}
			int num2 = Physics.RaycastNonAlloc(ray.origin, ray.direction, SnappingObstacleBuffer, num, _snappingObstacleMask, QueryTriggerInteraction.Ignore);
			Transform transform = ((CurrentPlacementObject != null) ? CurrentPlacementObject.transform : null);
			Transform root = planeHit.collider.transform.root;
			SnappingPlane component;
			bool flag = !planeHit.collider.TryGetComponent<SnappingPlane>(out component) || !component.IsOnDynamicRigidbodyParent;
			for (int i = 0; i < num2; i++)
			{
				Collider collider = SnappingObstacleBuffer[i].collider;
				if (collider == null || collider == planeHit.collider || (transform != null && collider.transform.IsChildOf(transform)))
				{
					continue;
				}
				if (_compoundMemberNetIds.Count > 0)
				{
					NetworkIdentity componentInParent = collider.GetComponentInParent<NetworkIdentity>();
					if (componentInParent != null && _compoundMemberNetIds.Contains(componentInParent.netId))
					{
						continue;
					}
				}
				if (!flag || !(collider.transform.root == root))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetValidSnappingPlane(RaycastHit hitInfo, out SnappingPlane snappingPlane)
		{
			snappingPlane = null;
			if (hitInfo.collider == null)
			{
				return false;
			}
			if (!hitInfo.collider.TryGetComponent<SnappingPlane>(out snappingPlane))
			{
				return false;
			}
			if (snappingPlane.transform.IsChildOf(CurrentPlacementObject.transform))
			{
				return false;
			}
			if (!snappingPlane.IsEnabled)
			{
				return false;
			}
			if (!snappingPlane.IsPlacementAllowed(CurrentPlacementObject))
			{
				return false;
			}
			if (snappingPlane.IsSingleUseSlot && !ValidateOneShotSlot(snappingPlane))
			{
				return false;
			}
			return true;
		}

		private bool ValidateOneShotSlot(SnappingPlane snappingPlane)
		{
			if (!_networkManager.TryGetNetworkObjectById(snappingPlane.netId, out var networkObject))
			{
				return false;
			}
			return ValidateSnappingPlaneContainer(networkObject, snappingPlane);
		}

		private bool ValidateSnappingPlaneContainer(GameObject networkedParentObject, SnappingPlane snappingPlane)
		{
			if (!networkedParentObject.TryGetComponent<ISnappingPlaneContainer>(out var component))
			{
				return true;
			}
			if (component.IsSnappingPlaneOccupied(snappingPlane))
			{
				return false;
			}
			return true;
		}

		private void HandleSnappingPlaneInteraction(SnappingPlane snappingPlane, RaycastHit hitInfo, IPlaceable placeable)
		{
			if (CurrentSnappingPlane == null)
			{
				HandleNewSnappingPlane(snappingPlane, hitInfo, placeable);
			}
			else
			{
				HandleExistingSnappingPlane(snappingPlane, hitInfo, placeable);
			}
		}

		private void HandleNewSnappingPlane(SnappingPlane snappingPlane, RaycastHit hitInfo, IPlaceable placeable)
		{
			EnableSnappingMode(snappingPlane);
			ObjectPlacementData data = CreatePlacementData(hitInfo, placeable);
			CurrentSnappingPlane.EnterObject(CurrentPlacementObject, data);
		}

		private void HandleExistingSnappingPlane(SnappingPlane snappingPlane, RaycastHit hitInfo, IPlaceable placeable)
		{
			ObjectPlacementData data = CreatePlacementData(hitInfo, placeable);
			if (CurrentSnappingPlane == snappingPlane)
			{
				bool rotateObjectButton = PlacingObjectInputs.GetRotateObjectButton();
				CurrentSnappingPlane.StayObject(CurrentPlacementObject, data, rotateObjectButton);
			}
			else
			{
				CurrentSnappingPlane.ExitObject(CurrentPlacementObject);
				EnableSnappingMode(snappingPlane);
				CurrentSnappingPlane.EnterObject(CurrentPlacementObject, data);
			}
		}

		private ObjectPlacementData CreatePlacementData(RaycastHit hitInfo, IPlaceable placeable)
		{
			return new ObjectPlacementData
			{
				PointOnPlane = hitInfo.point,
				Normal = hitInfo.normal,
				PlacementShaderScale = placeable.PlacementShaderScale,
				BoundsCenterLocalOffset = _currentBoundsCenterLocalOffset,
				PositionOffset = _currentPositionOffset,
				ConfigRotation = placeable.DefaultPlacingRotation
			};
		}

		private void EnableSnappingMode(SnappingPlane snappingPlane)
		{
			isSnappingModeActive = true;
			CurrentSnappingPlane = snappingPlane;
		}

		private void DisableSnappingMode()
		{
			isSnappingModeActive = false;
			CurrentSnappingPlane = null;
		}

		public void Reset()
		{
			if (isSnappingModeActive || CurrentSnappingPlane != null)
			{
				CurrentSnappingPlane.ExitObject(CurrentPlacementObject);
				DisableSnappingMode();
			}
			if (IsPlacementModeActive)
			{
				OnPlacementExit.Invoke();
			}
		}

		private void HandleInputs()
		{
			HandleObjectDistance();
			HandleObjectActions();
			HandleObjectRotation();
		}

		private void HandleObjectDistance()
		{
			float num = PlacingObjectInputs.GetMoveObjectDirectValue() * Time.deltaTime * objectPlacementSetting.objectMovingScrollDensity * ScrollSpeedMultiplier;
			_currentObjectDistanceToCamera += num;
			float min = ((_currentPlaceable != null && _currentPlaceable.MinPlacementDistance > 0f) ? _currentPlaceable.MinPlacementDistance : objectPlacementSetting.minObjectDistanceToCamera);
			float max = ((_currentPlaceable != null && _currentPlaceable.MaxPlacementDistance > 0f) ? _currentPlaceable.MaxPlacementDistance : objectPlacementSetting.maxObjectDistanceToCamera);
			_currentObjectDistanceToCamera = Mathf.Clamp(_currentObjectDistanceToCamera, min, max);
		}

		private void HandleObjectActions()
		{
			if (PlacingObjectInputs.IsFinishObjectPlacingButtonDown())
			{
				Release();
			}
		}

		private void HandleObjectRotation()
		{
			if (PlacingObjectInputs.GetRotateObjectButton())
			{
				HandleRotationMode();
				return;
			}
			_firstPersonController?.EnableCameraRotate();
			_firstPersonController?.EnableCharacterRotate();
		}

		private void HandleRotationMode()
		{
			_firstPersonController?.DisableCameraRotate();
			_firstPersonController?.DisableCharacterRotate();
			Rotate();
			if (PlacingObjectInputs.IsFinishObjectPlacingButtonDown())
			{
				_firstPersonController?.EnableCameraRotate();
				_firstPersonController?.EnableCharacterRotate();
			}
		}

		private void SetGrabbedObjectPosition()
		{
			_playerCameraTransform = _cameraTransform;
			Vector3 position = _playerCameraTransform.position + _playerCameraTransform.forward * _currentObjectDistanceToCamera;
			position -= CurrentPlacementObject.transform.TransformVector(_currentBoundsCenterLocalOffset);
			CurrentPlacementObject.transform.position = position;
		}

		private void UpdateCurrentObjectTransform()
		{
			if (IsPlacementModeActive && !isSnappingModeActive && !IsExternalSnapActive)
			{
				Vector3 b = _playerCameraTransform.position + _playerCameraTransform.forward * _currentObjectDistanceToCamera;
				b -= CurrentPlacementObject.transform.TransformVector(_currentBoundsCenterLocalOffset);
				CurrentPlacementObject.transform.position = Vector3.Lerp(CurrentPlacementObject.transform.position, b, Time.deltaTime * objectPlacementSetting.objectLerpingDensity);
				if (!PlacingObjectInputs.GetRotateObjectButton() && !_hasUserRotated)
				{
					Vector3 vector = CurrentPlacementObject.transform.TransformPoint(_currentBoundsCenterLocalOffset);
					Vector3 forward = _playerCameraTransform.position - vector;
					forward.y = 0f;
					forward.Normalize();
					Quaternion b2 = Quaternion.LookRotation(forward) * Quaternion.Euler(_currentPlaceable.DefaultPlacingRotation);
					CurrentPlacementObject.transform.rotation = Quaternion.Slerp(CurrentPlacementObject.transform.rotation, b2, Time.deltaTime * objectPlacementSetting.objectLerpingDensity);
				}
			}
		}

		public void Execute(GameObject obj)
		{
			if (!IsPlacementModeActive)
			{
				OnPlacementEnter.Invoke(obj);
			}
		}

		private void DisableAllSnappingPlaneCollidersInTransform(Transform t)
		{
			_disabledOwnSnappingColliders.Clear();
			SnappingPlane[] componentsInChildren = t.GetComponentsInChildren<SnappingPlane>(includeInactive: true);
			foreach (SnappingPlane snappingPlane in componentsInChildren)
			{
				if (snappingPlane == null)
				{
					continue;
				}
				Collider[] components = snappingPlane.GetComponents<Collider>();
				foreach (Collider collider in components)
				{
					if (!(collider == null) && collider.enabled)
					{
						collider.enabled = false;
						_disabledOwnSnappingColliders.Add(collider);
					}
				}
			}
		}

		private void EnableAllSnappingPlaneCollidersInTransform(Transform t)
		{
			foreach (Collider disabledOwnSnappingCollider in _disabledOwnSnappingColliders)
			{
				if (disabledOwnSnappingCollider != null)
				{
					disabledOwnSnappingCollider.enabled = true;
				}
			}
			_disabledOwnSnappingColliders.Clear();
		}

		private void DisablePlacedObjectsSnappingPlaneColliders(GameObject placementObject)
		{
			_disabledPlacedObjectSnappingColliders.Clear();
			SnappingPlane[] componentsInChildren = placementObject.GetComponentsInChildren<SnappingPlane>(includeInactive: true);
			HashSet<uint> visited = new HashSet<uint>();
			SnappingPlane[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (uint placedEntityNetworkID in array[i].PlacedEntityNetworkIDs)
				{
					DisablePlacedObjectSnappingPlaneCollidersRecursive(placedEntityNetworkID, visited);
				}
			}
		}

		private void DisablePlacedObjectSnappingPlaneCollidersRecursive(uint netId, HashSet<uint> visited)
		{
			if (!visited.Add(netId) || !_networkManager.TryGetNetworkObjectById(netId, out var networkObject))
			{
				return;
			}
			SnappingPlane[] componentsInChildren = networkObject.GetComponentsInChildren<SnappingPlane>(includeInactive: true);
			foreach (SnappingPlane snappingPlane in componentsInChildren)
			{
				Collider[] components = snappingPlane.GetComponents<Collider>();
				foreach (Collider collider in components)
				{
					if (collider != null && collider.enabled)
					{
						collider.enabled = false;
						_disabledPlacedObjectSnappingColliders.Add(collider);
					}
				}
				foreach (uint placedEntityNetworkID in snappingPlane.PlacedEntityNetworkIDs)
				{
					DisablePlacedObjectSnappingPlaneCollidersRecursive(placedEntityNetworkID, visited);
				}
			}
		}

		private void EnablePlacedObjectsSnappingPlaneColliders()
		{
			foreach (Collider disabledPlacedObjectSnappingCollider in _disabledPlacedObjectSnappingColliders)
			{
				if (disabledPlacedObjectSnappingCollider != null)
				{
					disabledPlacedObjectSnappingCollider.enabled = true;
				}
			}
			_disabledPlacedObjectSnappingColliders.Clear();
		}

		private void SetupSnappedObjectCollisionForwarding(HeldItem parentHeldItem)
		{
			List<GameObject> list = new List<GameObject>();
			parentHeldItem.GetAllSnappedObjectGameObjectsRecursive(list);
			CapsuleCollider capsuleCollider = _firstPersonController?.GetPlayerCapsuleCollider();
			foreach (GameObject item in list)
			{
				if (!item.TryGetComponent<Interactable>(out var component))
				{
					continue;
				}
				Collider[] rigidColliders = component.GetRigidColliders();
				if (rigidColliders != null)
				{
					Collider[] array = rigidColliders;
					foreach (Collider collider in array)
					{
						if (!(collider == null))
						{
							if (capsuleCollider != null)
							{
								Physics.IgnoreCollision(capsuleCollider, collider, ignore: true);
								_snappedPlayerIgnoredColliders.Add(collider);
							}
							if (!collider.isTrigger)
							{
								collider.isTrigger = true;
								_snappedCollidersSetToTrigger.Add(collider);
							}
						}
					}
				}
				PlacementCollisionForwarder placementCollisionForwarder = item.AddComponent<PlacementCollisionForwarder>();
				placementCollisionForwarder.Initialize(parentHeldItem, parentHeldItem.CollisionLayers);
				_snappedObjectForwarders.Add(placementCollisionForwarder);
				_snappedInteractables.Add(component);
			}
		}

		private void CleanupSnappedObjectCollisionForwarding()
		{
			foreach (PlacementCollisionForwarder snappedObjectForwarder in _snappedObjectForwarders)
			{
				if (snappedObjectForwarder != null)
				{
					UnityEngine.Object.Destroy(snappedObjectForwarder);
				}
			}
			_snappedObjectForwarders.Clear();
			foreach (Collider item in _snappedCollidersSetToTrigger)
			{
				if (item != null)
				{
					item.isTrigger = false;
				}
			}
			_snappedCollidersSetToTrigger.Clear();
			CapsuleCollider capsuleCollider = _firstPersonController?.GetPlayerCapsuleCollider();
			if (capsuleCollider != null)
			{
				foreach (Collider snappedPlayerIgnoredCollider in _snappedPlayerIgnoredColliders)
				{
					if (snappedPlayerIgnoredCollider != null)
					{
						Physics.IgnoreCollision(capsuleCollider, snappedPlayerIgnoredCollider, ignore: false);
					}
				}
			}
			_snappedPlayerIgnoredColliders.Clear();
			_snappedInteractables.Clear();
			_compoundMemberNetIds.Clear();
		}

		private void SetSnappedObjectsHighlight(ObjectHighlightType type)
		{
			foreach (Interactable snappedInteractable in _snappedInteractables)
			{
				if (!(snappedInteractable == null))
				{
					snappedInteractable.SetHighlight(type);
				}
			}
		}

		private void OnPlacementEnterActions(GameObject placingObject)
		{
			if (!placingObject.TryGetComponent<IPlaceable>(out var component))
			{
				EvilLogger.LogError("[ObjectPlacementManager] Missing IPlaceable component for placement object!", "OnPlacementEnterActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\ObjectPlacementManager.cs", 854);
				return;
			}
			uint parentNetworkID = component.PlacementNetworkData.ParentNetworkID;
			if (parentNetworkID != 0)
			{
				RemoveFromParentSnappingPlaneAsync(placingObject, component, parentNetworkID).Forget();
			}
			CurrentPlacementObject = placingObject;
			_hasUserRotated = false;
			_currentPlaceable = component;
			_currentInteractable = CurrentPlacementObject.GetComponent<Interactable>();
			_currentRenderers = CurrentPlacementObject.GetComponentsInChildren<MeshRenderer>();
			_currentBoundsCenterLocalOffset = component.BoundsCenterLocalOffset + component.PositionOffsetForPlacement;
			_currentPositionOffset = component.PositionOffsetForPlacement;
			_currentObjectDistanceToCamera = ((_currentPlaceable.DefaultPlacementDistance > 0f) ? _currentPlaceable.DefaultPlacementDistance : objectPlacementSetting.defaultObjectDistanceToCamera);
			_currentInteractable.SetInteractionAvailability(newValue: false);
			_currentInteractable.SetRigidCollidersTriggered(newValue: true);
			component.ClearColliders();
			if (placingObject.TryGetComponent<HeldItem>(out var component2))
			{
				_currentCollisionLayers = component2.CollisionLayers;
				component2.CollectSnappedObjectColliders();
				_compoundMemberNetIds.Clear();
				component2.GetSnappedDescendantNetIds(_compoundMemberNetIds);
				SetupSnappedObjectCollisionForwarding(component2);
				component2.CmdSetSnappedDescendantsTriggered(triggered: true);
			}
			DisableAllSnappingPlaneCollidersInTransform(CurrentPlacementObject.transform);
			DisablePlacedObjectsSnappingPlaneColliders(CurrentPlacementObject);
			Collider[] colliders = CurrentPlacementObject.GetColliders();
			_firstPersonController?.GetPlayerCapsuleCollider()?.SetIgnoreCollisions(colliders, ignore: true);
			Vector3 vector = ((_cameraTransform != null) ? (_cameraTransform.position - placingObject.transform.position) : Vector3.forward);
			vector.y = 0f;
			Quaternion quaternion = ((vector.sqrMagnitude > 0.0001f) ? Quaternion.LookRotation(vector.normalized) : Quaternion.identity);
			placingObject.transform.rotation = quaternion * Quaternion.Euler(component.DefaultPlacingRotation);
			SetGrabbedObjectPosition();
			EnablePlacementMode();
			component.OnPlacementModeEnter();
			_isGroundDetected = false;
			_collisionSampleTimer = 0f;
			_dropSampleTimer = 0f;
			_placementStreamTimer = 0f;
			_cachedFloatingColliding = false;
			_cachedDropResult = DropResult.Failed;
			RegisterDropCast();
			CreateDropIndicator();
		}

		private async UniTaskVoid RemoveFromParentSnappingPlaneAsync(GameObject grabbedObject, IPlaceable placeable, uint parentNetworkID)
		{
			(bool, GameObject) tuple = await _networkManager.TryGetNetworkObjectByIdAsync(parentNetworkID, 30, 100, this.GetCancellationTokenOnDestroy());
			if (!tuple.Item1 || tuple.Item2 == null)
			{
				return;
			}
			GameObject item = tuple.Item2;
			if (!(item != null) || !item.TryGetComponent<ISnappingPlaneContainer>(out var component))
			{
				return;
			}
			ushort snappingPlaneIndex = placeable.PlacementNetworkData.SnappingPlaneIndex;
			if (component.SnappingPlanes == null)
			{
				EvilLogger.LogError("[ObjectPlacementManager] SnappingPlanes array is null on " + item.name, "RemoveFromParentSnappingPlaneAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\ObjectPlacementManager.cs", 964);
				return;
			}
			if (snappingPlaneIndex >= component.SnappingPlanes.Length)
			{
				EvilLogger.LogError($"[ObjectPlacementManager] Invalid plane index: {snappingPlaneIndex} (max: {component.SnappingPlanes.Length - 1}) on {item.name}", "RemoveFromParentSnappingPlaneAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\ObjectPlacementManager.cs", 970);
				return;
			}
			SnappingPlane snappingPlane = component.SnappingPlanes[snappingPlaneIndex];
			if (snappingPlane != null)
			{
				snappingPlane.RemoveObject(grabbedObject);
			}
		}

		public void Release()
		{
			if (IsPlacementModeActive && _currentPlaceable != null)
			{
				RevalidatePlacementNow();
				if (_currentPlaceable.CanPlaceable)
				{
					OnPlacementExit.Invoke();
				}
			}
		}

		private void RevalidatePlacementNow()
		{
			if (IsPlacementModeActive && _currentPlaceable != null)
			{
				_collisionSampleTimer = 0f;
				if (!IsExternalSnapActive && !isSnappingModeActive && _currentPlaceable is HeldItem item)
				{
					_castingManager?.Process(_dropCastHandle);
					_cachedDropResult = DropCastHelper.ProcessCastResult(_lastDropCastResult, item, _dropSettings, BuildDropSelfExclusions(), _dropBlockingMask, CurrentPlacementObject.transform.rotation, _currentBoundsCenterLocalOffset, _currentRenderers, _compoundMemberNetIds);
					_isGroundDetected = _cachedDropResult.Success;
					_isTargetPositionColliding = CheckTargetPositionCollision(_cachedDropResult);
				}
				CheckCollisions();
			}
		}

		private void OnObjectReleaseActions()
		{
			if (_currentPlaceable == null)
			{
				EvilLogger.LogError("[ObjectPlacementManager] Missing IPlaceable reference for releasing object!", "OnObjectReleaseActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\ObjectPlacementManager.cs", 1032);
				return;
			}
			if (OnTrySlotAttach != null && OnTrySlotAttach())
			{
				HandleSlotAttachCleanup();
				return;
			}
			_currentInteractable.SetInteractionAvailability(newValue: true);
			_currentInteractable.SetHighlight(ObjectHighlightType.None);
			SetSnappedObjectsHighlight(ObjectHighlightType.None);
			_isTargetPositionColliding = false;
			EnableAllSnappingPlaneCollidersInTransform(CurrentPlacementObject.transform);
			EnablePlacedObjectsSnappingPlaneColliders();
			Collider[] colliders = CurrentPlacementObject.GetColliders();
			_firstPersonController?.GetPlayerCapsuleCollider()?.SetIgnoreCollisions(colliders, ignore: false);
			_currentPlaceable.Release();
			if (CurrentSnappingPlane != null)
			{
				if (CurrentSnappingPlane.MakeObjectsStatic)
				{
					_currentInteractable.SetRigidCollidersTriggered(newValue: false);
					CurrentSnappingPlane.PlaceObject(CurrentPlacementObject);
				}
				else
				{
					_currentInteractable.SetRigidCollidersTriggered(newValue: false);
					CurrentSnappingPlane.FallObject(CurrentPlacementObject);
				}
				Vector3 normal = (CurrentSnappingPlane.AlignToSurfaceNormal ? _lastSnapHitNormal : CurrentSnappingPlane.transform.up);
				PlayDropSurfaceEffectAtPosition(CurrentPlacementObject.transform.position, normal, CurrentSnappingPlane.GetComponent<Collider>());
				PlayDropSurfaceSoundAtPosition(CurrentPlacementObject.transform.position, CurrentSnappingPlane.GetComponent<Collider>(), CurrentPlacementObject.GetComponent<HeldItem>());
				CurrentSnappingPlane.ExitObject(CurrentPlacementObject);
				DisableSnappingMode();
			}
			else if (!TryDropInstant())
			{
				_currentInteractable.SetRigidCollidersTriggered(newValue: false);
			}
			_currentPlaceable.OnPlacementModeExit();
			CleanupSnappedObjectCollisionForwarding();
			if (CurrentPlacementObject.TryGetComponent<HeldItem>(out var component))
			{
				component.ClearSnappedObjectColliders();
				component.CmdSetSnappedDescendantsTriggered(triggered: false);
			}
			CurrentPlacementObject = null;
			_currentPlaceable = null;
			_currentInteractable = null;
			_currentRenderers = null;
			DisablePlacementMode();
		}

		private void HandleSlotAttachCleanup()
		{
			_currentInteractable.SetInteractionAvailability(newValue: true);
			_currentInteractable.SetHighlight(ObjectHighlightType.None);
			SetSnappedObjectsHighlight(ObjectHighlightType.None);
			_isTargetPositionColliding = false;
			EnableAllSnappingPlaneCollidersInTransform(CurrentPlacementObject.transform);
			EnablePlacedObjectsSnappingPlaneColliders();
			Collider[] colliders = CurrentPlacementObject.GetColliders();
			_firstPersonController?.GetPlayerCapsuleCollider()?.SetIgnoreCollisions(colliders, ignore: false);
			_currentPlaceable.Release();
			_currentPlaceable.OnPlacementModeExit();
			CleanupSnappedObjectCollisionForwarding();
			if (CurrentPlacementObject.TryGetComponent<HeldItem>(out var component))
			{
				component.ClearSnappedObjectColliders();
				component.CmdSetSnappedDescendantsTriggered(triggered: false);
			}
			CurrentPlacementObject = null;
			_currentPlaceable = null;
			_currentInteractable = null;
			_currentRenderers = null;
			DisablePlacementMode();
		}

		private void Rotate()
		{
			if (IsPlacementModeActive && _currentPlaceable != null)
			{
				float moveObjectHorizontalValue = PlacingObjectInputs.GetMoveObjectHorizontalValue();
				if (Mathf.Abs(moveObjectHorizontalValue) > 0.01f)
				{
					_hasUserRotated = true;
				}
				float angle = moveObjectHorizontalValue * Time.deltaTime * objectPlacementSetting.objectRotationDensity;
				Vector3 point = CurrentPlacementObject.transform.TransformPoint(_currentBoundsCenterLocalOffset);
				Vector3 axis = _currentPlaceable.PlacementRotationAxis switch
				{
					RotationAxis.X => CurrentPlacementObject.transform.right, 
					RotationAxis.Y => CurrentPlacementObject.transform.up, 
					RotationAxis.Z => CurrentPlacementObject.transform.forward, 
					_ => CurrentPlacementObject.transform.up, 
				};
				if (isSnappingModeActive && CurrentSnappingPlane != null && CurrentSnappingPlane.SnapsToCenter)
				{
					CurrentPlacementObject.transform.RotateAround(CurrentSnappingPlane.transform.position, axis, angle);
				}
				else
				{
					CurrentPlacementObject.transform.RotateAround(point, axis, angle);
				}
			}
		}

		private void EnablePlacementMode()
		{
			ActionMapsManager.EnterPlacingObjectMode();
			_inputActionPromptsPanel.ActivateWithPrompts(new string[3] { "PlacementMode_Move", "PlacementMode_Release", "PlacementMode_Rotate" });
			_firstPersonController?.DisableZoom();
		}

		private void DisablePlacementMode()
		{
			UnregisterDropCast();
			DestroyDropIndicator();
			ActionMapsManager.ExitPlacingObjectMode();
			_inputActionPromptsPanel.Hide();
			_firstPersonController?.EnableZoom();
		}

		private void RegisterDropCast()
		{
			if (_dropCastHandle == null && _castingManager != null && !(CurrentPlacementObject == null))
			{
				CastRequest request = CastRequest.Ray(CurrentPlacementObject.transform, _dropSettings.MaxDistance, (int)_dropSettings.GroundLayers | _dropBlockingMask, QueryTriggerInteraction.Collide);
				request.Offset = _currentBoundsCenterLocalOffset;
				request.UseTransformForward = false;
				request.Direction = Vector3.down;
				request.UpdateMode = UpdateMode.FixedUpdate;
				request.MaxHits = 32;
				_dropCastHandle = _castingManager.Register(request, OnDropCastResult);
			}
		}

		private void OnDropCastResult(CastResult result)
		{
			_lastDropCastResult = result;
		}

		private void UnregisterDropCast()
		{
			if (_dropCastHandle != null && _castingManager != null)
			{
				_castingManager.Unregister(_dropCastHandle);
				_dropCastHandle = null;
				_lastDropCastResult = default(CastResult);
			}
		}

		private bool CheckTargetPositionCollision(DropResult dropResult)
		{
			if (!dropResult.Success)
			{
				return false;
			}
			if (dropResult.SnappingPlane != null)
			{
				return false;
			}
			Transform transform = CurrentPlacementObject.transform;
			Quaternion deltaRot = dropResult.TargetRotation * Quaternion.Inverse(transform.rotation);
			Vector3 position = transform.position;
			Vector3 targetPosition = dropResult.TargetPosition;
			Collider collider = dropResult.HitInfo.collider;
			Collider playerCapsuleCollider = _playerCapsuleCollider;
			Collider[] rigidColliders = _currentInteractable.GetRigidColliders();
			if (HasOverlapAtTarget(rigidColliders, deltaRot, position, targetPosition, collider, playerCapsuleCollider, _currentCollisionLayers))
			{
				return true;
			}
			foreach (Interactable snappedInteractable in _snappedInteractables)
			{
				if (!(snappedInteractable == null))
				{
					Collider[] rigidColliders2 = snappedInteractable.GetRigidColliders();
					if (HasOverlapAtTarget(rigidColliders2, deltaRot, position, targetPosition, collider, playerCapsuleCollider, _currentCollisionLayers))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool HasOverlapAtTarget(Collider[] colliders, Quaternion deltaRot, Vector3 rootPos, Vector3 targetPos, Collider groundCollider, Collider playerCapsule, int mask, HashSet<Collider> extraExclusions = null)
		{
			if (colliders == null || colliders.Length == 0)
			{
				return false;
			}
			foreach (Collider collider in colliders)
			{
				if (collider == null || !collider.enabled)
				{
					continue;
				}
				int num = OverlapColliderAtTarget(collider, deltaRot, rootPos, targetPos, mask);
				for (int j = 0; j < num; j++)
				{
					Collider collider2 = TargetOverlapBuffer[j];
					if (collider2 == null || collider2 == groundCollider || collider2 == playerCapsule || (extraExclusions != null && extraExclusions.Contains(collider2)))
					{
						continue;
					}
					if (_compoundMemberNetIds.Count > 0)
					{
						NetworkIdentity componentInParent = collider2.GetComponentInParent<NetworkIdentity>();
						if (componentInParent != null && _compoundMemberNetIds.Contains(componentInParent.netId))
						{
							continue;
						}
					}
					return true;
				}
			}
			return false;
		}

		private bool HasMatrixHiddenOverlapAtLivePose()
		{
			if (CurrentPlacementObject == null || _currentInteractable == null)
			{
				return false;
			}
			Vector3 position = CurrentPlacementObject.transform.position;
			Quaternion identity = Quaternion.identity;
			Collider playerCapsuleCollider = _playerCapsuleCollider;
			_liveOverlapExclusions.Clear();
			AddRigidColliders(_liveOverlapExclusions, _currentInteractable);
			foreach (Interactable snappedInteractable in _snappedInteractables)
			{
				AddRigidColliders(_liveOverlapExclusions, snappedInteractable);
			}
			if (HasOverlapAtTarget(_currentInteractable.GetRigidColliders(), identity, position, position, null, playerCapsuleCollider, _placementObstacleMask, _liveOverlapExclusions))
			{
				return true;
			}
			foreach (Interactable snappedInteractable2 in _snappedInteractables)
			{
				if (!(snappedInteractable2 == null) && HasOverlapAtTarget(snappedInteractable2.GetRigidColliders(), identity, position, position, null, playerCapsuleCollider, _placementObstacleMask, _liveOverlapExclusions))
				{
					return true;
				}
			}
			return false;
		}

		private static void AddRigidColliders(HashSet<Collider> set, Interactable interactable)
		{
			if (interactable == null)
			{
				return;
			}
			Collider[] rigidColliders = interactable.GetRigidColliders();
			if (rigidColliders == null)
			{
				return;
			}
			Collider[] array = rigidColliders;
			foreach (Collider collider in array)
			{
				if (collider != null)
				{
					set.Add(collider);
				}
			}
		}

		private static void AddColliders(HashSet<Collider> set, Collider[] cols)
		{
			if (cols == null)
			{
				return;
			}
			foreach (Collider collider in cols)
			{
				if (collider != null)
				{
					set.Add(collider);
				}
			}
		}

		private HashSet<Collider> BuildDropSelfExclusions()
		{
			_dropSelfExclusions.Clear();
			if (_currentInteractable != null)
			{
				AddColliders(_dropSelfExclusions, _currentInteractable.GetRigidColliders());
				AddColliders(_dropSelfExclusions, _currentInteractable.GetTriggerColliders());
			}
			foreach (Interactable snappedInteractable in _snappedInteractables)
			{
				if (!(snappedInteractable == null))
				{
					AddColliders(_dropSelfExclusions, snappedInteractable.GetRigidColliders());
					AddColliders(_dropSelfExclusions, snappedInteractable.GetTriggerColliders());
				}
			}
			return _dropSelfExclusions;
		}

		private int OverlapColliderAtTarget(Collider col, Quaternion deltaRot, Vector3 rootPos, Vector3 targetPos, int mask)
		{
			return TargetOverlapHelper.OverlapColliderAtTarget(col, deltaRot, rootPos, targetPos, mask, targetCollisionShrinkFactor, TargetOverlapBuffer);
		}

		private bool TryDropInstant()
		{
			if (!(_currentPlaceable is HeldItem heldItem))
			{
				return false;
			}
			DropResult dropResult = DropCastHelper.ProcessCastResult(_lastDropCastResult, heldItem, _dropSettings, BuildDropSelfExclusions(), _dropBlockingMask, CurrentPlacementObject.transform.rotation, _currentBoundsCenterLocalOffset, _currentRenderers, _compoundMemberNetIds);
			if (!dropResult.Success)
			{
				return false;
			}
			if (dropResult.SnappingPlane != null)
			{
				return dropResult.SnappingPlane.SnapObjectExternal(heldItem, dropResult.HitInfo.point, dropResult.HitInfo.normal);
			}
			CurrentPlacementObject.transform.SetPositionAndRotation(dropResult.TargetPosition, dropResult.TargetRotation);
			_currentInteractable.SetRigidCollidersTriggered(newValue: false);
			heldItem.SyncGroundDrop(dropResult.TargetPosition, dropResult.TargetRotation);
			PlayDropSurfaceEffect(dropResult);
			PlayDropSurfaceSound(dropResult, heldItem);
			return true;
		}

		private void PlayDropSurfaceEffect(DropResult dropResult)
		{
			if (dropSurfaceEffectConfig == null || _networkedParticlesManager == null)
			{
				return;
			}
			Collider collider = dropResult.HitInfo.collider;
			if (!(collider == null))
			{
				SurfaceTypeComponent surfaceTypeComponent = collider.GetComponent<SurfaceTypeComponent>() ?? collider.GetComponentInParent<SurfaceTypeComponent>();
				ParticleKey particleKey = ((surfaceTypeComponent != null) ? dropSurfaceEffectConfig.GetParticleKey(surfaceTypeComponent.surfaceType) : dropSurfaceEffectConfig.DefaultParticleKey);
				if (particleKey.IsValid)
				{
					_networkedParticlesManager.PlayNetworkedOneShot(particleKey, dropResult.HitInfo.point, Quaternion.LookRotation(dropResult.HitInfo.normal));
				}
			}
		}

		private void PlayDropSurfaceEffectAtPosition(Vector3 position, Vector3 normal, Collider surfaceCollider)
		{
			if (!(dropSurfaceEffectConfig == null) && _networkedParticlesManager != null && !(surfaceCollider == null))
			{
				SurfaceTypeComponent surfaceTypeComponent = surfaceCollider.GetComponent<SurfaceTypeComponent>() ?? surfaceCollider.GetComponentInParent<SurfaceTypeComponent>();
				ParticleKey particleKey = ((surfaceTypeComponent != null) ? dropSurfaceEffectConfig.GetParticleKey(surfaceTypeComponent.surfaceType) : dropSurfaceEffectConfig.DefaultParticleKey);
				if (particleKey.IsValid)
				{
					_networkedParticlesManager.PlayNetworkedOneShot(particleKey, position, Quaternion.LookRotation(normal));
				}
			}
		}

		private void PlayDropSurfaceSound(DropResult dropResult, HeldItem heldItem)
		{
			if (dropSurfaceSoundConfig == null || _networkAudioRelay == null)
			{
				return;
			}
			Collider collider = dropResult.HitInfo.collider;
			if (!(collider == null))
			{
				SurfaceTypeComponent surfaceTypeComponent = collider.GetComponent<SurfaceTypeComponent>() ?? collider.GetComponentInParent<SurfaceTypeComponent>();
				ItemMaterial material = ((heldItem != null) ? heldItem.DropMaterial : ItemMaterial.Generic);
				SoundID id = ((surfaceTypeComponent != null) ? dropSurfaceSoundConfig.GetEvent(surfaceTypeComponent.surfaceType, material) : dropSurfaceSoundConfig.DefaultEvent);
				if (id.IsValid())
				{
					_networkAudioRelay.PlayOneShotExcludeSelf(id, dropResult.HitInfo.point);
				}
			}
		}

		private void PlayDropSurfaceSoundAtPosition(Vector3 position, Collider surfaceCollider, HeldItem heldItem)
		{
			if (!(dropSurfaceSoundConfig == null) && _networkAudioRelay != null && !(surfaceCollider == null))
			{
				SurfaceTypeComponent surfaceTypeComponent = surfaceCollider.GetComponent<SurfaceTypeComponent>() ?? surfaceCollider.GetComponentInParent<SurfaceTypeComponent>();
				ItemMaterial material = ((heldItem != null) ? heldItem.DropMaterial : ItemMaterial.Generic);
				SoundID id = ((surfaceTypeComponent != null) ? dropSurfaceSoundConfig.GetEvent(surfaceTypeComponent.surfaceType, material) : dropSurfaceSoundConfig.DefaultEvent);
				if (id.IsValid())
				{
					_networkAudioRelay.PlayOneShotExcludeSelf(id, position);
				}
			}
		}

		private void CreateDropIndicator()
		{
			if (!(_dropIndicatorSettings == null) && !(_dropIndicator != null) && !(_currentInteractable == null))
			{
				GameObject gameObject = new GameObject("DropIndicator");
				_dropIndicator = gameObject.AddComponent<DropIndicatorController>();
				_dropIndicator.Initialize(_dropIndicatorSettings, _currentInteractable, _currentBoundsCenterLocalOffset, _currentPlaceable.GhostPreviewMode, _currentPlaceable.GhostPreviewScale);
				_dropIndicator.Show();
			}
		}

		private void UpdateDropIndicator()
		{
			if (_dropIndicator == null || !(_currentPlaceable is HeldItem heldItem))
			{
				return;
			}
			if (_currentPlaceable.IsCollidingAny())
			{
				_isTargetPositionColliding = false;
				_dropIndicator.Hide();
				return;
			}
			if (isSnappingModeActive)
			{
				_isTargetPositionColliding = false;
				_dropIndicator.Hide();
				if (_dropIndicatorSnappingPlane != null && _dropIndicatorSnappingPlane != CurrentSnappingPlane)
				{
					_dropIndicatorSnappingPlane.DeactivatePlacementShader();
				}
				_dropIndicatorSnappingPlane = null;
				return;
			}
			_dropSampleTimer -= Time.deltaTime;
			if (_dropSampleTimer <= 0f)
			{
				_dropSampleTimer = placementSampleInterval;
				_cachedDropResult = DropCastHelper.ProcessCastResult(_lastDropCastResult, heldItem, _dropSettings, BuildDropSelfExclusions(), _dropBlockingMask, CurrentPlacementObject.transform.rotation, _currentBoundsCenterLocalOffset, _currentRenderers, _compoundMemberNetIds);
				_isGroundDetected = _cachedDropResult.Success;
				_isTargetPositionColliding = CheckTargetPositionCollision(_cachedDropResult);
			}
			_dropIndicator.UpdateIndicator(_cachedDropResult);
			UpdateDropIndicatorShader(_cachedDropResult, heldItem);
		}

		private void UpdateDropIndicatorShader(DropResult dropResult, HeldItem heldItem)
		{
			SnappingPlane snappingPlane = (dropResult.Success ? dropResult.SnappingPlane : null);
			if (_dropIndicatorSnappingPlane != null && _dropIndicatorSnappingPlane != snappingPlane)
			{
				_dropIndicatorSnappingPlane.DeactivatePlacementShader();
			}
			_dropIndicatorSnappingPlane = snappingPlane;
			if (_dropIndicatorSnappingPlane != null && dropResult.Success)
			{
				float placementRadiusMultiplier = heldItem.PlacementRadiusMultiplier;
				_dropIndicatorSnappingPlane.ActivatePlacementShader(dropResult.TargetPosition, _currentRenderers, placementRadiusMultiplier);
			}
		}

		private void DeactivateDropIndicatorShader()
		{
			if (_dropIndicatorSnappingPlane != null)
			{
				_dropIndicatorSnappingPlane.DeactivatePlacementShader();
				_dropIndicatorSnappingPlane = null;
			}
		}

		private void DestroyDropIndicator()
		{
			DeactivateDropIndicatorShader();
			if (_dropIndicator != null)
			{
				UnityEngine.Object.Destroy(_dropIndicator.gameObject);
				_dropIndicator = null;
			}
		}

		public void Activate()
		{
			IsActive = true;
		}

		public void Deactivate()
		{
			IsActive = false;
			base.enabled = false;
		}
	}
}
