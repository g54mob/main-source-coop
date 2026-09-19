using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class VoiceNetwork : NetworkBehaviour
{
	private class VoiceRecord
	{
		public byte[] pcm;

		public int sampleRate;

		public int channels;
	}

	private class IncomingBuffer
	{
		public AnimalType type;

		public int sampleRate;

		public int channels;

		public int totalBytes;

		public byte[] data;

		public int received;
	}

	[CompilerGenerated]
	private sealed class _003CBroadcastRecord_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VoiceNetwork _003C_003E4__this;

		public AnimalType type;

		private VoiceRecord _003Crec_003E5__2;

		private int _003Coffset_003E5__3;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CBroadcastRecord_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			VoiceNetwork voiceNetwork = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!voiceNetwork._serverStore.TryGetValue(type, out _003Crec_003E5__2))
				{
					return false;
				}
				voiceNetwork.RpcBeginRecord(type, _003Crec_003E5__2.pcm.Length, _003Crec_003E5__2.sampleRate, _003Crec_003E5__2.channels);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Coffset_003E5__3 = 0;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Coffset_003E5__3 < _003Crec_003E5__2.pcm.Length)
			{
				int num2 = Mathf.Min(voiceNetwork.chunkSize, _003Crec_003E5__2.pcm.Length - _003Coffset_003E5__3);
				byte[] array = new byte[num2];
				Array.Copy(_003Crec_003E5__2.pcm, _003Coffset_003E5__3, array, 0, num2);
				voiceNetwork.RpcRecordChunk(type, array);
				_003Coffset_003E5__3 += num2;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			voiceNetwork.RpcEndRecord(type);
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSendAllRoutine_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VoiceNetwork _003C_003E4__this;

		public NetworkConnectionToClient conn;

		private Dictionary<AnimalType, VoiceRecord>.Enumerator _003C_003E7__wrap1;

		private AnimalType _003Ctype_003E5__3;

		private VoiceRecord _003Crec_003E5__4;

		private int _003Coffset_003E5__5;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSendAllRoutine_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				VoiceNetwork voiceNetwork = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = voiceNetwork._serverStore.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					_003Coffset_003E5__5 = 0;
					goto IL_0152;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_0152;
					}
					IL_0152:
					if (_003Coffset_003E5__5 < _003Crec_003E5__4.pcm.Length)
					{
						int num2 = Mathf.Min(voiceNetwork.chunkSize, _003Crec_003E5__4.pcm.Length - _003Coffset_003E5__5);
						byte[] array = new byte[num2];
						Array.Copy(_003Crec_003E5__4.pcm, _003Coffset_003E5__5, array, 0, num2);
						voiceNetwork.TargetRecordChunk(conn, _003Ctype_003E5__3, array);
						_003Coffset_003E5__5 += num2;
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					voiceNetwork.TargetEndRecord(conn, _003Ctype_003E5__3);
					_003Crec_003E5__4 = null;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					KeyValuePair<AnimalType, VoiceRecord> current = _003C_003E7__wrap1.Current;
					_003Ctype_003E5__3 = current.Key;
					_003Crec_003E5__4 = current.Value;
					voiceNetwork.TargetBeginRecord(conn, _003Ctype_003E5__3, _003Crec_003E5__4.pcm.Length, _003Crec_003E5__4.sampleRate, _003Crec_003E5__4.channels);
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<AnimalType, VoiceRecord>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("Chunk Ayarı")]
	[Tooltip("Tek mesajda gönderilecek max byte (transport limitine göre)")]
	public int chunkSize = 8000;

	private readonly Dictionary<AnimalType, VoiceRecord> _serverStore = new Dictionary<AnimalType, VoiceRecord>();

	private readonly Dictionary<int, IncomingBuffer> _serverIncoming = new Dictionary<int, IncomingBuffer>();

	private readonly Dictionary<AnimalType, IncomingBuffer> _clientIncoming = new Dictionary<AnimalType, IncomingBuffer>();

	public static VoiceNetwork Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void UploadVoice(AnimalType type, byte[] pcm, int sampleRate, int channels)
	{
		if (pcm != null && pcm.Length != 0)
		{
			StartCoroutine(UploadRoutine(type, pcm, sampleRate, channels));
		}
	}

	private IEnumerator UploadRoutine(AnimalType type, byte[] pcm, int sampleRate, int channels)
	{
		CmdBeginUpload(type, pcm.Length, sampleRate, channels);
		yield return null;
		int offset = 0;
		while (offset < pcm.Length)
		{
			int num = Mathf.Min(chunkSize, pcm.Length - offset);
			byte[] array = new byte[num];
			Array.Copy(pcm, offset, array, 0, num);
			CmdUploadChunk(type, array);
			offset += num;
			yield return null;
		}
		CmdEndUpload(type);
	}

	[Command(requiresAuthority = false)]
	private void CmdBeginUpload(AnimalType type, int totalBytes, int sampleRate, int channels, NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteVarInt(totalBytes);
		writer.WriteVarInt(sampleRate);
		writer.WriteVarInt(channels);
		SendCommandInternal("System.Void VoiceNetwork::CmdBeginUpload(AnimalType,System.Int32,System.Int32,System.Int32,Mirror.NetworkConnectionToClient)", -1796514355, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	[Command(requiresAuthority = false)]
	private void CmdUploadChunk(AnimalType type, byte[] chunk, NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteBytesAndSize(chunk);
		SendCommandInternal("System.Void VoiceNetwork::CmdUploadChunk(AnimalType,System.Byte[],Mirror.NetworkConnectionToClient)", -761289287, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	[Command(requiresAuthority = false)]
	private void CmdEndUpload(AnimalType type, NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		SendCommandInternal("System.Void VoiceNetwork::CmdEndUpload(AnimalType,Mirror.NetworkConnectionToClient)", -1426624524, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	[IteratorStateMachine(typeof(_003CBroadcastRecord_003Ed__16))]
	[Server]
	private IEnumerator BroadcastRecord(AnimalType type)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator VoiceNetwork::BroadcastRecord(AnimalType)' called when server was not active");
			return null;
		}
		return new _003CBroadcastRecord_003Ed__16(0)
		{
			_003C_003E4__this = this,
			type = type
		};
	}

	[ClientRpc]
	private void RpcBeginRecord(AnimalType type, int totalBytes, int sampleRate, int channels)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteVarInt(totalBytes);
		writer.WriteVarInt(sampleRate);
		writer.WriteVarInt(channels);
		SendRPCInternal("System.Void VoiceNetwork::RpcBeginRecord(AnimalType,System.Int32,System.Int32,System.Int32)", -176466401, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcRecordChunk(AnimalType type, byte[] chunk)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteBytesAndSize(chunk);
		SendRPCInternal("System.Void VoiceNetwork::RpcRecordChunk(AnimalType,System.Byte[])", 135151537, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcEndRecord(AnimalType type)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		SendRPCInternal("System.Void VoiceNetwork::RpcEndRecord(AnimalType)", 2123151530, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void SendAllRecordsTo(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void VoiceNetwork::SendAllRecordsTo(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else
		{
			StartCoroutine(SendAllRoutine(conn));
		}
	}

	[IteratorStateMachine(typeof(_003CSendAllRoutine_003Ed__21))]
	[Server]
	private IEnumerator SendAllRoutine(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator VoiceNetwork::SendAllRoutine(Mirror.NetworkConnectionToClient)' called when server was not active");
			return null;
		}
		return new _003CSendAllRoutine_003Ed__21(0)
		{
			_003C_003E4__this = this,
			conn = conn
		};
	}

	[TargetRpc]
	private void TargetBeginRecord(NetworkConnectionToClient conn, AnimalType type, int totalBytes, int sampleRate, int channels)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteVarInt(totalBytes);
		writer.WriteVarInt(sampleRate);
		writer.WriteVarInt(channels);
		SendTargetRPCInternal(conn, "System.Void VoiceNetwork::TargetBeginRecord(Mirror.NetworkConnectionToClient,AnimalType,System.Int32,System.Int32,System.Int32)", 216559556, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetRecordChunk(NetworkConnectionToClient conn, AnimalType type, byte[] chunk)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		writer.WriteBytesAndSize(chunk);
		SendTargetRPCInternal(conn, "System.Void VoiceNetwork::TargetRecordChunk(Mirror.NetworkConnectionToClient,AnimalType,System.Byte[])", -2125587632, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetEndRecord(NetworkConnectionToClient conn, AnimalType type)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		SendTargetRPCInternal(conn, "System.Void VoiceNetwork::TargetEndRecord(Mirror.NetworkConnectionToClient,AnimalType)", 1434086267, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public bool HasRecord(AnimalType type)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Boolean VoiceNetwork::HasRecord(AnimalType)' called when server was not active");
			return default(bool);
		}
		return _serverStore.ContainsKey(type);
	}

	[Server]
	public void ServerClearAllRecords()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void VoiceNetwork::ServerClearAllRecords()' called when server was not active");
			return;
		}
		_serverStore.Clear();
		_serverIncoming.Clear();
		RpcClearAllRecords();
	}

	[ClientRpc]
	private void RpcClearAllRecords()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void VoiceNetwork::RpcClearAllRecords()", 847571968, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void ServerClearRecord(AnimalType type)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void VoiceNetwork::ServerClearRecord(AnimalType)' called when server was not active");
			return;
		}
		_serverStore.Remove(type);
		RpcClearRecord(type);
	}

	[ClientRpc]
	private void RpcClearRecord(AnimalType type)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_AnimalType(writer, type);
		SendRPCInternal("System.Void VoiceNetwork::RpcClearRecord(AnimalType)", -371510690, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdBeginUpload__AnimalType__Int32__Int32__Int32__NetworkConnectionToClient(AnimalType type, int totalBytes, int sampleRate, int channels, NetworkConnectionToClient sender)
	{
		int key = sender?.connectionId ?? (-1);
		_serverIncoming[key] = new IncomingBuffer
		{
			type = type,
			totalBytes = totalBytes,
			sampleRate = sampleRate,
			channels = channels,
			data = new byte[totalBytes],
			received = 0
		};
	}

	protected static void InvokeUserCode_CmdBeginUpload__AnimalType__Int32__Int32__Int32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdBeginUpload called on client.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_CmdBeginUpload__AnimalType__Int32__Int32__Int32__NetworkConnectionToClient(GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadVarInt(), reader.ReadVarInt(), reader.ReadVarInt(), senderConnection);
		}
	}

	protected void UserCode_CmdUploadChunk__AnimalType__Byte_005B_005D__NetworkConnectionToClient(AnimalType type, byte[] chunk, NetworkConnectionToClient sender)
	{
		int key = sender?.connectionId ?? (-1);
		if (_serverIncoming.TryGetValue(key, out var value) && value.received + chunk.Length <= value.totalBytes)
		{
			Array.Copy(chunk, 0, value.data, value.received, chunk.Length);
			value.received += chunk.Length;
		}
	}

	protected static void InvokeUserCode_CmdUploadChunk__AnimalType__Byte_005B_005D__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdUploadChunk called on client.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_CmdUploadChunk__AnimalType__Byte_005B_005D__NetworkConnectionToClient(GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadBytesAndSize(), senderConnection);
		}
	}

	protected void UserCode_CmdEndUpload__AnimalType__NetworkConnectionToClient(AnimalType type, NetworkConnectionToClient sender)
	{
		int key = sender?.connectionId ?? (-1);
		if (_serverIncoming.TryGetValue(key, out var value))
		{
			_serverStore[value.type] = new VoiceRecord
			{
				pcm = value.data,
				sampleRate = value.sampleRate,
				channels = value.channels
			};
			_serverIncoming.Remove(key);
			UnityEngine.Debug.Log($"[VoiceNetwork] Server kayıt aldı: {value.type} ({value.totalBytes} byte). Dağıtılıyor...");
			StartCoroutine(BroadcastRecord(value.type));
		}
	}

	protected static void InvokeUserCode_CmdEndUpload__AnimalType__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdEndUpload called on client.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_CmdEndUpload__AnimalType__NetworkConnectionToClient(GeneratedNetworkCode._Read_AnimalType(reader), senderConnection);
		}
	}

	protected void UserCode_RpcBeginRecord__AnimalType__Int32__Int32__Int32(AnimalType type, int totalBytes, int sampleRate, int channels)
	{
		_clientIncoming[type] = new IncomingBuffer
		{
			type = type,
			totalBytes = totalBytes,
			sampleRate = sampleRate,
			channels = channels,
			data = new byte[totalBytes],
			received = 0
		};
	}

	protected static void InvokeUserCode_RpcBeginRecord__AnimalType__Int32__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcBeginRecord called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_RpcBeginRecord__AnimalType__Int32__Int32__Int32(GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadVarInt(), reader.ReadVarInt(), reader.ReadVarInt());
		}
	}

	protected void UserCode_RpcRecordChunk__AnimalType__Byte_005B_005D(AnimalType type, byte[] chunk)
	{
		if (_clientIncoming.TryGetValue(type, out var value) && value.received + chunk.Length <= value.totalBytes)
		{
			Array.Copy(chunk, 0, value.data, value.received, chunk.Length);
			value.received += chunk.Length;
		}
	}

	protected static void InvokeUserCode_RpcRecordChunk__AnimalType__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcRecordChunk called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_RpcRecordChunk__AnimalType__Byte_005B_005D(GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadBytesAndSize());
		}
	}

	protected void UserCode_RpcEndRecord__AnimalType(AnimalType type)
	{
		if (_clientIncoming.TryGetValue(type, out var value))
		{
			if (VoiceClipStore.Instance != null)
			{
				VoiceClipStore.Instance.SetClipFromPcm(value.type, value.data, value.sampleRate, value.channels);
			}
			_clientIncoming.Remove(type);
			UnityEngine.Debug.Log($"[VoiceNetwork] Client kayıt aldı ve depoladı: {value.type}");
		}
	}

	protected static void InvokeUserCode_RpcEndRecord__AnimalType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcEndRecord called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_RpcEndRecord__AnimalType(GeneratedNetworkCode._Read_AnimalType(reader));
		}
	}

	protected void UserCode_TargetBeginRecord__NetworkConnectionToClient__AnimalType__Int32__Int32__Int32(NetworkConnectionToClient conn, AnimalType type, int totalBytes, int sampleRate, int channels)
	{
		_clientIncoming[type] = new IncomingBuffer
		{
			type = type,
			totalBytes = totalBytes,
			sampleRate = sampleRate,
			channels = channels,
			data = new byte[totalBytes],
			received = 0
		};
	}

	protected static void InvokeUserCode_TargetBeginRecord__NetworkConnectionToClient__AnimalType__Int32__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetBeginRecord called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_TargetBeginRecord__NetworkConnectionToClient__AnimalType__Int32__Int32__Int32(null, GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadVarInt(), reader.ReadVarInt(), reader.ReadVarInt());
		}
	}

	protected void UserCode_TargetRecordChunk__NetworkConnectionToClient__AnimalType__Byte_005B_005D(NetworkConnectionToClient conn, AnimalType type, byte[] chunk)
	{
		if (_clientIncoming.TryGetValue(type, out var value) && value.received + chunk.Length <= value.totalBytes)
		{
			Array.Copy(chunk, 0, value.data, value.received, chunk.Length);
			value.received += chunk.Length;
		}
	}

	protected static void InvokeUserCode_TargetRecordChunk__NetworkConnectionToClient__AnimalType__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetRecordChunk called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_TargetRecordChunk__NetworkConnectionToClient__AnimalType__Byte_005B_005D(null, GeneratedNetworkCode._Read_AnimalType(reader), reader.ReadBytesAndSize());
		}
	}

	protected void UserCode_TargetEndRecord__NetworkConnectionToClient__AnimalType(NetworkConnectionToClient conn, AnimalType type)
	{
		if (_clientIncoming.TryGetValue(type, out var value))
		{
			if (VoiceClipStore.Instance != null)
			{
				VoiceClipStore.Instance.SetClipFromPcm(value.type, value.data, value.sampleRate, value.channels);
			}
			_clientIncoming.Remove(type);
			UnityEngine.Debug.Log($"[VoiceNetwork] (LateJoin) Client kayıt aldı: {value.type}");
		}
	}

	protected static void InvokeUserCode_TargetEndRecord__NetworkConnectionToClient__AnimalType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetEndRecord called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_TargetEndRecord__NetworkConnectionToClient__AnimalType(null, GeneratedNetworkCode._Read_AnimalType(reader));
		}
	}

	protected void UserCode_RpcClearAllRecords()
	{
		_clientIncoming.Clear();
		VoiceClipStore.Instance?.ClearAll();
	}

	protected static void InvokeUserCode_RpcClearAllRecords(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcClearAllRecords called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_RpcClearAllRecords();
		}
	}

	protected void UserCode_RpcClearRecord__AnimalType(AnimalType type)
	{
		VoiceClipStore.Instance?.RemoveClip(type);
	}

	protected static void InvokeUserCode_RpcClearRecord__AnimalType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcClearRecord called on server.");
		}
		else
		{
			((VoiceNetwork)obj).UserCode_RpcClearRecord__AnimalType(GeneratedNetworkCode._Read_AnimalType(reader));
		}
	}

	static VoiceNetwork()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(VoiceNetwork), "System.Void VoiceNetwork::CmdBeginUpload(AnimalType,System.Int32,System.Int32,System.Int32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdBeginUpload__AnimalType__Int32__Int32__Int32__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterCommand(typeof(VoiceNetwork), "System.Void VoiceNetwork::CmdUploadChunk(AnimalType,System.Byte[],Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdUploadChunk__AnimalType__Byte_005B_005D__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterCommand(typeof(VoiceNetwork), "System.Void VoiceNetwork::CmdEndUpload(AnimalType,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdEndUpload__AnimalType__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::RpcBeginRecord(AnimalType,System.Int32,System.Int32,System.Int32)", InvokeUserCode_RpcBeginRecord__AnimalType__Int32__Int32__Int32);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::RpcRecordChunk(AnimalType,System.Byte[])", InvokeUserCode_RpcRecordChunk__AnimalType__Byte_005B_005D);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::RpcEndRecord(AnimalType)", InvokeUserCode_RpcEndRecord__AnimalType);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::RpcClearAllRecords()", InvokeUserCode_RpcClearAllRecords);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::RpcClearRecord(AnimalType)", InvokeUserCode_RpcClearRecord__AnimalType);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::TargetBeginRecord(Mirror.NetworkConnectionToClient,AnimalType,System.Int32,System.Int32,System.Int32)", InvokeUserCode_TargetBeginRecord__NetworkConnectionToClient__AnimalType__Int32__Int32__Int32);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::TargetRecordChunk(Mirror.NetworkConnectionToClient,AnimalType,System.Byte[])", InvokeUserCode_TargetRecordChunk__NetworkConnectionToClient__AnimalType__Byte_005B_005D);
		RemoteProcedureCalls.RegisterRpc(typeof(VoiceNetwork), "System.Void VoiceNetwork::TargetEndRecord(Mirror.NetworkConnectionToClient,AnimalType)", InvokeUserCode_TargetEndRecord__NetworkConnectionToClient__AnimalType);
	}
}
