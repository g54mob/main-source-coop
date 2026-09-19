using System;
using Fusion;
using UnityEngine;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public abstract class CharacterMovableBase : NetworkBehaviour
	{
		[SerializeField]
		public Transform CameraPositionTransform;

		[SerializeField]
		public Transform RotatePoint;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("MovementState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private MovementState _MovementState;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe MovementState MovementState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CharacterMovableBase.MovementState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(MovementState*)((byte*)Ptr + 0);
			}
			protected set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CharacterMovableBase.MovementState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(MovementState*)((byte*)Ptr + 0) = value;
			}
		}

		public bool IsAutomaticForwardMovement { get; set; }

		public abstract bool IsGrounded { get; }

		public abstract event Action OnChangePosition;

		public override void Spawned()
		{
			base.Spawned();
			OnSpawn();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			OnDespawn();
		}

		protected virtual void OnSpawn()
		{
		}

		protected virtual void OnDespawn()
		{
		}

		public abstract Vector3 GetVelocity();

		public abstract void DisableMovement();

		public abstract void EnableMovement();

		public abstract void MoveTowardsInput(Vector3 input, bool overrideSpeed = false);

		public abstract void MoveTowardsPosition(Vector3 position);

		public abstract void ChangePosition(Vector3 position, Quaternion? rotation, bool isForced);

		public abstract void Jump();

		public abstract void Crouch();

		public abstract void TryCrouchImmediately(bool isCrouching);

		public abstract void ForceCrouch(bool isCrouching);

		public abstract Vector3 GetPosition();

		public abstract void Warp(Vector3 position);

		public abstract bool IsMoving();

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			MovementState = _MovementState;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_MovementState = MovementState;
		}
	}
}
