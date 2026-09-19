using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CrocodileGameModule.Scripts
{
	[NetworkBehaviourWeaved(5)]
	public class CrocodileGameBehaviour : NetworkBehaviour
	{
		private static readonly int IsMouthOpenHash = Animator.StringToHash("IsMouthOpen");

		private static readonly int BiteHash = Animator.StringToHash("Bite");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private CrocodileGameAnimationFunctionReactor _animationFunctionReactor;

		[SerializeField]
		private CrocodileToothInteractable[] _teeth = Array.Empty<CrocodileToothInteractable>();

		[SerializeField]
		private EventReference _biteSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private CrocodileGameConfiguration _configuration;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		private float _biteDamage = 25f;

		private int _badTeethCount = 1;

		private int _pressedTeethMaskRenderCache = -1;

		private CrocodileGamePhase _phaseRenderCache;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Phase", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private CrocodileGamePhase _Phase;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PressedTeethMask", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PressedTeethMask;

		[WeaverGenerated]
		[DefaultForProperty("BadTeethMask", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BadTeethMask;

		[WeaverGenerated]
		[DefaultForProperty("BitingPlayer", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerRef _BitingPlayer;

		[WeaverGenerated]
		[DefaultForProperty("BiteDamageApplied", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _BiteDamageApplied;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe CrocodileGamePhase Phase
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((CrocodileGamePhase*)Ptr)[0];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				((CrocodileGamePhase*)Ptr)[0] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int PressedTeethMask
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.PressedTeethMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.PressedTeethMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int BadTeethMask
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BadTeethMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BadTeethMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe PlayerRef BitingPlayer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BitingPlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerRef*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BitingPlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerRef*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe bool BiteDamageApplied
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BiteDamageApplied. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrocodileGameBehaviour.BiteDamageApplied. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		public CrocodileGameConfiguration Configuration => _configuration;

		public event Action<int, PlayerRef> OnSafeToothPressed;

		public event Action<int, PlayerRef> OnBadToothPressed;

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, CrocodileGameConfiguration configuration, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
			ApplyConfiguration(configuration);
		}

		private void ApplyConfiguration(CrocodileGameConfiguration configuration)
		{
			_configuration = configuration;
			if (_configuration == null)
			{
				_biteDamage = 25f;
				_badTeethCount = 1;
			}
			else
			{
				_biteDamage = _configuration.BiteDamage;
				_badTeethCount = Mathf.Max(1, _configuration.BadTeethCount);
			}
		}

		public bool CanPressTooth(int toothIndex)
		{
			if (Phase == CrocodileGamePhase.Open && GetTooth(toothIndex) != null)
			{
				return !IsToothPressed(PressedTeethMask, toothIndex);
			}
			return false;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_teeth.Length == 0)
			{
				_teeth = GetComponentsInChildren<CrocodileToothInteractable>(includeInactive: true);
			}
			CrocodileToothInteractable[] teeth = _teeth;
			for (int i = 0; i < teeth.Length; i++)
			{
				teeth[i].ToothView?.Initialize();
			}
			if (base.HasStateAuthority)
			{
				Phase = CrocodileGamePhase.Open;
				StartNewRound(0);
			}
			_animationFunctionReactor.OnBiteDamage += HandleBiteDamage;
			_animationFunctionReactor.OnBiteFinished += HandleBiteFinished;
			_pressedTeethMaskRenderCache = PressedTeethMask;
			_phaseRenderCache = Phase;
			ApplyPressedTeethVisuals(0, PressedTeethMask);
			if (Phase == CrocodileGamePhase.Biting)
			{
				SetAllTeethGrabBlocked(blocked: true);
			}
			SetMouthOpen(Phase == CrocodileGamePhase.Open);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_animationFunctionReactor.OnBiteDamage -= HandleBiteDamage;
			_animationFunctionReactor.OnBiteFinished -= HandleBiteFinished;
		}

		public override void Render()
		{
			base.Render();
			if (_phaseRenderCache != Phase)
			{
				_phaseRenderCache = Phase;
				if (Phase == CrocodileGamePhase.Biting)
				{
					SetAllTeethGrabBlocked(blocked: true);
				}
				else if (Phase == CrocodileGamePhase.Open && PressedTeethMask == 0)
				{
					SetAllTeethGrabBlocked(blocked: false);
				}
			}
			if (_pressedTeethMaskRenderCache != PressedTeethMask)
			{
				int previousMask = ((_pressedTeethMaskRenderCache >= 0) ? _pressedTeethMaskRenderCache : 0);
				_pressedTeethMaskRenderCache = PressedTeethMask;
				ApplyPressedTeethVisuals(previousMask, PressedTeethMask);
			}
		}

		public void RequestPressTooth(int toothIndex, PlayerRef player)
		{
			if (base.Object.IsValid)
			{
				PressToothRpc(toothIndex, player);
			}
		}

		public void SetMouthOpen(bool isOpen)
		{
			_animator.SetBool(IsMouthOpenHash, isOpen);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3821423067u)]
		private void PressToothRpc([RpcPayload(4)] int toothIndex, [RpcPayload(4)] PlayerRef player)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3821423067u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::PressToothRpc(System.Int32,Fusion.PlayerRef)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(toothIndex, 4);
						writer.Write(player, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (Phase == CrocodileGamePhase.Open && IsValidToothIndex(toothIndex) && !IsToothPressed(PressedTeethMask, toothIndex))
			{
				PressedTeethMask = SetToothPressed(PressedTeethMask, toothIndex, isPressed: true);
				ApplyToothPressRpc(toothIndex, isPressed: true);
				if (IsBadTooth(toothIndex))
				{
					NotifyBadToothPressedRpc(toothIndex, player);
					BitingPlayer = player;
					Phase = CrocodileGamePhase.Biting;
					SetAllTeethGrabBlocked(blocked: true);
					StartBiteSequence();
				}
				else
				{
					NotifySafeToothPressedRpc(toothIndex, player);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2212218194u)]
		private void ApplyToothPressRpc([RpcPayload(4)] int toothIndex, [RpcPayload(4)] bool isPressed)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetPayloadSize(isPressed);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2212218194u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::ApplyToothPressRpc(System.Int32,System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(toothIndex, 4);
						writer.Write(isPressed);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CrocodileToothInteractable tooth = GetTooth(toothIndex);
			if (!(tooth == null))
			{
				tooth.ApplyPressState(isPressed, base.Object.HasStateAuthority);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 170633648u)]
		private void NotifySafeToothPressedRpc([RpcPayload(4)] int toothIndex, [RpcPayload(4)] PlayerRef player)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(170633648u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::NotifySafeToothPressedRpc(System.Int32,Fusion.PlayerRef)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(toothIndex, 4);
						writer.Write(player, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnSafeToothPressed?.Invoke(toothIndex, player);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4290867746u)]
		private void NotifyBadToothPressedRpc([RpcPayload(4)] int toothIndex, [RpcPayload(4)] PlayerRef player)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4290867746u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::NotifyBadToothPressedRpc(System.Int32,Fusion.PlayerRef)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(toothIndex, 4);
						writer.Write(player, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnBadToothPressed?.Invoke(toothIndex, player);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2258412761u)]
		private void PlayBiteAnimationRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2258412761u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::PlayBiteAnimationRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SetMouthOpen(isOpen: false);
			_animator.SetTrigger(BiteHash);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2892329155u)]
		private void SetMouthOpenRpc([RpcPayload(4)] bool isOpen)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isOpen);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2892329155u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CrocodileGameModule.Scripts.CrocodileGameBehaviour::SetMouthOpenRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isOpen);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SetMouthOpen(isOpen);
		}

		private void StartBiteSequence()
		{
			BiteDamageApplied = false;
			PlayBiteAnimationRpc();
		}

		private void HandleBiteDamage()
		{
			PlayOneShot(_biteSound);
			if (base.HasStateAuthority && Phase == CrocodileGamePhase.Biting && !BiteDamageApplied)
			{
				BiteDamageApplied = true;
				ApplyBiteDamage(BitingPlayer);
			}
		}

		private void HandleBiteFinished()
		{
			if (base.HasStateAuthority && Phase == CrocodileGamePhase.Biting)
			{
				CompleteBiteAndResetTeeth();
			}
		}

		private void CompleteBiteAndResetTeeth()
		{
			if (Phase == CrocodileGamePhase.Biting)
			{
				StartNewRound(0);
			}
		}

		private void StartNewRound(int pressedTeethMask)
		{
			int pressedTeethMask2 = PressedTeethMask;
			Phase = CrocodileGamePhase.Open;
			PressedTeethMask = pressedTeethMask;
			BadTeethMask = PickRandomBadTeethMask();
			BitingPlayer = PlayerRef.None;
			BiteDamageApplied = false;
			_phaseRenderCache = Phase;
			SetAllTeethGrabBlocked(blocked: false);
			ApplyPressedTeethVisuals(pressedTeethMask2, pressedTeethMask);
			SetMouthOpenRpc(isOpen: true);
		}

		private void ApplyBiteDamage(PlayerRef player)
		{
			if (!(player == PlayerRef.None) && _spawnedPlayersModel.Players.TryGetValue(player, out var value) && value.NetworkObject.TryGetComponent<PlayerDamageable>(out var component))
			{
				component.DamageRPC(_biteDamage, player.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForEnvironment(DamageType.CrocodileBite)));
			}
		}

		private int PickRandomBadTeethMask()
		{
			if (_teeth.Length == 0)
			{
				return 0;
			}
			int num = Mathf.Clamp(_badTeethCount, 1, _teeth.Length);
			int[] array = new int[_teeth.Length];
			for (int i = 0; i < _teeth.Length; i++)
			{
				array[i] = _teeth[i].ToothIndex;
			}
			for (int j = 0; j < num; j++)
			{
				int num2 = UnityEngine.Random.Range(j, _teeth.Length);
				ref int reference = ref array[j];
				ref int reference2 = ref array[num2];
				int num3 = array[num2];
				int num4 = array[j];
				reference = num3;
				reference2 = num4;
			}
			int num5 = 0;
			for (int k = 0; k < num; k++)
			{
				num5 |= 1 << array[k];
			}
			return num5;
		}

		private bool IsBadTooth(int toothIndex)
		{
			return (BadTeethMask & (1 << toothIndex)) != 0;
		}

		private void ApplyPressedTeethVisuals(int previousMask, int pressedTeethMask)
		{
			CrocodileToothInteractable[] teeth = _teeth;
			foreach (CrocodileToothInteractable crocodileToothInteractable in teeth)
			{
				int toothIndex = crocodileToothInteractable.ToothIndex;
				bool num = IsToothPressed(previousMask, toothIndex);
				bool flag = IsToothPressed(pressedTeethMask, toothIndex);
				if (num != flag)
				{
					crocodileToothInteractable.ApplyPressState(flag, base.HasStateAuthority);
				}
			}
		}

		private void SetAllTeethGrabBlocked(bool blocked)
		{
			CrocodileToothInteractable[] teeth = _teeth;
			foreach (CrocodileToothInteractable crocodileToothInteractable in teeth)
			{
				crocodileToothInteractable.SetLocalGrabBlocked(blocked);
				if (base.HasStateAuthority)
				{
					crocodileToothInteractable.SyncNetworkGrabBlocked(blocked);
				}
			}
		}

		private CrocodileToothInteractable GetTooth(int toothIndex)
		{
			CrocodileToothInteractable[] teeth = _teeth;
			foreach (CrocodileToothInteractable crocodileToothInteractable in teeth)
			{
				if (crocodileToothInteractable.ToothIndex == toothIndex)
				{
					return crocodileToothInteractable;
				}
			}
			return null;
		}

		private bool IsValidToothIndex(int toothIndex)
		{
			return GetTooth(toothIndex) != null;
		}

		private static bool IsToothPressed(int mask, int toothIndex)
		{
			return (mask & (1 << toothIndex)) != 0;
		}

		private static int SetToothPressed(int mask, int toothIndex, bool isPressed)
		{
			if (!isPressed)
			{
				return mask & ~(1 << toothIndex);
			}
			return mask | (1 << toothIndex);
		}

		private void PlayOneShot(EventReference reference)
		{
			if (!(_soundSourceBehaviour == null) && !reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSourceBehaviour == null) && !(_soundSourceBehaviour.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSourceBehaviour.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Phase = _Phase;
			PressedTeethMask = _PressedTeethMask;
			BadTeethMask = _BadTeethMask;
			BitingPlayer = _BitingPlayer;
			BiteDamageApplied = _BiteDamageApplied;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Phase = Phase;
			_PressedTeethMask = PressedTeethMask;
			_BadTeethMask = BadTeethMask;
			_BitingPlayer = BitingPlayer;
			_BiteDamageApplied = BiteDamageApplied;
		}

		[NetworkRpcWeavedInvoker(3821423067u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PressToothRpc_0040Invoker3821423067([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out PlayerRef value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).PressToothRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(2212218194u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyToothPressRpc_0040Invoker2212218194([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out bool value2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).ApplyToothPressRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(170633648u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifySafeToothPressedRpc_0040Invoker170633648([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out PlayerRef value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).NotifySafeToothPressedRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(4290867746u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyBadToothPressedRpc_0040Invoker4290867746([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out PlayerRef value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).NotifyBadToothPressedRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(2258412761u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayBiteAnimationRpc_0040Invoker2258412761([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).PlayBiteAnimationRpc();
		}

		[NetworkRpcWeavedInvoker(2892329155u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetMouthOpenRpc_0040Invoker2892329155([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrocodileGameBehaviour)context.TargetBehaviour).SetMouthOpenRpc(value);
		}
	}
}
