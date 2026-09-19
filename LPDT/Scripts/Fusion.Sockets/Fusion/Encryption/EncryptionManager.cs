#define TRACE
using System;
using System.Collections.Generic;

namespace Fusion.Encryption
{
	internal class EncryptionManager<THandler, TEncryption> : IDisposable where THandler : IEquatable<THandler> where TEncryption : IDataEncryption, new()
	{
		private readonly Dictionary<THandler, TEncryption> _cyphers = new Dictionary<THandler, TEncryption>();

		public void Dispose()
		{
			InternalLogStreams.LogTraceEncryption?.Log("Disposing EncryptionManager...");
			foreach (TEncryption value in _cyphers.Values)
			{
				value.Dispose();
			}
			_cyphers.Clear();
		}

		public void RegisterEncryptionKey(THandler handle, byte[] key)
		{
			if (HasEncryptionForHandle(handle))
			{
				InternalLogStreams.LogTraceEncryption?.Warn($"RegisterEncryptionKey: handle={handle} already registered");
				return;
			}
			TEncryption value = new TEncryption();
			value.Setup(key);
			_cyphers[handle] = value;
			InternalLogStreams.LogTraceEncryption?.Log($"RegisterEncryptionKey: handle={handle} key={BinUtils.BytesToHex(key)}");
		}

		public void DeleteEncryptionKey(THandler handle)
		{
			if (_cyphers.TryGetValue(handle, out var value))
			{
				_cyphers.Remove(handle);
				value.Dispose();
				InternalLogStreams.LogTraceEncryption?.Log($"DeleteEncryptionKey: handle={handle}");
			}
		}

		public bool HasEncryptionForHandle(THandler handle)
		{
			return _cyphers.ContainsKey(handle);
		}

		public unsafe bool Wrap(THandler handle, byte* buffer, ref int length, int capacity)
		{
			using (HostProfiler.Markers.EncryptionWrap())
			{
				return Encrypt(handle, buffer, ref length, capacity) && ComputeHash(handle, buffer, ref length, capacity);
			}
		}

		public unsafe bool Unwrap(THandler handle, byte* buffer, ref int length, int capacity)
		{
			using (HostProfiler.Markers.EncryptionUnwrap())
			{
				return VerifyHash(handle, buffer, ref length, capacity) && Decrypt(handle, buffer, ref length, capacity);
			}
		}

		public byte[] GenerateKey()
		{
			return new TEncryption().GenerateKey();
		}

		public bool ValidateKey(byte[] key)
		{
			return new TEncryption().ValidateKey(key);
		}

		public unsafe bool ComputeHash(THandler handle, byte* buffer, ref int length, int capacity)
		{
			if (_cyphers.TryGetValue(handle, out var value))
			{
				return value.ComputeHash(buffer, ref length, capacity);
			}
			InternalLogStreams.LogTraceEncryption?.Warn($"ComputeHash: handle={handle} not found");
			return false;
		}

		public unsafe bool VerifyHash(THandler handle, byte* buffer, ref int length, int capacity)
		{
			if (_cyphers.TryGetValue(handle, out var value))
			{
				return value.VerifyHash(buffer, ref length, capacity);
			}
			InternalLogStreams.LogTraceEncryption?.Warn($"VerifyHash: handle={handle} not found");
			return false;
		}

		public unsafe bool Encrypt(THandler handle, byte* buffer, ref int length, int capacity)
		{
			if (_cyphers.TryGetValue(handle, out var value))
			{
				return value.EncryptData(buffer, ref length, capacity);
			}
			InternalLogStreams.LogTraceEncryption?.Warn($"Encrypt: handle={handle} not found");
			return false;
		}

		public unsafe bool Decrypt(THandler handle, byte* buffer, ref int length, int capacity)
		{
			if (_cyphers.TryGetValue(handle, out var value))
			{
				return value.DecryptData(buffer, ref length, capacity);
			}
			InternalLogStreams.LogTraceEncryption?.Warn($"Decrypt: handle={handle} not found");
			return false;
		}
	}
}
