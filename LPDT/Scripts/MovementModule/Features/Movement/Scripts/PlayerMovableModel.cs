using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.Movement.Scripts
{
	public class PlayerMovableModel
	{
		private Vector3 _currentMovementInput;

		private readonly HashSet<LockMovementReasonEnum> _lockMovementReasons = new HashSet<LockMovementReasonEnum>();

		private readonly HashSet<NoRotationReasonEnum> _noRotationReasons = new HashSet<NoRotationReasonEnum>();

		private readonly HashSet<OnlyHeadRotationReasonEnum> _onlyHeadRotationReasons = new HashSet<OnlyHeadRotationReasonEnum>();

		private readonly HashSet<JumpBlockReasonEnum> _jumpBlockReasons = new HashSet<JumpBlockReasonEnum>();

		public Dictionary<PlayerRef, PlayerCharacterMovableBase> AllCharacterMovables = new Dictionary<PlayerRef, PlayerCharacterMovableBase>();

		public readonly Dictionary<PlayerRef, PlayerPhysics> AllMovablePhysics = new Dictionary<PlayerRef, PlayerPhysics>();

		public PlayerRotationMode RotationMode { get; private set; } = PlayerRotationMode.HeadAndBodyRotation;

		public bool IsFlying { get; private set; }

		public bool IsLookingAtCamera { get; set; }

		public IReadOnlyCollection<LockMovementReasonEnum> LockMovementReasons => _lockMovementReasons;

		public PlayerCharacterMovableBase LocalMovable { get; set; }

		public CharacterMovableMonoBase FreeFlyMovable { get; set; }

		public PlayerRotator Rotator { get; set; }

		public PlayerPhysics MovablePhysics { get; set; }

		public PlayerLookDetection PlayerLookDetection { get; set; }

		public Vector2 Velocity { get; internal set; }

		public Vector3 CurrentMovementInput
		{
			get
			{
				return _currentMovementInput;
			}
			set
			{
				_currentMovementInput = value;
				this.OnChangedRotationAngle?.Invoke();
			}
		}

		public NetworkedCompositeAnimator NetworkedAnimator { get; set; }

		public bool IsJumpBlocked => _jumpBlockReasons.Count > 0;

		public event Action<PlayerRef> AllCharacterMovablesPlayerAdded;

		public event Action OnChangedRotationAngle;

		public void AddCharacterMovable(PlayerRef playerRef, PlayerCharacterMovableBase movable)
		{
			AllCharacterMovables[playerRef] = movable;
			this.AllCharacterMovablesPlayerAdded?.Invoke(playerRef);
		}

		public void RemoveCharacterMovable(PlayerRef playerRef)
		{
			PurgePlayer(playerRef);
		}

		public void PurgePlayer(PlayerRef playerRef)
		{
			if (AllCharacterMovables.TryGetValue(playerRef, out var value) && (object)LocalMovable == value)
			{
				LocalMovable = null;
				NetworkedAnimator = null;
			}
			AllCharacterMovables.Remove(playerRef);
			AllMovablePhysics.Remove(playerRef);
		}

		public void AddNoRotationReason(NoRotationReasonEnum reason)
		{
			if (reason != NoRotationReasonEnum.None && _noRotationReasons.Add(reason))
			{
				ApplyRotationMode();
			}
		}

		public void RemoveNoRotationReason(NoRotationReasonEnum reason)
		{
			if (reason != NoRotationReasonEnum.None && _noRotationReasons.Remove(reason))
			{
				ApplyRotationMode();
			}
		}

		public bool HasNoRotationReason(NoRotationReasonEnum reason)
		{
			return _noRotationReasons.Contains(reason);
		}

		public void AddOnlyHeadRotationReason(OnlyHeadRotationReasonEnum reason)
		{
			if (reason != OnlyHeadRotationReasonEnum.None && _onlyHeadRotationReasons.Add(reason))
			{
				ApplyRotationMode();
			}
		}

		public void RemoveOnlyHeadRotationReason(OnlyHeadRotationReasonEnum reason)
		{
			if (reason != OnlyHeadRotationReasonEnum.None && _onlyHeadRotationReasons.Remove(reason))
			{
				ApplyRotationMode();
			}
		}

		public bool HasOnlyHeadRotationReason(OnlyHeadRotationReasonEnum reason)
		{
			return _onlyHeadRotationReasons.Contains(reason);
		}

		private void ApplyRotationMode()
		{
			if (_noRotationReasons.Count > 0)
			{
				RotationMode = PlayerRotationMode.NoRotation;
			}
			else if (_onlyHeadRotationReasons.Count > 0)
			{
				RotationMode = PlayerRotationMode.OnlyHeadRotation;
			}
			else
			{
				RotationMode = PlayerRotationMode.HeadAndBodyRotation;
			}
		}

		public void AddJumpBlockReason(JumpBlockReasonEnum reason)
		{
			if (reason != JumpBlockReasonEnum.None)
			{
				_jumpBlockReasons.Add(reason);
			}
		}

		public void RemoveJumpBlockReason(JumpBlockReasonEnum reason)
		{
			if (reason != JumpBlockReasonEnum.None)
			{
				_jumpBlockReasons.Remove(reason);
			}
		}

		public void SetIsFlying(bool isFlying)
		{
			IsFlying = isFlying;
		}

		public void AddLockMovementReason(LockMovementReasonEnum reason)
		{
			if (reason != LockMovementReasonEnum.None && _lockMovementReasons.Add(reason))
			{
				ApplyLockMovementReasons();
			}
		}

		public void RemoveLockMovementReason(LockMovementReasonEnum reason)
		{
			if (reason != LockMovementReasonEnum.None && _lockMovementReasons.Remove(reason))
			{
				ApplyLockMovementReasons();
			}
		}

		public bool HasLockMovementReason(LockMovementReasonEnum reason)
		{
			return _lockMovementReasons.Contains(reason);
		}

		private void ApplyLockMovementReasons()
		{
			if (LocalMovable == null)
			{
				return;
			}
			if (_lockMovementReasons.Count > 0)
			{
				LocalMovable.DisableMovement();
				LocalMovable.IsAutomaticForwardMovement = false;
				IsLookingAtCamera = false;
				AddNoRotationReason(NoRotationReasonEnum.LockMovement);
				if (_lockMovementReasons.Contains(LockMovementReasonEnum.FreeFly))
				{
					FreeFlyMovable.EnableMovement();
				}
				else
				{
					FreeFlyMovable.DisableMovement();
				}
			}
			else
			{
				LocalMovable.EnableMovement();
				FreeFlyMovable.DisableMovement();
				RemoveNoRotationReason(NoRotationReasonEnum.LockMovement);
			}
		}
	}
}
