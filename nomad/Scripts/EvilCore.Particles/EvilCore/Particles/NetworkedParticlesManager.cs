using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using VContainer;

namespace EvilCore.Particles
{
	public class NetworkedParticlesManager : NetworkBehaviour, INetworkedParticlesManager
	{
		private struct PersistentParticle
		{
			public ParticleHandle Handle;

			public string Key;

			public uint AttachedNetId;

			public Vector3 Position;

			public Quaternion Rotation;

			public Vector3 LocalOffset;
		}

		[Inject]
		private IParticlesManager _particlesManager;

		private readonly Dictionary<uint, PersistentParticle> _persistentParticles = new Dictionary<uint, PersistentParticle>();

		[SyncVar]
		private uint _nextParticleId = 1u;

		public uint Network_nextParticleId
		{
			get
			{
				return _nextParticleId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _nextParticleId, 1uL, null);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				CmdRequestActivePersistentParticles();
			}
		}

		private void OnDestroy()
		{
			foreach (KeyValuePair<uint, PersistentParticle> persistentParticle in _persistentParticles)
			{
				_particlesManager?.Stop(persistentParticle.Value.Handle, clear: true);
			}
			_persistentParticles.Clear();
		}

		public void PlayNetworkedOneShot(string key, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			if (base.isServer)
			{
				_particlesManager.PlayOneShot(key, position, rotation);
				RpcPlayOneShot(key, position, rotation);
			}
			else
			{
				CmdPlayOneShot(key, position, rotation);
			}
		}

		public void PlayNetworkedOneShot(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides)
		{
			if (base.isServer)
			{
				_particlesManager.PlayOneShot(key, position, rotation, overrides.ToLocal());
				RpcPlayOneShotWithOverrides(key, position, rotation, overrides);
			}
			else
			{
				CmdPlayOneShotWithOverrides(key, position, rotation, overrides);
			}
		}

		public void PlayNetworkedOneShotExcludeSelf(string key, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			_particlesManager.PlayOneShot(key, position, rotation);
			if (base.isServer)
			{
				RpcPlayOneShot(key, position, rotation);
			}
			else
			{
				CmdPlayOneShotExcludeSender(key, position, rotation);
			}
		}

		public void PlayNetworkedOneShotAttached(string key, NetworkIdentity attachTo, Vector3 localOffset = default(Vector3))
		{
			if (!(attachTo == null))
			{
				uint num = attachTo.netId;
				if (base.isServer)
				{
					_particlesManager.PlayOneShotAttached(key, attachTo.transform, localOffset);
					RpcPlayOneShotAttached(key, num, localOffset);
				}
				else
				{
					CmdPlayOneShotAttached(key, num, localOffset);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShot(string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShot(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", -1036180206, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotWithOverrides(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			GeneratedNetworkCode._Write_EvilCore_002EParticles_002ENetworkParticleOverrides(writer, overrides);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotWithOverrides(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,EvilCore.Particles.NetworkParticleOverrides)", -1767837408, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotExcludeSender(string key, Vector3 position, Quaternion rotation, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotExcludeSender(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,Mirror.NetworkConnectionToClient)", -231587492, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayOneShotAttached(string key, uint netId, Vector3 localOffset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVarUInt(netId);
			writer.WriteVector3(localOffset);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotAttached(System.String,System.UInt32,UnityEngine.Vector3)", 1190315687, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayOneShot(string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShot(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", -1104465529, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayOneShotWithOverrides(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			GeneratedNetworkCode._Write_EvilCore_002EParticles_002ENetworkParticleOverrides(writer, overrides);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShotWithOverrides(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,EvilCore.Particles.NetworkParticleOverrides)", -695824445, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayOneShotAttached(string key, uint netId, Vector3 localOffset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVarUInt(netId);
			writer.WriteVector3(localOffset);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShotAttached(System.String,System.UInt32,UnityEngine.Vector3)", 824783736, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetPlayOneShot(NetworkConnectionToClient target, string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendTargetRPCInternal(target, "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayOneShot(Mirror.NetworkConnectionToClient,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", 1929396414, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public uint PlayNetworkedPersistent(string key, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			if (base.isServer)
			{
				uint nextParticleId = _nextParticleId;
				Network_nextParticleId = nextParticleId + 1;
				uint num = nextParticleId;
				ParticleHandle handle = _particlesManager.Play(key, position, rotation);
				_persistentParticles[num] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = 0u,
					Position = position,
					Rotation = rotation
				};
				RpcPlayPersistent(num, key, position, rotation);
				return num;
			}
			CmdPlayPersistent(key, position, rotation);
			return 0u;
		}

		public uint PlayNetworkedPersistentAttached(string key, NetworkIdentity attachTo, Vector3 localOffset = default(Vector3))
		{
			if (attachTo == null)
			{
				return 0u;
			}
			uint attachedNetId = attachTo.netId;
			if (base.isServer)
			{
				uint nextParticleId = _nextParticleId;
				Network_nextParticleId = nextParticleId + 1;
				uint num = nextParticleId;
				ParticleHandle handle = _particlesManager.PlayAttached(key, attachTo.transform, localOffset);
				_persistentParticles[num] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = attachedNetId,
					LocalOffset = localOffset
				};
				RpcPlayPersistentAttached(num, key, attachedNetId, localOffset);
				return num;
			}
			CmdPlayPersistentAttached(key, attachedNetId, localOffset);
			return 0u;
		}

		public void StopNetworkedPersistent(uint particleId, bool clear = false)
		{
			if (base.isServer)
			{
				StopPersistentInternal(particleId, clear);
				RpcStopPersistent(particleId, clear);
			}
			else
			{
				CmdStopPersistent(particleId, clear);
			}
		}

		public bool IsNetworkedPersistentPlaying(uint particleId)
		{
			if (particleId == 0)
			{
				return false;
			}
			return _persistentParticles.ContainsKey(particleId);
		}

		private void StopPersistentInternal(uint particleId, bool clear)
		{
			if (_persistentParticles.TryGetValue(particleId, out var value))
			{
				_particlesManager.Stop(value.Handle, clear);
				_persistentParticles.Remove(particleId);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayPersistent(string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayPersistent(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", 1918578565, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPlayPersistentAttached(string key, uint netId, Vector3 localOffset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(key);
			writer.WriteVarUInt(netId);
			writer.WriteVector3(localOffset);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayPersistentAttached(System.String,System.UInt32,UnityEngine.Vector3)", -1782062478, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestActivePersistentParticles(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdRequestActivePersistentParticles(Mirror.NetworkConnectionToClient)", -1511343260, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private async UniTaskVoid SpreadActivePersistentParticlesAsync(NetworkConnectionToClient sender)
		{
			if (sender == null)
			{
				return;
			}
			List<uint> particleIds = new List<uint>(_persistentParticles.Keys);
			int sentThisBatch = 0;
			for (int i = 0; i < particleIds.Count; i++)
			{
				if (sender.connectionId < 0)
				{
					break;
				}
				if (_persistentParticles.TryGetValue(particleIds[i], out var value))
				{
					if (value.AttachedNetId != 0)
					{
						TargetPlayPersistentAttached(sender, particleIds[i], value.Key, value.AttachedNetId, value.LocalOffset);
					}
					else
					{
						TargetPlayPersistent(sender, particleIds[i], value.Key, value.Position, value.Rotation);
					}
					sentThisBatch++;
					if (sentThisBatch >= 5)
					{
						sentThisBatch = 0;
						await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
					}
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdStopPersistent(uint particleId, bool clear)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteBool(clear);
			SendCommandInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::CmdStopPersistent(System.UInt32,System.Boolean)", -2106218962, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayPersistent(uint particleId, string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayPersistent(System.UInt32,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", -1492538846, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayPersistentAttached(uint particleId, string key, uint netId, Vector3 localOffset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteString(key);
			writer.WriteVarUInt(netId);
			writer.WriteVector3(localOffset);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayPersistentAttached(System.UInt32,System.String,System.UInt32,UnityEngine.Vector3)", 1697233255, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcStopPersistent(uint particleId, bool clear)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteBool(clear);
			SendRPCInternal("System.Void EvilCore.Particles.NetworkedParticlesManager::RpcStopPersistent(System.UInt32,System.Boolean)", 338963201, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetPlayPersistent(NetworkConnectionToClient target, uint particleId, string key, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteString(key);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendTargetRPCInternal(target, "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayPersistent(Mirror.NetworkConnectionToClient,System.UInt32,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", -248161507, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetPlayPersistentAttached(NetworkConnectionToClient target, uint particleId, string key, uint netId, Vector3 localOffset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(particleId);
			writer.WriteString(key);
			writer.WriteVarUInt(netId);
			writer.WriteVector3(localOffset);
			SendTargetRPCInternal(target, "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayPersistentAttached(Mirror.NetworkConnectionToClient,System.UInt32,System.String,System.UInt32,UnityEngine.Vector3)", 1582095218, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdPlayOneShot__String__Vector3__Quaternion(string key, Vector3 position, Quaternion rotation)
		{
			_particlesManager.PlayOneShot(key, position, rotation);
			RpcPlayOneShot(key, position, rotation);
		}

		protected static void InvokeUserCode_CmdPlayOneShot__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShot called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayOneShot__String__Vector3__Quaternion(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides)
		{
			_particlesManager.PlayOneShot(key, position, rotation, overrides.ToLocal());
			RpcPlayOneShotWithOverrides(key, position, rotation, overrides);
		}

		protected static void InvokeUserCode_CmdPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotWithOverrides called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion(), GeneratedNetworkCode._Read_EvilCore_002EParticles_002ENetworkParticleOverrides(reader));
			}
		}

		protected void UserCode_CmdPlayOneShotExcludeSender__String__Vector3__Quaternion__NetworkConnectionToClient(string key, Vector3 position, Quaternion rotation, NetworkConnectionToClient sender)
		{
			_particlesManager.PlayOneShot(key, position, rotation);
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value != sender && value.isReady)
				{
					TargetPlayOneShot(value, key, position, rotation);
				}
			}
		}

		protected static void InvokeUserCode_CmdPlayOneShotExcludeSender__String__Vector3__Quaternion__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotExcludeSender called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayOneShotExcludeSender__String__Vector3__Quaternion__NetworkConnectionToClient(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion(), senderConnection);
			}
		}

		protected void UserCode_CmdPlayOneShotAttached__String__UInt32__Vector3(string key, uint netId, Vector3 localOffset)
		{
			if (NetworkServer.spawned.TryGetValue(netId, out var value))
			{
				_particlesManager.PlayOneShotAttached(key, value.transform, localOffset);
				RpcPlayOneShotAttached(key, netId, localOffset);
			}
		}

		protected static void InvokeUserCode_CmdPlayOneShotAttached__String__UInt32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayOneShotAttached called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayOneShotAttached__String__UInt32__Vector3(reader.ReadString(), reader.ReadVarUInt(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcPlayOneShot__String__Vector3__Quaternion(string key, Vector3 position, Quaternion rotation)
		{
			if (!base.isServer)
			{
				_particlesManager.PlayOneShot(key, position, rotation);
			}
		}

		protected static void InvokeUserCode_RpcPlayOneShot__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayOneShot called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcPlayOneShot__String__Vector3__Quaternion(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_RpcPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides)
		{
			if (!base.isServer)
			{
				_particlesManager.PlayOneShot(key, position, rotation, overrides.ToLocal());
			}
		}

		protected static void InvokeUserCode_RpcPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayOneShotWithOverrides called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion(), GeneratedNetworkCode._Read_EvilCore_002EParticles_002ENetworkParticleOverrides(reader));
			}
		}

		protected void UserCode_RpcPlayOneShotAttached__String__UInt32__Vector3(string key, uint netId, Vector3 localOffset)
		{
			if (!base.isServer && NetworkClient.spawned.TryGetValue(netId, out var value))
			{
				_particlesManager.PlayOneShotAttached(key, value.transform, localOffset);
			}
		}

		protected static void InvokeUserCode_RpcPlayOneShotAttached__String__UInt32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayOneShotAttached called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcPlayOneShotAttached__String__UInt32__Vector3(reader.ReadString(), reader.ReadVarUInt(), reader.ReadVector3());
			}
		}

		protected void UserCode_TargetPlayOneShot__NetworkConnectionToClient__String__Vector3__Quaternion(NetworkConnectionToClient target, string key, Vector3 position, Quaternion rotation)
		{
			_particlesManager.PlayOneShot(key, position, rotation);
		}

		protected static void InvokeUserCode_TargetPlayOneShot__NetworkConnectionToClient__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPlayOneShot called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_TargetPlayOneShot__NetworkConnectionToClient__String__Vector3__Quaternion(null, reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdPlayPersistent__String__Vector3__Quaternion(string key, Vector3 position, Quaternion rotation)
		{
			uint nextParticleId = _nextParticleId;
			Network_nextParticleId = nextParticleId + 1;
			uint num = nextParticleId;
			ParticleHandle handle = _particlesManager.Play(key, position, rotation);
			_persistentParticles[num] = new PersistentParticle
			{
				Handle = handle,
				Key = key,
				AttachedNetId = 0u,
				Position = position,
				Rotation = rotation
			};
			RpcPlayPersistent(num, key, position, rotation);
		}

		protected static void InvokeUserCode_CmdPlayPersistent__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayPersistent called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayPersistent__String__Vector3__Quaternion(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdPlayPersistentAttached__String__UInt32__Vector3(string key, uint netId, Vector3 localOffset)
		{
			if (NetworkServer.spawned.TryGetValue(netId, out var value))
			{
				uint nextParticleId = _nextParticleId;
				Network_nextParticleId = nextParticleId + 1;
				uint num = nextParticleId;
				ParticleHandle handle = _particlesManager.PlayAttached(key, value.transform, localOffset);
				_persistentParticles[num] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = netId,
					LocalOffset = localOffset
				};
				RpcPlayPersistentAttached(num, key, netId, localOffset);
			}
		}

		protected static void InvokeUserCode_CmdPlayPersistentAttached__String__UInt32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlayPersistentAttached called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdPlayPersistentAttached__String__UInt32__Vector3(reader.ReadString(), reader.ReadVarUInt(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdRequestActivePersistentParticles__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			SpreadActivePersistentParticlesAsync(sender).Forget();
		}

		protected static void InvokeUserCode_CmdRequestActivePersistentParticles__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestActivePersistentParticles called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdRequestActivePersistentParticles__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_CmdStopPersistent__UInt32__Boolean(uint particleId, bool clear)
		{
			StopPersistentInternal(particleId, clear);
			RpcStopPersistent(particleId, clear);
		}

		protected static void InvokeUserCode_CmdStopPersistent__UInt32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStopPersistent called on client.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_CmdStopPersistent__UInt32__Boolean(reader.ReadVarUInt(), reader.ReadBool());
			}
		}

		protected void UserCode_RpcPlayPersistent__UInt32__String__Vector3__Quaternion(uint particleId, string key, Vector3 position, Quaternion rotation)
		{
			if (!base.isServer)
			{
				ParticleHandle handle = _particlesManager.Play(key, position, rotation);
				_persistentParticles[particleId] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = 0u,
					Position = position,
					Rotation = rotation
				};
			}
		}

		protected static void InvokeUserCode_RpcPlayPersistent__UInt32__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayPersistent called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcPlayPersistent__UInt32__String__Vector3__Quaternion(reader.ReadVarUInt(), reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_RpcPlayPersistentAttached__UInt32__String__UInt32__Vector3(uint particleId, string key, uint netId, Vector3 localOffset)
		{
			if (!base.isServer && NetworkClient.spawned.TryGetValue(netId, out var value))
			{
				ParticleHandle handle = _particlesManager.PlayAttached(key, value.transform, localOffset);
				_persistentParticles[particleId] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = netId,
					LocalOffset = localOffset
				};
			}
		}

		protected static void InvokeUserCode_RpcPlayPersistentAttached__UInt32__String__UInt32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayPersistentAttached called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcPlayPersistentAttached__UInt32__String__UInt32__Vector3(reader.ReadVarUInt(), reader.ReadString(), reader.ReadVarUInt(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcStopPersistent__UInt32__Boolean(uint particleId, bool clear)
		{
			if (!base.isServer)
			{
				StopPersistentInternal(particleId, clear);
			}
		}

		protected static void InvokeUserCode_RpcStopPersistent__UInt32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStopPersistent called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_RpcStopPersistent__UInt32__Boolean(reader.ReadVarUInt(), reader.ReadBool());
			}
		}

		protected void UserCode_TargetPlayPersistent__NetworkConnectionToClient__UInt32__String__Vector3__Quaternion(NetworkConnectionToClient target, uint particleId, string key, Vector3 position, Quaternion rotation)
		{
			if (!_persistentParticles.ContainsKey(particleId))
			{
				ParticleHandle handle = _particlesManager.Play(key, position, rotation);
				_persistentParticles[particleId] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = 0u,
					Position = position,
					Rotation = rotation
				};
			}
		}

		protected static void InvokeUserCode_TargetPlayPersistent__NetworkConnectionToClient__UInt32__String__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPlayPersistent called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_TargetPlayPersistent__NetworkConnectionToClient__UInt32__String__Vector3__Quaternion(null, reader.ReadVarUInt(), reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_TargetPlayPersistentAttached__NetworkConnectionToClient__UInt32__String__UInt32__Vector3(NetworkConnectionToClient target, uint particleId, string key, uint netId, Vector3 localOffset)
		{
			if (!_persistentParticles.ContainsKey(particleId) && NetworkClient.spawned.TryGetValue(netId, out var value))
			{
				ParticleHandle handle = _particlesManager.PlayAttached(key, value.transform, localOffset);
				_persistentParticles[particleId] = new PersistentParticle
				{
					Handle = handle,
					Key = key,
					AttachedNetId = netId,
					LocalOffset = localOffset
				};
			}
		}

		protected static void InvokeUserCode_TargetPlayPersistentAttached__NetworkConnectionToClient__UInt32__String__UInt32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPlayPersistentAttached called on server.");
			}
			else
			{
				((NetworkedParticlesManager)obj).UserCode_TargetPlayPersistentAttached__NetworkConnectionToClient__UInt32__String__UInt32__Vector3(null, reader.ReadVarUInt(), reader.ReadString(), reader.ReadVarUInt(), reader.ReadVector3());
			}
		}

		static NetworkedParticlesManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShot(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdPlayOneShot__String__Vector3__Quaternion, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotWithOverrides(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,EvilCore.Particles.NetworkParticleOverrides)", InvokeUserCode_CmdPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotExcludeSender(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdPlayOneShotExcludeSender__String__Vector3__Quaternion__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayOneShotAttached(System.String,System.UInt32,UnityEngine.Vector3)", InvokeUserCode_CmdPlayOneShotAttached__String__UInt32__Vector3, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayPersistent(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdPlayPersistent__String__Vector3__Quaternion, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdPlayPersistentAttached(System.String,System.UInt32,UnityEngine.Vector3)", InvokeUserCode_CmdPlayPersistentAttached__String__UInt32__Vector3, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdRequestActivePersistentParticles(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRequestActivePersistentParticles__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::CmdStopPersistent(System.UInt32,System.Boolean)", InvokeUserCode_CmdStopPersistent__UInt32__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShot(System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcPlayOneShot__String__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShotWithOverrides(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,EvilCore.Particles.NetworkParticleOverrides)", InvokeUserCode_RpcPlayOneShotWithOverrides__String__Vector3__Quaternion__NetworkParticleOverrides);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayOneShotAttached(System.String,System.UInt32,UnityEngine.Vector3)", InvokeUserCode_RpcPlayOneShotAttached__String__UInt32__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayPersistent(System.UInt32,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcPlayPersistent__UInt32__String__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcPlayPersistentAttached(System.UInt32,System.String,System.UInt32,UnityEngine.Vector3)", InvokeUserCode_RpcPlayPersistentAttached__UInt32__String__UInt32__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::RpcStopPersistent(System.UInt32,System.Boolean)", InvokeUserCode_RpcStopPersistent__UInt32__Boolean);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayOneShot(Mirror.NetworkConnectionToClient,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_TargetPlayOneShot__NetworkConnectionToClient__String__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayPersistent(Mirror.NetworkConnectionToClient,System.UInt32,System.String,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_TargetPlayPersistent__NetworkConnectionToClient__UInt32__String__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedParticlesManager), "System.Void EvilCore.Particles.NetworkedParticlesManager::TargetPlayPersistentAttached(Mirror.NetworkConnectionToClient,System.UInt32,System.String,System.UInt32,UnityEngine.Vector3)", InvokeUserCode_TargetPlayPersistentAttached__NetworkConnectionToClient__UInt32__String__UInt32__Vector3);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarUInt(_nextParticleId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarUInt(_nextParticleId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _nextParticleId, null, reader.ReadVarUInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _nextParticleId, null, reader.ReadVarUInt());
			}
		}
	}
}
