using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Vehicle.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class EngineHeatInfoPanel : GameCanvasGroup
	{
		[SerializeField]
		private Image heatFillImage;

		[SerializeField]
		private TextMeshProUGUI heatLevelLabel;

		[SerializeField]
		private TextMeshProUGUI heatPercentText;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IPlayerService _playerService;

		private WorldFollowTooltip _follow;

		private Camera _camera;

		private void Awake()
		{
			_follow = GetComponent<WorldFollowTooltip>();
		}

		public void SetHeat(float ratio)
		{
			ratio = Mathf.Clamp01(ratio);
			if (heatLevelLabel != null)
			{
				heatLevelLabel.text = _localizationService?.Localize("@ui.heat_level") ?? "Heat Level";
			}
			if (heatFillImage != null)
			{
				heatFillImage.fillAmount = ratio;
				heatFillImage.color = GetHeatColor(ratio);
			}
			if (heatPercentText != null)
			{
				heatPercentText.text = Mathf.RoundToInt(ratio * 100f) + "%";
			}
		}

		public void Clear()
		{
			if (heatLevelLabel != null)
			{
				heatLevelLabel.text = "";
			}
			if (heatPercentText != null)
			{
				heatPercentText.text = "";
			}
			if (heatFillImage != null)
			{
				heatFillImage.fillAmount = 0f;
			}
		}

		public void SetWorldTarget(GameObject target)
		{
			_follow.SetTarget(target, ResolveCamera());
		}

		public void ClearWorldTarget()
		{
			_follow.ClearTarget();
		}

		private static Color32 GetHeatColor(float ratio)
		{
			if (ratio < 0.75f)
			{
				if (ratio < 0.5f)
				{
					return new Color32(100, 220, 100, 255);
				}
				return new Color32(220, 200, 90, 255);
			}
			if (ratio < 0.9f)
			{
				return new Color32(240, 150, 80, 255);
			}
			return new Color32(220, 70, 70, 255);
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
