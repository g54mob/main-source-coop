using EvilCore.Networking;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public interface IFirstPersonController : IUniqueNetworkComponent
	{
		FirstPersonControllerSettings FirstPersonControllerSettings { get; set; }

		Transform HeadFollowTransform { get; set; }

		bool CanMove { get; set; }

		bool CanRotate { get; set; }

		bool CanLook { get; set; }

		bool CanZoom { get; set; }

		bool CanJump { get; set; }

		bool CanCrouch { get; set; }

		bool CanSprint { get; set; }

		CapsuleCollider GetPlayerCapsuleCollider();

		void Init();

		void OnDisable();

		void SetFPSControllerAttributes();

		void OnCrouchKeyPressed();

		void EnableMovement();

		void DisableMovement();

		void EnableLook();

		void DisableLook();

		void EnableZoom();

		void DisableZoom();

		void EnableCrouch();

		void DisableCrouch();

		void EnableJump();

		void DisableJump();

		void EnableSprint();

		void DisableSprint();

		void SetMovementInputs();

		void SetLookInputs();

		void SetZoomInputs();

		void SetCrouchInputs();

		void SetJumpInputs();

		void SetSprintInputs();
	}
}
