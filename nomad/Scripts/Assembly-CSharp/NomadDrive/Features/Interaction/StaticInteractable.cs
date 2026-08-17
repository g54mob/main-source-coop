using System;
using System.Collections.Generic;
using System.Linq;
using EvilCore.Audio;
using EvilCore.Extensions;
using EvilCore.UI.Scripts;
using NomadDrive.Features.ColliderLOD;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.WorldGeneration.POISpawning;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction
{
	public abstract class StaticInteractable : MonoBehaviour, IInteractable, IColliderLodTarget
	{
		[Header("Identification")]
		[SerializeField]
		public string interactableName;

		[SerializeField]
		private string _identifierSuffix = "";

		[Inject]
		protected IAudioManager AudioManager;

		[Inject]
		protected INetworkedAudioManager NetworkAudioRelay;

		[Inject]
		protected IStaticInteractableManager StaticInteractableManager;

		private int _interactableId;

		private bool _availableForInteraction = true;

		private bool _isIgnored;

		private Collider[] _rigidColliders;

		private Collider[] _triggerColliders;

		private static readonly HashSet<string> AllowedLayers = new HashSet<string> { "Default", "NeutralHighlight", "CorrectHighlight", "IncorrectHighlight" };

		private bool _logicalRigidCollidersEnabled = true;

		private bool _lodRigidActive = true;

		[Header("Display")]
		[field: SerializeField]
		public Transform InteractionDisplayPoint { get; private set; }

		[field: SerializeField]
		public Transform ModelTransform { get; private set; }

		[field: SerializeField]
		public CrosshairType CrosshairType { get; set; } = CrosshairType.Interact;

		protected IStaticInteractionStateMachine BaseStateMachine { get; set; }

		public int InteractableId => _interactableId;

		public bool AvailableForInteraction => _availableForInteraction;

		string IInteractable.interactableName => interactableName;

		public List<Interaction> ActiveInteractions { get; private set; } = new List<Interaction>();

		public bool isNameLabelVisible { get; set; } = true;

		public bool isInteractionLabelVisible { get; set; } = true;

		public UnityEvent onHovered { get; private set; } = new UnityEvent();

		public UnityEvent onUnhovered { get; private set; } = new UnityEvent();

		public UnityEvent OnInteractionAvailabilityChanged { get; private set; } = new UnityEvent();

		public UnityEvent OnHoveringIgnored { get; private set; } = new UnityEvent();

		public UnityEvent OnInteractionActivityPerformed { get; private set; } = new UnityEvent();

		protected virtual bool UseStateMachine => true;

		public virtual bool ParticipatesInColliderLod => true;

		public virtual bool LodPositionMayChange => false;

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

		protected virtual void Awake()
		{
			base.gameObject.InjectGameObject();
			Init();
			_interactableId = ComputeInteractableId();
			if (UseStateMachine)
			{
				InitializeStateMachine();
			}
		}

		private void Init()
		{
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			_rigidColliders = componentsInChildren.Where((Collider c) => !c.isTrigger).ToArray();
			_triggerColliders = componentsInChildren.Where((Collider c) => c.isTrigger).ToArray();
			ActiveInteractions = GetComponents<Interaction>().ToList();
		}

		protected virtual void Start()
		{
			RegisterWithManager();
		}

		protected virtual void OnEnable()
		{
			onHovered.AddListener(OnHovered);
			onUnhovered.AddListener(OnUnhovered);
			ColliderLodRegistry.Register(this);
		}

		protected virtual void OnDisable()
		{
			onHovered.RemoveListener(OnHovered);
			onUnhovered.RemoveListener(OnUnhovered);
			ColliderLodRegistry.Unregister(this);
		}

		protected virtual void OnDestroy()
		{
			StaticInteractableManager?.Unregister(this);
		}

		protected virtual void OnValidate()
		{
			if (InteractionDisplayPoint == null)
			{
				InteractionDisplayPoint = base.transform;
			}
		}

		public int ComputeInteractableId()
		{
			Poi componentInParent = GetComponentInParent<Poi>(includeInactive: true);
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			int a = StaticInteractableId.Position(FloatingOriginManager.ToTrueWorld(transform.position));
			string text = StaticInteractableId.SiblingIndexPath(base.transform, transform);
			int b = StaticInteractableId.String(text + "|" + interactableName + "|" + _identifierSuffix);
			return StaticInteractableId.Combine(a, b);
		}

		private void RegisterWithManager()
		{
			if (StaticInteractableManager != null && !StaticInteractableManager.Register(this))
			{
				UpdateState();
			}
		}

		public abstract void ApplyStateFromManager(byte stateData, bool skipAnimation);

		protected void RequestStateChange(byte newState)
		{
			StaticInteractableManager?.RequestStateChange(_interactableId, newState);
		}

		public virtual void OnHovered()
		{
			EnableOutline();
		}

		public virtual void OnUnhovered()
		{
			DisableOutline();
		}

		public Interaction[] GetInteractions()
		{
			return ActiveInteractions.ToArray();
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

		public void SetInteractionAvailability(bool newValue)
		{
			_availableForInteraction = newValue;
			base.gameObject.layer = LayerMask.NameToLayer(newValue ? "Interactable" : "Default");
			OnInteractionAvailabilityChanged.Invoke();
		}

		public void IgnoreHovering()
		{
			_isIgnored = true;
		}

		public void UnignoreHovering()
		{
			_isIgnored = false;
		}

		public bool IsHoveringIgnored()
		{
			return _isIgnored;
		}

		protected Renderer[] GetRenderers()
		{
			return GetComponentsInChildren<Renderer>();
		}

		public void EnableOutline()
		{
			if (!_availableForInteraction)
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

		protected void SetLogicalRigidCollidersEnabled(bool enabled)
		{
			_logicalRigidCollidersEnabled = enabled;
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
	}
}
