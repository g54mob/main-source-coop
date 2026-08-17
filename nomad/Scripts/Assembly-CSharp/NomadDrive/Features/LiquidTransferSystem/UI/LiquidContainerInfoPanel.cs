using System;
using EvilCore.Extensions;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class LiquidContainerInfoPanel : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private LiquidContainerUIObject sourceLiquidContainerUIObject;

		[SerializeField]
		private LiquidContainerUIObject targetLiquidContainerUIObject;

		[SerializeField]
		private Image defaultSuccessImage;

		[SerializeField]
		private Image defaultFailureImage;

		[SerializeField]
		private TextMeshProUGUI transferInfoText;

		[Header("Configuration")]
		[SerializeField]
		private LiquidTransferUIInfoConfigs configs;

		[Inject]
		private IGameUIManager _guiManager;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private ILiquidTransferProcessorFactory _transferProcessorFactory;

		[Inject]
		private InputActionPromptsPanel _inputActionPromptsPanel;

		[Inject]
		private ILocalizationService _localizationService;

		private const string TransferPromptName = "Liquid_Transfer";

		private LiquidTransferProcessor _transferProcessor;

		private bool _isTransferPromptActive;

		private WorldFollowTooltip _follow;

		private Camera _camera;

		private ITransferDrinkLock _lockedDrinkable;

		private void Awake()
		{
			_follow = GetComponent<WorldFollowTooltip>();
			SetupUIObjects();
		}

		private Camera ResolveCamera()
		{
			if (_camera != null)
			{
				return _camera;
			}
			if (_playerService != null && _playerService.TryGetCameraTransform(out var cameraTransform) && cameraTransform != null)
			{
				_camera = cameraTransform.GetComponent<Camera>();
			}
			return _camera;
		}

		private void Update()
		{
			if (_transferProcessor != null)
			{
				if (EquippingInputs.IsLiquidTransferButtonDown())
				{
					_transferProcessor.Start();
				}
				if (EquippingInputs.IsLiquidTransferButtonUp())
				{
					_transferProcessor.Pause();
				}
			}
			if (targetLiquidContainerUIObject.LiquidContainer != null || sourceLiquidContainerUIObject.LiquidContainer != null)
			{
				RefreshContainerColors();
				sourceLiquidContainerUIObject.UpdateUI();
				targetLiquidContainerUIObject.UpdateUI();
			}
		}

		private void RefreshContainerColors()
		{
			if (sourceLiquidContainerUIObject.LiquidContainer != null)
			{
				sourceLiquidContainerUIObject.SetColor(GetColorForLiquidType(sourceLiquidContainerUIObject.LiquidContainer.CurrentLiquidType));
			}
			if (targetLiquidContainerUIObject.LiquidContainer != null)
			{
				targetLiquidContainerUIObject.SetColor(GetColorForLiquidType(targetLiquidContainerUIObject.LiquidContainer.CurrentLiquidType));
			}
		}

		public void OnLiquidContainerHovered(ILiquidContainer targetLiquidContainer)
		{
			if (targetLiquidContainer != null)
			{
				_guiManager.ShowCanvasGroup(GameCanvasGroupName.LiquidContainerInfo, interactable: false, blockRaycast: false);
				_follow.SetTarget((targetLiquidContainer as Component)?.gameObject, ResolveCamera());
				ILiquidContainer container;
				if (!_playerService.TryGetEquipmentManager(out var manager))
				{
					SetUI(TransferRequestResult.Error);
				}
				else if (manager.TryGetEquippedILiquidContainer(out container))
				{
					manager.OnItemDropped.AddListener(ResetTransfer);
					if (container != targetLiquidContainer)
					{
						_transferProcessor?.Cancel();
						_transferProcessor = _transferProcessorFactory.Create(container, targetLiquidContainer);
						TransferRequestResult transferRequestResult = _transferProcessor.GetTransferRequestResult();
						Color colorForLiquidType = GetColorForLiquidType(container.CurrentLiquidType);
						sourceLiquidContainerUIObject.Setup(container, colorForLiquidType, _localizationService);
						sourceLiquidContainerUIObject.UpdateUI();
						Color colorForLiquidType2 = GetColorForLiquidType(targetLiquidContainer.CurrentLiquidType);
						targetLiquidContainerUIObject.Setup(targetLiquidContainer, colorForLiquidType2, _localizationService);
						targetLiquidContainerUIObject.UpdateUI();
						SetUI(transferRequestResult);
						SetupRect(info: false);
					}
				}
				else
				{
					manager.OnItemDropped.RemoveListener(ResetTransfer);
					sourceLiquidContainerUIObject.Setup(null, Color.white, _localizationService);
					Color colorForLiquidType3 = GetColorForLiquidType(targetLiquidContainer.CurrentLiquidType);
					targetLiquidContainerUIObject.Setup(targetLiquidContainer, colorForLiquidType3, _localizationService);
					targetLiquidContainerUIObject.UpdateUI();
					SetUI(TransferRequestResult.Error);
					SetupRect(info: true);
				}
			}
			else
			{
				ResetTransfer();
				DisableAllInfo();
			}
		}

		private void ResetTransfer()
		{
			SetDrinkLocked(locked: false);
			_guiManager.HideCanvasGroup(GameCanvasGroupName.LiquidContainerInfo);
			_follow?.ClearTarget();
			if (_isTransferPromptActive)
			{
				_inputActionPromptsPanel.RemoveTemporaryPrompt("Liquid_Transfer");
				_isTransferPromptActive = false;
			}
			if (_transferProcessor != null)
			{
				_transferProcessor.Cancel();
				_transferProcessor = null;
			}
		}

		private void SetupUIObjects()
		{
			defaultSuccessImage.sprite = configs.defaultSuccessSprite;
			defaultFailureImage.sprite = configs.defaultFailureSprite;
			DisableAllInfo();
		}

		private void SetupRect(bool info)
		{
			try
			{
				TryGetComponent<RectTransform>(out var component);
				if (info)
				{
					component.SetSizeDeltaX(configs.rectWidthForInfo);
					component.SetSizeDeltaY(configs.rectHeightForInfo);
				}
				else
				{
					component.SetSizeDeltaX(configs.rectWidthForTransfer);
					component.SetSizeDeltaY(configs.rectHeightForTransfer);
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				throw;
			}
		}

		private string L(string key)
		{
			return _localizationService?.Localize(key) ?? key;
		}

		private void SetUI(TransferRequestResult result)
		{
			DisableAllInfo();
			SetDrinkLocked(result == TransferRequestResult.Allowed);
			switch (result)
			{
			case TransferRequestResult.Allowed:
				defaultSuccessImage.gameObject.SetActive(value: true);
				transferInfoText.text = L("@liquid.ready");
				transferInfoText.gameObject.SetActive(value: true);
				if (!_isTransferPromptActive)
				{
					_inputActionPromptsPanel.AddTemporaryPrompt("Liquid_Transfer");
					_isTransferPromptActive = true;
				}
				break;
			case TransferRequestResult.SourceOutputNotAllowed:
				transferInfoText.text = L("@liquid.source_blocked");
				defaultFailureImage.gameObject.SetActive(value: true);
				transferInfoText.gameObject.SetActive(value: true);
				break;
			case TransferRequestResult.TargetInputNotAllowed:
				transferInfoText.text = L("@liquid.target_blocked");
				defaultFailureImage.gameObject.SetActive(value: true);
				transferInfoText.gameObject.SetActive(value: true);
				break;
			case TransferRequestResult.IncompatibleLiquid:
				transferInfoText.text = L("@liquid.incompatible");
				defaultFailureImage.gameObject.SetActive(value: true);
				transferInfoText.gameObject.SetActive(value: true);
				break;
			case TransferRequestResult.SourceEmpty:
				transferInfoText.text = L("@liquid.source_empty");
				defaultFailureImage.gameObject.SetActive(value: true);
				transferInfoText.gameObject.SetActive(value: true);
				break;
			case TransferRequestResult.TargetFull:
				transferInfoText.text = L("@liquid.target_full");
				defaultFailureImage.gameObject.SetActive(value: true);
				transferInfoText.gameObject.SetActive(value: true);
				break;
			case TransferRequestResult.Error:
				DisableAllInfo();
				break;
			default:
				throw new ArgumentOutOfRangeException("result", result, null);
			}
		}

		private void DisableAllInfo()
		{
			transferInfoText.gameObject.SetActive(value: false);
			defaultSuccessImage.gameObject.SetActive(value: false);
			defaultFailureImage.gameObject.SetActive(value: false);
		}

		private void SetDrinkLocked(bool locked)
		{
			ITransferDrinkLock transferDrinkLock = null;
			if (locked && _playerService != null && _playerService.TryGetEquipmentManager(out var manager) && manager.EquippedEntity is ITransferDrinkLock transferDrinkLock2)
			{
				transferDrinkLock = transferDrinkLock2;
			}
			if (transferDrinkLock != _lockedDrinkable)
			{
				if (_lockedDrinkable != null)
				{
					_lockedDrinkable.SetTransferLocked(locked: false);
					_inputActionPromptsPanel?.UnsuppressPrompt(_lockedDrinkable.UseActionPromptId);
				}
				_lockedDrinkable = transferDrinkLock;
				if (_lockedDrinkable != null)
				{
					_lockedDrinkable.SetTransferLocked(locked: true);
					_inputActionPromptsPanel?.SuppressPrompt(_lockedDrinkable.UseActionPromptId);
				}
			}
		}

		private Color GetColorForLiquidType(LiquidType type)
		{
			return type switch
			{
				LiquidType.Water => configs.waterColor, 
				LiquidType.Gasoline => configs.gasolineColor, 
				LiquidType.Oil => configs.oilColor, 
				LiquidType.Coffee => configs.coffeeColor, 
				LiquidType.Milk => configs.milkColor, 
				LiquidType.Coolant => configs.coolantColor, 
				_ => configs.defaultColor, 
			};
		}
	}
}
