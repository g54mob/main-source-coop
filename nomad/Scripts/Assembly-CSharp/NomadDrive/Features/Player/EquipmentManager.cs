using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking.Parenting;
using EvilCore.Particles;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Tools;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class EquipmentManager : NetworkBehaviour, IEquipmentManager, IPlayerComponent
	{
		private HeldItem _equippedEntity;

		[Header("Drop Settings")]
		[SerializeField]
		private DropSettings _dropSettings = DropSettings.Default;

		[SerializeField]
		private DropSurfaceEffectConfig dropSurfaceEffectConfig;

		[SerializeField]
		private DropSurfaceSoundConfig dropSurfaceSoundConfig;

		[Header("Audio")]
		[SerializeField]
		private SoundID equipSound;

		[Header("Collision Clipping - Forward Raycast")]
		[SerializeField]
		private bool enableCollisionClipping = true;

		[SerializeField]
		private LayerMask clippingDetectionLayers = 2177;

		[SerializeField]
		private float defaultForwardRayLength = 0.6f;

		[SerializeField]
		private float forwardRaySphereRadius = 0.08f;

		[SerializeField]
		private float forwardRayRetractSmoothTime = 0.08f;

		[SerializeField]
		private float forwardRayReturnSmoothTime = 0.15f;

		[Header("Collision Clipping - Proximity Sphere")]
		[SerializeField]
		private float clippingCheckRadius = 0.12f;

		[SerializeField]
		private float maxRetractionDistance = 0.25f;

		[SerializeField]
		private float clippingRetractSmoothTime = 0.05f;

		[SerializeField]
		private float clippingReturnSmoothTime = 0.15f;

		[Header("Collision Clipping - Performance")]
		[Tooltip("Seconds between the expensive clipping physics samples (SphereCast + OverlapSphere/ComputePenetration). The smoothed offset is still applied every frame, so motion stays smooth.")]
		[SerializeField]
		private float clippingSampleInterval = 0.06f;

		private float _forwardRetraction;

		private float _forwardRetractionVelocity;

		private Vector3 _clippingOffset;

		private Vector3 _clippingOffsetVelocity;

		private Collider[] _playerIgnoreColliders;

		private SphereCollider _clippingProbe;

		private static readonly Collider[] ClippingOverlapCache;

		private float _clippingSampleTimer;

		private float _forwardRetractionTarget;

		private Vector3 _clippingPushTarget;

		private bool _clipDisabled;

		private float _clipRayLength;

		private float _clipMaxRetraction;

		private Vector3 _clipProbeOffset;

		private IPlayerService _playerService;

		private FirstPersonController _firstPersonController;

		private INetworkedAudioManager _networkAudioRelay;

		private UIFeedbackManager _uiFeedbackManager;

		private ICastingManager _castingManager;

		private INetworkedParticlesManager _networkedParticlesManager;

		private IRaycastHandle _dropCastHandle;

		private CastResult _lastDropCastResult;

		private int _dropBlockingMask;

		private readonly HashSet<Collider> _dropSelfExclusions = new HashSet<Collider>();

		private const float DropTargetShrink = 0.85f;

		private static readonly Collider[] DropOverlapBuffer;

		[SerializeField]
		private SlotHighlightSettings slotHighlightSettings;

		private bool _isObjectSlotDetectionModeEnabled;

		private readonly HashSet<ObjectSlotType> _searchingSlotTypes = new HashSet<ObjectSlotType>();

		private readonly HashSet<IObjectSlot> _detectedObjectSlots = new HashSet<IObjectSlot>();

		public float objectSlotDetectionRadius = 15f;

		[SerializeField]
		private LayerMask objectSlotDetectionLayers = 524544;

		[SerializeField]
		private float slotDetectionSampleInterval = 0.1f;

		private float _slotDetectionSampleTimer;

		private AttachableObject _slotDetectionAttachable;

		private Collider[] _slotDetectionOverlapBuffer = new Collider[256];

		private readonly HashSet<IObjectSlot> _currentlyDetectedSlotsBuffer = new HashSet<IObjectSlot>();

		private readonly List<IObjectSlot> _slotsToRemoveBuffer = new List<IObjectSlot>();

		public int SetupPriority => 5;

		[Header("Fields")]
		[field: SerializeField]
		public float ItemEquippingSmoothSpeed { get; set; } = 20f;

		[field: SerializeField]
		public NetworkedTransform EquipmentNetworkedTransform { get; set; }

		public HeldItem EquippedEntity
		{
			get
			{
				return _equippedEntity;
			}
			set
			{
				_equippedEntity = value;
			}
		}

		public bool IsItemEquipped => EquippedEntity != null;

		[Header("Events")]
		[field: SerializeField]
		public UnityEvent OnItemEquipped { get; set; } = new UnityEvent();

		[field: SerializeField]
		public UnityEvent OnItemUnequipped { get; set; } = new UnityEvent();

		[field: SerializeField]
		public UnityEvent OnItemDropped { get; set; } = new UnityEvent();

		[field: SerializeField]
		public UnityEvent OnGripPauseRequested { get; set; } = new UnityEvent();

		[field: SerializeField]
		public UnityEvent OnGripResumeRequested { get; set; } = new UnityEvent();

		public InputActionPromptsPanel InputActionPromptsPanel { get; set; }

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				base.enabled = true;
				_dropBlockingMask = LayerMask.GetMask("Default", "Interactable", "VehicleBody", "SnappedInteractableOnVehicle");
			}
			else
			{
				base.enabled = false;
			}
		}

		[Inject]
		private void Construct(InputActionPromptsPanel inputActionPromptsPanel, INetworkedAudioManager networkAudioRelay, IPlayerService playerService, UIFeedbackManager uiFeedbackManager, ICastingManager castingManager, INetworkedParticlesManager networkedParticlesManager)
		{
			InputActionPromptsPanel = inputActionPromptsPanel;
			_networkAudioRelay = networkAudioRelay;
			_playerService = playerService;
			_uiFeedbackManager = uiFeedbackManager;
			_castingManager = castingManager;
			_networkedParticlesManager = networkedParticlesManager;
		}

		private void OnEnable()
		{
			OnItemEquipped.AddListener(OnItemEquip);
			OnItemUnequipped.AddListener(OnItemUnequip);
		}

		private void OnDisable()
		{
			OnItemEquipped.RemoveListener(OnItemEquip);
			OnItemUnequipped.RemoveListener(OnItemUnequip);
		}

		private void Update()
		{
			if (!base.isLocalPlayer)
			{
				return;
			}
			UpdateEquipmentCrouchOffset();
			bool sample = false;
			if (enableCollisionClipping && _equippedEntity != null && !_clipDisabled)
			{
				_clippingSampleTimer -= Time.deltaTime;
				if (_clippingSampleTimer <= 0f)
				{
					_clippingSampleTimer = clippingSampleInterval;
					sample = true;
				}
			}
			UpdateForwardRayClipping(sample);
			UpdateCollisionClipping(sample);
			if (EquippingInputs.IsEnterPlacementModeButtonDown())
			{
				if (_equippedEntity == null)
				{
					return;
				}
				_equippedEntity.StartPlacing();
			}
			if (_equippedEntity is DirectHeldItem directHeldItem)
			{
				HeldItemUseInputType useInputType = directHeldItem.useInputType;
				if (EquippingInputs.IsUseObjectButton(useInputType) && directHeldItem != null)
				{
					directHeldItem.OnUseButton();
				}
				if (EquippingInputs.IsUseObjectButtonDown(useInputType) && directHeldItem != null)
				{
					directHeldItem.OnUseButtonDown();
				}
				if (EquippingInputs.IsUseObjectButtonUp(useInputType) && directHeldItem != null)
				{
					directHeldItem.OnUseButtonUp();
				}
			}
		}

		private void UpdateEquipmentCrouchOffset()
		{
			if (!(_equippedEntity == null) && (!(_firstPersonController == null) || _playerService.TryGetFirstPersonController(out _firstPersonController)))
			{
				float standingEyeHeight = _firstPersonController.FirstPersonControllerSettings.standingEyeHeight;
				float num = _firstPersonController.CurrentEyeHeight - standingEyeHeight;
				Vector3 positionOnHand = _equippedEntity.PositionOnHand;
				positionOnHand.y += num;
				EquipmentNetworkedTransform.transform.localPosition = positionOnHand;
			}
		}

		private void UpdateForwardRayClipping(bool sample)
		{
			if (!enableCollisionClipping || _equippedEntity == null || _clipDisabled)
			{
				_forwardRetractionTarget = 0f;
				DecayForwardRetraction();
				return;
			}
			if (!_playerService.TryGetCameraTransform(out var cameraTransform))
			{
				_forwardRetractionTarget = 0f;
				DecayForwardRetraction();
				return;
			}
			if (sample)
			{
				_forwardRetractionTarget = SampleForwardRetraction(cameraTransform);
			}
			float smoothTime = ((_forwardRetractionTarget >= _forwardRetraction) ? forwardRayRetractSmoothTime : forwardRayReturnSmoothTime);
			_forwardRetraction = Mathf.SmoothDamp(_forwardRetraction, _forwardRetractionTarget, ref _forwardRetractionVelocity, smoothTime);
			ApplyForwardRetraction(cameraTransform);
		}

		private float SampleForwardRetraction(Transform cameraTransform)
		{
			if (Physics.SphereCast(EquipmentNetworkedTransform.transform.position, direction: cameraTransform.forward, radius: forwardRaySphereRadius, hitInfo: out var hitInfo, maxDistance: _clipRayLength, layerMask: clippingDetectionLayers, queryTriggerInteraction: QueryTriggerInteraction.Ignore) && !IsPlayerCollider(hitInfo.collider))
			{
				return (1f - hitInfo.distance / _clipRayLength) * _clipMaxRetraction;
			}
			return 0f;
		}

		private void DecayForwardRetraction()
		{
			if (_forwardRetraction < 0.0001f)
			{
				_forwardRetraction = 0f;
				_forwardRetractionVelocity = 0f;
				return;
			}
			_forwardRetraction = Mathf.SmoothDamp(_forwardRetraction, 0f, ref _forwardRetractionVelocity, forwardRayReturnSmoothTime);
			if (_forwardRetraction < 0.0001f)
			{
				_forwardRetraction = 0f;
				_forwardRetractionVelocity = 0f;
			}
		}

		private void ApplyForwardRetraction(Transform cameraTransform)
		{
			if (!(_forwardRetraction < 0.0001f))
			{
				Transform parent = EquipmentNetworkedTransform.transform.parent;
				Vector3 vector = -cameraTransform.forward * _forwardRetraction;
				Vector3 vector2 = ((parent != null) ? parent.InverseTransformDirection(vector) : vector);
				EquipmentNetworkedTransform.transform.localPosition += vector2;
			}
		}

		private void UpdateCollisionClipping(bool sample)
		{
			if (!enableCollisionClipping || _equippedEntity == null || _clipDisabled)
			{
				_clippingPushTarget = Vector3.zero;
				DecayClippingOffset();
				return;
			}
			if (sample)
			{
				_clippingPushTarget = SampleClippingPush();
			}
			if (_clippingPushTarget.sqrMagnitude > 0.0001f)
			{
				_clippingOffset = Vector3.SmoothDamp(_clippingOffset, _clippingPushTarget, ref _clippingOffsetVelocity, clippingRetractSmoothTime);
			}
			else
			{
				if (_clippingOffset == Vector3.zero)
				{
					return;
				}
				_clippingOffset = Vector3.SmoothDamp(_clippingOffset, Vector3.zero, ref _clippingOffsetVelocity, clippingReturnSmoothTime);
				if (_clippingOffset.magnitude < 0.0001f)
				{
					_clippingOffset = Vector3.zero;
					_clippingOffsetVelocity = Vector3.zero;
				}
			}
			ApplyClippingOffset();
		}

		private Vector3 SampleClippingPush()
		{
			EnsurePlayerIgnoreCollidersCached();
			EnsureClippingProbe();
			_clippingProbe.transform.position = EquipmentNetworkedTransform.transform.TransformPoint(_clipProbeOffset);
			Vector3 position = _clippingProbe.transform.position;
			Quaternion rotation = _clippingProbe.transform.rotation;
			int num = Physics.OverlapSphereNonAlloc(position, _clippingProbe.radius, ClippingOverlapCache, clippingDetectionLayers, QueryTriggerInteraction.Ignore);
			Vector3 result = Vector3.zero;
			for (int i = 0; i < num; i++)
			{
				Collider collider = ClippingOverlapCache[i];
				if (!(collider == null) && !(collider == _clippingProbe) && !IsPlayerCollider(collider) && Physics.ComputePenetration(_clippingProbe, position, rotation, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance))
				{
					result += direction * distance;
				}
			}
			if (result.magnitude > _clipMaxRetraction)
			{
				result = result.normalized * _clipMaxRetraction;
			}
			return result;
		}

		private void DecayClippingOffset()
		{
			if (!(_clippingOffset == Vector3.zero))
			{
				_clippingOffset = Vector3.SmoothDamp(_clippingOffset, Vector3.zero, ref _clippingOffsetVelocity, clippingReturnSmoothTime);
				if (_clippingOffset.magnitude < 0.0001f)
				{
					_clippingOffset = Vector3.zero;
					_clippingOffsetVelocity = Vector3.zero;
				}
				else
				{
					ApplyClippingOffset();
				}
			}
		}

		private void ApplyClippingOffset()
		{
			if (!(_clippingOffset == Vector3.zero))
			{
				Transform parent = EquipmentNetworkedTransform.transform.parent;
				Vector3 vector = ((parent != null) ? parent.InverseTransformDirection(_clippingOffset) : _clippingOffset);
				EquipmentNetworkedTransform.transform.localPosition += vector;
			}
		}

		private bool IsPlayerCollider(Collider col)
		{
			if (_playerIgnoreColliders == null)
			{
				return false;
			}
			for (int i = 0; i < _playerIgnoreColliders.Length; i++)
			{
				if (_playerIgnoreColliders[i] == col)
				{
					return true;
				}
			}
			return false;
		}

		private void EnsureClippingProbe()
		{
			if (_clippingProbe != null)
			{
				return;
			}
			Vector3 localPosition = Vector3.zero;
			float clippingProbeRadius = clippingCheckRadius;
			if (_equippedEntity != null && _equippedEntity.heldItemConfig != null)
			{
				HeldItemConfig heldItemConfig = _equippedEntity.heldItemConfig;
				if (heldItemConfig.clippingProbeOffset != Vector3.zero)
				{
					localPosition = heldItemConfig.clippingProbeOffset;
				}
				if (heldItemConfig.clippingProbeRadius > 0f)
				{
					clippingProbeRadius = heldItemConfig.clippingProbeRadius;
				}
			}
			GameObject gameObject = new GameObject("_ClippingProbe");
			gameObject.transform.SetParent(EquipmentNetworkedTransform.transform, worldPositionStays: false);
			gameObject.transform.localPosition = localPosition;
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			_clippingProbe = gameObject.AddComponent<SphereCollider>();
			_clippingProbe.radius = clippingProbeRadius;
			_clippingProbe.isTrigger = true;
			_playerIgnoreColliders = null;
		}

		private void DestroyClippingProbe()
		{
			if (!(_clippingProbe == null))
			{
				Object.Destroy(_clippingProbe.gameObject);
				_clippingProbe = null;
			}
		}

		private void EnsurePlayerIgnoreCollidersCached()
		{
			if (_playerIgnoreColliders == null)
			{
				_playerIgnoreColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
		}

		private void ResolveClippingConfig()
		{
			HeldItemConfig heldItemConfig = ((_equippedEntity != null) ? _equippedEntity.heldItemConfig : null);
			_clipDisabled = heldItemConfig != null && heldItemConfig.disableCollisionClipping;
			_clipRayLength = ((heldItemConfig != null && heldItemConfig.clippingRayLength > 0f) ? heldItemConfig.clippingRayLength : defaultForwardRayLength);
			_clipMaxRetraction = ((heldItemConfig != null && heldItemConfig.collisionClippingMaxRetraction > 0f) ? heldItemConfig.collisionClippingMaxRetraction : maxRetractionDistance);
			_clipProbeOffset = ((heldItemConfig != null) ? heldItemConfig.clippingProbeOffset : Vector3.zero);
			_clippingSampleTimer = 0f;
			_forwardRetractionTarget = 0f;
			_clippingPushTarget = Vector3.zero;
		}

		private void CheckItemPlacementModeStatus(bool isEquipping)
		{
			if (_equippedEntity.TryGetComponent<AttachableObject>(out var component))
			{
				if (isEquipping)
				{
					EnableObjectSlotDetectionMode(component.targetSlots, component);
				}
				else
				{
					DisableObjectSlotDetectionMode();
				}
			}
		}

		public void Equip(HeldItem heldItem)
		{
			if (!(heldItem == null) && !(heldItem.netIdentity == null) && heldItem.netId != 0 && _playerService?.LocalPlayer?.IsPlayerSitting() != true && _playerService?.LocalPlayer?.IsDowned != true)
			{
				if (IsItemEquipped)
				{
					DropInstant();
				}
				_clippingOffset = Vector3.zero;
				_clippingOffsetVelocity = Vector3.zero;
				_forwardRetraction = 0f;
				_forwardRetractionVelocity = 0f;
				_playerIgnoreColliders = null;
				DestroyClippingProbe();
				SoundID id = (heldItem.EquipSoundOverride.IsValid() ? heldItem.EquipSoundOverride : equipSound);
				if (id.IsValid())
				{
					_networkAudioRelay?.PlayOneShotAttachedExcludeSelf(id, base.netIdentity);
				}
				EquipmentNetworkedTransform.transform.SetLocalPositionAndRotation(heldItem.PositionOnHand, Quaternion.Euler(heldItem.RotationOnHand));
				heldItem.GetComponent<NetworkedTransform>().SetParent(parentingConfig: new NetworkedTransformParentingConfig
				{
					PositionOffset = Vector3.zero,
					RotationOffset = Vector3.zero,
					KeepPositionAxes = false,
					KeepRotationAxes = false
				}, subTransformIndex: EquipmentNetworkedTransform.NetworkedTransformIndex, parent: EquipmentNetworkedTransform);
				_equippedEntity = heldItem;
				ResolveClippingConfig();
				if (heldItem.AttachedSnappingPlane != null)
				{
					heldItem.AttachedSnappingPlane.DetachForEquip(heldItem.gameObject);
					heldItem.AttachedSnappingPlane = null;
					heldItem.AttachedSnappingPlaneContainer = null;
				}
				CheckItemPlacementModeStatus(isEquipping: true);
				heldItem.OnEquipped.Invoke();
				NetworkIdentity itemIdentity = heldItem.netIdentity;
				CmdEquip(itemIdentity);
				OnItemEquipped.Invoke();
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdEquip(NetworkIdentity itemIdentity, NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Player.EquipmentManager::CmdEquip(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", -909072217, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void Unequip(HeldItem heldItem = null, bool keepSlotDetection = false)
		{
			if (!keepSlotDetection && _equippedEntity.TryGetComponent<AttachableObject>(out var _))
			{
				DisableObjectSlotDetectionMode();
			}
			if (_equippedEntity == null)
			{
				return;
			}
			if (heldItem != null && _equippedEntity != heldItem)
			{
				EvilLogger.LogError("Unequipping failed because reference entity is not match the equipped one!", "Unequip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 643);
				return;
			}
			_equippedEntity.GetComponent<NetworkedTransform>().SetParent(null, default(NetworkedTransformParentingConfig), 0);
			NetworkIdentity component2 = _equippedEntity.GetComponent<NetworkIdentity>();
			CmdUnequip(component2);
			if (!keepSlotDetection)
			{
				CheckItemPlacementModeStatus(isEquipping: false);
			}
			_equippedEntity.OnUnequipped.Invoke();
			_equippedEntity = null;
			OnItemUnequipped.Invoke();
		}

		[Command(requiresAuthority = false)]
		private void CmdUnequip(NetworkIdentity itemIdentity, NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Player.EquipmentManager::CmdUnequip(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", 80741068, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void Consume()
		{
			if (!(_equippedEntity == null))
			{
				if (_equippedEntity.TryGetComponent<AttachableObject>(out var _))
				{
					DisableObjectSlotDetectionMode();
				}
				_equippedEntity.GetComponent<NetworkedTransform>().SetParent(null, default(NetworkedTransformParentingConfig), 0);
				CheckItemPlacementModeStatus(isEquipping: false);
				_equippedEntity = null;
				OnItemUnequipped.Invoke();
			}
		}

		public void Drop(HeldItem heldItem = null)
		{
			DropInstant(heldItem);
		}

		public bool DropInstant(HeldItem heldItem = null)
		{
			if (heldItem != null && _equippedEntity != heldItem)
			{
				EvilLogger.LogError("DropInstant failed: reference entity does not match equipped item!", "DropInstant", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 749);
				return false;
			}
			if (_equippedEntity == null)
			{
				return false;
			}
			NetworkIdentity component = _equippedEntity.GetComponent<NetworkIdentity>();
			HeldItem equippedEntity = _equippedEntity;
			_castingManager?.Process(_dropCastHandle);
			_dropSelfExclusions.Clear();
			Collider[] componentsInChildren = equippedEntity.GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (collider != null)
				{
					_dropSelfExclusions.Add(collider);
				}
			}
			Vector3 vector = Vector3.forward;
			if (_playerService.TryGetCameraTransform(out var cameraTransform) && cameraTransform != null)
			{
				vector = cameraTransform.position - equippedEntity.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.0001f)
				{
					vector = -cameraTransform.forward;
				}
			}
			Quaternion desiredRotation = Quaternion.LookRotation(vector.normalized, Vector3.up) * Quaternion.Euler(equippedEntity.DefaultPlacingRotation);
			Vector3 anchorLocalOffset = equippedEntity.BoundsCenterLocalOffset + equippedEntity.PositionOffsetForPlacement;
			DropResult dropResult = DropCastHelper.ProcessCastResult(_lastDropCastResult, equippedEntity, _dropSettings, _dropSelfExclusions, _dropBlockingMask, desiredRotation, anchorLocalOffset);
			if (!dropResult.Success)
			{
				_uiFeedbackManager.CreateFloatingMessage("@vehicle.no_valid_ground", FeedbackType.Error);
				return false;
			}
			if (dropResult.SnappingPlane != null)
			{
				UnequipForInstantDrop(heldItem);
				dropResult.SnappingPlane.SnapObjectExternal(equippedEntity, dropResult.HitInfo.point, dropResult.HitInfo.normal);
				PlayDropSurfaceEffect(dropResult);
				PlayDropSurfaceSound(dropResult, equippedEntity);
				OnItemDropped.Invoke();
				return true;
			}
			if (IsDropTargetBlocked(equippedEntity, dropResult))
			{
				_uiFeedbackManager.CreateFloatingMessage("@vehicle.drop_blocked", FeedbackType.Error);
				return false;
			}
			UnequipForInstantDrop(heldItem);
			equippedEntity.transform.SetPositionAndRotation(dropResult.TargetPosition, dropResult.TargetRotation);
			CmdDropInstant(component, dropResult.TargetPosition, dropResult.TargetRotation);
			PlayDropSurfaceEffect(dropResult);
			PlayDropSurfaceSound(dropResult, equippedEntity);
			OnItemDropped.Invoke();
			return true;
		}

		private bool IsDropTargetBlocked(HeldItem item, DropResult dropResult)
		{
			if (!dropResult.Success)
			{
				return false;
			}
			if (dropResult.SnappingPlane != null)
			{
				return false;
			}
			Transform transform = item.transform;
			Quaternion deltaRot = dropResult.TargetRotation * Quaternion.Inverse(transform.rotation);
			Vector3 position = transform.position;
			Vector3 targetPosition = dropResult.TargetPosition;
			Collider collider = dropResult.HitInfo.collider;
			Collider collider2 = _playerService?.CapsuleCollider;
			int mask = item.CollisionLayers;
			Collider[] rigidColliders = item.GetRigidColliders();
			if (rigidColliders == null)
			{
				return false;
			}
			Collider[] array = rigidColliders;
			foreach (Collider collider3 in array)
			{
				if (collider3 == null)
				{
					continue;
				}
				int num = TargetOverlapHelper.OverlapColliderAtTarget(collider3, deltaRot, position, targetPosition, mask, 0.85f, DropOverlapBuffer);
				for (int j = 0; j < num; j++)
				{
					Collider collider4 = DropOverlapBuffer[j];
					if (!(collider4 == null) && !(collider4 == collider) && !(collider4 == collider2) && !_dropSelfExclusions.Contains(collider4))
					{
						return true;
					}
				}
			}
			return false;
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

		private void UnequipForInstantDrop(HeldItem heldItem = null)
		{
			if (_equippedEntity.TryGetComponent<AttachableObject>(out var _))
			{
				DisableObjectSlotDetectionMode();
			}
			if (!(_equippedEntity == null))
			{
				if (heldItem != null && _equippedEntity != heldItem)
				{
					EvilLogger.LogError("UnequipForInstantDrop failed: reference entity mismatch!", "UnequipForInstantDrop", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 936);
					return;
				}
				_equippedEntity.GetComponent<NetworkedTransform>().SetParent(null, default(NetworkedTransformParentingConfig), 0);
				NetworkIdentity component2 = _equippedEntity.GetComponent<NetworkIdentity>();
				CmdUnequipForInstantDrop(component2);
				CheckItemPlacementModeStatus(isEquipping: false);
				_equippedEntity.OnUnequipped.Invoke();
				_equippedEntity = null;
				OnItemUnequipped.Invoke();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdUnequipForInstantDrop(NetworkIdentity itemIdentity, NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Player.EquipmentManager::CmdUnequipForInstantDrop(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", 1831728919, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdDropInstant(NetworkIdentity itemIdentity, Vector3 targetPosition, Quaternion targetRotation, NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			writer.WriteVector3(targetPosition);
			writer.WriteQuaternion(targetRotation);
			SendCommandInternal("System.Void NomadDrive.Features.Player.EquipmentManager::CmdDropInstant(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Quaternion,Mirror.NetworkConnectionToClient)", -1855616473, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSyncInstantDropPosition(NetworkIdentity itemIdentity, Vector3 targetPosition, Quaternion targetRotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(itemIdentity);
			writer.WriteVector3(targetPosition);
			writer.WriteQuaternion(targetRotation);
			SendRPCInternal("System.Void NomadDrive.Features.Player.EquipmentManager::RpcSyncInstantDropPosition(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Quaternion)", -1068035543, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		protected virtual void OnItemEquip()
		{
			ActionMapsManager.EnterEquippingMode();
			InputActionPromptsPanel.ActivateWithPrompts(GetEquipPromptNames(_equippedEntity));
			RegisterDropCast();
		}

		private static string[] GetEquipPromptNames(HeldItem item)
		{
			if (item is DirectHeldItem directHeldItem)
			{
				string text = directHeldItem.UseActionPromptId ?? ((directHeldItem.useInputType == HeldItemUseInputType.Primary) ? "HeldItem_Primary_Use" : "HeldItem_Secondary_Use");
				return new string[2] { text, "Enter_Placement_Mode" };
			}
			return new string[1] { "Enter_Placement_Mode" };
		}

		protected virtual void OnItemUnequip()
		{
			UnregisterDropCast();
			DestroyClippingProbe();
			ActionMapsManager.ExitEquippingMode();
			InputActionPromptsPanel.Hide();
		}

		private void RegisterDropCast()
		{
			if (_dropCastHandle == null && _castingManager != null && _playerService.TryGetCameraTransform(out var cameraTransform))
			{
				CastRequest request = CastRequest.Ray(cameraTransform, _dropSettings.MaxDistance, (int)_dropSettings.GroundLayers | _dropBlockingMask, QueryTriggerInteraction.Collide);
				request.Offset = Vector3.forward * _dropSettings.ForwardOffset;
				request.UseTransformForward = false;
				request.Direction = Vector3.down;
				request.UpdateMode = UpdateMode.Manual;
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

		public bool IsEquippedObjectAttachable()
		{
			AttachableObject component;
			if (_equippedEntity != null)
			{
				return _equippedEntity.TryGetComponent<AttachableObject>(out component);
			}
			return false;
		}

		public bool TryGetEquippedILiquidContainer(out ILiquidContainer container)
		{
			if (_equippedEntity != null && _equippedEntity.TryGetComponent<ILiquidContainer>(out container))
			{
				return true;
			}
			container = null;
			return false;
		}

		private void FixedUpdate()
		{
			if (!_isObjectSlotDetectionModeEnabled)
			{
				return;
			}
			_slotDetectionSampleTimer -= Time.fixedDeltaTime;
			if (_slotDetectionSampleTimer > 0f)
			{
				return;
			}
			_slotDetectionSampleTimer = slotDetectionSampleInterval;
			int num = Physics.OverlapSphereNonAlloc(base.transform.position, objectSlotDetectionRadius, _slotDetectionOverlapBuffer, objectSlotDetectionLayers, QueryTriggerInteraction.Collide);
			if (num == _slotDetectionOverlapBuffer.Length)
			{
				_slotDetectionOverlapBuffer = new Collider[_slotDetectionOverlapBuffer.Length * 2];
				return;
			}
			_currentlyDetectedSlotsBuffer.Clear();
			for (int i = 0; i < num; i++)
			{
				IObjectSlot objectSlot = ResolveSlotFromCollider(_slotDetectionOverlapBuffer[i]);
				if (objectSlot != null && !objectSlot.IsOccupied && _searchingSlotTypes.Contains(objectSlot.SlotType))
				{
					_currentlyDetectedSlotsBuffer.Add(objectSlot);
				}
			}
			foreach (IObjectSlot item in _currentlyDetectedSlotsBuffer)
			{
				if (!_detectedObjectSlots.Contains(item))
				{
					_detectedObjectSlots.Add(item);
					OnObjectSlotAdded(item);
				}
			}
			_slotsToRemoveBuffer.Clear();
			foreach (IObjectSlot detectedObjectSlot in _detectedObjectSlots)
			{
				if (!_currentlyDetectedSlotsBuffer.Contains(detectedObjectSlot))
				{
					_slotsToRemoveBuffer.Add(detectedObjectSlot);
				}
			}
			foreach (IObjectSlot item2 in _slotsToRemoveBuffer)
			{
				_detectedObjectSlots.Remove(item2);
				OnObjectSlotRemoved(item2);
			}
		}

		private static IObjectSlot ResolveSlotFromCollider(Collider collider)
		{
			if (collider == null)
			{
				return null;
			}
			if (collider.TryGetComponent<IObjectSlot>(out var component))
			{
				return component;
			}
			StaticInteractableRouter component3;
			if (collider.TryGetComponent<InteractableRouter>(out var component2))
			{
				if (component2.RouteInteractable != null)
				{
					return component2.RouteInteractable.GetComponent<IObjectSlot>();
				}
			}
			else if (collider.TryGetComponent<StaticInteractableRouter>(out component3) && component3.RouteInteractable is IObjectSlot result)
			{
				return result;
			}
			return null;
		}

		private void EnableObjectSlotDetectionMode(List<ObjectSlotType> slotTypes, AttachableObject attachable)
		{
			_slotDetectionAttachable = attachable;
			_searchingSlotTypes.Clear();
			foreach (ObjectSlotType slotType in slotTypes)
			{
				_searchingSlotTypes.Add(slotType);
			}
			_slotDetectionSampleTimer = 0f;
			_isObjectSlotDetectionModeEnabled = true;
		}

		public void DisableObjectSlotDetectionMode()
		{
			_isObjectSlotDetectionModeEnabled = false;
			_slotDetectionAttachable = null;
			foreach (IObjectSlot detectedObjectSlot in _detectedObjectSlots)
			{
				OnObjectSlotRemoved(detectedObjectSlot);
			}
			_detectedObjectSlots.Clear();
			_searchingSlotTypes.Clear();
		}

		private void OnObjectSlotAdded(IObjectSlot objectSlot)
		{
			if (_slotDetectionAttachable != null && _slotDetectionAttachable.HighlightSource != null)
			{
				Material overrideMaterial = ((slotHighlightSettings != null) ? slotHighlightSettings.sweepHighlightMaterial : null);
				objectSlot.SetupHighlightModel(_slotDetectionAttachable.HighlightSource, Vector3.zero, Quaternion.identity, overrideMaterial);
			}
			objectSlot.SetHighlight(ObjectHighlightType.Neutral);
			objectSlot.UnignoreHovering();
		}

		private void OnObjectSlotRemoved(IObjectSlot objectSlot)
		{
			objectSlot.ClearHighlightModel();
			objectSlot.SetHighlight(ObjectHighlightType.None);
			objectSlot.IgnoreHovering();
		}

		static EquipmentManager()
		{
			ClippingOverlapCache = new Collider[16];
			DropOverlapBuffer = new Collider[32];
			RemoteProcedureCalls.RegisterCommand(typeof(EquipmentManager), "System.Void NomadDrive.Features.Player.EquipmentManager::CmdEquip(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdEquip__NetworkIdentity__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(EquipmentManager), "System.Void NomadDrive.Features.Player.EquipmentManager::CmdUnequip(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdUnequip__NetworkIdentity__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(EquipmentManager), "System.Void NomadDrive.Features.Player.EquipmentManager::CmdUnequipForInstantDrop(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdUnequipForInstantDrop__NetworkIdentity__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(EquipmentManager), "System.Void NomadDrive.Features.Player.EquipmentManager::CmdDropInstant(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Quaternion,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdDropInstant__NetworkIdentity__Vector3__Quaternion__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(EquipmentManager), "System.Void NomadDrive.Features.Player.EquipmentManager::RpcSyncInstantDropPosition(Mirror.NetworkIdentity,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcSyncInstantDropPosition__NetworkIdentity__Vector3__Quaternion);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdEquip__NetworkIdentity__NetworkConnectionToClient(NetworkIdentity itemIdentity, NetworkConnectionToClient conn)
		{
			if (conn != null && conn == base.connectionToClient)
			{
				HeldItem component;
				if (itemIdentity == null)
				{
					EvilLogger.LogError("Net Identity Missing!!", "CmdEquip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 594);
				}
				else if (!itemIdentity.TryGetComponent<HeldItem>(out component))
				{
					EvilLogger.LogError("Advanced Entity Component Missing!!", "CmdEquip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 600);
				}
				else if (!component.IsEquipped && conn != null && conn.connectionId != -1)
				{
					itemIdentity.RemoveClientAuthority();
					itemIdentity.AssignClientAuthority(conn);
					component.IsEquipped = true;
					component.SetInteractionAvailability(newValue: false);
					component.SetRigidCollidersTriggered(newValue: true);
					component.ServerSetSnappedDescendantsTriggered(triggered: true);
					LootLifecycleManager.Instance?.ServerUntrack(itemIdentity);
				}
			}
		}

		protected static void InvokeUserCode_CmdEquip__NetworkIdentity__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdEquip called on client.");
			}
			else
			{
				((EquipmentManager)obj).UserCode_CmdEquip__NetworkIdentity__NetworkConnectionToClient(reader.ReadNetworkIdentity(), senderConnection);
			}
		}

		protected void UserCode_CmdUnequip__NetworkIdentity__NetworkConnectionToClient(NetworkIdentity itemIdentity, NetworkConnectionToClient conn)
		{
			if (conn != null && conn == base.connectionToClient)
			{
				if (itemIdentity == null)
				{
					EvilLogger.LogError("Net Identity Missing!!", "CmdUnequip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 675);
					return;
				}
				if (!itemIdentity.TryGetComponent<HeldItem>(out var component))
				{
					EvilLogger.LogError("Advanced Entity Component Missing!!", "CmdUnequip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 681);
					return;
				}
				if (!component.IsEquipped)
				{
					EvilLogger.LogError("Unequpping failed! Advanced Entity is not equipped!", "CmdUnequip", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 687);
					return;
				}
				_ = itemIdentity.connectionToClient;
				itemIdentity.RemoveClientAuthority();
				itemIdentity.AssignClientAuthority(NetworkServer.connections[0]);
				component.IsEquipped = false;
				component.ServerSetSnappedDescendantsTriggered(triggered: false);
			}
		}

		protected static void InvokeUserCode_CmdUnequip__NetworkIdentity__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUnequip called on client.");
			}
			else
			{
				((EquipmentManager)obj).UserCode_CmdUnequip__NetworkIdentity__NetworkConnectionToClient(reader.ReadNetworkIdentity(), senderConnection);
			}
		}

		protected void UserCode_CmdUnequipForInstantDrop__NetworkIdentity__NetworkConnectionToClient(NetworkIdentity itemIdentity, NetworkConnectionToClient conn)
		{
			if (conn != null && conn == base.connectionToClient)
			{
				if (itemIdentity == null)
				{
					EvilLogger.LogError("CmdUnequipForInstantDrop: Net Identity Missing!", "CmdUnequipForInstantDrop", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 966);
					return;
				}
				if (!itemIdentity.TryGetComponent<HeldItem>(out var component))
				{
					EvilLogger.LogError("CmdUnequipForInstantDrop: HeldItem Component Missing!", "CmdUnequipForInstantDrop", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 972);
					return;
				}
				if (!component.IsEquipped)
				{
					EvilLogger.LogError("CmdUnequipForInstantDrop: Item is not equipped!", "CmdUnequipForInstantDrop", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 978);
					return;
				}
				itemIdentity.RemoveClientAuthority();
				itemIdentity.AssignClientAuthority(NetworkServer.connections[0]);
				component.IsEquipped = false;
				component.ServerSetSnappedDescendantsTriggered(triggered: false);
			}
		}

		protected static void InvokeUserCode_CmdUnequipForInstantDrop__NetworkIdentity__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUnequipForInstantDrop called on client.");
			}
			else
			{
				((EquipmentManager)obj).UserCode_CmdUnequipForInstantDrop__NetworkIdentity__NetworkConnectionToClient(reader.ReadNetworkIdentity(), senderConnection);
			}
		}

		protected void UserCode_CmdDropInstant__NetworkIdentity__Vector3__Quaternion__NetworkConnectionToClient(NetworkIdentity itemIdentity, Vector3 targetPosition, Quaternion targetRotation, NetworkConnectionToClient conn)
		{
			if (conn == null || conn != base.connectionToClient)
			{
				return;
			}
			if (itemIdentity == null)
			{
				EvilLogger.LogError("CmdDropInstant: Item Identity Missing!", "CmdDropInstant", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 1002);
				return;
			}
			if (!itemIdentity.TryGetComponent<HeldItem>(out var component))
			{
				EvilLogger.LogError("CmdDropInstant: HeldItem Component Missing!", "CmdDropInstant", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\EquipmentManager.cs", 1008);
				return;
			}
			if (itemIdentity.TryGetComponent<NetworkedTransform>(out var component2))
			{
				component2.ServerSetParent(null, default(NetworkedTransformParentingConfig), 0);
			}
			component.transform.SetPositionAndRotation(targetPosition, targetRotation);
			component.SetInteractionAvailability(newValue: true);
			component.SetRigidCollidersTriggered(newValue: false);
			component.SetRigidCollidersEnabled(newValue: true);
			RpcSyncInstantDropPosition(itemIdentity, targetPosition, targetRotation);
		}

		protected static void InvokeUserCode_CmdDropInstant__NetworkIdentity__Vector3__Quaternion__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDropInstant called on client.");
			}
			else
			{
				((EquipmentManager)obj).UserCode_CmdDropInstant__NetworkIdentity__Vector3__Quaternion__NetworkConnectionToClient(reader.ReadNetworkIdentity(), reader.ReadVector3(), reader.ReadQuaternion(), senderConnection);
			}
		}

		protected void UserCode_RpcSyncInstantDropPosition__NetworkIdentity__Vector3__Quaternion(NetworkIdentity itemIdentity, Vector3 targetPosition, Quaternion targetRotation)
		{
			if (!(itemIdentity == null))
			{
				if (itemIdentity.TryGetComponent<NetworkedTransform>(out var component))
				{
					component.ClientForceClearParent();
				}
				itemIdentity.transform.SetPositionAndRotation(targetPosition, targetRotation);
			}
		}

		protected static void InvokeUserCode_RpcSyncInstantDropPosition__NetworkIdentity__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSyncInstantDropPosition called on server.");
			}
			else
			{
				((EquipmentManager)obj).UserCode_RpcSyncInstantDropPosition__NetworkIdentity__Vector3__Quaternion(reader.ReadNetworkIdentity(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}
	}
}
