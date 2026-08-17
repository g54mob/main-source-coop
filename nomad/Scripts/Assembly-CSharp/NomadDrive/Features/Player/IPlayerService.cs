using System;
using ECM2;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player.Animation;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public interface IPlayerService
	{
		Player LocalPlayer { get; }

		IEquipmentManager EquipmentManager { get; }

		IObjectPlacementManager ObjectPlacementManager { get; }

		IInteractionManager InteractionManager { get; }

		FirstPersonController FirstPersonController { get; }

		PlayerAnimationsManager PlayerAnimationsManager { get; }

		Character Character { get; }

		CharacterMovement CharacterMovement { get; }

		CapsuleCollider CapsuleCollider { get; }

		Transform CameraTransform { get; }

		PlayerStatsManager StatsManager { get; }

		bool IsPlayerSpawned { get; }

		event Action OnPlayerRegistered;

		event Action OnPlayerCleared;

		void Register(Player player);

		void Clear();

		bool TryGetFirstPersonController(out FirstPersonController controller);

		bool TryGetInteractionManager(out IInteractionManager manager);

		bool TryGetEquipmentManager(out IEquipmentManager manager);

		bool TryGetObjectPlacementManager(out IObjectPlacementManager manager);

		bool TryGetStatsManager(out PlayerStatsManager manager);

		bool TryGetCharacterMovement(out CharacterMovement movement);

		bool TryGetCameraTransform(out Transform cameraTransform);
	}
}
