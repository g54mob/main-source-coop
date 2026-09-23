using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class SpectatorController : NetworkBehaviour
	{
		private static readonly Vector3 SpectateEyeOffset = new Vector3(0f, 1.6f, 0f);

		[SerializeField]
		private PlayerMovement movement;

		[SerializeField]
		private ThirdPersonCamera thirdPersonCamera;

		private GameModeController roundManager;

		private bool subscribed;

		private PlayerRole lastActiveRole;

		private bool deathScreenPending;

		private bool companionPending;

		private float companionRequestDeadline;

		private const float CompanionRequestTimeoutSeconds = 5f;

		private PlayerCompanion companion;

		private readonly List<NetworkObject> spectatable = new List<NetworkObject>();

		private int spectateIndex;

		private SpectatorFreeCam freeCam;

		private PlayerEarAnchor ears;

		private PlayerVoxelBody ownBody;

		private bool IsCompanion
		{
			get
			{
				if (companion == null)
				{
					companion = GetComponent<PlayerCompanion>();
				}
				if (companion != null)
				{
					return companion.IsCompanion;
				}
				return false;
			}
		}

		public bool IsSpectating { get; private set; }

		public bool IsFreeCam { get; private set; }

		public NetworkObject CurrentTarget
		{
			get
			{
				if (IsSpectating && !IsFreeCam && spectateIndex >= 0 && spectateIndex < spectatable.Count)
				{
					return spectatable[spectateIndex];
				}
				return null;
			}
		}

		public void SetReferences(PlayerMovement movement, ThirdPersonCamera thirdPersonCamera)
		{
			this.movement = movement;
			this.thirdPersonCamera = thirdPersonCamera;
		}

		private SpectatorFreeCam ResolveFreeCam()
		{
			if (freeCam != null || thirdPersonCamera == null)
			{
				return freeCam;
			}
			freeCam = thirdPersonCamera.GetComponent<SpectatorFreeCam>();
			if (freeCam == null)
			{
				freeCam = thirdPersonCamera.gameObject.AddComponent<SpectatorFreeCam>();
			}
			freeCam.enabled = false;
			return freeCam;
		}

		private void Update()
		{
			if (!base.IsOwner)
			{
				return;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
				if (roundManager == null)
				{
					return;
				}
			}
			if (!subscribed)
			{
				subscribed = true;
				roundManager.OnLocalEliminated += HandleEliminated;
				roundManager.OnRoundRestarted += StopSpectating;
			}
			if (roundManager.IsLocalPlayerParticipating)
			{
				lastActiveRole = roundManager.LocalRole;
				if (IsSpectating)
				{
					StopSpectating();
				}
			}
			if (IsCompanion)
			{
				companionPending = false;
				if (IsSpectating)
				{
					StopSpectating();
				}
			}
			else if (companionPending && Time.unscaledTime >= companionRequestDeadline)
			{
				Debug.LogWarning("[SpectatorController] Yoldas istegine cevap gelmedi - izleyici moduna geciliyor.");
				companionPending = false;
			}
			if (!IsSpectating && !deathScreenPending && !companionPending && !IsCompanion && !roundManager.IsLocalPlayerParticipating)
			{
				FreezeOwnCharacter();
				StartSpectating();
			}
			if (IsSpectating)
			{
				UpdateSpectateInput();
				ListenWhereWatching();
			}
		}

		private void ListenWhereWatching()
		{
			if (ears == null)
			{
				ears = GetComponent<PlayerEarAnchor>();
			}
			if (!(ears == null))
			{
				NetworkObject currentTarget = CurrentTarget;
				if (currentTarget != null)
				{
					ears.ListenFrom(currentTarget.transform, atHeadHeight: true);
				}
				else if (IsFreeCam && thirdPersonCamera != null)
				{
					ears.ListenFrom(thirdPersonCamera.transform, atHeadHeight: false);
				}
				else
				{
					ears.ListenFrom(null, atHeadHeight: false);
				}
			}
		}

		public override void OnNetworkDespawn()
		{
			if (subscribed && !(roundManager == null))
			{
				roundManager.OnLocalEliminated -= HandleEliminated;
				roundManager.OnRoundRestarted -= StopSpectating;
				subscribed = false;
			}
		}

		private void HandleEliminated()
		{
			deathScreenPending = true;
			FreezeOwnCharacter();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			string subtitle = ((roundManager != null) ? roundManager.LocalDeathSubtitle(lastActiveRole) : "");
			if (DeathScreenView.Instance != null)
			{
				DeathScreenView.Instance.Show(subtitle, AfterDeathScreen);
			}
			else
			{
				AfterDeathScreen();
			}
		}

		private void AfterDeathScreen()
		{
			if (roundManager == null || !roundManager.OffersCompanionChoice || CompanionChoiceView.Instance == null)
			{
				StartSpectating();
			}
			else
			{
				CompanionChoiceView.Instance.Show(BecomeCompanion, StartSpectating);
			}
		}

		private void BecomeCompanion()
		{
			deathScreenPending = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (roundManager is CompanionRoundManager companionRoundManager)
			{
				companionPending = true;
				companionRequestDeadline = Time.unscaledTime + 5f;
				companionRoundManager.RequestBecomeCompanionServerRpc(CompanionRoundManager.CompanionSide.Hunters);
			}
			else
			{
				Debug.LogWarning("[SpectatorController] Yoldas secimi sunuldu ama mod bunu karsilamiyor - izleyici moduna geciliyor.");
				StartSpectating();
			}
		}

		private void StartSpectating()
		{
			deathScreenPending = false;
			IsSpectating = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			SetOwnBodyHidden(hidden: true);
			RefreshSpectatable();
			spectateIndex = 0;
			ApplySpectateTarget();
		}

		private void StopSpectating()
		{
			if (DeathScreenView.Instance != null)
			{
				DeathScreenView.Instance.Cancel();
			}
			if (CompanionChoiceView.Instance != null)
			{
				CompanionChoiceView.Instance.Cancel();
			}
			SetFreeCam(active: false);
			deathScreenPending = false;
			companionPending = false;
			SetOwnBodyHidden(hidden: false);
			if (ears != null)
			{
				ears.ListenFrom(null, atHeadHeight: false);
			}
			if (!IsSpectating)
			{
				Unfreeze();
				return;
			}
			IsSpectating = false;
			spectatable.Clear();
			if (thirdPersonCamera != null)
			{
				thirdPersonCamera.Target = base.transform;
				thirdPersonCamera.PivotOffsetLocal = SpectateEyeOffset;
			}
			Unfreeze();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void SetOwnBodyHidden(bool hidden)
		{
			if (ownBody == null)
			{
				ownBody = GetComponent<PlayerVoxelBody>();
			}
			if (ownBody != null)
			{
				ownBody.SetOwnerSpectating(hidden);
			}
		}

		private void FreezeOwnCharacter()
		{
			if (!(movement == null))
			{
				movement.SpectatorFrozen = true;
				movement.ResetWallCling();
			}
		}

		private void Unfreeze()
		{
			if (movement != null)
			{
				movement.SpectatorFrozen = false;
			}
		}

		private void SetFreeCam(bool active)
		{
			if (IsFreeCam == active)
			{
				return;
			}
			SpectatorFreeCam spectatorFreeCam = ResolveFreeCam();
			if (spectatorFreeCam == null)
			{
				return;
			}
			IsFreeCam = active;
			if (active)
			{
				spectatorFreeCam.Solid = false;
				spectatorFreeCam.AdoptCurrentPose();
				if (thirdPersonCamera != null)
				{
					thirdPersonCamera.enabled = false;
				}
				spectatorFreeCam.enabled = true;
			}
			else
			{
				spectatorFreeCam.enabled = false;
				if (thirdPersonCamera != null)
				{
					thirdPersonCamera.enabled = true;
				}
				RefreshSpectatable();
				ApplySpectateTarget();
			}
		}

		private void UpdateSpectateInput()
		{
			if (GameMenuState.IsMenuOpen)
			{
				return;
			}
			if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
			{
				SetFreeCam(!IsFreeCam);
				return;
			}
			if (IsFreeCam)
			{
				if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
				{
					SetFreeCam(active: false);
				}
				return;
			}
			bool flag = (Keyboard.current != null && Keyboard.current.dKey.wasPressedThisFrame) || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
			bool flag2 = (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame) || (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame);
			RefreshSpectatable();
			if (spectatable.Count == 0)
			{
				if (thirdPersonCamera != null && thirdPersonCamera.Target != base.transform)
				{
					thirdPersonCamera.Target = base.transform;
					thirdPersonCamera.PivotOffsetLocal = SpectateEyeOffset;
				}
				return;
			}
			if (flag)
			{
				spectateIndex++;
			}
			if (flag2)
			{
				spectateIndex--;
			}
			spectateIndex = (spectateIndex % spectatable.Count + spectatable.Count) % spectatable.Count;
			ApplySpectateTarget();
		}

		private void RefreshSpectatable()
		{
			NetworkObject networkObject = ((spectateIndex >= 0 && spectateIndex < spectatable.Count) ? spectatable[spectateIndex] : null);
			spectatable.Clear();
			if (NetworkManager.Singleton == null || NetworkManager.Singleton.SpawnManager == null)
			{
				return;
			}
			IReadOnlyList<NetworkObject> playerObjects = NetworkManager.Singleton.SpawnManager.PlayerObjects;
			for (int i = 0; i < playerObjects.Count; i++)
			{
				NetworkObject networkObject2 = playerObjects[i];
				if (!(networkObject2 == null) && networkObject2.OwnerClientId != base.OwnerClientId)
				{
					PlayerVoxelBody component = networkObject2.GetComponent<PlayerVoxelBody>();
					if (!(component != null) || !component.IsLocallyConcealed)
					{
						spectatable.Add(networkObject2);
					}
				}
			}
			if (!(networkObject == null))
			{
				int num = spectatable.IndexOf(networkObject);
				spectateIndex = ((num >= 0) ? num : 0);
			}
		}

		private void ApplySpectateTarget()
		{
			if (!(thirdPersonCamera == null) && spectateIndex >= 0 && spectateIndex < spectatable.Count)
			{
				NetworkObject networkObject = spectatable[spectateIndex];
				if (!(networkObject == null))
				{
					thirdPersonCamera.Target = networkObject.transform;
					thirdPersonCamera.PivotOffsetLocal = SpectateEyeOffset;
				}
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "SpectatorController";
		}
	}
}
