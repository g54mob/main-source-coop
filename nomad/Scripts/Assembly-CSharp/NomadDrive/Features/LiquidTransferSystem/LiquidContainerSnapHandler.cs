using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking.Parenting;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Animations;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidContainerSnapHandler : NetworkBehaviour
	{
		[SerializeField]
		private LiquidSnapConfig snapConfig;

		[SerializeField]
		private bool debugMode;

		[SerializeField]
		private LiquidSnapPose debugPose;

		[Inject]
		private IPlayerService playerService;

		private NetworkedTransform _networkedTransform;

		private HeldItem _heldItem;

		private ILiquidSnapTarget _pendingTarget;

		private LiquidSnapTargetType _targetTypeLocal;

		private bool _isSnapTargetLocal;

		private bool _isAnimating;

		private bool _constraintSuspended;

		private float _entryWeight;

		private Tween _weightTween;

		public bool IsSnapped => _isSnapTargetLocal;

		private bool IsEquippedNow
		{
			get
			{
				if (_heldItem != null)
				{
					return _heldItem.IsEquipped;
				}
				return false;
			}
		}

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_heldItem = GetComponent<HeldItem>();
			_networkedTransform = GetComponent<NetworkedTransform>();
			if (_networkedTransform == null)
			{
				EvilLogger.LogError("LiquidContainerSnapHandler requires a NetworkedTransform on the same GameObject", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\LiquidTransferSystem\\Scripts\\Core\\LiquidContainerSnapHandler.cs", 57);
				base.enabled = false;
			}
		}

		private void OnEnable()
		{
			if (_heldItem != null)
			{
				_heldItem.OnUnequipped.AddListener(OnHeldItemUnequipped);
			}
		}

		private void OnDisable()
		{
			if (_heldItem != null)
			{
				_heldItem.OnUnequipped.RemoveListener(OnHeldItemUnequipped);
			}
			_weightTween.Stop();
			_isAnimating = false;
			_isSnapTargetLocal = false;
			_pendingTarget = null;
			_entryWeight = 0f;
			_heldItem?.EndSnapStream();
			RestoreConstraint();
		}

		public void Snap(ILiquidSnapTarget target)
		{
			if (target != null && IsEquippedNow && (debugMode || (!(snapConfig == null) && snapConfig.TryGetPose(target.TargetType, out var _))))
			{
				_pendingTarget = target;
				_targetTypeLocal = target.TargetType;
				_isSnapTargetLocal = true;
				StartEntryTween(skipAnimation: false);
				if (base.isOwned)
				{
					playerService?.EquipmentManager?.OnGripPauseRequested?.Invoke();
					_heldItem.BeginSnapStream();
				}
			}
		}

		public void Unsnap()
		{
			if (_isSnapTargetLocal)
			{
				_isSnapTargetLocal = false;
				_pendingTarget = null;
				StartExitTween();
				if (base.isOwned)
				{
					playerService?.EquipmentManager?.OnGripResumeRequested?.Invoke();
				}
			}
		}

		private void OnHeldItemUnequipped()
		{
			if (_isSnapTargetLocal)
			{
				Unsnap();
			}
		}

		private void StartEntryTween(bool skipAnimation)
		{
			_weightTween.Stop();
			if (skipAnimation)
			{
				_entryWeight = 1f;
				_isAnimating = false;
				return;
			}
			_isAnimating = true;
			float duration = ((snapConfig != null) ? snapConfig.entryDuration : 0.25f);
			Ease ease = ((snapConfig != null) ? snapConfig.entryEase : Ease.OutCubic);
			_weightTween = Tween.Custom(this, _entryWeight, 1f, duration, delegate(LiquidContainerSnapHandler handler, float value)
			{
				handler._entryWeight = value;
			}, ease).OnComplete(this, delegate(LiquidContainerSnapHandler handler)
			{
				handler._isAnimating = false;
			});
		}

		private void StartExitTween()
		{
			_weightTween.Stop();
			_isAnimating = true;
			float duration = ((snapConfig != null) ? snapConfig.exitDuration : 0.2f);
			Ease ease = ((snapConfig != null) ? snapConfig.exitEase : Ease.InOutCubic);
			_weightTween = Tween.Custom(this, _entryWeight, 0f, duration, delegate(LiquidContainerSnapHandler handler, float value)
			{
				handler._entryWeight = value;
			}, ease).OnComplete(this, delegate(LiquidContainerSnapHandler handler)
			{
				handler._isAnimating = false;
				handler._heldItem?.EndSnapStream();
			});
		}

		private void LateUpdate()
		{
			if (!base.isOwned)
			{
				return;
			}
			bool num = (_isAnimating || _isSnapTargetLocal) && IsEquippedNow;
			ParentConstraint parentConstraint = ((_networkedTransform != null) ? _networkedTransform.ParentConstraint : null);
			if (!num)
			{
				if (_constraintSuspended && parentConstraint != null && parentConstraint.sourceCount > 0)
				{
					parentConstraint.constraintActive = true;
					_constraintSuspended = false;
				}
			}
			else
			{
				if (parentConstraint == null || parentConstraint.sourceCount == 0)
				{
					return;
				}
				Transform sourceTransform = parentConstraint.GetSource(0).sourceTransform;
				if (sourceTransform == null)
				{
					return;
				}
				Vector3 position = sourceTransform.position;
				Quaternion rotation = sourceTransform.rotation;
				Vector3 b = position;
				Quaternion b2 = rotation;
				float num2 = 0f;
				if (TryGetTargetWorldPose(out var position2, out var rotation2))
				{
					b = position2;
					b2 = rotation2;
					num2 = ((debugMode && _isSnapTargetLocal) ? 1f : _entryWeight);
				}
				base.transform.SetPositionAndRotation(Vector3.Lerp(position, b, num2), Quaternion.Slerp(rotation, b2, num2));
				if (num2 > 0f)
				{
					if (parentConstraint.constraintActive)
					{
						parentConstraint.constraintActive = false;
						_constraintSuspended = true;
					}
				}
				else
				{
					if (!parentConstraint.constraintActive)
					{
						parentConstraint.constraintActive = true;
					}
					_constraintSuspended = false;
				}
				_heldItem.StreamSnapPose(base.transform.position, base.transform.rotation);
			}
		}

		private bool TryGetTargetWorldPose(out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			ILiquidSnapTarget pendingTarget = _pendingTarget;
			if (pendingTarget?.FillAnchor == null)
			{
				return false;
			}
			LiquidSnapPose pose;
			if (debugMode && _isSnapTargetLocal)
			{
				pose = debugPose;
			}
			else if (snapConfig == null || !snapConfig.TryGetPose(_targetTypeLocal, out pose))
			{
				return false;
			}
			Transform fillAnchor = pendingTarget.FillAnchor;
			position = fillAnchor.TransformPoint(pose.positionOffset);
			rotation = fillAnchor.rotation * Quaternion.Euler(pose.rotationOffset);
			return true;
		}

		private void RestoreConstraint()
		{
			_constraintSuspended = false;
			ParentConstraint parentConstraint = ((_networkedTransform != null) ? _networkedTransform.ParentConstraint : null);
			if (parentConstraint != null && parentConstraint.sourceCount > 0)
			{
				parentConstraint.constraintActive = true;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
