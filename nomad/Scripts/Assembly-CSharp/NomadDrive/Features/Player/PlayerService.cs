using System;
using ECM2;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player.Animation;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public class PlayerService : IPlayerService
	{
		public Player LocalPlayer { get; private set; }

		public IEquipmentManager EquipmentManager { get; private set; }

		public IObjectPlacementManager ObjectPlacementManager { get; private set; }

		public IInteractionManager InteractionManager { get; private set; }

		public Character Character { get; private set; }

		public CharacterMovement CharacterMovement { get; private set; }

		public CapsuleCollider CapsuleCollider { get; private set; }

		public FirstPersonController FirstPersonController { get; private set; }

		public PlayerAnimationsManager PlayerAnimationsManager { get; private set; }

		public Transform CameraTransform { get; private set; }

		public PlayerStatsManager StatsManager { get; private set; }

		public bool IsPlayerSpawned => LocalPlayer != null;

		public event Action OnPlayerRegistered;

		public event Action OnPlayerCleared;

		public void Register(Player player)
		{
			if (player == null)
			{
				EvilLogger.LogError("[PlayerService] Cannot register null player!", "Register", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\PlayerService.cs", 46);
				return;
			}
			if (IsPlayerSpawned)
			{
				Clear();
			}
			LocalPlayer = player;
			EquipmentManager = GetComponentSafe<IEquipmentManager>(player, "IEquipmentManager");
			ObjectPlacementManager = GetComponentSafe<IObjectPlacementManager>(player, "IObjectPlacementManager");
			InteractionManager = GetComponentSafe<IInteractionManager>(player, "IInteractionManager");
			FirstPersonController = GetComponentSafe<FirstPersonController>(player, "FirstPersonController");
			PlayerAnimationsManager = GetComponentSafe<PlayerAnimationsManager>(player, "PlayerAnimationsManager");
			Character = GetComponentSafe<Character>(player, "Character");
			CharacterMovement = GetComponentSafe<CharacterMovement>(player, "CharacterMovement");
			StatsManager = GetComponentSafe<PlayerStatsManager>(player, "PlayerStatsManager");
			CapsuleCollider = player.GetComponentInChildren<CapsuleCollider>();
			_ = CapsuleCollider == null;
			CameraTransform = FirstPersonController?.cameraTransform;
			_ = CameraTransform == null;
			Action action = this.OnPlayerRegistered;
			if (action == null)
			{
				return;
			}
			Delegate[] invocationList = action.GetInvocationList();
			foreach (Delegate obj in invocationList)
			{
				try
				{
					((Action)obj)();
				}
				catch (Exception arg)
				{
					EvilLogger.LogError($"[PlayerService] OnPlayerRegistered subscriber threw: {arg}", "Register", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\PlayerService.cs", 94);
				}
			}
		}

		public void Clear()
		{
			LocalPlayer = null;
			EquipmentManager = null;
			ObjectPlacementManager = null;
			InteractionManager = null;
			FirstPersonController = null;
			PlayerAnimationsManager = null;
			Character = null;
			CharacterMovement = null;
			CapsuleCollider = null;
			CameraTransform = null;
			StatsManager = null;
			this.OnPlayerCleared?.Invoke();
		}

		public bool TryGetFirstPersonController(out FirstPersonController controller)
		{
			controller = FirstPersonController;
			return controller != null;
		}

		public bool TryGetInteractionManager(out IInteractionManager manager)
		{
			manager = InteractionManager;
			return manager != null;
		}

		public bool TryGetEquipmentManager(out IEquipmentManager manager)
		{
			manager = EquipmentManager;
			return manager != null;
		}

		public bool TryGetObjectPlacementManager(out IObjectPlacementManager manager)
		{
			manager = ObjectPlacementManager;
			return manager != null;
		}

		public bool TryGetStatsManager(out PlayerStatsManager manager)
		{
			manager = StatsManager;
			return manager != null;
		}

		public bool TryGetCharacterMovement(out CharacterMovement movement)
		{
			movement = CharacterMovement;
			return movement != null;
		}

		public bool TryGetCameraTransform(out Transform cameraTransform)
		{
			cameraTransform = CameraTransform;
			return cameraTransform != null;
		}

		private T GetComponentSafe<T>(Player player, string componentName) where T : class
		{
			T component = player.GetComponent<T>();
			_ = component;
			return component;
		}
	}
}
