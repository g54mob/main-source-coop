using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class RestorationNetworkSync : NetworkBehaviour
	{
		private const int MaxChunkSize = 48000;

		private RestorableVehicleSurface[] _surfaces;

		private readonly SyncDictionary<byte, float> _paintAges = new SyncDictionary<byte, float>();

		private bool _surfacesRegistered;

		private readonly Dictionary<(byte, byte), byte[][]> _chunkBuffers = new Dictionary<(byte, byte), byte[][]>();

		private readonly Dictionary<(byte, byte), byte[][]> _serverChunkBuffers = new Dictionary<(byte, byte), byte[][]>();

		private void Start()
		{
			RegisterSurfaces();
		}

		private void RegisterSurfaces()
		{
			if (!_surfacesRegistered)
			{
				_surfaces = GetComponentsInChildren<RestorableVehicleSurface>();
				for (byte b = 0; b < _surfaces.Length; b++)
				{
					_surfaces[b].RegisterWithNetworkSync(this, b);
				}
				_surfacesRegistered = true;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncDictionary<byte, float> paintAges = _paintAges;
			paintAges.OnChange = (Action<SyncIDictionary<byte, float>.Operation, byte, float>)Delegate.Combine(paintAges.OnChange, new Action<SyncIDictionary<byte, float>.Operation, byte, float>(OnPaintAgeChanged));
			if (!base.isServer)
			{
				CmdRequestTextureSync();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			SyncDictionary<byte, float> paintAges = _paintAges;
			paintAges.OnChange = (Action<SyncIDictionary<byte, float>.Operation, byte, float>)Delegate.Remove(paintAges.OnChange, new Action<SyncIDictionary<byte, float>.Operation, byte, float>(OnPaintAgeChanged));
		}

		private void OnPaintAgeChanged(SyncIDictionary<byte, float>.Operation op, byte key, float item)
		{
			if (!base.isServer && _surfaces != null && key < _surfaces.Length && _paintAges.TryGetValue(key, out var value))
			{
				_surfaces[key].SetPaintAge(value);
			}
		}

		[Server]
		public void NotifyPaintApplied(byte surfaceIndex)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::NotifyPaintApplied(System.Byte)' called when server was not active");
			}
			else
			{
				_paintAges[surfaceIndex] = 0f;
			}
		}

		[Server]
		public void SetPaintAge(byte surfaceIndex, float age)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::SetPaintAge(System.Byte,System.Single)' called when server was not active");
			}
			else
			{
				_paintAges[surfaceIndex] = age;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestTextureSync(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdRequestTextureSync(Mirror.NetworkConnectionToClient)", 582203670, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private async UniTaskVoid SendTextureSyncAsync(NetworkConnectionToClient conn, CancellationToken ct)
		{
			await UniTask.Yield(ct);
			RegisterSurfaces();
			if (_surfaces == null || _surfaces.Length == 0)
			{
				return;
			}
			for (byte i = 0; i < _surfaces.Length; i++)
			{
				if (ct.IsCancellationRequested)
				{
					return;
				}
				RestorableVehicleSurface restorableVehicleSurface = _surfaces[i];
				SendTextureData(conn, i, 0, restorableVehicleSurface.PaintColorTexture);
				SendTextureData(conn, i, 1, restorableVehicleSurface.DirtMaskTexture);
				SendTextureData(conn, i, 2, restorableVehicleSurface.RustMaskTexture);
				SendTextureData(conn, i, 3, restorableVehicleSurface.PaintMaskTexture);
				SendTextureData(conn, i, 4, restorableVehicleSurface.PolishMaskTexture);
				SendTextureData(conn, i, 5, restorableVehicleSurface.MetalMaskTexture);
				await UniTask.Yield(ct);
			}
			foreach (KeyValuePair<byte, float> paintAge in _paintAges)
			{
				if (ct.IsCancellationRequested)
				{
					break;
				}
				TargetRpcReceivePaintAge(conn, paintAge.Key, paintAge.Value);
			}
		}

		private void SendTextureData(NetworkConnectionToClient conn, byte surfaceIndex, byte texType, CwPaintableTexture tex)
		{
			if (tex == null || !tex.Activated)
			{
				return;
			}
			byte[] pngData = tex.GetPngData();
			if (pngData == null || pngData.Length == 0)
			{
				return;
			}
			if (pngData.Length <= 48000)
			{
				TargetRpcReceiveTexture(conn, surfaceIndex, texType, pngData);
				return;
			}
			int num = (pngData.Length + 48000 - 1) / 48000;
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 48000;
				int num3 = Math.Min(48000, pngData.Length - num2);
				byte[] array = new byte[num3];
				Buffer.BlockCopy(pngData, num2, array, 0, num3);
				TargetRpcReceiveTextureChunk(conn, surfaceIndex, texType, (byte)i, (byte)num, array);
			}
		}

		[TargetRpc]
		private void TargetRpcReceiveTexture(NetworkConnectionToClient _, byte surfaceIndex, byte texType, byte[] data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			writer.WriteBytesAndSize(data);
			SendTargetRPCInternal(_, "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceiveTexture(Mirror.NetworkConnectionToClient,System.Byte,System.Byte,System.Byte[])", -1915382692, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetRpcReceiveTextureChunk(NetworkConnectionToClient _, byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			NetworkWriterExtensions.WriteByte(writer, chunkIndex);
			NetworkWriterExtensions.WriteByte(writer, totalChunks);
			writer.WriteBytesAndSize(chunkData);
			SendTargetRPCInternal(_, "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceiveTextureChunk(Mirror.NetworkConnectionToClient,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", -1154335721, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void ApplyReceivedTextureData(byte surfaceIndex, byte texType, byte[] data)
		{
			if (_surfaces == null || surfaceIndex >= _surfaces.Length)
			{
				return;
			}
			RestorableVehicleSurface restorableVehicleSurface = _surfaces[surfaceIndex];
			CwPaintableMeshTexture cwPaintableMeshTexture = texType switch
			{
				0 => restorableVehicleSurface.PaintColorTexture, 
				1 => restorableVehicleSurface.DirtMaskTexture, 
				2 => restorableVehicleSurface.RustMaskTexture, 
				3 => restorableVehicleSurface.PaintMaskTexture, 
				4 => restorableVehicleSurface.PolishMaskTexture, 
				5 => restorableVehicleSurface.MetalMaskTexture, 
				_ => null, 
			};
			if (!(cwPaintableMeshTexture == null))
			{
				if (!cwPaintableMeshTexture.Activated)
				{
					LoadTextureWhenActivated(cwPaintableMeshTexture, data, this.GetCancellationTokenOnDestroy()).Forget();
				}
				else
				{
					cwPaintableMeshTexture.LoadFromData(data);
				}
			}
		}

		private async UniTaskVoid LoadTextureWhenActivated(CwPaintableTexture tex, byte[] data, CancellationToken ct)
		{
			for (int i = 0; i < 50; i++)
			{
				if (ct.IsCancellationRequested)
				{
					break;
				}
				if (tex.Activated)
				{
					tex.LoadFromData(data);
					break;
				}
				await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
			}
		}

		[TargetRpc]
		private void TargetRpcReceivePaintAge(NetworkConnectionToClient _, byte surfaceIndex, float age)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			writer.WriteFloat(age);
			SendTargetRPCInternal(_, "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceivePaintAge(Mirror.NetworkConnectionToClient,System.Byte,System.Single)", 324842755, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public void SyncSurfaceOperation(byte surfaceIndex, SurfaceOperation op)
		{
			if (base.isServer)
			{
				RpcExecuteSurfaceOperation(surfaceIndex, (byte)op);
			}
			else
			{
				CmdSyncSurfaceOperation(surfaceIndex, (byte)op);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncSurfaceOperation(byte surfaceIndex, byte op)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, op);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncSurfaceOperation(System.Byte,System.Byte)", -1813907419, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcExecuteSurfaceOperation(byte surfaceIndex, byte op)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, op);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcExecuteSurfaceOperation(System.Byte,System.Byte)", -1656587252, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public void SyncSurfaceTextures(byte surfaceIndex)
		{
			if (_surfaces != null && surfaceIndex < _surfaces.Length)
			{
				RestorableVehicleSurface surface = _surfaces[surfaceIndex];
				if (base.isServer)
				{
					BroadcastSurfaceTextures(surfaceIndex, surface);
				}
				else
				{
					SendSurfaceTexturesToServer(surfaceIndex, surface);
				}
			}
		}

		private void BroadcastSurfaceTextures(byte surfaceIndex, RestorableVehicleSurface surface)
		{
			BroadcastTextureIfActive(surfaceIndex, 0, surface.PaintColorTexture);
			BroadcastTextureIfActive(surfaceIndex, 1, surface.DirtMaskTexture);
			BroadcastTextureIfActive(surfaceIndex, 2, surface.RustMaskTexture);
			BroadcastTextureIfActive(surfaceIndex, 3, surface.PaintMaskTexture);
			BroadcastTextureIfActive(surfaceIndex, 4, surface.PolishMaskTexture);
			BroadcastTextureIfActive(surfaceIndex, 5, surface.MetalMaskTexture);
		}

		private void BroadcastTextureIfActive(byte surfaceIndex, byte texType, CwPaintableTexture tex)
		{
			if (tex == null || !tex.Activated)
			{
				return;
			}
			byte[] pngData = tex.GetPngData();
			if (pngData == null || pngData.Length == 0)
			{
				return;
			}
			if (pngData.Length <= 48000)
			{
				RpcReceiveSurfaceTexture(surfaceIndex, texType, pngData);
				return;
			}
			int num = (pngData.Length + 48000 - 1) / 48000;
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 48000;
				int num3 = Math.Min(48000, pngData.Length - num2);
				byte[] array = new byte[num3];
				Buffer.BlockCopy(pngData, num2, array, 0, num3);
				RpcReceiveSurfaceTextureChunk(surfaceIndex, texType, (byte)i, (byte)num, array);
			}
		}

		private void SendSurfaceTexturesToServer(byte surfaceIndex, RestorableVehicleSurface surface)
		{
			SendTextureToServer(surfaceIndex, 0, surface.PaintColorTexture);
			SendTextureToServer(surfaceIndex, 1, surface.DirtMaskTexture);
			SendTextureToServer(surfaceIndex, 2, surface.RustMaskTexture);
			SendTextureToServer(surfaceIndex, 3, surface.PaintMaskTexture);
			SendTextureToServer(surfaceIndex, 4, surface.PolishMaskTexture);
			SendTextureToServer(surfaceIndex, 5, surface.MetalMaskTexture);
		}

		private void SendTextureToServer(byte surfaceIndex, byte texType, CwPaintableTexture tex)
		{
			if (tex == null || !tex.Activated)
			{
				return;
			}
			byte[] pngData = tex.GetPngData();
			if (pngData == null || pngData.Length == 0)
			{
				return;
			}
			if (pngData.Length <= 48000)
			{
				CmdSyncTexture(surfaceIndex, texType, pngData);
				return;
			}
			int num = (pngData.Length + 48000 - 1) / 48000;
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 48000;
				int num3 = Math.Min(48000, pngData.Length - num2);
				byte[] array = new byte[num3];
				Buffer.BlockCopy(pngData, num2, array, 0, num3);
				CmdSyncTextureChunk(surfaceIndex, texType, (byte)i, (byte)num, array);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncTexture(byte surfaceIndex, byte texType, byte[] data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			writer.WriteBytesAndSize(data);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncTexture(System.Byte,System.Byte,System.Byte[])", 430176289, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncTextureChunk(byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			NetworkWriterExtensions.WriteByte(writer, chunkIndex);
			NetworkWriterExtensions.WriteByte(writer, totalChunks);
			writer.WriteBytesAndSize(chunkData);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncTextureChunk(System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", 564379012, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void ReassembleAndApplyOnServer(byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			(byte, byte) key = (surfaceIndex, texType);
			if (!_serverChunkBuffers.TryGetValue(key, out var value))
			{
				value = new byte[totalChunks][];
				_serverChunkBuffers[key] = value;
			}
			value[chunkIndex] = chunkData;
			for (int i = 0; i < totalChunks; i++)
			{
				if (value[i] == null)
				{
					return;
				}
			}
			_serverChunkBuffers.Remove(key);
			int num = 0;
			for (int j = 0; j < totalChunks; j++)
			{
				num += value[j].Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			for (int k = 0; k < totalChunks; k++)
			{
				Buffer.BlockCopy(value[k], 0, array, num2, value[k].Length);
				num2 += value[k].Length;
			}
			ApplyTextureData(surfaceIndex, texType, array);
		}

		[ClientRpc]
		private void RpcReceiveSurfaceTexture(byte surfaceIndex, byte texType, byte[] data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			writer.WriteBytesAndSize(data);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcReceiveSurfaceTexture(System.Byte,System.Byte,System.Byte[])", 2010550977, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcReceiveSurfaceTextureChunk(byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, surfaceIndex);
			NetworkWriterExtensions.WriteByte(writer, texType);
			NetworkWriterExtensions.WriteByte(writer, chunkIndex);
			NetworkWriterExtensions.WriteByte(writer, totalChunks);
			writer.WriteBytesAndSize(chunkData);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcReceiveSurfaceTextureChunk(System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", -2129169180, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void ApplyTextureData(byte surfaceIndex, byte texType, byte[] data)
		{
			if (_surfaces != null && surfaceIndex < _surfaces.Length)
			{
				RestorableVehicleSurface restorableVehicleSurface = _surfaces[surfaceIndex];
				CwPaintableMeshTexture cwPaintableMeshTexture = texType switch
				{
					0 => restorableVehicleSurface.PaintColorTexture, 
					1 => restorableVehicleSurface.DirtMaskTexture, 
					2 => restorableVehicleSurface.RustMaskTexture, 
					3 => restorableVehicleSurface.PaintMaskTexture, 
					4 => restorableVehicleSurface.PolishMaskTexture, 
					5 => restorableVehicleSurface.MetalMaskTexture, 
					_ => null, 
				};
				if (!(cwPaintableMeshTexture == null) && cwPaintableMeshTexture.Activated)
				{
					cwPaintableMeshTexture.LoadFromData(data);
				}
			}
		}

		public RestorationNetworkSync()
		{
			InitSyncObject(_paintAges);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestTextureSync__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			SendTextureSyncAsync(sender, this.GetCancellationTokenOnDestroy()).Forget();
		}

		protected static void InvokeUserCode_CmdRequestTextureSync__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestTextureSync called on client.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_CmdRequestTextureSync__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_TargetRpcReceiveTexture__NetworkConnectionToClient__Byte__Byte__Byte_005B_005D(NetworkConnectionToClient _, byte surfaceIndex, byte texType, byte[] data)
		{
			ApplyReceivedTextureData(surfaceIndex, texType, data);
		}

		protected static void InvokeUserCode_TargetRpcReceiveTexture__NetworkConnectionToClient__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetRpcReceiveTexture called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_TargetRpcReceiveTexture__NetworkConnectionToClient__Byte__Byte__Byte_005B_005D(null, NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		protected void UserCode_TargetRpcReceiveTextureChunk__NetworkConnectionToClient__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkConnectionToClient _, byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			(byte, byte) key = (surfaceIndex, texType);
			if (!_chunkBuffers.TryGetValue(key, out var value))
			{
				value = new byte[totalChunks][];
				_chunkBuffers[key] = value;
			}
			value[chunkIndex] = chunkData;
			for (int i = 0; i < totalChunks; i++)
			{
				if (value[i] == null)
				{
					return;
				}
			}
			_chunkBuffers.Remove(key);
			int num = 0;
			for (int j = 0; j < totalChunks; j++)
			{
				num += value[j].Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			for (int k = 0; k < totalChunks; k++)
			{
				Buffer.BlockCopy(value[k], 0, array, num2, value[k].Length);
				num2 += value[k].Length;
			}
			ApplyReceivedTextureData(surfaceIndex, texType, array);
		}

		protected static void InvokeUserCode_TargetRpcReceiveTextureChunk__NetworkConnectionToClient__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetRpcReceiveTextureChunk called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_TargetRpcReceiveTextureChunk__NetworkConnectionToClient__Byte__Byte__Byte__Byte__Byte_005B_005D(null, NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		protected void UserCode_TargetRpcReceivePaintAge__NetworkConnectionToClient__Byte__Single(NetworkConnectionToClient _, byte surfaceIndex, float age)
		{
			if (_surfaces != null && surfaceIndex < _surfaces.Length)
			{
				_surfaces[surfaceIndex].SetPaintAge(age);
			}
		}

		protected static void InvokeUserCode_TargetRpcReceivePaintAge__NetworkConnectionToClient__Byte__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetRpcReceivePaintAge called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_TargetRpcReceivePaintAge__NetworkConnectionToClient__Byte__Single(null, NetworkReaderExtensions.ReadByte(reader), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSyncSurfaceOperation__Byte__Byte(byte surfaceIndex, byte op)
		{
			if (_surfaces != null && surfaceIndex < _surfaces.Length)
			{
				_surfaces[surfaceIndex].ExecuteSurfaceOperation((SurfaceOperation)op);
			}
			RpcExecuteSurfaceOperation(surfaceIndex, op);
		}

		protected static void InvokeUserCode_CmdSyncSurfaceOperation__Byte__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncSurfaceOperation called on client.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_CmdSyncSurfaceOperation__Byte__Byte(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_RpcExecuteSurfaceOperation__Byte__Byte(byte surfaceIndex, byte op)
		{
			if (!base.isServer && _surfaces != null && surfaceIndex < _surfaces.Length)
			{
				_surfaces[surfaceIndex].ExecuteSurfaceOperation((SurfaceOperation)op);
			}
		}

		protected static void InvokeUserCode_RpcExecuteSurfaceOperation__Byte__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcExecuteSurfaceOperation called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_RpcExecuteSurfaceOperation__Byte__Byte(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdSyncTexture__Byte__Byte__Byte_005B_005D(byte surfaceIndex, byte texType, byte[] data)
		{
			ApplyTextureData(surfaceIndex, texType, data);
			RpcReceiveSurfaceTexture(surfaceIndex, texType, data);
		}

		protected static void InvokeUserCode_CmdSyncTexture__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncTexture called on client.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_CmdSyncTexture__Byte__Byte__Byte_005B_005D(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		protected void UserCode_CmdSyncTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			ReassembleAndApplyOnServer(surfaceIndex, texType, chunkIndex, totalChunks, chunkData);
			RpcReceiveSurfaceTextureChunk(surfaceIndex, texType, chunkIndex, totalChunks, chunkData);
		}

		protected static void InvokeUserCode_CmdSyncTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncTextureChunk called on client.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_CmdSyncTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		protected void UserCode_RpcReceiveSurfaceTexture__Byte__Byte__Byte_005B_005D(byte surfaceIndex, byte texType, byte[] data)
		{
			if (!base.isServer)
			{
				ApplyTextureData(surfaceIndex, texType, data);
			}
		}

		protected static void InvokeUserCode_RpcReceiveSurfaceTexture__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveSurfaceTexture called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_RpcReceiveSurfaceTexture__Byte__Byte__Byte_005B_005D(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		protected void UserCode_RpcReceiveSurfaceTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(byte surfaceIndex, byte texType, byte chunkIndex, byte totalChunks, byte[] chunkData)
		{
			if (base.isServer)
			{
				return;
			}
			(byte, byte) key = (surfaceIndex, texType);
			if (!_chunkBuffers.TryGetValue(key, out var value))
			{
				value = new byte[totalChunks][];
				_chunkBuffers[key] = value;
			}
			value[chunkIndex] = chunkData;
			for (int i = 0; i < totalChunks; i++)
			{
				if (value[i] == null)
				{
					return;
				}
			}
			_chunkBuffers.Remove(key);
			int num = 0;
			for (int j = 0; j < totalChunks; j++)
			{
				num += value[j].Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			for (int k = 0; k < totalChunks; k++)
			{
				Buffer.BlockCopy(value[k], 0, array, num2, value[k].Length);
				num2 += value[k].Length;
			}
			ApplyTextureData(surfaceIndex, texType, array);
		}

		protected static void InvokeUserCode_RpcReceiveSurfaceTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveSurfaceTextureChunk called on server.");
			}
			else
			{
				((RestorationNetworkSync)obj).UserCode_RpcReceiveSurfaceTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader), reader.ReadBytesAndSize());
			}
		}

		static RestorationNetworkSync()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdRequestTextureSync(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRequestTextureSync__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncSurfaceOperation(System.Byte,System.Byte)", InvokeUserCode_CmdSyncSurfaceOperation__Byte__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncTexture(System.Byte,System.Byte,System.Byte[])", InvokeUserCode_CmdSyncTexture__Byte__Byte__Byte_005B_005D, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::CmdSyncTextureChunk(System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", InvokeUserCode_CmdSyncTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcExecuteSurfaceOperation(System.Byte,System.Byte)", InvokeUserCode_RpcExecuteSurfaceOperation__Byte__Byte);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcReceiveSurfaceTexture(System.Byte,System.Byte,System.Byte[])", InvokeUserCode_RpcReceiveSurfaceTexture__Byte__Byte__Byte_005B_005D);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::RpcReceiveSurfaceTextureChunk(System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", InvokeUserCode_RpcReceiveSurfaceTextureChunk__Byte__Byte__Byte__Byte__Byte_005B_005D);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceiveTexture(Mirror.NetworkConnectionToClient,System.Byte,System.Byte,System.Byte[])", InvokeUserCode_TargetRpcReceiveTexture__NetworkConnectionToClient__Byte__Byte__Byte_005B_005D);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceiveTextureChunk(Mirror.NetworkConnectionToClient,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte[])", InvokeUserCode_TargetRpcReceiveTextureChunk__NetworkConnectionToClient__Byte__Byte__Byte__Byte__Byte_005B_005D);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationNetworkSync), "System.Void NomadDrive.Features.Restoration.RestorationNetworkSync::TargetRpcReceivePaintAge(Mirror.NetworkConnectionToClient,System.Byte,System.Single)", InvokeUserCode_TargetRpcReceivePaintAge__NetworkConnectionToClient__Byte__Single);
		}
	}
}
