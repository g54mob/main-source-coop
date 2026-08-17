using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Restoration
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class SprayPaintInfoPanel : GameCanvasGroup
	{
		[SerializeField]
		private Image paintColorIcon;

		[SerializeField]
		private Image capacityFillImage;

		[SerializeField]
		private TextMeshProUGUI capacityPercentText;

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

		public void SetTool(Color paintColor, float capacityRatio)
		{
			SetColor(paintColor);
			UpdateCapacity(capacityRatio);
		}

		public void SetColor(Color paintColor)
		{
			if (paintColorIcon != null)
			{
				paintColorIcon.color = paintColor;
			}
		}

		public void UpdateCapacity(float capacityRatio)
		{
			if (capacityFillImage != null)
			{
				capacityFillImage.fillAmount = capacityRatio;
			}
			if (capacityPercentText != null)
			{
				capacityPercentText.text = _localizationService?.FormatPercent(capacityRatio) ?? $"{Mathf.RoundToInt(capacityRatio * 100f)}%";
			}
		}

		public void Clear()
		{
			if (capacityFillImage != null)
			{
				capacityFillImage.fillAmount = 0f;
			}
			if (capacityPercentText != null)
			{
				capacityPercentText.text = "";
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
