using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Attachables.UI
{
	[RequireComponent(typeof(WorldFollowTooltip))]
	public class ObjectInfoPanel : MonoBehaviour
	{
		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IPlayerService _playerService;

		[SerializeField]
		private TextMeshProUGUI _attachableObjectNameTextMesh;

		[SerializeField]
		private TextMeshProUGUI _conditionStateTextMesh;

		[SerializeField]
		private TextMeshProUGUI _conditionPerTextMesh;

		private ConditionComponent _activeCondition;

		private WorldFollowTooltip _follow;

		private Camera _camera;

		private void Awake()
		{
			_follow = GetComponent<WorldFollowTooltip>();
		}

		public void SetObject(ConditionComponent conditionComponent)
		{
			if (_activeCondition != null)
			{
				_activeCondition.OnConditionChanged.RemoveListener(OnActiveConditionChanged);
			}
			_activeCondition = conditionComponent;
			if (_activeCondition != null)
			{
				_activeCondition.OnConditionChanged.AddListener(OnActiveConditionChanged);
			}
			UpdateUI(_activeCondition);
			if (_activeCondition != null)
			{
				_follow.SetTarget(_activeCondition.gameObject, ResolveCamera());
			}
			else
			{
				_follow.ClearTarget();
			}
		}

		private void OnActiveConditionChanged(float oldValue, float newValue)
		{
			UpdateUI(_activeCondition);
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

		public void UpdateUI(ConditionComponent conditionComponent)
		{
			if (!(_activeCondition == null) && !(conditionComponent != _activeCondition))
			{
				float condition = conditionComponent.Condition;
				string conditionState = GetConditionState(condition);
				Color32 conditionColor = GetConditionColor(condition);
				string displayName = conditionComponent.DisplayName;
				_attachableObjectNameTextMesh.text = _localizationService?.LocalizeName(displayName) ?? displayName;
				_conditionStateTextMesh.text = _localizationService?.Localize(conditionState) ?? conditionState;
				_conditionStateTextMesh.color = conditionColor;
				_conditionPerTextMesh.text = condition.ToString("0") + "%";
			}
		}

		private string GetConditionState(float condition)
		{
			if (condition >= 75f)
			{
				if (condition <= 100f)
				{
					return "@condition.excellent";
				}
			}
			else
			{
				if (condition >= 25f)
				{
					if (condition >= 50f)
					{
						return "@condition.good";
					}
					return "@condition.fair";
				}
				if (condition > 0f)
				{
					return "@condition.poor";
				}
			}
			return "@condition.broken";
		}

		private Color32 GetConditionColor(float condition)
		{
			if (condition >= 75f)
			{
				if (condition <= 100f)
				{
					return new Color32(100, 220, 100, 255);
				}
			}
			else
			{
				if (condition >= 25f)
				{
					if (condition >= 50f)
					{
						return new Color32(150, 200, 120, 255);
					}
					return new Color32(220, 180, 80, 255);
				}
				if (condition > 0f)
				{
					return new Color32(240, 150, 80, 255);
				}
			}
			return new Color32(200, 80, 80, 255);
		}
	}
}
