using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Inputs;
using EvilCore.Inputs.UI;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction.UI.InteractionUIObjects;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction.UI
{
	public class InteractionUI : MonoBehaviour
	{
		[SerializeField]
		private RectTransform Container;

		[SerializeField]
		private TextMeshProUGUI interactableNameText;

		[SerializeField]
		private List<Interaction> activeInteractions = new List<Interaction>();

		private readonly Dictionary<Interaction, InteractionUIObject> _interactionUIObjects = new Dictionary<Interaction, InteractionUIObject>();

		private readonly Dictionary<Interaction, Action<InteractionState>> _interactionHandlers = new Dictionary<Interaction, Action<InteractionState>>();

		private Transform _currentInteractionTransformPoint;

		[Inject]
		private CrosshairPanel _crossHairPanel;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IInputGlyphService _inputGlyphService;

		[Inject]
		private InputActionPromptsDatabase _inputActionPromptsDatabase;

		[Inject]
		private ILocalizationService _localizationService;

		private IInteractionManager _interactionManager;

		private IEquipmentManager _equipmentManager;

		private Camera _playerCamera;

		private UnityAction _onAvailabilityChangedHandler;

		private UnityAction _onInteractionActivityHandler;

		[SerializeField]
		private IInteractable currentInteractable;

		private const string InteractionPrimaryInputId = "Interaction_Primary";

		private const string InteractionSecondaryInputId = "Interaction_Secondary";

		[field: SerializeField]
		public RectTransform InteractionsLabel { get; private set; }

		[field: SerializeField]
		public RectTransform InteractableNameLabel { get; private set; }

		[field: SerializeField]
		public BasicInteractionUIObject BasicInteractionUIPrefab { get; private set; }

		[field: SerializeField]
		public HoldingInteractionUIObject HoldingInteractionUIPrefab { get; private set; }

		public bool IsInitialized { get; private set; }

		private string L(string text)
		{
			return _localizationService?.Localize(text) ?? text;
		}

		public void Init()
		{
			try
			{
				InitializeReferences();
				RegisterEvents();
				SetInteractableUI(null);
				if (_inputGlyphService != null)
				{
					_inputGlyphService.OnGlyphsChanged += RefreshInteractionUI;
				}
				IsInitialized = true;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("Error initializing InteractionDisplayer: " + ex.Message + "\n" + ex.StackTrace, "Init", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\UI\\InteractionUI.cs", 79);
			}
		}

		private void OnDestroy()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged -= RefreshInteractionUI;
			}
			if (IsInitialized)
			{
				UnregisterEvents();
			}
		}

		private void InitializeReferences()
		{
			if (_playerService == null || !_playerService.IsPlayerSpawned)
			{
				EvilLogger.LogError("[InteractionUI] PlayerService is null or player is not spawned yet!", "InitializeReferences", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\UI\\InteractionUI.cs", 95);
				return;
			}
			if (!_playerService.TryGetInteractionManager(out _interactionManager))
			{
				EvilLogger.LogError("[InteractionUI] InteractionManager not found!", "InitializeReferences", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\UI\\InteractionUI.cs", 101);
				return;
			}
			if (!_playerService.TryGetEquipmentManager(out _equipmentManager))
			{
				EvilLogger.LogError("[InteractionUI] EquipmentManager not found!", "InitializeReferences", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\UI\\InteractionUI.cs", 107);
				return;
			}
			if (_playerService.TryGetCameraTransform(out var cameraTransform))
			{
				_playerCamera = cameraTransform.GetComponent<Camera>();
			}
			if (_playerCamera == null)
			{
				EvilLogger.LogError("[InteractionUI] PlayerCamera not found!", "InitializeReferences", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\UI\\InteractionUI.cs", 118);
			}
		}

		private void RegisterEvents()
		{
			_equipmentManager.OnItemDropped.AddListener(RefreshInteractionUI);
			_equipmentManager.OnItemEquipped.AddListener(RefreshInteractionUI);
			_equipmentManager.OnItemUnequipped.AddListener(RefreshInteractionUI);
			_interactionManager.OnInteractableHovered += OnInteractableHovered;
			_interactionManager.OnInteractableUnhovered += OnInteractableUnhovered;
		}

		private void UnregisterEvents()
		{
			_equipmentManager.OnItemDropped.RemoveListener(RefreshInteractionUI);
			_equipmentManager.OnItemUnequipped.RemoveListener(RefreshInteractionUI);
			_equipmentManager.OnItemEquipped.RemoveListener(RefreshInteractionUI);
			_interactionManager.OnInteractableHovered -= OnInteractableHovered;
			_interactionManager.OnInteractableUnhovered -= OnInteractableUnhovered;
		}

		private void OnInteractableHovered(IInteractable interactable)
		{
			Reset();
			RegisterInteractable(interactable);
			SetupInteractionUI(interactable);
		}

		private void OnInteractableUnhovered(IInteractable interactable)
		{
			Reset();
			UnregisterInteractable(interactable);
			SetInteractableUI(null);
		}

		private void RegisterInteractable(IInteractable interactable)
		{
			if (interactable == null)
			{
				return;
			}
			currentInteractable = interactable;
			_onInteractionActivityHandler = RefreshInteractionUI;
			currentInteractable.OnInteractionActivityPerformed.AddListener(_onInteractionActivityHandler);
			_onAvailabilityChangedHandler = delegate
			{
				if (currentInteractable != null && currentInteractable.AvailableForInteraction)
				{
					RefreshInteractionUI();
				}
				else
				{
					ClearAllInteractions();
					SetInteractableUI(null);
				}
			};
			currentInteractable.OnInteractionAvailabilityChanged.AddListener(_onAvailabilityChangedHandler);
		}

		private void UnregisterInteractable(IInteractable interactable)
		{
			if (interactable != null)
			{
				currentInteractable = null;
				if (_onInteractionActivityHandler != null)
				{
					interactable.OnInteractionActivityPerformed.RemoveListener(_onInteractionActivityHandler);
					_onInteractionActivityHandler = null;
				}
				if (_onAvailabilityChangedHandler != null)
				{
					interactable.OnInteractionAvailabilityChanged.RemoveListener(_onAvailabilityChangedHandler);
					_onAvailabilityChangedHandler = null;
				}
			}
		}

		private void LateUpdate()
		{
			UpdateUIPosition();
			UpdateActiveInteractionUIs();
		}

		private void Reset()
		{
			ClearAllInteractions();
			_currentInteractionTransformPoint = null;
			SetInteractableUI(null);
			_crossHairPanel.Set(CrosshairType.Default);
		}

		private Sprite GetInteractionKeySprite(InteractionKey key)
		{
			string inputId = ((key == InteractionKey.Primary) ? "Interaction_Primary" : "Interaction_Secondary");
			InputActionPrompt primaryActionData = _inputActionPromptsDatabase.GetPrimaryActionData(inputId);
			if (primaryActionData == null)
			{
				return null;
			}
			return _inputGlyphService.GetSpriteForAction(primaryActionData.rewiredActionName);
		}

		private void UpdateActiveInteractionUIs()
		{
			if (_interactionUIObjects.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<Interaction, InteractionUIObject> interactionUIObject in _interactionUIObjects)
			{
				if (!(interactionUIObject.Key == null) && !(interactionUIObject.Value == null))
				{
					Sprite interactionKeySprite = GetInteractionKeySprite(interactionUIObject.Key.InteractionKey);
					interactionUIObject.Value.UpdateUI(L(interactionUIObject.Key.interactionString), interactionKeySprite);
				}
			}
		}

		private void UpdateUIPosition()
		{
			if (_currentInteractionTransformPoint == null || _playerCamera == null)
			{
				return;
			}
			Vector3 position = _playerCamera.WorldToScreenPoint(_currentInteractionTransformPoint.position);
			if (!(position.z < 0f))
			{
				Canvas componentInParent = Container.GetComponentInParent<Canvas>();
				if (componentInParent != null)
				{
					RectTransform component = componentInParent.GetComponent<RectTransform>();
					Vector2 vector = new Vector2(position.x / (float)Screen.width, position.y / (float)Screen.height);
					Vector2 sizeDelta = component.sizeDelta;
					Vector2 vector2 = new Vector2((vector.x - 0.5f) * sizeDelta.x, (vector.y - 0.5f) * sizeDelta.y);
					Container.localPosition = vector2;
				}
				else
				{
					position.z = 0f;
					Container.position = position;
				}
			}
		}

		public void SetupInteractionUI(IInteractable interactable)
		{
			if (_interactionManager == null || interactable == null)
			{
				return;
			}
			if (!interactable.AvailableForInteraction)
			{
				Reset();
				return;
			}
			_currentInteractionTransformPoint = interactable.InteractionDisplayPoint;
			SetInteractableUI(interactable);
			UpdateCrosshair(interactable.CrosshairType);
			UpdateUIPosition();
			foreach (Interaction activeInteraction in interactable.ActiveInteractions)
			{
				if (!activeInteraction.StateHandler.CurrentState.Equals(InteractionState.Deactivated) && (activeInteraction.Condition == null || activeInteraction.Condition()))
				{
					CreateInteractionUI(activeInteraction);
				}
			}
		}

		private void RefreshInteractionUI()
		{
			ClearAllInteractions();
			if (currentInteractable == null || !currentInteractable.AvailableForInteraction)
			{
				return;
			}
			SetInteractableUI(currentInteractable);
			UpdateCrosshair(currentInteractable.CrosshairType);
			foreach (Interaction activeInteraction in currentInteractable.ActiveInteractions)
			{
				if (!activeInteraction.StateHandler.CurrentState.Equals(InteractionState.Deactivated) && (activeInteraction.Condition == null || activeInteraction.Condition()))
				{
					CreateInteractionUI(activeInteraction);
				}
			}
		}

		private void CreateInteractionUI(Interaction interaction)
		{
			if (!(interaction is BasicInteraction interaction2))
			{
				if (interaction is HoldInteraction interaction3)
				{
					CreateHoldInteractionUI(interaction3);
				}
			}
			else
			{
				CreateBasicInteractionUI(interaction2);
			}
		}

		private void CreateBasicInteractionUI(BasicInteraction interaction)
		{
			if (!_interactionUIObjects.ContainsKey(interaction))
			{
				BasicInteractionUIObject basicInteractionUIObject = UnityEngine.Object.Instantiate(BasicInteractionUIPrefab, InteractionsLabel);
				Sprite interactionKeySprite = GetInteractionKeySprite(interaction.InteractionKey);
				basicInteractionUIObject.Set(interactionKeySprite, L(interaction.interactionString));
				activeInteractions.Add(interaction);
				_interactionUIObjects[interaction] = basicInteractionUIObject;
			}
		}

		private void CreateHoldInteractionUI(HoldInteraction interaction)
		{
			if (!_interactionUIObjects.ContainsKey(interaction))
			{
				HoldingInteractionUIObject holdingInteractionUIObject = UnityEngine.Object.Instantiate(HoldingInteractionUIPrefab, InteractionsLabel);
				Sprite interactionKeySprite = GetInteractionKeySprite(interaction.InteractionKey);
				holdingInteractionUIObject.Set(interactionKeySprite, L(interaction.interactionString), interaction.InteractionDuration);
				activeInteractions.Add(interaction);
				_interactionUIObjects[interaction] = holdingInteractionUIObject;
				Action<InteractionState> value = delegate(InteractionState state)
				{
					OnInteractionStateChange(interaction, state);
				};
				_interactionHandlers[interaction] = value;
				interaction.OnInteractionProcessStateChanged += value;
			}
		}

		private void OnInteractionStateChange(Interaction interaction, InteractionState state)
		{
			if (!(interaction is HoldInteraction) || !_interactionUIObjects.TryGetValue(interaction, out var value))
			{
				return;
			}
			HoldingInteractionUIObject holdingInteractionUIObject = value as HoldingInteractionUIObject;
			if (!(holdingInteractionUIObject == null))
			{
				switch (state)
				{
				case InteractionState.Started:
					holdingInteractionUIObject.FillInteractionProgressImage();
					break;
				case InteractionState.Completed:
					holdingInteractionUIObject.ResetInteractionProgressImageFillAmount();
					break;
				case InteractionState.Cancelled:
					holdingInteractionUIObject.CancelFillInteractionProgressImage();
					break;
				}
			}
		}

		private void ClearAllInteractions()
		{
			foreach (Interaction activeInteraction in activeInteractions)
			{
				if (_interactionHandlers.TryGetValue(activeInteraction, out var value))
				{
					activeInteraction.OnInteractionProcessStateChanged -= value;
				}
			}
			foreach (Interaction activeInteraction2 in activeInteractions)
			{
				if (_interactionUIObjects.TryGetValue(activeInteraction2, out var value2) && value2 != null)
				{
					UnityEngine.Object.Destroy(value2.gameObject);
				}
			}
			_interactionHandlers.Clear();
			_interactionUIObjects.Clear();
			activeInteractions.Clear();
		}

		private void SetInteractableUI(IInteractable interactable)
		{
			if (interactable == null)
			{
				if (interactableNameText != null)
				{
					interactableNameText.text = "";
				}
				SetNameHeaderVisible(state: false);
				SetInteractionLabelVisible(state: false);
				return;
			}
			string text = ((_localizationService != null) ? _localizationService.LocalizeName(interactable.interactableName) : interactable.interactableName);
			if (interactableNameText != null)
			{
				interactableNameText.text = text ?? string.Empty;
			}
			bool nameHeaderVisible = interactable.isNameLabelVisible && !string.IsNullOrWhiteSpace(text);
			SetNameHeaderVisible(nameHeaderVisible);
			SetInteractionLabelVisible(interactable.isInteractionLabelVisible);
		}

		private void SetNameHeaderVisible(bool state)
		{
			if (InteractableNameLabel != null)
			{
				InteractableNameLabel.GetComponent<CanvasGroup>().alpha = (state ? 1f : 0f);
			}
		}

		private void SetInteractionLabelVisible(bool state)
		{
			if (InteractionsLabel != null)
			{
				InteractionsLabel.GetComponent<CanvasGroup>().alpha = (state ? 1f : 0f);
			}
		}

		private void UpdateCrosshair(CrosshairType crosshairType)
		{
			_crossHairPanel.Set(crosshairType);
		}
	}
}
