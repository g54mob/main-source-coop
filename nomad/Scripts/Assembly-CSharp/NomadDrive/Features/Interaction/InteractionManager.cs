using System;
using System.Collections;
using System.Collections.Generic;
using EvilCore;
using EvilCore.DynamicCasting;
using EvilCore.Networking;
using EvilCore.UI.Scripts;
using Mirror;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction
{
	public class InteractionManager : NetworkBehaviour, IInteractionManager, IInitialize, IUniqueNetworkComponent, IPlayerComponent
	{
		[SerializeField]
		[ReadOnly]
		public float interactionDistance = 2f;

		[SerializeField]
		public float interactionIdentifierThreshold = 0.15f;

		[Header("Dynamic Interaction Distance")]
		[SerializeField]
		private float baseInteractionDistance = 1.2f;

		[SerializeField]
		private float pitchDistanceMultiplier = 2.5f;

		[SerializeField]
		private AnimationCurve pitchDistanceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _maxInteractionDistance;

		[SerializeField]
		private LayerMask interactableLayerMask;

		[SerializeField]
		private LayerMask obstacleLayerMask;

		[SerializeField]
		private float _elapsedInteractionTime;

		private bool _interactionKeyPressed;

		private Coroutine _interactionStarterCoroutine;

		private UnityAction _primaryInteractKeyPressedHandler;

		private UnityAction _primaryInteractKeyReleasedHandler;

		private UnityAction _secondaryInteractKeyPressedHandler;

		private UnityAction _secondaryInteractKeyReleasedHandler;

		private UnityAction<GameObject> _objectGrabHandler;

		private UnityAction _objectReleaseHandler;

		private UnityAction _objectUseHandler;

		private Action<IInteractable> _interactableHoverHandler;

		private Action<IInteractable> _interactableUnhoverHandler;

		[Inject]
		private CrosshairPanel _crosshairPanel;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private ICastingManager _castingManager;

		private IRaycastHandle _interactionCastHandle;

		private Transform _cachedCameraTransform;

		public int SetupPriority => 0;

		public IInteractable ActiveInteractable { get; set; }

		[field: SerializeField]
		public bool IsInitialized { get; set; }

		[field: SerializeField]
		public bool IsActive { get; set; }

		public event Action<IInteractable> OnInteractableHovered;

		public event Action<IInteractable> OnInteractableUnhovered;

		public event Action<IInteractable> OnInteractionAnimTriggered;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				Init();
				Activate();
				DisableInteraction();
			}
			else
			{
				IsActive = false;
				base.enabled = false;
			}
		}

		private new void OnValidate()
		{
			_maxInteractionDistance = baseInteractionDistance * pitchDistanceMultiplier;
		}

		public void Init()
		{
			_interactableHoverHandler = OnInteractableHoverActions;
			_interactableUnhoverHandler = OnInteractableUnhoverActions;
			OnInteractableHovered += _interactableHoverHandler;
			OnInteractableUnhovered += _interactableUnhoverHandler;
			_objectGrabHandler = OnObjectGrab;
			_objectReleaseHandler = OnObjectRelease;
			_objectUseHandler = OnObjectUse;
			if (_playerService.TryGetObjectPlacementManager(out var manager))
			{
				manager.OnPlacementEnter.AddListener(_objectGrabHandler);
				manager.OnPlacementExit.AddListener(_objectReleaseHandler);
			}
			interactableLayerMask = LayerMask.GetMask("Interactable", "SnappedInteractableOnVehicle");
			RegisterInteractionCast();
			if (_playerService.TryGetCameraTransform(out var cameraTransform))
			{
				_cachedCameraTransform = cameraTransform;
			}
			interactionDistance = CalculateDynamicInteractionDistance();
			IsInitialized = true;
		}

		private void RegisterInteractionCast()
		{
			if (_castingManager != null && _playerService != null && _playerService.TryGetCameraTransform(out var cameraTransform))
			{
				CastRequest request = CastRequest.Ray(cameraTransform, interactionDistance, interactableLayerMask, QueryTriggerInteraction.Collide);
				request.UpdateMode = UpdateMode.LateUpdate;
				request.MaxHits = 16;
				_interactionCastHandle = _castingManager.Register(request, OnInteractionCastResult);
			}
		}

		private void OnInteractionCastResult(CastResult result)
		{
			if (IsInitialized && IsActive && base.isLocalPlayer)
			{
				IInteractable filteredInteractable = GetFilteredInteractable(result);
				if (filteredInteractable != null)
				{
					HoverInteractable(filteredInteractable);
				}
				else
				{
					CleanUp(ActiveInteractable);
				}
			}
		}

		public void Activate()
		{
			IsActive = true;
		}

		public void Deactivate()
		{
			_castingManager?.Unregister(_interactionCastHandle);
			IsActive = false;
			base.enabled = false;
		}

		private void Update()
		{
			if (IsInitialized && IsActive && base.isLocalPlayer)
			{
				if ((int)interactableLayerMask != 0)
				{
					UpdateDynamicInteractionDistance();
				}
				CheckInputs();
			}
		}

		private void CheckInputs()
		{
			if (base.isLocalPlayer)
			{
				if (BaseInputs.IsPrimaryInteractionButtonDown())
				{
					HandleInteractionKeyPressed(InteractionKey.Primary);
				}
				if (BaseInputs.IsPrimaryInteractionButtonUp())
				{
					HandleInteractionKeyReleased(InteractionKey.Primary);
				}
				if (BaseInputs.IsSecondaryInteractionButtonDown())
				{
					HandleInteractionKeyPressed(InteractionKey.Secondary);
				}
				if (BaseInputs.IsSecondaryInteractionButtonUp())
				{
					HandleInteractionKeyReleased(InteractionKey.Secondary);
				}
			}
		}

		private IInteractable GetFilteredInteractable(CastResult result)
		{
			if (!result.DidHit || result.HitCount == 0)
			{
				return null;
			}
			List<(RaycastHit, IInteractable)> list = new List<(RaycastHit, IInteractable)>();
			for (int i = 0; i < result.HitCount; i++)
			{
				RaycastHit raycastHit = result.Hits[i];
				if (raycastHit.collider == null)
				{
					continue;
				}
				IInteractable interactable = null;
				bool flag = false;
				InteractableRouter component2;
				StaticInteractable component3;
				StaticInteractableRouter component4;
				if (raycastHit.collider.TryGetComponent<Interactable>(out var component))
				{
					if (component.IsHoveringIgnored())
					{
						flag = true;
					}
					else
					{
						interactable = component;
					}
				}
				else if (raycastHit.collider.TryGetComponent<InteractableRouter>(out component2))
				{
					if (component2.RouteInteractable == null || component2.RouteInteractable.IsHoveringIgnored())
					{
						flag = true;
					}
					else
					{
						interactable = component2.RouteInteractable;
					}
				}
				else if (raycastHit.collider.TryGetComponent<StaticInteractable>(out component3))
				{
					if (component3.IsHoveringIgnored())
					{
						flag = true;
					}
					else
					{
						interactable = component3;
					}
				}
				else if (raycastHit.collider.TryGetComponent<StaticInteractableRouter>(out component4))
				{
					if (component4.RouteInteractable == null || component4.RouteInteractable.IsHoveringIgnored())
					{
						flag = true;
					}
					else
					{
						interactable = component4.RouteInteractable;
					}
				}
				else
				{
					flag = true;
				}
				if (!flag && interactable != null && !IsBlockedByObstacle(raycastHit, interactable))
				{
					list.Add((raycastHit, interactable));
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			(RaycastHit, IInteractable) tuple = list[0];
			float distance = tuple.Item1.distance;
			foreach (var item in list)
			{
				RaycastHit raycastHit2;
				(raycastHit2, _) = item;
				if (raycastHit2.distance < distance)
				{
					raycastHit2 = item.Item1;
					distance = raycastHit2.distance;
					tuple = item;
				}
			}
			return tuple.Item2;
		}

		private bool IsBlockedByObstacle(RaycastHit interactableHit, IInteractable target)
		{
			if ((int)obstacleLayerMask == 0)
			{
				return false;
			}
			if (!_playerService.TryGetCameraTransform(out var cameraTransform))
			{
				return false;
			}
			Vector3 position = cameraTransform.position;
			Vector3 forward = cameraTransform.forward;
			float num = interactableHit.distance - 0.02f;
			if (num <= 0f)
			{
				return false;
			}
			if (!Physics.Raycast(position, forward, out var hitInfo, num, obstacleLayerMask, QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			IInteractable componentInParent = hitInfo.collider.GetComponentInParent<Interactable>();
			IInteractable interactable = componentInParent ?? hitInfo.collider.GetComponentInParent<StaticInteractable>();
			if (interactable != null && interactable == target)
			{
				return false;
			}
			return true;
		}

		private void HoverInteractable(IInteractable interactable)
		{
			if (interactable != ActiveInteractable && interactable != null)
			{
				if (ActiveInteractable != null)
				{
					ActiveInteractable.onUnhovered.Invoke();
					CleanUp(ActiveInteractable);
				}
				this.OnInteractableHovered?.Invoke(interactable);
			}
		}

		private void OnInteractableHoverActions(IInteractable interactable)
		{
			ActiveInteractable = interactable;
			ActiveInteractable.onHovered.Invoke();
		}

		private void OnInteractableUnhoverActions(IInteractable interactable)
		{
			if (!IsHoldInteractionInProgress())
			{
				_interactionKeyPressed = false;
				if (ActiveInteractable != null)
				{
					ActiveInteractable.onUnhovered.Invoke();
					ResetInteractions();
					ActiveInteractable = null;
				}
				_crosshairPanel.Reset();
			}
		}

		private bool IsHoldInteractionInProgress()
		{
			if (ActiveInteractable == null)
			{
				return false;
			}
			Interaction[] interactions = ActiveInteractable.GetInteractions();
			foreach (Interaction interaction in interactions)
			{
				if (interaction.InteractionType == InteractionType.Hold && interaction.StateHandler.IsInState(InteractionState.Started))
				{
					return true;
				}
			}
			return false;
		}

		private bool HasBasicInteractionOnSameKey(InteractionKey key)
		{
			if (ActiveInteractable == null)
			{
				return false;
			}
			Interaction[] interactions = ActiveInteractable.GetInteractions();
			foreach (Interaction interaction in interactions)
			{
				if (interaction.InteractionType == InteractionType.Basic && interaction.InteractionKey == key)
				{
					return true;
				}
			}
			return false;
		}

		private void ResetInteractions()
		{
			Interaction[] interactions = ActiveInteractable.GetInteractions();
			foreach (Interaction interaction in interactions)
			{
				if (interaction.InteractionType == InteractionType.Hold && !interaction.StateHandler.IsInState(InteractionState.Started))
				{
					(interaction as HoldInteraction).Reset();
				}
			}
		}

		public void HandleInteractionKeyPressed(InteractionKey pressedInteractionKey)
		{
			if (ActiveInteractable == null || !ActiveInteractable.AvailableForInteraction || _interactionKeyPressed)
			{
				return;
			}
			Interaction[] interactions = ActiveInteractable.GetInteractions();
			_interactionKeyPressed = true;
			Interaction[] array = interactions;
			foreach (Interaction interaction in array)
			{
				if (interaction.InteractionKey == pressedInteractionKey && !interaction.StateHandler.IsInState(InteractionState.Deactivated) && (interaction.Condition == null || interaction.Condition()))
				{
					_interactionStarterCoroutine = StartCoroutine(HandleInteractionStarterCo(interaction));
				}
			}
		}

		private IEnumerator HandleInteractionStarterCo(Interaction interaction)
		{
			if (interaction == null)
			{
				yield break;
			}
			if (interaction.InteractionType == InteractionType.Basic)
			{
				(interaction as BasicInteraction).StartInteraction();
			}
			bool needsThreshold = interaction.InteractionType == InteractionType.Hold && HasBasicInteractionOnSameKey(interaction.InteractionKey);
			while (_interactionKeyPressed)
			{
				_elapsedInteractionTime += Time.deltaTime;
				if (interaction.InteractionType == InteractionType.Basic)
				{
					if (_elapsedInteractionTime >= interactionIdentifierThreshold)
					{
						(interaction as BasicInteraction).ResetInteraction();
					}
				}
				else if (interaction.InteractionType == InteractionType.Hold && (!needsThreshold || _elapsedInteractionTime >= interactionIdentifierThreshold))
				{
					(interaction as HoldInteraction).StartInteraction();
					this.OnInteractionAnimTriggered?.Invoke(ActiveInteractable);
					_elapsedInteractionTime = 0f;
					break;
				}
				yield return null;
			}
		}

		public void HandleInteractionKeyReleased(InteractionKey releasedInteractionKey)
		{
			if (!_interactionKeyPressed)
			{
				return;
			}
			_interactionKeyPressed = false;
			if (ActiveInteractable == null)
			{
				return;
			}
			Interaction[] interactions = ActiveInteractable.GetInteractions();
			foreach (Interaction interaction in interactions)
			{
				if (interaction.InteractionKey != releasedInteractionKey || interaction.StateHandler.IsInState(InteractionState.Deactivated) || (interaction.Condition != null && !interaction.Condition()))
				{
					continue;
				}
				if (interaction as BasicInteraction != null)
				{
					if (_elapsedInteractionTime < interactionIdentifierThreshold)
					{
						(interaction as BasicInteraction).CompleteInteraction();
						this.OnInteractionAnimTriggered?.Invoke(ActiveInteractable);
					}
				}
				else if (interaction as HoldInteraction != null)
				{
					if (interaction.StateHandler.IsInState(InteractionState.Started))
					{
						(interaction as HoldInteraction).Cancel();
					}
					else if (interaction.StateHandler.IsInState(InteractionState.Completed))
					{
						(interaction as HoldInteraction).Reset();
					}
				}
			}
			_elapsedInteractionTime = 0f;
		}

		private void CleanUp(IInteractable interactable)
		{
			if (interactable != null)
			{
				this.OnInteractableUnhovered?.Invoke(interactable);
			}
		}

		private void OnObjectGrab(GameObject grabbedObject)
		{
			DisableInteraction();
		}

		private void OnObjectRelease()
		{
			EnableInteraction();
		}

		private void OnObjectUse()
		{
			EnableInteraction();
		}

		public void EnableInteraction()
		{
			interactableLayerMask = LayerMask.GetMask("Interactable", "SnappedInteractableOnVehicle");
			interactionDistance = CalculateDynamicInteractionDistance();
			UpdateCastRequest();
		}

		public void DisableInteraction()
		{
			interactableLayerMask = 0;
			UpdateCastRequest();
		}

		private float CalculateDynamicInteractionDistance()
		{
			_maxInteractionDistance = baseInteractionDistance * pitchDistanceMultiplier;
			if (_cachedCameraTransform == null)
			{
				return baseInteractionDistance;
			}
			float num = _cachedCameraTransform.localEulerAngles.x;
			if (num > 180f)
			{
				num -= 360f;
			}
			float time = Mathf.Clamp01(Mathf.Abs(num) / 80f);
			return Mathf.Lerp(baseInteractionDistance, _maxInteractionDistance, pitchDistanceCurve.Evaluate(time));
		}

		private void UpdateDynamicInteractionDistance()
		{
			float num = CalculateDynamicInteractionDistance();
			if (Mathf.Abs(num - interactionDistance) > 0.01f)
			{
				interactionDistance = num;
				UpdateCastRequest();
			}
		}

		private void UpdateCastRequest()
		{
			if (_castingManager != null && _interactionCastHandle != null && _playerService != null && _playerService.TryGetCameraTransform(out var cameraTransform))
			{
				CastRequest newRequest = CastRequest.Ray(cameraTransform, interactionDistance, interactableLayerMask, QueryTriggerInteraction.Collide);
				newRequest.UpdateMode = UpdateMode.LateUpdate;
				newRequest.MaxHits = 16;
				_castingManager.UpdateRequest(_interactionCastHandle, newRequest);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
