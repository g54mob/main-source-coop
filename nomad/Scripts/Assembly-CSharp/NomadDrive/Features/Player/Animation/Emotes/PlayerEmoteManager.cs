using System;
using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Player.Core;
using UnityEngine;

namespace NomadDrive.Features.Player.Animation.Emotes
{
	public class PlayerEmoteManager : NetworkBehaviour, IPlayerComponent
	{
		[Header("References")]
		[SerializeField]
		private EmoteDatabase _emoteDatabase;

		private Animator _animator;

		private AnimatorOverrideController _overrideController;

		private RuntimeAnimatorController _originalController;

		private EmoteConfig _currentEmote;

		private Coroutine _emoteCoroutine;

		private FirstPersonController _firstPersonController;

		private bool _isLocalPlayer;

		private const int EmoteFullBodyLayer = 2;

		private const int EmoteUpperBodyLayer = 3;

		private const string EmotePlaceholderClipName = "EmotePlaceholder";

		private static readonly int EmoteTriggerHash;

		public int SetupPriority => 25;

		public bool IsPlayingEmote => _currentEmote != null;

		public event Action<EmoteConfig> OnEmoteStarted;

		public event Action OnEmoteStopped;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			base.enabled = isLocalPlayer;
		}

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_firstPersonController = GetComponent<FirstPersonController>();
		}

		private void Start()
		{
			if (_animator.runtimeAnimatorController != null)
			{
				_originalController = _animator.runtimeAnimatorController;
				_overrideController = new AnimatorOverrideController(_originalController);
				_animator.runtimeAnimatorController = _overrideController;
			}
		}

		private void Update()
		{
			if (_isLocalPlayer && IsPlayingEmote && _currentEmote.bodyMode == EmoteBodyMode.FullBody && (Mathf.Abs(OnFootInputs.GetMoveHorizontal()) > 0.1f || Mathf.Abs(OnFootInputs.GetMoveVertical()) > 0.1f))
			{
				CancelEmote();
			}
		}

		public void TriggerEmote(int emoteIndex)
		{
			if (!(_emoteDatabase == null))
			{
				EmoteConfig emote = _emoteDatabase.GetEmote(emoteIndex);
				if (!(emote == null))
				{
					TriggerEmote(emote);
				}
			}
		}

		public void TriggerEmote(EmoteConfig emote)
		{
			if (!(emote == null) && !(emote.clip == null))
			{
				if (IsPlayingEmote)
				{
					StopEmoteInternal();
				}
				_currentEmote = emote;
				_overrideController["EmotePlaceholder"] = emote.clip;
				int layerIndex = ((emote.bodyMode == EmoteBodyMode.FullBody) ? 2 : 3);
				_animator.SetLayerWeight(layerIndex, 1f);
				_animator.SetTrigger(EmoteTriggerHash);
				if (emote.bodyMode == EmoteBodyMode.FullBody)
				{
					_firstPersonController.DisableMovement();
				}
				this.OnEmoteStarted?.Invoke(emote);
				if (!emote.isLooping)
				{
					_emoteCoroutine = StartCoroutine(EmoteDurationCoroutine(emote));
				}
				if (_isLocalPlayer)
				{
					int index = _emoteDatabase.GetIndex(emote);
					CmdTriggerEmote(index);
				}
			}
		}

		public void CancelEmote()
		{
			if (IsPlayingEmote)
			{
				StopEmoteInternal();
				if (_isLocalPlayer)
				{
					CmdCancelEmote();
				}
			}
		}

		private void StopEmoteInternal()
		{
			if (_emoteCoroutine != null)
			{
				StopCoroutine(_emoteCoroutine);
				_emoteCoroutine = null;
			}
			if (_currentEmote != null)
			{
				int layerIndex = ((_currentEmote.bodyMode == EmoteBodyMode.FullBody) ? 2 : 3);
				_animator.SetLayerWeight(layerIndex, 0f);
				if (_currentEmote.bodyMode == EmoteBodyMode.FullBody)
				{
					_firstPersonController.EnableMovement();
				}
				_currentEmote = null;
			}
			this.OnEmoteStopped?.Invoke();
		}

		private IEnumerator EmoteDurationCoroutine(EmoteConfig emote)
		{
			yield return new WaitForSeconds(emote.clip.length);
			StopEmoteInternal();
		}

		[Command(requiresAuthority = false)]
		private void CmdTriggerEmote(int emoteIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(emoteIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::CmdTriggerEmote(System.Int32)", -1994674970, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcTriggerEmote(int emoteIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(emoteIndex);
			SendRPCInternal("System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::RpcTriggerEmote(System.Int32)", 722764653, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdCancelEmote()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::CmdCancelEmote()", -1995930947, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcCancelEmote()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::RpcCancelEmote()", 952607520, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		static PlayerEmoteManager()
		{
			EmoteTriggerHash = Animator.StringToHash("EmoteTrigger");
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerEmoteManager), "System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::CmdTriggerEmote(System.Int32)", InvokeUserCode_CmdTriggerEmote__Int32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerEmoteManager), "System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::CmdCancelEmote()", InvokeUserCode_CmdCancelEmote, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerEmoteManager), "System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::RpcTriggerEmote(System.Int32)", InvokeUserCode_RpcTriggerEmote__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerEmoteManager), "System.Void NomadDrive.Features.Player.Animation.Emotes.PlayerEmoteManager::RpcCancelEmote()", InvokeUserCode_RpcCancelEmote);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdTriggerEmote__Int32(int emoteIndex)
		{
			RpcTriggerEmote(emoteIndex);
		}

		protected static void InvokeUserCode_CmdTriggerEmote__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTriggerEmote called on client.");
			}
			else
			{
				((PlayerEmoteManager)obj).UserCode_CmdTriggerEmote__Int32(reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcTriggerEmote__Int32(int emoteIndex)
		{
			TriggerEmote(emoteIndex);
		}

		protected static void InvokeUserCode_RpcTriggerEmote__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcTriggerEmote called on server.");
			}
			else
			{
				((PlayerEmoteManager)obj).UserCode_RpcTriggerEmote__Int32(reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdCancelEmote()
		{
			RpcCancelEmote();
		}

		protected static void InvokeUserCode_CmdCancelEmote(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdCancelEmote called on client.");
			}
			else
			{
				((PlayerEmoteManager)obj).UserCode_CmdCancelEmote();
			}
		}

		protected void UserCode_RpcCancelEmote()
		{
			if (IsPlayingEmote)
			{
				StopEmoteInternal();
			}
		}

		protected static void InvokeUserCode_RpcCancelEmote(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcCancelEmote called on server.");
			}
			else
			{
				((PlayerEmoteManager)obj).UserCode_RpcCancelEmote();
			}
		}
	}
}
