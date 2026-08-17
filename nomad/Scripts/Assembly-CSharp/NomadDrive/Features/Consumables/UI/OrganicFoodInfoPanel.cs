using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Consumables.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class OrganicFoodInfoPanel : GameCanvasGroup
	{
		[Header("Text")]
		[SerializeField]
		private TextMeshProUGUI foodNameText;

		[SerializeField]
		private TextMeshProUGUI phaseNameText;

		[SerializeField]
		private TextMeshProUGUI countdownText;

		[Header("Progress")]
		[SerializeField]
		private Image progressFillImage;

		[Header("Phase Colors")]
		[SerializeField]
		private bool tintByPhase = true;

		[SerializeField]
		private Color rawColor = new Color(0.85f, 0.45f, 0.45f);

		[SerializeField]
		private Color midCookedColor = new Color(0.9f, 0.7f, 0.35f);

		[SerializeField]
		private Color cookedColor = new Color(0.55f, 0.8f, 0.4f);

		[SerializeField]
		private Color burnedColor = new Color(0.35f, 0.28f, 0.25f);

		[SerializeField]
		private Color spoiledColor = new Color(0.55f, 0.65f, 0.3f);

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IGameUIManager _guiManager;

		private WorldFollowTooltip _follow;

		private Camera _camera;

		private OrganicFood _boundFood;

		private void Awake()
		{
			_follow = GetComponent<WorldFollowTooltip>();
		}

		public void SetFood(OrganicFood food)
		{
			if (food == null)
			{
				ClearFood();
				return;
			}
			_boundFood = food;
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.OrganicFoodInfo, interactable: false, blockRaycast: false);
			_follow.SetTarget(food.gameObject, ResolveCamera());
			Refresh();
		}

		public void ClearFood()
		{
			_boundFood = null;
			_follow?.ClearTarget();
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.OrganicFoodInfo);
		}

		private void Update()
		{
			if (!(_boundFood == null))
			{
				Refresh();
			}
		}

		private void Refresh()
		{
			if (_boundFood == null)
			{
				return;
			}
			bool isSpoiled = _boundFood.IsSpoiled;
			CookedLevel cookedLevel = _boundFood.CookedLevel;
			if (foodNameText != null)
			{
				foodNameText.text = _boundFood.interactableName;
			}
			if (phaseNameText != null)
			{
				phaseNameText.text = L(PhaseKey(cookedLevel, isSpoiled));
				if (tintByPhase)
				{
					phaseNameText.color = PhaseColor(cookedLevel, isSpoiled);
				}
			}
			float fillAmount = (isSpoiled ? 0f : ((cookedLevel == CookedLevel.Burned) ? 1f : _boundFood.CurrentCookPhaseProgress01));
			if (progressFillImage != null)
			{
				progressFillImage.fillAmount = fillAmount;
				if (tintByPhase)
				{
					progressFillImage.color = PhaseColor(cookedLevel, isSpoiled);
				}
			}
			if (countdownText != null)
			{
				if (!isSpoiled && cookedLevel != CookedLevel.Burned && _boundFood.IsCookingProcessActive)
				{
					int num = Mathf.CeilToInt(_boundFood.SecondsToNextCookLevel);
					countdownText.text = ((_localizationService != null) ? _localizationService.Localize("@cooking.seconds_format", num) : $"{num}s");
				}
				else
				{
					countdownText.text = string.Empty;
				}
			}
		}

		private static string PhaseKey(CookedLevel level, bool spoiled)
		{
			if (spoiled)
			{
				return "@cooking.spoiled";
			}
			return level switch
			{
				CookedLevel.Raw => "@cooking.raw", 
				CookedLevel.MidCooked => "@cooking.mid_cooked", 
				CookedLevel.WellCooked => "@cooking.cooked", 
				CookedLevel.Burned => "@cooking.burned", 
				_ => "@cooking.raw", 
			};
		}

		private Color PhaseColor(CookedLevel level, bool spoiled)
		{
			if (spoiled)
			{
				return spoiledColor;
			}
			return level switch
			{
				CookedLevel.Raw => rawColor, 
				CookedLevel.MidCooked => midCookedColor, 
				CookedLevel.WellCooked => cookedColor, 
				CookedLevel.Burned => burnedColor, 
				_ => rawColor, 
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
