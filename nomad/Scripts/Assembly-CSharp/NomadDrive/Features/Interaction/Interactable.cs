using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EvilCore.Audio;
using EvilCore.Extensions;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.ColliderLOD;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction
{
	public abstract class Interactable : NetworkBehaviour, IInteractable, IColliderLodTarget, IFloatingOriginShiftable
	{
		[SerializeField]
		public string interactableName;

		public bool IsLateJoinCompleted;

		[Inject]
		protected IAudioManager AudioManager;

		[Inject]
		protected INetworkedAudioManager NetworkAudioRelay;

		[SerializeField]
		[SyncVar(hook = "OnAvailableForInteractionChange")]
		private bool availableForInteraction = true;

		private Rigidbody _rigidbody;

		private Collider[] _rigidColliders;

		private Collider[] _triggerColliders;

		private int[] _rigidColliderOriginalLayers;

		private const string SnappedOnVehicleLayerName = "SnappedInteractableOnVehicle";

		private static int _snappedOnVehicleLayer;

		private bool _logicalRigidCollidersEnabled = true;

		private bool _lodRigidActive = true;

		private NetworkTransformBase[] _networkTransforms;

		private Coroutine _streamingSettleCoroutine;

		[SyncVar(hook = "OnTransformStreamingChanged")]
		private bool _transformStreamingActive;

		private bool _isIgnored;

		private static readonly HashSet<string> AllowedLayers;

		private NetworkedTransform _parentingTransform;

		[SyncVar(hook = "OnRigidCollidersEnableChanged")]
		private bool _isRigidCollidersEnabled = true;

		[SyncVar(hook = "OnTriggerCollidersEnabledChanged")]
		private bool _isTriggerCollidersEnabled = true;

		[SyncVar(hook = "OnRigidCollidersTriggeredChanged")]
		private bool _isRigidCollidersTriggered;

		[SyncVar(hook = "OnVehicleIsolatedChanged")]
		private bool _isVehicleIsolated;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate_availableForInteraction;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__transformStreamingActive;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isRigidCollidersEnabled;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isTriggerCollidersEnabled;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isRigidCollidersTriggered;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isVehicleIsolated;

		[field: SerializeField]
		public Transform InteractionDisplayPoint { get; private set; }

		[field: SerializeField]
		public Transform ModelTransform { get; private set; }

		public bool AvailableForInteraction => availableForInteraction;

		[field: SerializeField]
		public CrosshairType CrosshairType { get; set; } = CrosshairType.Interact;

		public List<Interaction> ActiveInteractions { get; private set; } = new List<Interaction>();

		public bool isNameLabelVisible { get; set; } = true;

		public bool isInteractionLabelVisible { get; set; } = true;

		public UnityEvent onHovered { get; private set; } = new UnityEvent();

		public UnityEvent onUnhovered { get; private set; } = new UnityEvent();

		public UnityEvent OnInteractionAvailabilityChanged { get; private set; } = new UnityEvent();

		public UnityEvent OnHoveringIgnored { get; private set; } = new UnityEvent();

		public UnityEvent OnInteractionActivityPerformed { get; private set; } = new UnityEvent();

		string IInteractable.interactableName => interactableName;

		protected IInteractionStateMachine BaseStateMachine { get; set; }

		protected virtual bool UseStateMachine => true;

		private static int SnappedOnVehicleLayer
		{
			get
			{
				if (_snappedOnVehicleLayer == -2)
				{
					_snappedOnVehicleLayer = LayerMask.NameToLayer("SnappedInteractableOnVehicle");
				}
				return _snappedOnVehicleLayer;
			}
		}

		public virtual bool ParticipatesInColliderLod => true;

		public virtual bool LodPositionMayChange => true;

		private bool HasNetworkedParent
		{
			get
			{
				if (_parentingTransform != null)
				{
					return _parentingTransform.HasParent;
				}
				return false;
			}
		}

		private bool IsExternallyPositioned
		{
			get
			{
				if (!(base.transform.parent != null))
				{
					return HasNetworkedParent;
				}
				return true;
			}
		}

		public bool IsVehicleColliderIsolated => _isVehicleIsolated;

		public bool NetworkavailableForInteraction
		{
			get
			{
				return availableForInteraction;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref availableForInteraction, 1uL, _Mirror_SyncVarHookDelegate_availableForInteraction);
			}
		}

		public bool Network_transformStreamingActive
		{
			get
			{
				return _transformStreamingActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _transformStreamingActive, 2uL, _Mirror_SyncVarHookDelegate__transformStreamingActive);
			}
		}

		public bool Network_isRigidCollidersEnabled
		{
			get
			{
				return _isRigidCollidersEnabled;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isRigidCollidersEnabled, 4uL, _Mirror_SyncVarHookDelegate__isRigidCollidersEnabled);
			}
		}

		public bool Network_isTriggerCollidersEnabled
		{
			get
			{
				return _isTriggerCollidersEnabled;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isTriggerCollidersEnabled, 8uL, _Mirror_SyncVarHookDelegate__isTriggerCollidersEnabled);
			}
		}

		public bool Network_isRigidCollidersTriggered
		{
			get
			{
				return _isRigidCollidersTriggered;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isRigidCollidersTriggered, 16uL, _Mirror_SyncVarHookDelegate__isRigidCollidersTriggered);
			}
		}

		public bool Network_isVehicleIsolated
		{
			get
			{
				return _isVehicleIsolated;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isVehicleIsolated, 32uL, _Mirror_SyncVarHookDelegate__isVehicleIsolated);
			}
		}

		GameObject IInteractable.gameObject => base.gameObject;

		Transform IInteractable.transform => base.transform;

		Transform IColliderLodTarget.transform => base.transform;

		protected virtual void InitializeStateMachine()
		{
		}

		protected virtual void ConfigureStates()
		{
		}

		public virtual void UpdateState()
		{
			BaseStateMachine?.RefreshCurrentState();
		}

		public Collider[] GetRigidColliders()
		{
			return _rigidColliders;
		}

		public Collider[] GetTriggerColliders()
		{
			return _triggerColliders;
		}

		public void SetLodColliderActive(bool active)
		{
			_lodRigidActive = active;
			ApplyRigidColliderEnabledState();
		}

		private void ApplyRigidColliderEnabledState()
		{
			if (_rigidColliders == null)
			{
				return;
			}
			bool flag = _logicalRigidCollidersEnabled && _lodRigidActive;
			Collider[] rigidColliders = _rigidColliders;
			foreach (Collider collider in rigidColliders)
			{
				if (!(collider == null))
				{
					collider.enabled = flag;
				}
			}
		}

		private void CacheNetworkTransforms()
		{
			NetworkIdentity component = GetComponent<NetworkIdentity>();
			NetworkTransformBase[] componentsInChildren = GetComponentsInChildren<NetworkTransformBase>(includeInactive: true);
			List<NetworkTransformBase> list = new List<NetworkTransformBase>(componentsInChildren.Length);
			NetworkTransformBase[] array = componentsInChildren;
			foreach (NetworkTransformBase networkTransformBase in array)
			{
				if (!(networkTransformBase == null) && !(networkTransformBase.GetComponentInParent<NetworkIdentity>() != component))
				{
					list.Add(networkTransformBase);
				}
			}
			_networkTransforms = list.ToArray();
		}

		protected virtual bool ShouldStreamTransform()
		{
			return _transformStreamingActive;
		}

		private void ApplyNetworkTransformSyncState()
		{
			if (_networkTransforms == null)
			{
				return;
			}
			bool flag = ShouldStreamTransform();
			for (int i = 0; i < _networkTransforms.Length; i++)
			{
				NetworkTransformBase networkTransformBase = _networkTransforms[i];
				if (!(networkTransformBase == null) && networkTransformBase.enabled != flag)
				{
					networkTransformBase.enabled = flag;
				}
			}
		}

		public void RefreshNetworkTransformSyncState()
		{
			ApplyNetworkTransformSyncState();
		}

		private void OnTransformStreamingChanged(bool oldValue, bool newValue)
		{
			ApplyNetworkTransformSyncState();
		}

		[Server]
		public void ServerSetTransformStreaming(bool active)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.Interactable::ServerSetTransformStreaming(System.Boolean)' called when server was not active");
			}
			else
			{
				Network_transformStreamingActive = active;
			}
		}

		[Server]
		public void ServerOpenTransformStreamingWindow(float settleSeconds)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.Interactable::ServerOpenTransformStreamingWindow(System.Single)' called when server was not active");
				return;
			}
			Network_transformStreamingActive = true;
			if (_streamingSettleCoroutine != null)
			{
				StopCoroutine(_streamingSettleCoroutine);
			}
			if (settleSeconds > 0f)
			{
				_streamingSettleCoroutine = StartCoroutine(CloseTransformStreamingAfter(settleSeconds));
			}
		}

		private IEnumerator CloseTransformStreamingAfter(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			Network_transformStreamingActive = false;
			_streamingSettleCoroutine = null;
		}

		protected virtual void Awake()
		{
			base.gameObject.InjectGameObject();
			Init();
			if (UseStateMachine)
			{
				InitializeStateMachine();
			}
		}

		private void Init()
		{
			SetupRigidbody();
			SetupColliders();
			CacheNetworkTransforms();
			_parentingTransform = GetComponent<NetworkedTransform>();
			ActiveInteractions.Clear();
			ActiveInteractions.AddRange(GetComponents<Interaction>());
		}

		private void SetupRigidbody()
		{
			_rigidbody = (TryGetComponent<Rigidbody>(out _rigidbody) ? _rigidbody : null);
			if (_rigidbody != null)
			{
				_rigidbody.isKinematic = true;
			}
		}

		private void SetupColliders()
		{
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			_rigidColliders = componentsInChildren.Where((Collider c) => !c.isTrigger).ToArray();
			_triggerColliders = componentsInChildren.Where((Collider c) => c.isTrigger).ToArray();
			_rigidColliderOriginalLayers = new int[_rigidColliders.Length];
			for (int num = 0; num < _rigidColliders.Length; num++)
			{
				_rigidColliderOriginalLayers[num] = _rigidColliders[num].gameObject.layer;
			}
		}

		protected virtual void OnEnable()
		{
			onHovered.AddListener(OnHovered);
			onUnhovered.AddListener(OnUnhovered);
			ColliderLodRegistry.Register(this);
			FloatingOriginManager.RegisterShiftable(this);
		}

		protected virtual void OnDisable()
		{
			onHovered.RemoveListener(OnHovered);
			onUnhovered.RemoveListener(OnUnhovered);
			ColliderLodRegistry.Unregister(this);
			FloatingOriginManager.UnregisterShiftable(this);
		}

		protected virtual void Start()
		{
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (InteractionDisplayPoint == null)
			{
				InteractionDisplayPoint = base.transform;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			IsLateJoinCompleted = true;
			ApplyNetworkTransformSyncState();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				SetForLateJoiner();
			}
		}

		protected virtual void SetForLateJoiner()
		{
			IsLateJoinCompleted = true;
			if (_isVehicleIsolated)
			{
				ApplyVehicleIsolation(isolated: true);
			}
			ApplyNetworkTransformSyncState();
		}

		private static Vector3 CurrentOriginShift()
		{
			if (!(FloatingOriginManager.Instance != null))
			{
				return Vector3.zero;
			}
			return FloatingOriginManager.Instance.TotalShift;
		}

		public void OnOriginShift(Vector3 delta)
		{
			if (!IsExternallyPositioned)
			{
				if (_rigidbody != null)
				{
					_rigidbody.position += delta;
				}
				base.transform.position += delta;
			}
		}

		public sealed override void OnSerialize(NetworkWriter writer, bool initialState)
		{
			base.OnSerialize(writer, initialState);
			if (initialState)
			{
				writer.WriteVector3(base.transform.position);
				writer.WriteQuaternion(base.transform.rotation);
				writer.WriteVector3(CurrentOriginShift());
			}
		}

		public sealed override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			base.OnDeserialize(reader, initialState);
			if (!initialState)
			{
				return;
			}
			Vector3 position = reader.ReadVector3();
			Quaternion rotation = reader.ReadQuaternion();
			Vector3 vector = reader.ReadVector3();
			if (!base.isOwned && !IsExternallyPositioned)
			{
				position += CurrentOriginShift() - vector;
				base.transform.SetPositionAndRotation(position, rotation);
				if (_rigidbody != null)
				{
					_rigidbody.position = position;
					_rigidbody.rotation = rotation;
				}
			}
		}

		protected virtual void OnHovered()
		{
			EnableOutline();
		}

		protected virtual void OnUnhovered()
		{
			DisableOutline();
		}

		public Interaction[] GetInteractions()
		{
			return ActiveInteractions.ToArray();
		}

		protected Renderer[] GetRenderers()
		{
			return GetComponentsInChildren<Renderer>();
		}

		public void SetRenderersVisibility(bool visible)
		{
			Renderer[] renderers = GetRenderers();
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].enabled = visible;
			}
		}

		[Command(requiresAuthority = false)]
		public void SetRigidCollidersEnabled(bool newValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(newValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.Interactable::SetRigidCollidersEnabled(System.Boolean)", 195387795, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void SetTriggerCollidersEnabled(bool newValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(newValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.Interactable::SetTriggerCollidersEnabled(System.Boolean)", 1831194652, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void SetRigidCollidersTriggered(bool newValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(newValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.Interactable::SetRigidCollidersTriggered(System.Boolean)", -962517241, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void SetVehicleColliderIsolated(bool newValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(newValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.Interactable::SetVehicleColliderIsolated(System.Boolean)", 1507364661, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnRigidCollidersEnableChanged(bool _, bool newValue)
		{
			_logicalRigidCollidersEnabled = newValue;
			ApplyRigidColliderEnabledState();
		}

		private void OnTriggerCollidersEnabledChanged(bool _, bool newValue)
		{
			Collider[] triggerColliders = _triggerColliders;
			for (int i = 0; i < triggerColliders.Length; i++)
			{
				triggerColliders[i].enabled = newValue;
			}
		}

		private void OnRigidCollidersTriggeredChanged(bool _, bool newValue)
		{
			Collider[] rigidColliders = _rigidColliders;
			for (int i = 0; i < rigidColliders.Length; i++)
			{
				rigidColliders[i].isTrigger = newValue;
			}
		}

		private void OnVehicleIsolatedChanged(bool _, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				ApplyVehicleIsolation(newValue);
			}
		}

		private void ApplyVehicleIsolation(bool isolated)
		{
			if (_rigidColliders == null)
			{
				return;
			}
			int snappedOnVehicleLayer = SnappedOnVehicleLayer;
			if (snappedOnVehicleLayer < 0)
			{
				return;
			}
			for (int i = 0; i < _rigidColliders.Length; i++)
			{
				Collider collider = _rigidColliders[i];
				if (!(collider == null) && !(collider.gameObject == base.gameObject))
				{
					collider.gameObject.layer = (isolated ? snappedOnVehicleLayer : _rigidColliderOriginalLayers[i]);
				}
			}
		}

		public void OnAvailableForInteractionChange(bool _, bool newValue)
		{
			base.gameObject.layer = LayerMask.NameToLayer(newValue ? "Interactable" : "Default");
			OnInteractionAvailabilityChanged.Invoke();
		}

		public void SetInteractionAvailability(bool newValue)
		{
			CmdSetInteractionAvailability(newValue);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetInteractionAvailability(bool newValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(newValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.Interactable::CmdSetInteractionAvailability(System.Boolean)", -2098424299, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void ChangeName(string nameString)
		{
			interactableName = nameString;
		}

		public void UnignoreHovering()
		{
			_isIgnored = false;
		}

		public void IgnoreHovering()
		{
			_isIgnored = true;
		}

		public virtual bool IsHoveringIgnored()
		{
			return _isIgnored;
		}

		public virtual void RefreshInteractionState()
		{
			base.gameObject.layer = LayerMask.NameToLayer(availableForInteraction ? "Interactable" : "Default");
			if (!ParticipatesInColliderLod)
			{
				_lodRigidActive = true;
			}
			_logicalRigidCollidersEnabled = _isRigidCollidersEnabled;
			ApplyRigidColliderEnabledState();
			if (_triggerColliders != null)
			{
				Collider[] triggerColliders = _triggerColliders;
				foreach (Collider collider in triggerColliders)
				{
					if (collider != null)
					{
						collider.enabled = _isTriggerCollidersEnabled;
					}
				}
			}
			if (_rigidColliders != null)
			{
				Collider[] triggerColliders = _rigidColliders;
				foreach (Collider collider2 in triggerColliders)
				{
					if (collider2 != null)
					{
						collider2.isTrigger = _isRigidCollidersTriggered;
					}
				}
			}
			ApplyVehicleIsolation(_isVehicleIsolated);
			UpdateState();
		}

		protected BasicInteraction GetBasicInteraction(InteractionKey interactionKey = InteractionKey.Primary)
		{
			foreach (Interaction activeInteraction in ActiveInteractions)
			{
				if (activeInteraction is BasicInteraction basicInteraction && basicInteraction.InteractionKey == interactionKey)
				{
					return basicInteraction;
				}
			}
			return null;
		}

		protected HoldInteraction GetHoldInteraction(InteractionKey interactionKey = InteractionKey.Primary)
		{
			foreach (Interaction activeInteraction in ActiveInteractions)
			{
				if (activeInteraction is HoldInteraction holdInteraction && holdInteraction.InteractionKey == interactionKey)
				{
					return holdInteraction;
				}
			}
			return null;
		}

		public BasicInteraction CreateBasicInteraction(Action interactAction, string interactionString, InteractionKey interactionKey = InteractionKey.Primary, bool overrideIfExists = false)
		{
			if (GetBasicInteraction(interactionKey) != null)
			{
				if (overrideIfExists)
				{
					SetBasicInteraction(interactAction, interactionString, interactionKey);
					return GetBasicInteraction(interactionKey);
				}
				return null;
			}
			BasicInteraction basicInteraction = base.gameObject.AddComponentWithInjection<BasicInteraction>();
			basicInteraction.InteractionKey = interactionKey;
			basicInteraction.InteractionType = InteractionType.Basic;
			basicInteraction.SetInteractionString(interactionString);
			basicInteraction.OnInteractionCompleted = new UnityEvent();
			basicInteraction.OnInteractionCompleted.AddListener(interactAction.Invoke);
			ActiveInteractions.Add(basicInteraction);
			OnInteractionActivityPerformed.Invoke();
			return basicInteraction;
		}

		public HoldInteraction CreateHoldInteraction(Action interactCompleteAction, string interactionString, float interactionDuration, InteractionKey interactionKey = InteractionKey.Primary, bool overrideIfExists = false)
		{
			if (GetHoldInteraction(interactionKey) != null)
			{
				if (overrideIfExists)
				{
					SetHoldInteraction(interactCompleteAction, interactionString, interactionDuration, interactionKey);
					return GetHoldInteraction(interactionKey);
				}
				return null;
			}
			HoldInteraction holdInteraction = base.gameObject.AddComponentWithInjection<HoldInteraction>();
			holdInteraction.InteractionKey = interactionKey;
			holdInteraction.InteractionType = InteractionType.Hold;
			holdInteraction.SetInteractionString(interactionString);
			holdInteraction.SetInteractionDuration(interactionDuration);
			holdInteraction.OnInteractionCompleted = new UnityEvent();
			holdInteraction.OnInteractionCompleted.AddListener(interactCompleteAction.Invoke);
			ActiveInteractions.Add(holdInteraction);
			OnInteractionActivityPerformed.Invoke();
			return holdInteraction;
		}

		public void CancelHoldInteraction(InteractionKey interactionKey = InteractionKey.Primary)
		{
			HoldInteraction holdInteraction = GetHoldInteraction(interactionKey);
			if (holdInteraction != null)
			{
				holdInteraction.OnInteractionCancelled.Invoke();
			}
		}

		public BasicInteraction SetBasicInteraction(Action actionToChange, string interactionString, InteractionKey interactionKey = InteractionKey.Primary)
		{
			BasicInteraction basicInteraction = GetBasicInteraction(interactionKey);
			if (basicInteraction != null)
			{
				basicInteraction.SetInteractionString(interactionString);
				basicInteraction.ResetCompleteAction(actionToChange);
				basicInteraction.StateHandler.SetState(InteractionState.Ready);
				OnInteractionActivityPerformed.Invoke();
				return basicInteraction;
			}
			return null;
		}

		public HoldInteraction SetHoldInteraction(Action actionToChange, string interactionString, float interactionDuration, InteractionKey interactionKey = InteractionKey.Primary)
		{
			HoldInteraction holdInteraction = GetHoldInteraction(interactionKey);
			if (holdInteraction != null)
			{
				holdInteraction.SetInteractionDuration(interactionDuration);
				holdInteraction.SetInteractionString(interactionString);
				holdInteraction.ResetCompleteAction(actionToChange);
				holdInteraction.StateHandler.SetState(InteractionState.Ready);
				OnInteractionActivityPerformed.Invoke();
				return holdInteraction;
			}
			return null;
		}

		protected void RemoveBasicInteraction(InteractionKey interactionKey = InteractionKey.Primary)
		{
			BasicInteraction basicInteraction = GetBasicInteraction(interactionKey);
			if (basicInteraction != null)
			{
				ActiveInteractions.Remove(basicInteraction);
				UnityEngine.Object.Destroy(basicInteraction);
				OnInteractionActivityPerformed.Invoke();
			}
		}

		protected void RemoveHoldInteraction(InteractionKey interactionKey = InteractionKey.Primary)
		{
			HoldInteraction holdInteraction = GetHoldInteraction(interactionKey);
			if (holdInteraction != null)
			{
				ActiveInteractions.Remove(holdInteraction);
				UnityEngine.Object.Destroy(holdInteraction);
				OnInteractionActivityPerformed.Invoke();
			}
		}

		protected void RemoveAllInteractions()
		{
			foreach (Interaction item in ActiveInteractions.ToList())
			{
				ActiveInteractions.Remove(item);
				UnityEngine.Object.Destroy(item);
			}
			OnInteractionActivityPerformed.Invoke();
		}

		internal void ActivateAllInteractions()
		{
			foreach (Interaction activeInteraction in ActiveInteractions)
			{
				if (activeInteraction.StateHandler.CurrentState == InteractionState.Deactivated)
				{
					activeInteraction.Activate();
				}
			}
			OnInteractionActivityPerformed.Invoke();
		}

		internal void DeactivateAllInteractions()
		{
			foreach (Interaction activeInteraction in ActiveInteractions)
			{
				if (activeInteraction.StateHandler.CurrentState != InteractionState.Deactivated)
				{
					activeInteraction.Deactivate();
				}
			}
			OnInteractionActivityPerformed.Invoke();
		}

		public void EnableOutline()
		{
			if (!availableForInteraction)
			{
				return;
			}
			Renderer[] renderers = GetRenderers();
			foreach (Renderer renderer in renderers)
			{
				if (renderer.gameObject.layer == LayerMask.NameToLayer("Default"))
				{
					renderer.gameObject.layer = LayerMask.NameToLayer("Selection");
				}
			}
		}

		public void DisableOutline()
		{
			Renderer[] renderers = GetRenderers();
			foreach (Renderer renderer in renderers)
			{
				if (renderer.gameObject.layer == LayerMask.NameToLayer("Selection"))
				{
					renderer.gameObject.layer = LayerMask.NameToLayer("Default");
				}
			}
		}

		public void SetHighlight(ObjectHighlightType type)
		{
			string layerName = type switch
			{
				ObjectHighlightType.None => "Default", 
				ObjectHighlightType.Neutral => "NeutralHighlight", 
				ObjectHighlightType.CorrectPlacement => "CorrectHighlight", 
				ObjectHighlightType.IncorrectPlacement => "IncorrectHighlight", 
				_ => string.Empty, 
			};
			Renderer[] renderers = GetRenderers();
			foreach (Renderer renderer in renderers)
			{
				if (AllowedLayers.Contains(LayerMask.LayerToName(renderer.gameObject.layer)) && renderer.gameObject.layer != LayerMask.NameToLayer(layerName))
				{
					renderer.gameObject.layer = LayerMask.NameToLayer(layerName);
				}
			}
		}

		protected Interactable()
		{
			_Mirror_SyncVarHookDelegate_availableForInteraction = OnAvailableForInteractionChange;
			_Mirror_SyncVarHookDelegate__transformStreamingActive = OnTransformStreamingChanged;
			_Mirror_SyncVarHookDelegate__isRigidCollidersEnabled = OnRigidCollidersEnableChanged;
			_Mirror_SyncVarHookDelegate__isTriggerCollidersEnabled = OnTriggerCollidersEnabledChanged;
			_Mirror_SyncVarHookDelegate__isRigidCollidersTriggered = OnRigidCollidersTriggeredChanged;
			_Mirror_SyncVarHookDelegate__isVehicleIsolated = OnVehicleIsolatedChanged;
		}

		static Interactable()
		{
			_snappedOnVehicleLayer = -2;
			AllowedLayers = new HashSet<string> { "Default", "NeutralHighlight", "CorrectHighlight", "IncorrectHighlight" };
			RemoteProcedureCalls.RegisterCommand(typeof(Interactable), "System.Void NomadDrive.Features.Interaction.Interactable::SetRigidCollidersEnabled(System.Boolean)", InvokeUserCode_SetRigidCollidersEnabled__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Interactable), "System.Void NomadDrive.Features.Interaction.Interactable::SetTriggerCollidersEnabled(System.Boolean)", InvokeUserCode_SetTriggerCollidersEnabled__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Interactable), "System.Void NomadDrive.Features.Interaction.Interactable::SetRigidCollidersTriggered(System.Boolean)", InvokeUserCode_SetRigidCollidersTriggered__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Interactable), "System.Void NomadDrive.Features.Interaction.Interactable::SetVehicleColliderIsolated(System.Boolean)", InvokeUserCode_SetVehicleColliderIsolated__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Interactable), "System.Void NomadDrive.Features.Interaction.Interactable::CmdSetInteractionAvailability(System.Boolean)", InvokeUserCode_CmdSetInteractionAvailability__Boolean, requiresAuthority: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_SetRigidCollidersEnabled__Boolean(bool newValue)
		{
			Network_isRigidCollidersEnabled = newValue;
		}

		protected static void InvokeUserCode_SetRigidCollidersEnabled__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetRigidCollidersEnabled called on client.");
			}
			else
			{
				((Interactable)obj).UserCode_SetRigidCollidersEnabled__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_SetTriggerCollidersEnabled__Boolean(bool newValue)
		{
			Network_isTriggerCollidersEnabled = newValue;
		}

		protected static void InvokeUserCode_SetTriggerCollidersEnabled__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetTriggerCollidersEnabled called on client.");
			}
			else
			{
				((Interactable)obj).UserCode_SetTriggerCollidersEnabled__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_SetRigidCollidersTriggered__Boolean(bool newValue)
		{
			Network_isRigidCollidersTriggered = newValue;
		}

		protected static void InvokeUserCode_SetRigidCollidersTriggered__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetRigidCollidersTriggered called on client.");
			}
			else
			{
				((Interactable)obj).UserCode_SetRigidCollidersTriggered__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_SetVehicleColliderIsolated__Boolean(bool newValue)
		{
			Network_isVehicleIsolated = newValue;
		}

		protected static void InvokeUserCode_SetVehicleColliderIsolated__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetVehicleColliderIsolated called on client.");
			}
			else
			{
				((Interactable)obj).UserCode_SetVehicleColliderIsolated__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_CmdSetInteractionAvailability__Boolean(bool newValue)
		{
			NetworkavailableForInteraction = newValue;
		}

		protected static void InvokeUserCode_CmdSetInteractionAvailability__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetInteractionAvailability called on client.");
			}
			else
			{
				((Interactable)obj).UserCode_CmdSetInteractionAvailability__Boolean(reader.ReadBool());
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(availableForInteraction);
				writer.WriteBool(_transformStreamingActive);
				writer.WriteBool(_isRigidCollidersEnabled);
				writer.WriteBool(_isTriggerCollidersEnabled);
				writer.WriteBool(_isRigidCollidersTriggered);
				writer.WriteBool(_isVehicleIsolated);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(availableForInteraction);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteBool(_transformStreamingActive);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteBool(_isRigidCollidersEnabled);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteBool(_isTriggerCollidersEnabled);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				writer.WriteBool(_isRigidCollidersTriggered);
			}
			if ((syncVarDirtyBits & 0x20L) != 0L)
			{
				writer.WriteBool(_isVehicleIsolated);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref availableForInteraction, _Mirror_SyncVarHookDelegate_availableForInteraction, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _transformStreamingActive, _Mirror_SyncVarHookDelegate__transformStreamingActive, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isRigidCollidersEnabled, _Mirror_SyncVarHookDelegate__isRigidCollidersEnabled, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isTriggerCollidersEnabled, _Mirror_SyncVarHookDelegate__isTriggerCollidersEnabled, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isRigidCollidersTriggered, _Mirror_SyncVarHookDelegate__isRigidCollidersTriggered, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isVehicleIsolated, _Mirror_SyncVarHookDelegate__isVehicleIsolated, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref availableForInteraction, _Mirror_SyncVarHookDelegate_availableForInteraction, reader.ReadBool());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _transformStreamingActive, _Mirror_SyncVarHookDelegate__transformStreamingActive, reader.ReadBool());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isRigidCollidersEnabled, _Mirror_SyncVarHookDelegate__isRigidCollidersEnabled, reader.ReadBool());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isTriggerCollidersEnabled, _Mirror_SyncVarHookDelegate__isTriggerCollidersEnabled, reader.ReadBool());
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isRigidCollidersTriggered, _Mirror_SyncVarHookDelegate__isRigidCollidersTriggered, reader.ReadBool());
			}
			if ((num & 0x20L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isVehicleIsolated, _Mirror_SyncVarHookDelegate__isVehicleIsolated, reader.ReadBool());
			}
		}
	}
}
