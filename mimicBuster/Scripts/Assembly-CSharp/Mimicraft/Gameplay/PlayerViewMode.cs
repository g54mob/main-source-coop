using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.VoxelEditor;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerViewMode : MonoBehaviour
	{
		private PlayerMovement movement;

		private ThirdPersonCamera thirdPersonCamera;

		private SpectatorFreeCam freeCam;

		private SpectatorController spectator;

		private NetworkObject networkObject;

		public bool IsActive { get; private set; }

		private void Update()
		{
			if (!IsOwner() || GameMenuState.IsMenuOpen || GameMenuState.InputCaptured)
			{
				return;
			}
			if (!Allowed())
			{
				if (IsActive)
				{
					Exit();
				}
			}
			else if (GameInput.ViewMode.WasPressedThisFrame())
			{
				if (IsActive)
				{
					Exit();
				}
				else
				{
					Enter();
				}
			}
			else if (IsActive && GameInput.EditorToggle.WasPressedThisFrame())
			{
				Exit();
			}
		}

		private bool Allowed()
		{
			GameModeController current = GameModeController.Current;
			if (current == null || !current.LocalPlayerShouldHaveModel || !current.IsLocalPlayerParticipating)
			{
				return false;
			}
			if (!VoxelEditorSettings.IsMovementMode)
			{
				return false;
			}
			if (spectator == null)
			{
				spectator = GetComponent<SpectatorController>();
			}
			if (spectator != null && spectator.IsSpectating)
			{
				return false;
			}
			return Resolve();
		}

		private bool IsOwner()
		{
			if (networkObject == null)
			{
				networkObject = GetComponent<NetworkObject>();
			}
			if (networkObject != null)
			{
				return networkObject.IsOwner;
			}
			return false;
		}

		private bool Resolve()
		{
			if (movement == null)
			{
				movement = GetComponent<PlayerMovement>();
			}
			if (thirdPersonCamera == null)
			{
				thirdPersonCamera = GetComponentInChildren<ThirdPersonCamera>(includeInactive: true);
			}
			if (thirdPersonCamera == null)
			{
				return false;
			}
			if (freeCam == null)
			{
				freeCam = thirdPersonCamera.GetComponent<SpectatorFreeCam>();
				if (freeCam == null)
				{
					freeCam = thirdPersonCamera.gameObject.AddComponent<SpectatorFreeCam>();
				}
				freeCam.enabled = false;
			}
			return true;
		}

		private void Enter()
		{
			if (!IsActive && Resolve())
			{
				IsActive = true;
				if (movement != null)
				{
					movement.SpectatorFrozen = true;
				}
				freeCam.Solid = true;
				freeCam.AdoptCurrentPose();
				thirdPersonCamera.enabled = false;
				freeCam.enabled = true;
			}
		}

		private void Exit()
		{
			if (IsActive)
			{
				IsActive = false;
				if (freeCam != null)
				{
					freeCam.enabled = false;
				}
				if (thirdPersonCamera != null)
				{
					thirdPersonCamera.enabled = true;
				}
				if (movement != null)
				{
					movement.SpectatorFrozen = false;
				}
			}
		}

		private void OnDisable()
		{
			Exit();
		}
	}
}
