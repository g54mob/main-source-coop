using System;
using Mimicraft.Cameras;
using Mimicraft.Customization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerMovement : NetworkBehaviour
	{
		private enum MovementSound
		{
			Footstep = 0,
			Jump = 1,
			DoubleJump = 2,
			Land = 3
		}

		private const float MoveSpeed = 4f;

		private const float RunMultiplier = 1.6f;

		private const float JumpHeight = 1.2f;

		private const float Gravity = -20f;

		private const float GroundedStickVelocity = -1f;

		public const float MaxHorizontalSpeed = 6.4f;

		private const int MaxAirJumps = 1;

		private const float CoyoteSeconds = 0.12f;

		private const float CrouchHeightScale = 0.55f;

		private const float CrouchSpeedMultiplier = 0.5f;

		[SerializeField]
		private PlayerCameraRig cameraRig;

		[SerializeField]
		private PlayerAnimator playerAnimator;

		private CharacterController controller;

		private int airJumpsUsed;

		private bool crouched;

		[Tooltip("Koşarken blend tree'ye giden girdinin çarpanı. Ağaç 1'i yürüme olarak okuyorsa 2 koşmayı 2'ye koyar - yani yeni bir parametre değil, aynı eksenlerin daha ilerisi. Yatay ve dikey aynı çarpanı alır, böylece çapraz koşu çapraz kalır.")]
		[SerializeField]
		[Min(1f)]
		private float runAnimationScale = 2f;

		private bool wasGrounded = true;

		private float standHeight;

		private Vector3 standCenter;

		private GameModeController roundManager;

		private PlayerHealth health;

		private PlayerWeapons weapons;

		private float verticalVelocity;

		private readonly WallClimbMover hiderMover = new WallClimbMover();

		[Tooltip("Which layers a Modelci may climb. Everything by default. Does NOT affect standing on the ground - an unclimbable surface is still solid, you just cannot stick to its wall.")]
		[SerializeField]
		private LayerMask climbableLayers = -1;

		private readonly NetworkVariable<bool> isArmed = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private readonly NetworkVariable<bool> isRunning = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private readonly NetworkVariable<bool> isWallClinging = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private readonly NetworkVariable<byte> wallGrabs = new NetworkVariable<byte>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private bool lockedOn;

		private readonly NetworkVariable<bool> isHolstered = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private bool couldHoldLastFrame;

		private string heldWeaponIdLastFrame = "";

		private bool crouchLatched;

		private bool crouchKeyWasDown;

		private float coyoteUntil;

		private const float RunFootstepIntervalScale = 0.65f;

		private AudioSource footstepSource;

		private PlayerCharacterAppearance appearance;

		private float lastFootstepTime = float.NegativeInfinity;

		private NetworkTransform networkTransform;

		private readonly Collider[] overlapHits = new Collider[16];

		private int playerLayers = -1;

		private PlayerEditSession editSession;

		private FirstPersonLook look;

		private const float UnexplainedJumpDistance = 5f;

		private const float UnexplainedJumpLogInterval = 1f;

		private const float StuckBuriedSeconds = 3f;

		private const float StuckAirborneSeconds = 8f;

		private const float StuckProbeInterval = 1f;

		private Vector3 lastTrackedPosition;

		private bool hasTrackedPosition;

		private float airborneSeconds;

		private float buriedSeconds;

		private float nextStuckProbeTime;

		private float nextJumpLogTime;

		private PlayerVoxelBody voxelBody;

		private const double UnstuckCooldownSeconds = 5.0;

		private double nextUnstuckAllowedServerTime;

		public bool IsRunning => isRunning.Value;

		public bool IsWallClingingNetworked => isWallClinging.Value;

		public byte WallGrabCount => wallGrabs.Value;

		public bool InputEnabled { get; set; } = true;

		public bool SpectatorFrozen { get; set; }

		public bool Frozen
		{
			get
			{
				if (InputEnabled && !SpectatorFrozen)
				{
					return RagdollFrozen;
				}
				return true;
			}
		}

		public bool RagdollFrozen { get; set; }

		public bool IsArmed => isArmed.Value;

		public bool IsHolstered => isHolstered.Value;

		public bool IsCompanion { get; set; }

		public bool IsWallClinging => hiderMover.IsClinging;

		private CharacterVoice VoiceType
		{
			get
			{
				if (appearance == null)
				{
					appearance = GetComponentInParent<PlayerCharacterAppearance>();
				}
				if (!(appearance != null))
				{
					return CharacterVoice.Male;
				}
				return appearance.VoiceType;
			}
		}

		private float JumpVelocity => Mathf.Sqrt(48f);

		private Vector3 FeetPosition => base.transform.position + controller.center - Vector3.up * (controller.height * 0.5f);

		private float CrouchCameraDrop
		{
			get
			{
				if (!crouched)
				{
					return 0f;
				}
				return (0f - standHeight) * 0.45f;
			}
		}

		public string ArmedStateReport { get; private set; } = "-";

		public bool IsSupported
		{
			get
			{
				if (!controller.isGrounded && !hiderMover.IsGrounded)
				{
					return hiderMover.IsClinging;
				}
				return true;
			}
		}

		private void SetWallClingingNetworked(bool value)
		{
			if (base.IsOwner && base.IsSpawned && isWallClinging.Value != value)
			{
				isWallClinging.Value = value;
			}
		}

		private void SetRunningNetworked(bool value)
		{
			if (base.IsOwner && base.IsSpawned && isRunning.Value != value)
			{
				isRunning.Value = value;
			}
		}

		public float RunLiftHeadroom(float wanted, float alreadyRisen)
		{
			return hiderMover.Headroom(controller, wanted, alreadyRisen);
		}

		public void ResetWallCling()
		{
			hiderMover.ResetOrientation();
		}

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
			hiderMover.ClimbableLayers = climbableLayers;
			if (playerAnimator == null)
			{
				playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			}
			health = GetComponent<PlayerHealth>();
			voxelBody = GetComponent<PlayerVoxelBody>();
			footstepSource = base.gameObject.AddComponent<AudioSource>();
			footstepSource.playOnAwake = false;
			controller.slopeLimit = 89f;
			standHeight = controller.height;
			standCenter = controller.center;
		}

		public void SetCameraRig(PlayerCameraRig cameraRig)
		{
			this.cameraRig = cameraRig;
		}

		private void StopController()
		{
			if (controller != null && controller.enabled)
			{
				controller.Move(Vector3.zero);
			}
			SetRunningNetworked(value: false);
			SetWallClingingNetworked(value: false);
			SetCrouched(wants: false);
			ReportCrouch(crouched);
			crouchLatched = false;
		}

		private bool WantsCrouch()
		{
			bool flag = (GameMenuState.InputCaptured ? crouched : GameInput.Crouch.IsPressed());
			if (!GameSettings.ToggleCrouch)
			{
				crouchLatched = false;
				crouchKeyWasDown = flag;
				return flag;
			}
			if (flag && !crouchKeyWasDown)
			{
				crouchLatched = !crouchLatched;
			}
			crouchKeyWasDown = flag;
			return crouchLatched;
		}

		public override void OnNetworkSpawn()
		{
			cameraRig.Initialize(base.IsOwner);
			SpatialAudio.Apply(footstepSource);
			if (base.IsOwner && GetComponent<PlayerViewMode>() == null)
			{
				base.gameObject.AddComponent<PlayerViewMode>();
			}
			PlayerEarAnchor playerEarAnchor = GetComponent<PlayerEarAnchor>();
			if (playerEarAnchor == null)
			{
				playerEarAnchor = base.gameObject.AddComponent<PlayerEarAnchor>();
			}
			playerEarAnchor.Initialize(cameraRig, base.IsOwner);
			if (base.IsServer)
			{
				GameModeController current = GameModeController.Current;
				if (current != null && current.TryGetLobbySpawn(out var position, out var rotation))
				{
					TeleportClientRpc(position, rotation);
				}
				else
				{
					TeleportClientRpc(new Vector3((float)base.OwnerClientId * 3f, 0f, 0f), Quaternion.identity);
					SpawnPlacementQueue.NoteUnplaced(base.OwnerClientId);
					Debug.Log($"[PlayerMovement] {base.OwnerClientId} icin spawn noktasi yoktu " + "(mod " + ((current == null) ? "henuz yok" : current.GetType().Name) + ") - varsayilan konuma alindi, sira bekliyor.");
				}
			}
			NetworkVariable<bool> networkVariable = isArmed;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnArmedChanged));
			OnArmedChanged(previous: false, isArmed.Value);
			if (!base.IsOwner)
			{
				base.enabled = false;
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<bool> networkVariable = isArmed;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnArmedChanged));
		}

		private void Update()
		{
			UpdateArmedState();
			if (!InputEnabled || SpectatorFrozen || RagdollFrozen)
			{
				ReportAnimatorInput(Vector2.zero);
				StopController();
				return;
			}
			bool flag = GameMenuState.InputCaptured || RoundCinematicDirector.IsPlaying;
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (roundManager == null)
			{
				ReportAnimatorInput(Vector2.zero);
				StopController();
				return;
			}
			bool flag2 = ((cameraRig != null) ? cameraRig.IsFirstPerson : roundManager.LocalPlayerUsesHunterMovement);
			bool flag3 = flag2 || !roundManager.LocalPlayerShouldHaveModel;
			bool flag4 = flag3 && !IsCompanion;
			if (roundManager.IsLocalPlayerFrozen)
			{
				ReportAnimatorInput(Vector2.zero);
				StopController();
				return;
			}
			Vector2 vector = (flag ? Vector2.zero : GameInput.Move.ReadValue<Vector2>());
			bool flag5 = !flag && GameInput.Run.IsPressed();
			bool flag6 = !flag && GameInput.Jump.WasPressedThisFrame();
			SetRunningNetworked(flag5 && vector.sqrMagnitude > 0.01f);
			SetWallClingingNetworked(hiderMover.IsClinging);
			Vector3 position = base.transform.position;
			lockedOn = false;
			bool flag7 = flag3 && !roundManager.LocalPlayerMayWallClimb;
			SetCrouched(flag7 && WantsCrouch());
			ReportCrouch(crouched);
			float num = (crouched ? 0.5f : 1f);
			if (flag2)
			{
				Vector3 forward = base.transform.forward;
				Vector3 right = base.transform.right;
				Vector3 vector2 = forward * vector.y + right * vector.x;
				if (vector2.sqrMagnitude > 1f)
				{
					vector2.Normalize();
				}
				float num2 = 4f * (flag5 ? 1.6f : 1f) * num;
				if (controller.isGrounded)
				{
					if (!wasGrounded)
					{
						EmitMovementSound(MovementSound.Land);
					}
					airJumpsUsed = 0;
					coyoteUntil = Time.unscaledTime + 0.12f;
					verticalVelocity = -1f;
					if (flag6 && !crouched)
					{
						PerformGroundJump();
					}
				}
				else if (flag6 && !crouched && Time.unscaledTime <= coyoteUntil)
				{
					PerformGroundJump();
				}
				else if (flag6 && airJumpsUsed < 1 && !crouched)
				{
					airJumpsUsed++;
					verticalVelocity = JumpVelocity;
					EmitMovementSound(MovementSound.DoubleJump);
					ReportJump();
					if (base.IsOwner && base.IsSpawned)
					{
						DoubleJumpEffectServerRpc(FeetPosition);
					}
				}
				else
				{
					verticalVelocity += -20f * Time.deltaTime;
				}
				Vector3 vector3 = vector2 * num2 + Vector3.up * verticalVelocity;
				controller.Move(vector3 * Time.deltaTime);
				wasGrounded = controller.isGrounded;
			}
			else
			{
				Transform tpsCameraTransform = cameraRig.TpsCameraTransform;
				Vector3 rawCamForward = ((tpsCameraTransform != null) ? tpsCameraTransform.forward : base.transform.forward);
				Vector3 rawCamRight = ((tpsCameraTransform != null) ? tpsCameraTransform.right : base.transform.right);
				lockedOn = !flag && tpsCameraTransform != null && cameraRig != null && cameraRig.IsOverShoulder;
				hiderMover.FacingDirection = (lockedOn ? Vector3.ProjectOnPlane(tpsCameraTransform.forward, Vector3.up) : Vector3.zero);
				bool localPlayerMayWallClimb = roundManager.LocalPlayerMayWallClimb;
				bool climbHeld = (flag ? hiderMover.IsClinging : GameInput.Climb.IsPressed());
				hiderMover.BodyBoundsLocal = ((voxelBody != null && voxelBody.TryGetLiveBoundsLocal(out var bounds)) ? new Bounds?(bounds) : ((Bounds?)null));
				hiderMover.Tick(controller, rawCamForward, rawCamRight, vector, flag6, flag5, climbHeld, Time.deltaTime, localPlayerMayWallClimb, flag3 ? 1 : 0, flag3 ? 0.12f : 0f, num);
				if (flag4)
				{
					ReportCharacterMovementFeedback();
				}
				if (base.IsOwner && base.IsSpawned)
				{
					if (hiderMover.GrabbedThisTick)
					{
						wallGrabs.Value += 1;
						ClingEffectServerRpc(hiderMover.SurfacePoint, hiderMover.SurfaceNormal, attached: true, ContactRadius(hiderMover.BodyBoundsLocal));
					}
					else if (hiderMover.ReleasedThisTick)
					{
						ClingEffectServerRpc(hiderMover.SurfacePoint, hiderMover.SurfaceNormal, attached: false, ContactRadius(hiderMover.BodyBoundsLocal));
					}
				}
			}
			Vector2 input = ((flag2 || hiderMover.IsClinging || lockedOn) ? vector : new Vector2(0f, Mathf.Clamp01(vector.magnitude)));
			if (flag5 && !crouched && vector.sqrMagnitude > 0.01f)
			{
				input *= runAnimationScale;
			}
			ReportAnimatorInput(input);
			bool flag8 = (flag2 ? controller.isGrounded : hiderMover.IsGrounded);
			bool flag9 = !IsCompanion && (flag3 || flag5);
			TickFootsteps(vector.sqrMagnitude > 0.01f && flag8 && !hiderMover.IsClinging && flag9, flag5);
			KeepInsidePlayArea(position);
		}

		private void KeepInsidePlayArea(Vector3 positionBeforeMove)
		{
			Vector3 vector = base.transform.TransformVector(controller.center);
			if (!MapBounds.IsInsidePlayArea(base.transform.position + vector) && MapBounds.IsInsidePlayArea(positionBeforeMove + vector))
			{
				controller.enabled = false;
				base.transform.position = positionBeforeMove;
				controller.enabled = true;
				NotifyRepositioned();
			}
		}

		private void PerformGroundJump()
		{
			verticalVelocity = JumpVelocity;
			coyoteUntil = 0f;
			EmitMovementSound(MovementSound.Jump);
			ReportJump();
		}

		private void ReportCharacterMovementFeedback()
		{
			if (hiderMover.LandedThisTick)
			{
				EmitMovementSound(MovementSound.Land);
			}
			if (hiderMover.JumpedThisTick)
			{
				EmitMovementSound(MovementSound.Jump);
				ReportJump();
			}
			else if (hiderMover.AirJumpedThisTick)
			{
				EmitMovementSound(MovementSound.DoubleJump);
				ReportJump();
				if (base.IsOwner && base.IsSpawned)
				{
					DoubleJumpEffectServerRpc(FeetPosition);
				}
			}
		}

		private void TickFootsteps(bool stepping, bool running)
		{
			if (stepping && !crouched)
			{
				float num = AudioLibrary.FootstepInterval * (running ? 0.65f : 1f);
				if (!(Time.unscaledTime - lastFootstepTime < num))
				{
					lastFootstepTime = Time.unscaledTime;
					EmitMovementSound(MovementSound.Footstep);
				}
			}
		}

		private void EmitMovementSound(MovementSound kind)
		{
			PlayMovementSoundHere(kind);
			if (base.IsSpawned)
			{
				MovementSoundServerRpc(kind);
			}
		}

		private void PlayMovementSoundHere(MovementSound kind)
		{
			if (footstepSource == null)
			{
				return;
			}
			AudioClip audioClip = kind switch
			{
				MovementSound.Jump => AudioLibrary.NextJumpEffortClip(VoiceType), 
				MovementSound.DoubleJump => AudioLibrary.NextDoubleJumpClip(VoiceType), 
				MovementSound.Land => AudioLibrary.NextLandClip(), 
				_ => AudioLibrary.NextFootstepClip(), 
			};
			if (!(audioClip == null))
			{
				if (kind == MovementSound.Jump || kind == MovementSound.DoubleJump)
				{
					PlayerEffortVoice.Play(this, audioClip);
				}
				else
				{
					footstepSource.PlayOneShot(audioClip, AudioLibrary.VolumeOf(audioClip));
				}
			}
		}

		[Rpc(SendTo.Server, Delivery = RpcDelivery.Unreliable)]
		private void MovementSoundServerRpc(MovementSound kind)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					Delivery = RpcDelivery.Unreliable
				};
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(4125333906u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
				bufferWriter.WriteValueSafe(in kind, default(FastBufferWriter.ForEnums));
				__endSendRpc(ref bufferWriter, 4125333906u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				MovementSoundClientRpc(kind);
			}
		}

		[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable)]
		private void MovementSoundClientRpc(MovementSound kind)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					Delivery = RpcDelivery.Unreliable
				};
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(1950140745u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Unreliable);
				bufferWriter.WriteValueSafe(in kind, default(FastBufferWriter.ForEnums));
				__endSendRpc(ref bufferWriter, 1950140745u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Unreliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (!base.IsOwner)
				{
					PlayMovementSoundHere(kind);
				}
			}
		}

		private void SetCrouched(bool wants)
		{
			if (wants != crouched && (wants || HasHeadroom()))
			{
				crouched = wants;
				float num = (wants ? (standHeight * 0.55f) : standHeight);
				Vector3 center = standCenter;
				center.y = standCenter.y - (standHeight - num) * 0.5f;
				controller.height = num;
				controller.center = center;
			}
		}

		private bool HasHeadroom()
		{
			if (standHeight - controller.height <= 0.001f)
			{
				return true;
			}
			float num = Mathf.Max(0.01f, controller.radius - 0.02f);
			float y = controller.center.y - controller.height * 0.5f;
			Vector3 vector = base.transform.position + new Vector3(controller.center.x, y, controller.center.z);
			Collider[] array = Physics.OverlapCapsule(vector + Vector3.up * num, vector + Vector3.up * (standHeight - num), num, -1, QueryTriggerInteraction.Ignore);
			foreach (Collider collider in array)
			{
				if (collider != null && !collider.transform.IsChildOf(base.transform))
				{
					return false;
				}
			}
			return true;
		}

		private void ReportJump()
		{
			if (base.IsOwner && playerAnimator != null)
			{
				playerAnimator.TriggerJump();
			}
		}

		private void ReportCrouch(bool value)
		{
			if (base.IsOwner)
			{
				if (playerAnimator != null)
				{
					playerAnimator.SetCrouch(value);
				}
				if (cameraRig != null)
				{
					cameraRig.SetCrouchDrop(CrouchCameraDrop);
				}
			}
		}

		[ServerRpc]
		private void DoubleJumpEffectServerRpc(Vector3 position)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(1801560207u, serverRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				__endSendServerRpc(ref bufferWriter, 1801560207u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				DoubleJumpEffectClientRpc(position);
			}
		}

		[ClientRpc]
		private void DoubleJumpEffectClientRpc(Vector3 position)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(2320224781u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				__endSendClientRpc(ref bufferWriter, 2320224781u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnDoubleJumpEffect(position);
			}
		}

		[ServerRpc]
		private void ClingEffectServerRpc(Vector3 position, Vector3 normal, bool attached, float radius)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(2785546657u, serverRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				bufferWriter.WriteValueSafe(in normal);
				bufferWriter.WriteValueSafe(in attached, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in radius, default(FastBufferWriter.ForPrimitives));
				__endSendServerRpc(ref bufferWriter, 2785546657u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ClingEffectClientRpc(position, normal, attached, radius);
			}
		}

		[ClientRpc]
		private void ClingEffectClientRpc(Vector3 position, Vector3 normal, bool attached, float radius)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(1153958278u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				bufferWriter.WriteValueSafe(in normal);
				bufferWriter.WriteValueSafe(in attached, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in radius, default(FastBufferWriter.ForPrimitives));
				__endSendClientRpc(ref bufferWriter, 1153958278u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnClingEffect(position, normal, attached, radius);
			}
		}

		private static float ContactRadius(Bounds? body)
		{
			if (body.HasValue)
			{
				Bounds valueOrDefault = body.GetValueOrDefault();
				return (valueOrDefault.extents.x + valueOrDefault.extents.y) * 0.5f;
			}
			return 0f;
		}

		private void ReportAnimatorInput(Vector2 input)
		{
			if (base.IsOwner && playerAnimator != null)
			{
				playerAnimator.SetInput(input);
			}
		}

		private void UpdateArmedState()
		{
			if (base.IsOwner)
			{
				if (roundManager == null)
				{
					roundManager = GameModeController.Current;
				}
				if (weapons == null)
				{
					weapons = GetComponent<PlayerWeapons>();
				}
				bool flag = roundManager != null && roundManager.IsLocalPlayerArmed;
				bool flag2 = health != null && health.Health.Value > 0;
				bool flag3 = roundManager != null && roundManager.CurrentPhase.Value != RoundPhase.WaitingForPlayers && roundManager.ModeUsesWeapons;
				bool flag4 = flag3 && weapons != null && weapons.Equipped != null;
				bool flag5 = flag2 && (flag || flag4);
				string text = ((weapons != null && weapons.Equipped != null) ? weapons.Equipped.WeaponId : "");
				bool flag6 = isHolstered.Value;
				if (!flag5 || !couldHoldLastFrame || text != heldWeaponIdLastFrame)
				{
					flag6 = false;
				}
				if (flag5 && !Frozen && !GameMenuState.InputCaptured && GameInput.Holster.WasPressedThisFrame())
				{
					flag6 = !flag6;
				}
				couldHoldLastFrame = flag5;
				heldWeaponIdLastFrame = text;
				if (isHolstered.Value != flag6)
				{
					isHolstered.Value = flag6;
				}
				isArmed.Value = flag5 && !flag6;
				ArmedStateReport = $"alive={flag2}, isHunter={flag}, roundRunning={flag3}, holstered={flag6}" + ", faz=" + ((roundManager != null) ? roundManager.CurrentPhase.Value.ToString() : "mod yok") + ", modSilahli=" + ((roundManager != null) ? roundManager.ModeUsesWeapons.ToString() : "?");
			}
		}

		private void OnArmedChanged(bool previous, bool current)
		{
			if (playerAnimator != null)
			{
				playerAnimator.SetArmed(current);
			}
		}

		[ClientRpc]
		public void TeleportClientRpc(Vector3 position, Quaternion rotation)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(1976332429u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				bufferWriter.WriteValueSafe(in rotation);
				__endSendClientRpc(ref bufferWriter, 1976332429u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (base.IsOwner)
				{
					TeleportLocal(position, rotation);
				}
			}
		}

		public void TeleportLocal(Vector3 position, Quaternion rotation)
		{
			controller.enabled = false;
			base.transform.SetPositionAndRotation(position, Upright(rotation));
			controller.enabled = true;
			ResolveOverlaps();
			MarkTeleportForRemotes();
			NotifyTeleported();
		}

		private void MarkTeleportForRemotes()
		{
			if (networkTransform == null)
			{
				networkTransform = GetComponent<NetworkTransform>();
			}
			if (!(networkTransform == null) && networkTransform.IsSpawned && networkTransform.CanCommitToTransform)
			{
				controller.enabled = false;
				networkTransform.Teleport(base.transform.position, base.transform.rotation, base.transform.localScale);
				controller.enabled = true;
			}
		}

		public void ResolveOverlaps()
		{
			if (controller == null || !controller.enabled)
			{
				return;
			}
			if (playerLayers < 0)
			{
				playerLayers = LayerMask.GetMask("PlayerController", "VoxelBody");
			}
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = base.transform.TransformPoint(controller.center);
				float radius = controller.radius;
				float num = Mathf.Max(0f, controller.height * 0.5f - radius);
				int num2 = Physics.OverlapCapsuleNonAlloc(vector + Vector3.up * num, vector - Vector3.up * num, radius, overlapHits, ~playerLayers, QueryTriggerInteraction.Ignore);
				bool flag = false;
				for (int j = 0; j < num2; j++)
				{
					Collider collider = overlapHits[j];
					if (!(collider == null) && !collider.transform.IsChildOf(base.transform) && Physics.ComputePenetration(controller, base.transform.position, base.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance) && !(distance <= 0.001f))
					{
						controller.enabled = false;
						base.transform.position += direction * (distance + 0.01f);
						controller.enabled = true;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
			}
		}

		private static Quaternion Upright(Quaternion rotation)
		{
			Vector3 vector = Vector3.ProjectOnPlane(rotation * Vector3.forward, Vector3.up);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(rotation * Vector3.up, Vector3.up);
			}
			if (!(vector.sqrMagnitude < 0.0001f))
			{
				return Quaternion.LookRotation(vector.normalized, Vector3.up);
			}
			return Quaternion.identity;
		}

		public void NotifyTeleported()
		{
			hiderMover.ResetOrientation();
			NotifyRepositioned();
			if (look == null)
			{
				look = GetComponentInChildren<FirstPersonLook>(includeInactive: true);
			}
			if (look != null)
			{
				look.ClearAimPunch();
			}
			if (editSession == null)
			{
				editSession = GetComponent<PlayerEditSession>();
			}
			if (editSession != null)
			{
				editSession.RefocusEditorCamera();
			}
		}

		public void NotifyRepositioned()
		{
			verticalVelocity = -1f;
			coyoteUntil = 0f;
			hiderMover.ResetFall();
			lastTrackedPosition = base.transform.position;
			hasTrackedPosition = true;
			airborneSeconds = 0f;
			buriedSeconds = 0f;
		}

		private void LateUpdate()
		{
			if (base.IsSpawned && base.IsOwner)
			{
				TrackUnexplainedJump();
				TrackStuck();
			}
		}

		private void TrackUnexplainedJump()
		{
			Vector3 position = base.transform.position;
			if (!hasTrackedPosition)
			{
				hasTrackedPosition = true;
				lastTrackedPosition = position;
				return;
			}
			float num = Vector3.Distance(position, lastTrackedPosition);
			Vector3 vector = lastTrackedPosition;
			lastTrackedPosition = position;
			if (!(num < 5f) && !(Time.unscaledTime < nextJumpLogTime))
			{
				nextJumpLogTime = Time.unscaledTime + 1f;
				Debug.LogWarning($"[PlayerMovement] Aciklanamayan sicrama: {num:F1} m, {vector} -> {position}. " + $"clinging={hiderMover.IsClinging} grounded={hiderMover.IsGrounded} " + $"vy={verticalVelocity:F1} gomulu={voxelBody != null && voxelBody.IsBuried}", this);
			}
		}

		private void TrackStuck()
		{
			if (!InputEnabled || SpectatorFrozen || RagdollFrozen || GameMenuState.InputCaptured)
			{
				airborneSeconds = 0f;
				buriedSeconds = 0f;
				return;
			}
			bool flag = controller.isGrounded || hiderMover.IsGrounded || hiderMover.IsClinging;
			airborneSeconds = (flag ? 0f : (airborneSeconds + Time.deltaTime));
			if (Time.unscaledTime >= nextStuckProbeTime)
			{
				nextStuckProbeTime = Time.unscaledTime + 1f;
				bool flag2 = voxelBody != null && voxelBody.IsBuried;
				buriedSeconds = (flag2 ? (buriedSeconds + 1f) : 0f);
				if (flag2 && voxelBody.TryUnbury())
				{
					buriedSeconds = 0f;
					NotifyRepositioned();
				}
			}
			if (!(buriedSeconds < 3f) || !(airborneSeconds < 8f))
			{
				Debug.LogWarning($"[PlayerMovement] Sikismis oyuncu kurtariliyor - gomulu {buriedSeconds:F0}s, " + $"havada {airborneSeconds:F0}s, konum {base.transform.position}.", this);
				airborneSeconds = 0f;
				buriedSeconds = 0f;
				RequestUnstuckServerRpc();
			}
		}

		[ServerRpc]
		private void RequestUnstuckServerRpc()
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(1178513141u, serverRpcParams, RpcDelivery.Reliable);
				__endSendServerRpc(ref bufferWriter, 1178513141u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsServer && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(base.NetworkManager.ServerTime.Time < nextUnstuckAllowedServerTime))
			{
				nextUnstuckAllowedServerTime = base.NetworkManager.ServerTime.Time + 5.0;
				GameModeController current = GameModeController.Current;
				if (current != null && current.TryGetRescueSpawn(base.OwnerClientId, out var position, out var rotation))
				{
					TeleportClientRpc(position, rotation);
				}
			}
		}

		protected override void __initializeVariables()
		{
			if (isArmed == null)
			{
				throw new Exception("PlayerMovement.isArmed cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isArmed.Initialize(this);
			__nameNetworkVariable(isArmed, "isArmed");
			NetworkVariableFields.Add(isArmed);
			if (isRunning == null)
			{
				throw new Exception("PlayerMovement.isRunning cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isRunning.Initialize(this);
			__nameNetworkVariable(isRunning, "isRunning");
			NetworkVariableFields.Add(isRunning);
			if (isWallClinging == null)
			{
				throw new Exception("PlayerMovement.isWallClinging cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isWallClinging.Initialize(this);
			__nameNetworkVariable(isWallClinging, "isWallClinging");
			NetworkVariableFields.Add(isWallClinging);
			if (wallGrabs == null)
			{
				throw new Exception("PlayerMovement.wallGrabs cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			wallGrabs.Initialize(this);
			__nameNetworkVariable(wallGrabs, "wallGrabs");
			NetworkVariableFields.Add(wallGrabs);
			if (isHolstered == null)
			{
				throw new Exception("PlayerMovement.isHolstered cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isHolstered.Initialize(this);
			__nameNetworkVariable(isHolstered, "isHolstered");
			NetworkVariableFields.Add(isHolstered);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(4125333906u, __rpc_handler_4125333906, "MovementSoundServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1950140745u, __rpc_handler_1950140745, "MovementSoundClientRpc", RpcInvokePermission.Everyone);
			__registerRpc(1801560207u, __rpc_handler_1801560207, "DoubleJumpEffectServerRpc", RpcInvokePermission.Owner);
			__registerRpc(2320224781u, __rpc_handler_2320224781, "DoubleJumpEffectClientRpc", RpcInvokePermission.Server);
			__registerRpc(2785546657u, __rpc_handler_2785546657, "ClingEffectServerRpc", RpcInvokePermission.Owner);
			__registerRpc(1153958278u, __rpc_handler_1153958278, "ClingEffectClientRpc", RpcInvokePermission.Server);
			__registerRpc(1976332429u, __rpc_handler_1976332429, "TeleportClientRpc", RpcInvokePermission.Server);
			__registerRpc(1178513141u, __rpc_handler_1178513141, "RequestUnstuckServerRpc", RpcInvokePermission.Owner);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_4125333906(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out MovementSound value, default(FastBufferWriter.ForEnums));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).MovementSoundServerRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1950140745(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out MovementSound value, default(FastBufferWriter.ForEnums));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).MovementSoundClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1801560207(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
			}
			else
			{
				reader.ReadValueSafe(out Vector3 value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).DoubleJumpEffectServerRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2320224781(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).DoubleJumpEffectClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2785546657(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
				return;
			}
			reader.ReadValueSafe(out Vector3 value);
			reader.ReadValueSafe(out Vector3 value2);
			reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
			reader.ReadValueSafe(out float value4, default(FastBufferWriter.ForPrimitives));
			target.__rpc_exec_stage = __RpcExecStage.Execute;
			((PlayerMovement)target).ClingEffectServerRpc(value, value2, value3, value4);
			target.__rpc_exec_stage = __RpcExecStage.Send;
		}

		private static void __rpc_handler_1153958278(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Vector3 value2);
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe(out float value4, default(FastBufferWriter.ForPrimitives));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).ClingEffectClientRpc(value, value2, value3, value4);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1976332429(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Quaternion value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).TeleportClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1178513141(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
			}
			else
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerMovement)target).RequestUnstuckServerRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerMovement";
		}
	}
}
