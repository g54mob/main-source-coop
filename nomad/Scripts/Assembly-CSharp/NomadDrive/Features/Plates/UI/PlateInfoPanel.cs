using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Plates.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class PlateInfoPanel : GameCanvasGroup
	{
		[SerializeField]
		private TextMeshProUGUI plateCodeText;

		[SerializeField]
		private TextMeshProUGUI rarityText;

		[SerializeField]
		private TextMeshProUGUI probabilityText;

		[SerializeField]
		private TextMeshProUGUI conditionText;

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

		public void SetPlate(Plate plate)
		{
			if (plate != null)
			{
				UpdateUI(plate);
				Show(interactable: false, blockRaycast: false);
				_follow.SetTarget(plate.gameObject, ResolveCamera());
			}
			else
			{
				Hide();
				_follow.ClearTarget();
			}
		}

		private void UpdateUI(Plate plate)
		{
			if (plateCodeText != null)
			{
				plateCodeText.text = plate.PlateCode;
			}
			PlateConfig.TierThreshold tier = plate.Tier;
			if (rarityText != null)
			{
				rarityText.text = ((tier != null) ? Localize(tier.nameKey) : string.Empty);
				if (tier != null)
				{
					rarityText.color = tier.color;
				}
			}
			if (probabilityText != null)
			{
				probabilityText.text = FormatOdds(plate.CumulativeOdds);
			}
			if (conditionText != null)
			{
				string text = ((_localizationService != null) ? _localizationService.FormatNumber(plate.Quality, 3) : plate.Quality.ToString("0.000"));
				conditionText.text = ((_localizationService != null) ? _localizationService.Localize("@plate.condition_format", text) : ("Condition - " + text));
			}
		}

		private string FormatOdds(long denominator)
		{
			string text = ((_localizationService != null) ? _localizationService.FormatNumber(denominator) : denominator.ToString("N0"));
			if (_localizationService == null)
			{
				return "1 / " + text;
			}
			return _localizationService.Localize("@plate.odds_format", text);
		}

		private string Localize(string key)
		{
			if (_localizationService == null)
			{
				return key;
			}
			return _localizationService.Localize(key);
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
