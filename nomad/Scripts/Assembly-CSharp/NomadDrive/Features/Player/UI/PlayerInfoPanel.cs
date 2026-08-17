using NomadDrive.Features.Player.Animation;
using NomadDrive.Features.Player.PlayerStateMachine;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Player.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class PlayerInfoPanel : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private TMP_Text nameLabel;

		[SerializeField]
		private Image micIcon;

		[SerializeField]
		private RectTransform micIconRect;

		private CanvasGroup _canvasGroup;

		private PlayerInfoPanelConfig _config;

		private Transform _followTarget;

		private Transform _cameraTransform;

		private Player _trackedPlayer;

		private PlayerAnimationStateMachine _stateMachine;

		private uint _trackedNetId;

		private Vector3 _currentOffset;

		private Vector3 _offsetVelocity;

		private bool _rawSpeaking;

		private bool _displayedSpeaking;

		private float _speakingHoldExpiry;

		private Tween _pulseTween;

		private Tween _fadeTween;

		private string _lastDisplayName;

		public uint TrackedNetId => _trackedNetId;

		public Player TrackedPlayer => _trackedPlayer;

		public Transform FollowTarget => _followTarget;

		private void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			if (_canvasGroup != null)
			{
				_canvasGroup.alpha = 0f;
				_canvasGroup.blocksRaycasts = false;
				_canvasGroup.interactable = false;
			}
			NormalizeRootRectTransform();
		}

		private void NormalizeRootRectTransform()
		{
			if (base.transform is RectTransform rectTransform)
			{
				Vector2 pivot = (rectTransform.anchorMax = (rectTransform.anchorMin = new Vector2(0.5f, 0.5f)));
				rectTransform.pivot = pivot;
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}

		public void Bind(Player remotePlayer, Transform followTarget, Transform cameraTransform, PlayerInfoPanelConfig config)
		{
			_trackedPlayer = remotePlayer;
			_trackedNetId = ((remotePlayer != null) ? remotePlayer.netId : 0u);
			_stateMachine = ((remotePlayer != null) ? remotePlayer.GetComponent<PlayerAnimationStateMachine>() : null);
			_followTarget = followTarget;
			_cameraTransform = cameraTransform;
			_config = config;
			base.transform.SetParent(null, worldPositionStays: true);
			float num = ((config != null) ? config.baseCanvasScale : 0.005f);
			base.transform.localScale = Vector3.one * num;
			_currentOffset = ResolveTargetOffset();
			_offsetVelocity = Vector3.zero;
			ApplyWorldPlacement();
			_lastDisplayName = null;
			RefreshDisplayName();
			ApplySpeakingVisual(isSpeaking: false, force: true);
		}

		public void SetCameraTransform(Transform cameraTransform)
		{
			_cameraTransform = cameraTransform;
		}

		public void SetFollowTarget(Transform followTarget)
		{
			_followTarget = followTarget;
		}

		public void RefreshDisplayName()
		{
			if (!(nameLabel == null) && !(_trackedPlayer == null))
			{
				string displayName = _trackedPlayer.DisplayName;
				string text = (string.IsNullOrEmpty(displayName) ? "Player" : displayName);
				if (!(text == _lastDisplayName))
				{
					_lastDisplayName = text;
					nameLabel.text = text;
				}
			}
		}

		public void SetSpeakingState(bool isSpeaking)
		{
			_rawSpeaking = isSpeaking;
			if (isSpeaking)
			{
				_speakingHoldExpiry = Time.unscaledTime + ((_config != null) ? _config.speakingHoldSeconds : 0.2f);
				if (!_displayedSpeaking)
				{
					ApplySpeakingVisual(isSpeaking: true);
				}
			}
		}

		private void LateUpdate()
		{
			if (_config != null)
			{
				Vector3 vector = ResolveTargetOffset();
				_currentOffset = ((_config.offsetSmoothTime > 0f) ? Vector3.SmoothDamp(_currentOffset, vector, ref _offsetVelocity, _config.offsetSmoothTime) : vector);
			}
			ApplyWorldPlacement();
		}

		private Vector3 ResolveTargetOffset()
		{
			if (_config == null)
			{
				return _currentOffset;
			}
			if (_trackedPlayer != null && _trackedPlayer.IsDowned)
			{
				return _config.downedOffset;
			}
			switch ((_stateMachine != null) ? _stateMachine.CurrentStateKey : PlayerState.Idle)
			{
			case PlayerState.Sit:
				return _config.sittingOffset;
			case PlayerState.CrouchedIdle:
			case PlayerState.CrouchedWalk:
			case PlayerState.CrouchedSprint:
				return _config.crouchedOffset;
			default:
				return _config.standingOffset;
			}
		}

		public Vector3 GetWorldAnchorPosition()
		{
			if (_followTarget == null)
			{
				return base.transform.position;
			}
			return _followTarget.position + _followTarget.rotation * _currentOffset;
		}

		private void ApplyWorldPlacement()
		{
			if (_followTarget == null || _config == null)
			{
				return;
			}
			Vector3 vector = _followTarget.rotation * _currentOffset;
			base.transform.position = _followTarget.position + vector;
			if (!(_cameraTransform == null))
			{
				Vector3 vector2 = base.transform.position - _cameraTransform.position;
				vector2.y = 0f;
				if (!(vector2.sqrMagnitude < 0.0001f))
				{
					float y = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
					base.transform.rotation = Quaternion.Euler(0f, y, 0f);
				}
			}
		}

		public void Tick(float distance, bool occluded)
		{
			if (_config == null || _canvasGroup == null)
			{
				return;
			}
			RefreshDisplayName();
			if (_displayedSpeaking && !_rawSpeaking && Time.unscaledTime >= _speakingHoldExpiry)
			{
				ApplySpeakingVisual(isSpeaking: false);
			}
			if (distance >= _config.maxVisibleDistance)
			{
				if (_canvasGroup.alpha != 0f)
				{
					_canvasGroup.alpha = 0f;
				}
				return;
			}
			float num = 1f;
			float num2 = 1f;
			if (distance > _config.fullSizeDistance)
			{
				float num3 = Mathf.InverseLerp(_config.fullSizeDistance, _config.maxVisibleDistance, distance);
				num = Mathf.Lerp(1f, _config.minScale, num3);
				float num4 = Mathf.Clamp01(num3 / Mathf.Max(0.01f, _config.fadeRangeRatio));
				num2 = 1f - num4;
			}
			if (occluded)
			{
				num2 *= _config.occludedAlphaMultiplier;
			}
			base.transform.localScale = Vector3.one * (_config.baseCanvasScale * num);
			_canvasGroup.alpha = num2;
		}

		private void ApplySpeakingVisual(bool isSpeaking, bool force = false)
		{
			if (!force && _displayedSpeaking == isSpeaking)
			{
				return;
			}
			_displayedSpeaking = isSpeaking;
			if (_pulseTween.isAlive)
			{
				_pulseTween.Stop();
			}
			if (_fadeTween.isAlive)
			{
				_fadeTween.Stop();
			}
			if (micIconRect != null)
			{
				micIconRect.localScale = Vector3.one;
			}
			if (micIcon == null)
			{
				return;
			}
			float duration = ((_config != null) ? _config.speakingFadeDuration : 0.15f);
			if (isSpeaking)
			{
				_fadeTween = Tween.Alpha(micIcon, 1f, duration);
				if (micIconRect != null && _config != null)
				{
					_pulseTween = Tween.Scale(micIconRect, Vector3.one * _config.speakingPulseScale, _config.speakingPulseDuration, Ease.InOutSine, -1, CycleMode.Yoyo);
				}
			}
			else if (force)
			{
				Color color = micIcon.color;
				color.a = 0f;
				micIcon.color = color;
			}
			else
			{
				_fadeTween = Tween.Alpha(micIcon, 0f, duration);
			}
		}

		public void Unbind()
		{
			if (_pulseTween.isAlive)
			{
				_pulseTween.Stop();
			}
			if (_fadeTween.isAlive)
			{
				_fadeTween.Stop();
			}
			_trackedPlayer = null;
			_trackedNetId = 0u;
			_followTarget = null;
			_cameraTransform = null;
		}

		private void OnDestroy()
		{
			if (_pulseTween.isAlive)
			{
				_pulseTween.Stop();
			}
			if (_fadeTween.isAlive)
			{
				_fadeTween.Stop();
			}
		}
	}
}
