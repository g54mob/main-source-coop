using UnityEngine;

namespace Features.Movement.Scripts
{
	public abstract class CharacterMovableMonoBase : MonoBehaviour
	{
		[SerializeField]
		public Transform CameraPositionTransform;

		public bool IsAutomaticForwardMovement { get; set; }

		public abstract Vector3 GetVelocity();

		public abstract void DisableMovement();

		public abstract void EnableMovement();

		public abstract void MoveTowardsInput(Vector3 input, bool overrideSpeed = false);

		public abstract void MoveTowardsPosition(Vector3 position);

		public abstract void ChangePosition(Vector3 position);

		public abstract void Jump();

		public abstract void Crouch();

		public abstract void ForceCrouch(bool isCrouching);

		public abstract Vector3 GetPosition();

		public abstract void Warp(Vector3 position);

		public abstract bool IsMoving();
	}
}
