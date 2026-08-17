using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class InteractableRail : Interactable
	{
		[Header("Rail Settings")]
		[SerializeField]
		private Transform connectedTransform;

		[SerializeField]
		private Vector3 railDirection;

		[SerializeField]
		private float railDistance;

		[Header("Animation Settings")]
		[SerializeField]
		private float openingDuration = 0.5f;

		[SerializeField]
		private float closingDuration = 0.5f;

		[SerializeField]
		private AnimationEaseConfig openingEaseConfig = new AnimationEaseConfig();

		[SerializeField]
		private AnimationEaseConfig closingEaseConfig = new AnimationEaseConfig();

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openingSound;

		[SerializeField]
		private SoundID closingSound;

		private Vector3 _initialPosition;

		private Vector3 _targetPosition;

		private Sequence _currentSequence;

		[SyncVar(hook = "OnIsOpenChanged")]
		private bool _isOpen;

		private InteractionStateMachine<RailState> _stateMachine;

		private Action _onAnimateComplete;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOpen;

		public UnityEvent OnOpened { get; } = new UnityEvent();

		public UnityEvent OnClosed { get; } = new UnityEvent();

		public bool IsOpen
		{
			get
			{
				return _isOpen;
			}
			private set
			{
				Network_isOpen = value;
			}
		}

		protected override bool UseStateMachine => true;

		public bool Network_isOpen
		{
			get
			{
				return _isOpen;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isOpen, 64uL, _Mirror_SyncVarHookDelegate__isOpen);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<RailState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(RailState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", Open).WithCrosshair(CrosshairType.Interact)).RegisterState(RailState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Close).WithCrosshair(CrosshairType.Interact));
		}

		private RailState DetermineState()
		{
			if (!_isOpen)
			{
				return RailState.Closed;
			}
			return RailState.Opened;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		protected override void Awake()
		{
			base.Awake();
			InitializeRail();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SetForLateJoiners();
		}

		private void SetForLateJoiners()
		{
			if (IsOpen)
			{
				base.transform.localPosition = _targetPosition;
			}
			else
			{
				base.transform.localPosition = _initialPosition;
			}
		}

		private void OnDestroy()
		{
			_currentSequence.Stop();
		}

		private void InitializeRail()
		{
			_initialPosition = base.transform.localPosition;
			_targetPosition = _initialPosition + railDirection * railDistance;
		}

		public void SetDurations(float openingDuration, float closingDuration)
		{
			this.openingDuration = openingDuration;
			this.closingDuration = closingDuration;
		}

		public void SetSounds(SoundID openingSound, SoundID closingSound)
		{
			this.openingSound = openingSound;
			this.closingSound = closingSound;
		}

		private void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					HandleOpening();
				}
				else
				{
					HandleClosing();
				}
			}
		}

		private void HandleOpening()
		{
			PlaySoundNetworked(openingSound);
			OnOpened.Invoke();
			AnimateRail(_targetPosition, openingDuration, openingEaseConfig, delegate
			{
				UpdateState();
			});
		}

		private void HandleClosing()
		{
			PlaySoundNetworked(closingSound);
			AnimateRail(_initialPosition, closingDuration, closingEaseConfig, delegate
			{
				OnClosed.Invoke();
				UpdateState();
			});
		}

		private void AnimateRail(Vector3 targetPos, float duration, AnimationEaseConfig easeConfig, Action onComplete)
		{
			SetInteractionAvailability(newValue: false);
			_onAnimateComplete = onComplete;
			_currentSequence.Stop();
			_currentSequence = Sequence.Create().Chain(easeConfig.CreateLocalPositionTween(base.transform, targetPos, duration)).ChainCallback(this, delegate(InteractableRail target)
			{
				target.SetInteractionAvailability(newValue: true);
				target._onAnimateComplete?.Invoke();
			});
		}

		private void PlaySoundNetworked(SoundID sound)
		{
			if (sound.IsValid() && base.isServer)
			{
				NetworkAudioRelay?.PlayOneShot(sound, base.transform.position);
			}
		}

		public void Open()
		{
			if (!IsOpen)
			{
				CmdOpen();
			}
		}

		public void Close()
		{
			if (IsOpen)
			{
				CmdClose();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdOpen()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableRail::CmdOpen()", -274336257, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdClose()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableRail::CmdClose()", 418649651, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public InteractableRail()
		{
			_Mirror_SyncVarHookDelegate__isOpen = OnIsOpenChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdOpen()
		{
			IsOpen = true;
		}

		protected static void InvokeUserCode_CmdOpen(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdOpen called on client.");
			}
			else
			{
				((InteractableRail)obj).UserCode_CmdOpen();
			}
		}

		protected void UserCode_CmdClose()
		{
			IsOpen = false;
		}

		protected static void InvokeUserCode_CmdClose(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdClose called on client.");
			}
			else
			{
				((InteractableRail)obj).UserCode_CmdClose();
			}
		}

		static InteractableRail()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableRail), "System.Void NomadDrive.Features.Interaction.InteractableRail::CmdOpen()", InvokeUserCode_CmdOpen, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableRail), "System.Void NomadDrive.Features.Interaction.InteractableRail::CmdClose()", InvokeUserCode_CmdClose, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isOpen);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteBool(_isOpen);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
			}
		}
	}
}
