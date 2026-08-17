using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Planting.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class PlantPotInfoPanel : GameCanvasGroup
	{
		[Header("Text")]
		[SerializeField]
		private TextMeshProUGUI plantNameText;

		[SerializeField]
		private TextMeshProUGUI statusText;

		[SerializeField]
		private TextMeshProUGUI phaseText;

		[SerializeField]
		private TextMeshProUGUI percentageText;

		[SerializeField]
		private TextMeshProUGUI countdownText;

		[Header("Progress")]
		[SerializeField]
		private Image progressFillImage;

		[Header("State Colors")]
		[SerializeField]
		private bool tintByState = true;

		[SerializeField]
		private Color growingColor = new Color(0.4f, 0.85f, 0.35f);

		[SerializeField]
		private Color readyColor = new Color(1f, 0.82f, 0.25f);

		[SerializeField]
		private Color waitingColor = new Color(0.4f, 0.65f, 1f);

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IGameUIManager _guiManager;

		private WorldFollowTooltip _follow;

		private Camera _camera;

		private PlantPot _boundPot;

		private void Awake()
		{
			_follow = GetComponent<WorldFollowTooltip>();
		}

		public void SetPlantPot(PlantPot pot)
		{
			if (pot == null || pot.PlantState == PlantState.Empty || pot.PlantType == PlantType.None)
			{
				ClearPlantPot();
				return;
			}
			_boundPot = pot;
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.PlantPotInfo, interactable: false, blockRaycast: false);
			_follow.SetTarget(pot.gameObject, ResolveCamera());
			Refresh();
		}

		public void ClearPlantPot()
		{
			_boundPot = null;
			_follow?.ClearTarget();
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.PlantPotInfo);
		}

		private void Update()
		{
			if (!(_boundPot == null))
			{
				if (_boundPot.PlantState == PlantState.Empty || _boundPot.PlantType == PlantType.None)
				{
					ClearPlantPot();
				}
				else
				{
					Refresh();
				}
			}
		}

		private void Refresh()
		{
			if (_boundPot == null)
			{
				return;
			}
			PlantState plantState = _boundPot.PlantState;
			if (plantNameText != null)
			{
				plantNameText.text = _boundPot.PlantDisplayName;
			}
			if (statusText != null)
			{
				statusText.text = L(StatusKey(plantState));
				if (tintByState)
				{
					statusText.color = ColorForState(plantState);
				}
			}
			float num = FillForState(plantState);
			if (progressFillImage != null)
			{
				progressFillImage.fillAmount = num;
				if (tintByState)
				{
					progressFillImage.color = ColorForState(plantState);
				}
			}
			if (phaseText != null)
			{
				int totalPhases = _boundPot.TotalPhases;
				if (_boundPot.HasPlant && totalPhases > 0)
				{
					int num2 = Mathf.Clamp(_boundPot.CurrentPhase + 1, 1, totalPhases);
					phaseText.text = ((_localizationService != null) ? _localizationService.Localize("@plant.phase_format", num2, totalPhases) : $"Phase {num2}/{totalPhases}");
				}
				else
				{
					phaseText.text = string.Empty;
				}
			}
			if (percentageText != null)
			{
				if (plantState == PlantState.Growing || plantState == PlantState.FullyGrown)
				{
					percentageText.text = _localizationService?.FormatPercent(num) ?? $"{Mathf.RoundToInt(num * 100f)}%";
				}
				else
				{
					percentageText.text = string.Empty;
				}
			}
			if (countdownText != null)
			{
				if (plantState == PlantState.Growing)
				{
					int num3 = Mathf.CeilToInt(_boundPot.SecondsToNextPhase);
					countdownText.text = ((_localizationService != null) ? _localizationService.Localize("@plant.seconds_format", num3) : $"{num3}s");
				}
				else
				{
					countdownText.text = string.Empty;
				}
			}
		}

		private float FillForState(PlantState state)
		{
			return state switch
			{
				PlantState.Growing => _boundPot.CurrentPhaseProgress01, 
				PlantState.FullyGrown => 1f, 
				_ => 0f, 
			};
		}

		private static string StatusKey(PlantState state)
		{
			switch (state)
			{
			case PlantState.Planted:
			case PlantState.NeedsWater:
				return "@plant.waiting_water";
			case PlantState.Growing:
				return "@plant.growing";
			case PlantState.FullyGrown:
				return "@plant.ready_harvest";
			default:
				return "@plant.waiting_water";
			}
		}

		private Color ColorForState(PlantState state)
		{
			return state switch
			{
				PlantState.Growing => growingColor, 
				PlantState.FullyGrown => readyColor, 
				_ => waitingColor, 
			};
		}

		private string L(string key)
		{
			return _localizationService?.Localize(key) ?? key;
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
	}
}
