using System;
using Ami.BroAudio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Locking
{
	public class Lock : Interactable
	{
		[Header("Attached Hinge (source of truth)")]
		[SerializeField]
		private InteractableHinge attachedHinge;

		[Header("Top Piece (slides up + rotates before whole-lock fall)")]
		[SerializeField]
		private Transform topPiece;

		[SerializeField]
		private Vector3 slideUpOffset = new Vector3(0f, 0.1f, 0f);

		[SerializeField]
		private float slideDuration = 0.3f;

		[SerializeField]
		private Vector3 rotateEuler = new Vector3(0f, 90f, 30f);

		[SerializeField]
		private float rotateDuration = 0.2f;

		[Header("Whole-Lock Fall")]
		[SerializeField]
		private float despawnAfterFallSeconds = 3f;

		[SerializeField]
		private float fallTorqueImpulse = 1.5f;

		[Header("Locked-Attempt Shake")]
		[SerializeField]
		private Transform shakeTarget;

		[SerializeField]
		private float shakeStrength = 0.015f;

		[SerializeField]
		private float shakeDuration = 0.25f;

		[SerializeField]
		private float shakeFrequency = 18f;

		[Header("Audio")]
		[Tooltip("Unlock basariyla tamamlandiginda calan ses (PlayUnlockingAnimation tum clientlerde tetiklenir).")]
		[SerializeField]
		private SoundID unlockSound;

		[Tooltip("Uyumsuz key veya key olmadan unlock denemesi sirasinda calan ses (PlayLockedShake tum clientlerde tetiklenir).")]
		[SerializeField]
		private SoundID unlockFailedSound;

		[Header("Unlock Interaction")]
		[SerializeField]
		private float unlockHoldDuration = 0.75f;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private INetworkManager _networkManager;

		private Chest _ownerChest;

		private InteractionStateMachine<LockState> _stateMachine;

		private Sequence _openSequence;

		private Tween _shakeTween;

		private bool _hasRunOpening;

		private bool _isSubscribedToEquipment;

		public ChestType LockType
		{
			get
			{
				if (!(_ownerChest != null))
				{
					return ChestType.Common;
				}
				return _ownerChest.ChestType;
			}
		}

		protected override bool UseStateMachine => true;

		public static event Action<Lock> OnAnyChestUnlocked;

		protected override void Awake()
		{
			base.Awake();
			_ownerChest = GetComponentInParent<Chest>();
			if (_ownerChest == null)
			{
				EvilLogger.LogError("[Lock] " + base.name + ": parent Chest not found — Lock must be a child of a Chest GameObject for tier matching to work", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\Lock.cs", 74);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<LockState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(LockState.Sealed, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Primary, "@interaction.unlock", HandleUnlock, unlockHoldDuration).WithCondition(IsHoldingMatchingKey).WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(LockState.WrongKeyHovered, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Invalid).WithNameLabelVisibility(visible: true).WithInteractionLabelVisibility(visible: false)).RegisterState(LockState.Removed, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default).WithNameLabelVisibility(visible: false).WithInteractionLabelVisibility(visible: false));
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		private LockState DetermineState()
		{
			if (attachedHinge == null || !attachedHinge.IsLocked)
			{
				return LockState.Removed;
			}
			if (_playerService?.EquipmentManager?.EquippedEntity is Key key && key.KeyType != LockType)
			{
				return LockState.WrongKeyHovered;
			}
			return LockState.Sealed;
		}

		protected override void Start()
		{
			base.Start();
			if (attachedHinge != null)
			{
				attachedHinge.OnLocked.AddListener(UpdateState);
				attachedHinge.OnUnlocked.AddListener(UpdateState);
			}
			TrySubscribeToEquipment();
			if (!_isSubscribedToEquipment && _playerService != null)
			{
				_playerService.OnPlayerRegistered += TrySubscribeToEquipment;
			}
			UpdateState();
		}

		private void OnDestroy()
		{
			_openSequence.Stop();
			_shakeTween.Stop();
			if (attachedHinge != null)
			{
				attachedHinge.OnLocked.RemoveListener(UpdateState);
				attachedHinge.OnUnlocked.RemoveListener(UpdateState);
			}
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= TrySubscribeToEquipment;
				if (_isSubscribedToEquipment && _playerService.EquipmentManager != null)
				{
					_playerService.EquipmentManager.OnItemEquipped.RemoveListener(UpdateState);
					_playerService.EquipmentManager.OnItemUnequipped.RemoveListener(UpdateState);
				}
			}
		}

		private void TrySubscribeToEquipment()
		{
			if (!_isSubscribedToEquipment && _playerService?.EquipmentManager != null)
			{
				_playerService.EquipmentManager.OnItemEquipped.AddListener(UpdateState);
				_playerService.EquipmentManager.OnItemUnequipped.AddListener(UpdateState);
				_isSubscribedToEquipment = true;
				UpdateState();
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (attachedHinge != null && !attachedHinge.IsLocked)
			{
				IgnoreHovering();
				base.gameObject.SetActive(value: false);
			}
			UpdateState();
		}

		private bool IsHoldingMatchingKey()
		{
			if (_playerService?.EquipmentManager?.EquippedEntity is Key key)
			{
				return key.KeyType == LockType;
			}
			return false;
		}

		private void HandleUnlock()
		{
			if (!(attachedHinge == null) && attachedHinge.IsLocked)
			{
				Key key = _playerService?.EquipmentManager?.EquippedEntity as Key;
				if (!(key == null) && key.KeyType == LockType)
				{
					uint keyNetId = key.netId;
					_playerService.EquipmentManager.Consume();
					CmdUnlock(keyNetId);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdUnlock(uint keyNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(keyNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Locking.Lock::CmdUnlock(System.UInt32)", -2104457167, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void PlayLockedShake()
		{
			if (!_shakeTween.isAlive)
			{
				if (unlockFailedSound.IsValid())
				{
					AudioManager?.PlayOneShot(unlockFailedSound, base.transform.position);
				}
				if (!(shakeStrength <= 0f) && !(shakeDuration <= 0f))
				{
					Transform target = ((shakeTarget != null) ? shakeTarget : base.transform);
					_shakeTween = Tween.ShakeLocalPosition(target, Vector3.one * shakeStrength, shakeDuration, shakeFrequency);
				}
			}
		}

		public void PlayUnlockingAnimation()
		{
			if (_hasRunOpening)
			{
				return;
			}
			_hasRunOpening = true;
			IgnoreHovering();
			Lock.OnAnyChestUnlocked?.Invoke(this);
			if (unlockSound.IsValid())
			{
				AudioManager?.PlayOneShot(unlockSound, base.transform.position);
			}
			if (topPiece == null)
			{
				DropWholeLock();
				return;
			}
			Vector3 localPosition = topPiece.localPosition;
			Quaternion localRotation = topPiece.localRotation;
			Vector3 endValue = localPosition + slideUpOffset;
			Quaternion endValue2 = localRotation * Quaternion.Euler(rotateEuler);
			_openSequence.Stop();
			_openSequence = Sequence.Create().Chain(Tween.LocalPosition(topPiece, endValue, slideDuration, Ease.OutQuad)).Chain(Tween.LocalRotation(topPiece, endValue2, rotateDuration, Ease.InQuad))
				.ChainCallback(this, delegate(Lock target)
				{
					target.DropWholeLock();
				});
		}

		private void DropWholeLock()
		{
			base.transform.SetParent(null, worldPositionStays: true);
			if (!TryGetComponent<Rigidbody>(out var component))
			{
				component = base.gameObject.AddComponent<Rigidbody>();
			}
			component.isKinematic = false;
			component.useGravity = true;
			component.AddTorque(UnityEngine.Random.insideUnitSphere * fallTorqueImpulse, ForceMode.Impulse);
			if (base.isServer)
			{
				ServerOpenTransformStreamingWindow(despawnAfterFallSeconds);
			}
			Invoke("HideSelf", despawnAfterFallSeconds);
		}

		private void HideSelf()
		{
			base.gameObject.SetActive(value: false);
		}

		public void ServerSnapRemoved()
		{
			_hasRunOpening = true;
			_openSequence.Stop();
			IgnoreHovering();
			base.gameObject.SetActive(value: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdUnlock__UInt32(uint keyNetId)
		{
			if (!(attachedHinge == null) && attachedHinge.IsLocked)
			{
				Key component;
				if (!_networkManager.TryGetNetworkObjectById(keyNetId, out var networkObject))
				{
					EvilLogger.LogError($"[Lock] CmdUnlock: Could not find Key {keyNetId}", "CmdUnlock", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\Lock.cs", 211);
				}
				else if (!networkObject.TryGetComponent<Key>(out component))
				{
					EvilLogger.LogError($"[Lock] CmdUnlock: Object {keyNetId} is not a Key", "CmdUnlock", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\Lock.cs", 217);
				}
				else if (component.KeyType == LockType)
				{
					NetworkServer.Destroy(networkObject);
					attachedHinge.ServerSetLocked(locked: false);
				}
			}
		}

		protected static void InvokeUserCode_CmdUnlock__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUnlock called on client.");
			}
			else
			{
				((Lock)obj).UserCode_CmdUnlock__UInt32(reader.ReadVarUInt());
			}
		}

		static Lock()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Lock), "System.Void NomadDrive.Features.Locking.Lock::CmdUnlock(System.UInt32)", InvokeUserCode_CmdUnlock__UInt32, requiresAuthority: false);
		}
	}
}
