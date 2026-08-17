using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio.BroAdapter;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using VContainer;

namespace EvilCore.Audio
{
	public class NetworkedAudioManager : NetworkBehaviour, INetworkedAudioManager
	{
		private struct ServerLoop
		{
			public SoundID Id;

			public Vector3 Position;

			public uint AttachNetId;

			public bool IsAttached;

			public float Volume;

			public float Pitch;
		}

		[Inject]
		private IAudioManager _audioManager;

		private readonly Dictionary<uint, ServerLoop> _serverLoops = new Dictionary<uint, ServerLoop>();

		private readonly Dictionary<uint, AudioHandle> _localLoops = new Dictionary<uint, AudioHandle>();

		private uint _nextSoundId = 1u;

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				CmdRequestActiveLoops();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			foreach (KeyValuePair<uint, AudioHandle> localLoop in _localLoops)
			{
				_audioManager?.ReleaseInstance(localLoop.Value);
			}
			_localLoops.Clear();
		}

		public void PlayOneShot(SoundID id, Vector3 position)
		{
			if (id.IsValid())
			{
				if (base.isServer)
				{
					_audioManager.PlayOneShot(id, position);
					RpcPlayOneShot(id, position);
				}
				else
				{
					CmdPlayOneShot(id, position);
				}
			}
		}

		public void PlayOneShotExcludeSelf(SoundID id, Vector3 position)
		{
			if (id.IsValid())
			{
				_audioManager.PlayOneShot(id, position);
				if (base.isServer)
				{
					RpcPlayOneShot(id, position);
				}
				else
				{
					CmdPlayOneShotExcludeSender(id, position);
				}
			}
		}

		public void PlayOneShotAttached(SoundID id, NetworkIdentity attachTo)
		{
			if (id.IsValid() && !(attachTo == null))
			{
				uint num = attachTo.netId;
				if (base.isServer)
				{
					_audioManager.PlayOneShotAttached(id, attachTo.gameObject);
					RpcPlayOneShotAttached(id, num);
				}
				else
				{
					CmdPlayOneShotAttached(id, num);
				}
			}
		}

		public void PlayOneShotAttachedExcludeSelf(SoundID id, NetworkIdentity attachTo)
		{
			if (id.IsValid() && !(attachTo == null))
			{
				uint num = attachTo.netId;
				_audioManager.PlayOneShotAttached(id, attachTo.gameObject);
				if (base.isServer)
				{
					RpcPlayOneShotAttached(id, num);
				}
				else
				{
					CmdPlayOneShotAttachedExcludeSender(id, num);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShot(SoundID id, Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShot(Ami.BroAudio.SoundID,UnityEngine.Vector3)", -1243316377, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotExcludeSender(SoundID id, Vector3 position, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotExcludeSender(Ami.BroAudio.SoundID,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", -2144714685, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotAttached(SoundID id, uint netId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotAttached(Ami.BroAudio.SoundID,System.UInt32)", 1658803276, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotAttachedExcludeSender(SoundID id, uint netId, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotAttachedExcludeSender(Ami.BroAudio.SoundID,System.UInt32,Mirror.NetworkConnectionToClient)", 1700468998, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayOneShot(SoundID id, Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcPlayOneShot(Ami.BroAudio.SoundID,UnityEngine.Vector3)", -485681258, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayOneShotAttached(SoundID id, uint netId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcPlayOneShotAttached(Ami.BroAudio.SoundID,System.UInt32)", -411428991, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetPlayOneShot(NetworkConnectionToClient target, SoundID id, Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			SendTargetRPCInternal(target, "System.Void EvilCore.Audio.NetworkedAudioManager::TargetPlayOneShot(Mirror.NetworkConnectionToClient,Ami.BroAudio.SoundID,UnityEngine.Vector3)", 2000084703, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetPlayOneShotAttached(NetworkConnectionToClient target, SoundID id, uint netId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			SendTargetRPCInternal(target, "System.Void EvilCore.Audio.NetworkedAudioManager::TargetPlayOneShotAttached(Mirror.NetworkConnectionToClient,Ami.BroAudio.SoundID,System.UInt32)", -970025632, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public uint PlayLooping(SoundID id, Vector3 position)
		{
			if (!id.IsValid())
			{
				return 0u;
			}
			if (!base.isServer)
			{
				return 0u;
			}
			uint num = AllocateId();
			_serverLoops[num] = new ServerLoop
			{
				Id = id,
				Position = position,
				IsAttached = false,
				Volume = 1f,
				Pitch = 1f
			};
			StartLoopLocal(num, id, position, 0u, isAttached: false);
			RpcStartLoop(num, id, position);
			return num;
		}

		public uint PlayLoopingAttached(SoundID id, NetworkIdentity attachTo)
		{
			if (!id.IsValid() || attachTo == null)
			{
				return 0u;
			}
			if (!base.isServer)
			{
				return 0u;
			}
			uint num = AllocateId();
			_serverLoops[num] = new ServerLoop
			{
				Id = id,
				AttachNetId = attachTo.netId,
				IsAttached = true,
				Volume = 1f,
				Pitch = 1f
			};
			StartLoopLocal(num, id, Vector3.zero, attachTo.netId, isAttached: true);
			RpcStartLoopAttached(num, id, attachTo.netId);
			return num;
		}

		public void StopLooping(uint soundId, bool allowFadeout = true)
		{
			if (soundId == 0)
			{
				return;
			}
			if (base.isServer)
			{
				if (_serverLoops.Remove(soundId))
				{
					StopLoopLocal(soundId, allowFadeout);
					RpcStopLoop(soundId, allowFadeout);
				}
			}
			else
			{
				CmdStopLoop(soundId, allowFadeout);
			}
		}

		public bool IsLoopingPlaying(uint soundId)
		{
			if (soundId == 0)
			{
				return false;
			}
			if (base.isServer)
			{
				return _serverLoops.ContainsKey(soundId);
			}
			return _localLoops.ContainsKey(soundId);
		}

		public void SetLoopingParameter(uint soundId, string parameterName, float value)
		{
			if (soundId == 0)
			{
				return;
			}
			if (base.isServer)
			{
				if (_serverLoops.TryGetValue(soundId, out var value2))
				{
					switch (parameterName)
					{
					case "volume":
					case "Volume":
						value2.Volume = value;
						break;
					case "pitch":
					case "Pitch":
						value2.Pitch = value;
						break;
					}
					_serverLoops[soundId] = value2;
				}
				ApplyLoopParameterLocal(soundId, parameterName, value);
				RpcSetLoopParameter(soundId, parameterName, value);
			}
			else
			{
				CmdSetLoopParameter(soundId, parameterName, value);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdStopLoop(uint id, bool allowFadeout)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(id);
			writer.WriteBool(allowFadeout);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdStopLoop(System.UInt32,System.Boolean)", 414751395, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetLoopParameter(uint id, string parameterName, float value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(id);
			writer.WriteString(parameterName);
			writer.WriteFloat(value);
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdSetLoopParameter(System.UInt32,System.String,System.Single)", -860249398, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestActiveLoops(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void EvilCore.Audio.NetworkedAudioManager::CmdRequestActiveLoops(Mirror.NetworkConnectionToClient)", -1711578179, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcStartLoop(uint loopId, SoundID id, Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(loopId);
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcStartLoop(System.UInt32,Ami.BroAudio.SoundID,UnityEngine.Vector3)", -2027668058, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcStartLoopAttached(uint loopId, SoundID id, uint netId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(loopId);
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcStartLoopAttached(System.UInt32,Ami.BroAudio.SoundID,System.UInt32)", -1906810111, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcStopLoop(uint id, bool allowFadeout)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(id);
			writer.WriteBool(allowFadeout);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcStopLoop(System.UInt32,System.Boolean)", 1668353296, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSetLoopParameter(uint id, string parameterName, float value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(id);
			writer.WriteString(parameterName);
			writer.WriteFloat(value);
			SendRPCInternal("System.Void EvilCore.Audio.NetworkedAudioManager::RpcSetLoopParameter(System.UInt32,System.String,System.Single)", -84903905, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetStartLoop(NetworkConnectionToClient target, uint loopId, SoundID id, Vector3 position, float volume, float pitch)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(loopId);
			writer.WriteSoundID(id);
			writer.WriteVector3(position);
			writer.WriteFloat(volume);
			writer.WriteFloat(pitch);
			SendTargetRPCInternal(target, "System.Void EvilCore.Audio.NetworkedAudioManager::TargetStartLoop(Mirror.NetworkConnectionToClient,System.UInt32,Ami.BroAudio.SoundID,UnityEngine.Vector3,System.Single,System.Single)", 1779056759, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetStartLoopAttached(NetworkConnectionToClient target, uint loopId, SoundID id, uint netId, float volume, float pitch)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(loopId);
			writer.WriteSoundID(id);
			writer.WriteVarUInt(netId);
			writer.WriteFloat(volume);
			writer.WriteFloat(pitch);
			SendTargetRPCInternal(target, "System.Void EvilCore.Audio.NetworkedAudioManager::TargetStartLoopAttached(Mirror.NetworkConnectionToClient,System.UInt32,Ami.BroAudio.SoundID,System.UInt32,System.Single,System.Single)", 493270440, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void StartLoopLocal(uint loopId, SoundID id, Vector3 position, uint attachNetId, bool isAttached)
		{
			if (!_localLoops.ContainsKey(loopId) && id.IsValid())
			{
				NetworkIdentity value2;
				AudioHandle value = ((!isAttached || !NetworkClient.spawned.TryGetValue(attachNetId, out value2)) ? _audioManager.PlayEvent(id, position) : _audioManager.PlayEventAttached(id, value2.gameObject));
				if (value.IsValid)
				{
					_localLoops[loopId] = value;
				}
			}
		}

		private void StopLoopLocal(uint loopId, bool allowFadeout)
		{
			if (_localLoops.TryGetValue(loopId, out var value))
			{
				_audioManager.StopEvent(value, (!allowFadeout) ? AudioStopMode.Immediate : AudioStopMode.AllowFadeout);
				_localLoops.Remove(loopId);
			}
		}

		private void ApplyLoopParameterLocal(uint loopId, string parameterName, float value)
		{
			if (_localLoops.TryGetValue(loopId, out var value2))
			{
				_audioManager.SetParameter(value2, parameterName, value);
			}
		}

		private uint AllocateId()
		{
			uint result = _nextSoundId++;
			if (_nextSoundId == 0)
			{
				_nextSoundId = 1u;
			}
			return result;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdPlayOneShot__SoundID__Vector3(SoundID id, Vector3 position)
		{
			_audioManager.PlayOneShot(id, position);
			RpcPlayOneShot(id, position);
		}

		protected static void InvokeUserCode_CmdPlayOneShot__SoundID__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShot called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdPlayOneShot__SoundID__Vector3(reader.ReadSoundID(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdPlayOneShotExcludeSender__SoundID__Vector3__NetworkConnectionToClient(SoundID id, Vector3 position, NetworkConnectionToClient sender)
		{
			_audioManager.PlayOneShot(id, position);
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value != sender && value.isReady)
				{
					TargetPlayOneShot(value, id, position);
				}
			}
		}

		protected static void InvokeUserCode_CmdPlayOneShotExcludeSender__SoundID__Vector3__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotExcludeSender called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdPlayOneShotExcludeSender__SoundID__Vector3__NetworkConnectionToClient(reader.ReadSoundID(), reader.ReadVector3(), senderConnection);
			}
		}

		protected void UserCode_CmdPlayOneShotAttached__SoundID__UInt32(SoundID id, uint netId)
		{
			if (NetworkServer.spawned.TryGetValue(netId, out var value))
			{
				_audioManager.PlayOneShotAttached(id, value.gameObject);
				RpcPlayOneShotAttached(id, netId);
			}
		}

		protected static void InvokeUserCode_CmdPlayOneShotAttached__SoundID__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotAttached called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdPlayOneShotAttached__SoundID__UInt32(reader.ReadSoundID(), reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdPlayOneShotAttachedExcludeSender__SoundID__UInt32__NetworkConnectionToClient(SoundID id, uint netId, NetworkConnectionToClient sender)
		{
			if (!NetworkServer.spawned.TryGetValue(netId, out var value))
			{
				return;
			}
			_audioManager.PlayOneShotAttached(id, value.gameObject);
			foreach (NetworkConnectionToClient value2 in NetworkServer.connections.Values)
			{
				if (value2 != sender && value2.isReady)
				{
					TargetPlayOneShotAttached(value2, id, netId);
				}
			}
		}

		protected static void InvokeUserCode_CmdPlayOneShotAttachedExcludeSender__SoundID__UInt32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotAttachedExcludeSender called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdPlayOneShotAttachedExcludeSender__SoundID__UInt32__NetworkConnectionToClient(reader.ReadSoundID(), reader.ReadVarUInt(), senderConnection);
			}
		}

		protected void UserCode_RpcPlayOneShot__SoundID__Vector3(SoundID id, Vector3 position)
		{
			if (!base.isServer)
			{
				_audioManager.PlayOneShot(id, position);
			}
		}

		protected static void InvokeUserCode_RpcPlayOneShot__SoundID__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayOneShot called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcPlayOneShot__SoundID__Vector3(reader.ReadSoundID(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcPlayOneShotAttached__SoundID__UInt32(SoundID id, uint netId)
		{
			if (!base.isServer && NetworkClient.spawned.TryGetValue(netId, out var value))
			{
				_audioManager.PlayOneShotAttached(id, value.gameObject);
			}
		}

		protected static void InvokeUserCode_RpcPlayOneShotAttached__SoundID__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayOneShotAttached called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcPlayOneShotAttached__SoundID__UInt32(reader.ReadSoundID(), reader.ReadVarUInt());
			}
		}

		protected void UserCode_TargetPlayOneShot__NetworkConnectionToClient__SoundID__Vector3(NetworkConnectionToClient target, SoundID id, Vector3 position)
		{
			if (!base.isServer)
			{
				_audioManager.PlayOneShot(id, position);
			}
		}

		protected static void InvokeUserCode_TargetPlayOneShot__NetworkConnectionToClient__SoundID__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPlayOneShot called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_TargetPlayOneShot__NetworkConnectionToClient__SoundID__Vector3(null, reader.ReadSoundID(), reader.ReadVector3());
			}
		}

		protected void UserCode_TargetPlayOneShotAttached__NetworkConnectionToClient__SoundID__UInt32(NetworkConnectionToClient target, SoundID id, uint netId)
		{
			if (!base.isServer && NetworkClient.spawned.TryGetValue(netId, out var value))
			{
				_audioManager.PlayOneShotAttached(id, value.gameObject);
			}
		}

		protected static void InvokeUserCode_TargetPlayOneShotAttached__NetworkConnectionToClient__SoundID__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPlayOneShotAttached called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_TargetPlayOneShotAttached__NetworkConnectionToClient__SoundID__UInt32(null, reader.ReadSoundID(), reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdStopLoop__UInt32__Boolean(uint id, bool allowFadeout)
		{
			if (_serverLoops.Remove(id))
			{
				StopLoopLocal(id, allowFadeout);
				RpcStopLoop(id, allowFadeout);
			}
		}

		protected static void InvokeUserCode_CmdStopLoop__UInt32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStopLoop called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdStopLoop__UInt32__Boolean(reader.ReadVarUInt(), reader.ReadBool());
			}
		}

		protected void UserCode_CmdSetLoopParameter__UInt32__String__Single(uint id, string parameterName, float value)
		{
			if (_serverLoops.TryGetValue(id, out var value2))
			{
				switch (parameterName)
				{
				case "volume":
				case "Volume":
					value2.Volume = value;
					break;
				case "pitch":
				case "Pitch":
					value2.Pitch = value;
					break;
				}
				_serverLoops[id] = value2;
				ApplyLoopParameterLocal(id, parameterName, value);
				RpcSetLoopParameter(id, parameterName, value);
			}
		}

		protected static void InvokeUserCode_CmdSetLoopParameter__UInt32__String__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetLoopParameter called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdSetLoopParameter__UInt32__String__Single(reader.ReadVarUInt(), reader.ReadString(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdRequestActiveLoops__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			if (sender == null)
			{
				return;
			}
			foreach (KeyValuePair<uint, ServerLoop> serverLoop in _serverLoops)
			{
				if (serverLoop.Value.IsAttached)
				{
					TargetStartLoopAttached(sender, serverLoop.Key, serverLoop.Value.Id, serverLoop.Value.AttachNetId, serverLoop.Value.Volume, serverLoop.Value.Pitch);
				}
				else
				{
					TargetStartLoop(sender, serverLoop.Key, serverLoop.Value.Id, serverLoop.Value.Position, serverLoop.Value.Volume, serverLoop.Value.Pitch);
				}
			}
		}

		protected static void InvokeUserCode_CmdRequestActiveLoops__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestActiveLoops called on client.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_CmdRequestActiveLoops__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_RpcStartLoop__UInt32__SoundID__Vector3(uint loopId, SoundID id, Vector3 position)
		{
			if (!base.isServer)
			{
				StartLoopLocal(loopId, id, position, 0u, isAttached: false);
			}
		}

		protected static void InvokeUserCode_RpcStartLoop__UInt32__SoundID__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStartLoop called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcStartLoop__UInt32__SoundID__Vector3(reader.ReadVarUInt(), reader.ReadSoundID(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcStartLoopAttached__UInt32__SoundID__UInt32(uint loopId, SoundID id, uint netId)
		{
			if (!base.isServer)
			{
				StartLoopLocal(loopId, id, Vector3.zero, netId, isAttached: true);
			}
		}

		protected static void InvokeUserCode_RpcStartLoopAttached__UInt32__SoundID__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStartLoopAttached called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcStartLoopAttached__UInt32__SoundID__UInt32(reader.ReadVarUInt(), reader.ReadSoundID(), reader.ReadVarUInt());
			}
		}

		protected void UserCode_RpcStopLoop__UInt32__Boolean(uint id, bool allowFadeout)
		{
			if (!base.isServer)
			{
				StopLoopLocal(id, allowFadeout);
			}
		}

		protected static void InvokeUserCode_RpcStopLoop__UInt32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStopLoop called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcStopLoop__UInt32__Boolean(reader.ReadVarUInt(), reader.ReadBool());
			}
		}

		protected void UserCode_RpcSetLoopParameter__UInt32__String__Single(uint id, string parameterName, float value)
		{
			if (!base.isServer)
			{
				ApplyLoopParameterLocal(id, parameterName, value);
			}
		}

		protected static void InvokeUserCode_RpcSetLoopParameter__UInt32__String__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetLoopParameter called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_RpcSetLoopParameter__UInt32__String__Single(reader.ReadVarUInt(), reader.ReadString(), reader.ReadFloat());
			}
		}

		protected void UserCode_TargetStartLoop__NetworkConnectionToClient__UInt32__SoundID__Vector3__Single__Single(NetworkConnectionToClient target, uint loopId, SoundID id, Vector3 position, float volume, float pitch)
		{
			if (!base.isServer)
			{
				StartLoopLocal(loopId, id, position, 0u, isAttached: false);
				ApplyLoopParameterLocal(loopId, "volume", volume);
				ApplyLoopParameterLocal(loopId, "pitch", pitch);
			}
		}

		protected static void InvokeUserCode_TargetStartLoop__NetworkConnectionToClient__UInt32__SoundID__Vector3__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetStartLoop called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_TargetStartLoop__NetworkConnectionToClient__UInt32__SoundID__Vector3__Single__Single(null, reader.ReadVarUInt(), reader.ReadSoundID(), reader.ReadVector3(), reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_TargetStartLoopAttached__NetworkConnectionToClient__UInt32__SoundID__UInt32__Single__Single(NetworkConnectionToClient target, uint loopId, SoundID id, uint netId, float volume, float pitch)
		{
			if (!base.isServer)
			{
				StartLoopLocal(loopId, id, Vector3.zero, netId, isAttached: true);
				ApplyLoopParameterLocal(loopId, "volume", volume);
				ApplyLoopParameterLocal(loopId, "pitch", pitch);
			}
		}

		protected static void InvokeUserCode_TargetStartLoopAttached__NetworkConnectionToClient__UInt32__SoundID__UInt32__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetStartLoopAttached called on server.");
			}
			else
			{
				((NetworkedAudioManager)obj).UserCode_TargetStartLoopAttached__NetworkConnectionToClient__UInt32__SoundID__UInt32__Single__Single(null, reader.ReadVarUInt(), reader.ReadSoundID(), reader.ReadVarUInt(), reader.ReadFloat(), reader.ReadFloat());
			}
		}

		static NetworkedAudioManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShot(Ami.BroAudio.SoundID,UnityEngine.Vector3)", InvokeUserCode_CmdPlayOneShot__SoundID__Vector3, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotExcludeSender(Ami.BroAudio.SoundID,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdPlayOneShotExcludeSender__SoundID__Vector3__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotAttached(Ami.BroAudio.SoundID,System.UInt32)", InvokeUserCode_CmdPlayOneShotAttached__SoundID__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdPlayOneShotAttachedExcludeSender(Ami.BroAudio.SoundID,System.UInt32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdPlayOneShotAttachedExcludeSender__SoundID__UInt32__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdStopLoop(System.UInt32,System.Boolean)", InvokeUserCode_CmdStopLoop__UInt32__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdSetLoopParameter(System.UInt32,System.String,System.Single)", InvokeUserCode_CmdSetLoopParameter__UInt32__String__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::CmdRequestActiveLoops(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRequestActiveLoops__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcPlayOneShot(Ami.BroAudio.SoundID,UnityEngine.Vector3)", InvokeUserCode_RpcPlayOneShot__SoundID__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcPlayOneShotAttached(Ami.BroAudio.SoundID,System.UInt32)", InvokeUserCode_RpcPlayOneShotAttached__SoundID__UInt32);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcStartLoop(System.UInt32,Ami.BroAudio.SoundID,UnityEngine.Vector3)", InvokeUserCode_RpcStartLoop__UInt32__SoundID__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcStartLoopAttached(System.UInt32,Ami.BroAudio.SoundID,System.UInt32)", InvokeUserCode_RpcStartLoopAttached__UInt32__SoundID__UInt32);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcStopLoop(System.UInt32,System.Boolean)", InvokeUserCode_RpcStopLoop__UInt32__Boolean);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::RpcSetLoopParameter(System.UInt32,System.String,System.Single)", InvokeUserCode_RpcSetLoopParameter__UInt32__String__Single);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::TargetPlayOneShot(Mirror.NetworkConnectionToClient,Ami.BroAudio.SoundID,UnityEngine.Vector3)", InvokeUserCode_TargetPlayOneShot__NetworkConnectionToClient__SoundID__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::TargetPlayOneShotAttached(Mirror.NetworkConnectionToClient,Ami.BroAudio.SoundID,System.UInt32)", InvokeUserCode_TargetPlayOneShotAttached__NetworkConnectionToClient__SoundID__UInt32);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::TargetStartLoop(Mirror.NetworkConnectionToClient,System.UInt32,Ami.BroAudio.SoundID,UnityEngine.Vector3,System.Single,System.Single)", InvokeUserCode_TargetStartLoop__NetworkConnectionToClient__UInt32__SoundID__Vector3__Single__Single);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedAudioManager), "System.Void EvilCore.Audio.NetworkedAudioManager::TargetStartLoopAttached(Mirror.NetworkConnectionToClient,System.UInt32,Ami.BroAudio.SoundID,System.UInt32,System.Single,System.Single)", InvokeUserCode_TargetStartLoopAttached__NetworkConnectionToClient__UInt32__SoundID__UInt32__Single__Single);
		}
	}
}
