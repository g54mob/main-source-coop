using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Tools
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class EquippedItemBatteryInfoPanel : GameCanvasGroup
	{
		[SerializeField]
		private Image batteryAmountFillImage;

		[SerializeField]
		private TextMeshProUGUI equippedItemNameInfo;

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

		public void SetItem(string itemName, float chargeRatio)
		{
			string format = _localizationService?.Localize("@ui.battery_label") ?? "@ui.battery_label";
			equippedItemNameInfo.text = string.Format(format, itemName);
			batteryAmountFillImage.fillAmount = chargeRatio;
		}

		public void UpdateCharge(float chargeRatio)
		{
			batteryAmountFillImage.fillAmount = chargeRatio;
		}

		public void ClearItem()
		{
			equippedItemNameInfo.text = "";
			batteryAmountFillImage.fillAmount = 0f;
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
