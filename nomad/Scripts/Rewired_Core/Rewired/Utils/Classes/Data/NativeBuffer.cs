using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class NativeBuffer : IDisposable
	{
		private IntPtr KzrASGBQzkPHeGbPjgHVGIzDFaCpc;

		private int cctKyWiMFHTAaSecxwWfwWAoByWD;

		private bool tPHmrINQAjowvjjuWfMImNKobHBO;

		public IntPtr Pointer => KzrASGBQzkPHeGbPjgHVGIzDFaCpc;

		public int Length => cctKyWiMFHTAaSecxwWfwWAoByWD;

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index >= cctKyWiMFHTAaSecxwWfwWAoByWD)
				{
					throw new IndexOutOfRangeException();
				}
				return Marshal.ReadByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, index);
			}
			set
			{
				if (index < 0 || index >= cctKyWiMFHTAaSecxwWfwWAoByWD)
				{
					throw new IndexOutOfRangeException();
				}
				Marshal.WriteByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, index, value);
			}
		}

		public NativeBuffer(int P_0)
		{
			Resize(P_0, preserveData: false);
		}

		public IntPtr GetPointer(int offset = 0)
		{
			if (KzrASGBQzkPHeGbPjgHVGIzDFaCpc == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			if (offset == 0)
			{
				return KzrASGBQzkPHeGbPjgHVGIzDFaCpc;
			}
			if (offset < 0 || offset >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return NativeTools.OffsetIntPtr(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, offset);
		}

		public string DumpToHexString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < cctKyWiMFHTAaSecxwWfwWAoByWD; i++)
			{
				stringBuilder.Append(ReadByte(i).ToString("x2"));
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString();
		}

		public bool ReadBit(int byteIndex, byte bit)
		{
			if (1 + byteIndex > Length || byteIndex < 0)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			if (bit >= 8)
			{
				throw new ArgumentOutOfRangeException("bit");
			}
			return (Marshal.ReadByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, byteIndex) & (1 << (int)bit)) != 0;
		}

		public byte ReadByte(int startIndex)
		{
			if (1 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return Marshal.ReadByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public short ReadShort(int startIndex)
		{
			if (2 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return Marshal.ReadInt16(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public ushort ReadUShort(int startIndex)
		{
			if (2 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return (ushort)Marshal.ReadInt16(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public int ReadInt(int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return Marshal.ReadInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public uint ReadUInt(int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return (uint)Marshal.ReadInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public long ReadLong(int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return Marshal.ReadInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public ulong ReadULong(int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return (ulong)Marshal.ReadInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex);
		}

		public float ReadFloat(int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return new eZPRuEYHSdKzxpOIPYwKrgFSfSJe(Marshal.ReadInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex)).nmZvzWjziPhRkzpXIMlOnltAnufg;
		}

		public double ReadDouble(int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return new ptpxyONhuPjoTKFgeggMFPKuSBvNA(Marshal.ReadInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex)).FeDNyFolikqtmdlIllBWSLcwQpLK;
		}

		public void Read(byte[] buffer, int numBytesToRead, int readStartIndex = 0, int writeStartIndex = 0)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int num = buffer.Length;
			if (num <= 0)
			{
				throw new ArgumentOutOfRangeException("bytes.Length must be > 0.");
			}
			if (numBytesToRead <= 0)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be > 0");
			}
			if (numBytesToRead > num)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be <= bufferLength.");
			}
			if (numBytesToRead > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be <= Length.");
			}
			if (writeStartIndex >= num)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be < bufferLength.");
			}
			if (writeStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
			}
			if (readStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be < Length.");
			}
			if (readStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
			}
			if (writeStartIndex + numBytesToRead > num)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex + numBytesToRead must be < bufferLength.");
			}
			if (numBytesToRead + readStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
			}
			NativeTools.CopyMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, buffer, readStartIndex, writeStartIndex, numBytesToRead);
		}

		public void Read(IntPtr buffer, int bufferLength, int numBytesToRead, int readStartIndex = 0, int writeStartIndex = 0)
		{
			if (buffer == IntPtr.Zero)
			{
				throw new ArgumentNullException("bytes");
			}
			if (bufferLength <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferLength must be > 0.");
			}
			if (numBytesToRead <= 0)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be > 0");
			}
			if (numBytesToRead > bufferLength)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be <= bufferLength.");
			}
			if (numBytesToRead > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead must be <= Length.");
			}
			if (writeStartIndex >= bufferLength)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be < bufferLength.");
			}
			if (writeStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
			}
			if (readStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be < Length.");
			}
			if (readStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
			}
			if (writeStartIndex + numBytesToRead > bufferLength)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex + numBytesToRead must be < bufferLength.");
			}
			if (numBytesToRead + readStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
			}
			NativeTools.CopyMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, buffer, readStartIndex, writeStartIndex, numBytesToRead);
		}

		public int TryReadBytes(byte[] buffer, int numBytesToRead, int readStartIndex = 0, int writeStartIndex = 0)
		{
			if (buffer == null || numBytesToRead <= 0)
			{
				return 0;
			}
			int num = buffer.Length;
			if (num == 0)
			{
				return 0;
			}
			if (readStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				return 0;
			}
			if (writeStartIndex >= num)
			{
				return 0;
			}
			if (readStartIndex < 0)
			{
				readStartIndex = 0;
			}
			if (writeStartIndex < 0)
			{
				writeStartIndex = 0;
			}
			if (readStartIndex + numBytesToRead > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				numBytesToRead = cctKyWiMFHTAaSecxwWfwWAoByWD - readStartIndex;
			}
			if (writeStartIndex + numBytesToRead > num)
			{
				numBytesToRead = num - writeStartIndex;
			}
			if (numBytesToRead == 0)
			{
				return 0;
			}
			if (!NativeTools.CopyMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, buffer, readStartIndex, writeStartIndex, numBytesToRead, throwOnError: false))
			{
				return 0;
			}
			return numBytesToRead;
		}

		public int TryReadBytes(IntPtr buffer, int bufferLength, int numBytesToRead, int readStartIndex = 0, int writeStartIndex = 0)
		{
			if (buffer == IntPtr.Zero || numBytesToRead <= 0)
			{
				return 0;
			}
			if (readStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				return 0;
			}
			if (writeStartIndex >= bufferLength)
			{
				return 0;
			}
			if (readStartIndex < 0)
			{
				readStartIndex = 0;
			}
			if (writeStartIndex < 0)
			{
				writeStartIndex = 0;
			}
			if (readStartIndex + numBytesToRead > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				numBytesToRead = cctKyWiMFHTAaSecxwWfwWAoByWD - readStartIndex;
			}
			if (writeStartIndex + numBytesToRead > bufferLength)
			{
				numBytesToRead = bufferLength - writeStartIndex;
			}
			if (!NativeTools.CopyMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, buffer, readStartIndex, writeStartIndex, numBytesToRead, throwOnError: false))
			{
				return 0;
			}
			return numBytesToRead;
		}

		public void WriteBit(int byteIndex, byte bit, bool value)
		{
			if (1 + byteIndex > Length || byteIndex < 0)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			if (bit >= 8)
			{
				throw new ArgumentOutOfRangeException("bit");
			}
			if (value)
			{
				Marshal.WriteByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, byteIndex, (byte)(Marshal.ReadByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, byteIndex) | (byte)(1 << (int)bit)));
			}
			else
			{
				Marshal.WriteByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, byteIndex, (byte)(Marshal.ReadByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, byteIndex) & (byte)(~(1 << (int)bit))));
			}
		}

		public void Write(byte @byte, int startIndex)
		{
			if (1 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteByte(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, @byte);
		}

		public void Write(short bytes, int startIndex)
		{
			if (2 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt16(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, bytes);
		}

		public void Write(ushort bytes, int startIndex)
		{
			if (2 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt16(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, (short)bytes);
		}

		public void Write(int bytes, int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, bytes);
		}

		public void Write(uint bytes, int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, (int)bytes);
		}

		public void Write(long bytes, int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, bytes);
		}

		public void Write(ulong bytes, int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, (long)bytes);
		}

		public void Write(float bytes, int startIndex)
		{
			if (4 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt32(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, new eZPRuEYHSdKzxpOIPYwKrgFSfSJe(bytes).PBLXOQoRhflJpgYLYUieWNvLAFyP);
		}

		public void Write(double bytes, int startIndex)
		{
			if (8 + startIndex > cctKyWiMFHTAaSecxwWfwWAoByWD || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			Marshal.WriteInt64(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, startIndex, new ptpxyONhuPjoTKFgeggMFPKuSBvNA(bytes).ZkiuauWKhGFHDVKXnrvTMVUJBlsW);
		}

		public void Write(byte[] bytes, int numBytesToWrite, int writeStartIndex = 0, int readStartIndex = 0)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int num = bytes.Length;
			if (num <= 0)
			{
				throw new ArgumentOutOfRangeException("bytes.Length must be > 0.");
			}
			if (numBytesToWrite <= 0)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be > 0");
			}
			if (numBytesToWrite > num)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be <= bufferLength.");
			}
			if (numBytesToWrite > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be <= Length.");
			}
			if (readStartIndex >= num)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be < bufferLength.");
			}
			if (readStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
			}
			if (writeStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be < Length.");
			}
			if (writeStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
			}
			if (readStartIndex + numBytesToWrite > num)
			{
				throw new ArgumentOutOfRangeException("readStartIndex + numBytesToWrite must be < bufferLength.");
			}
			if (numBytesToWrite + writeStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
			}
			NativeTools.CopyMemory(bytes, KzrASGBQzkPHeGbPjgHVGIzDFaCpc, readStartIndex, writeStartIndex, numBytesToWrite);
		}

		public void Write(IntPtr bytes, int bufferLength, int numBytesToWrite, int writeStartIndex = 0, int readStartIndex = 0)
		{
			if (bytes == IntPtr.Zero)
			{
				throw new ArgumentNullException("bytes");
			}
			if (bufferLength <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferLength must be > 0.");
			}
			if (numBytesToWrite <= 0)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be > 0");
			}
			if (numBytesToWrite > bufferLength)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be <= bufferLength.");
			}
			if (numBytesToWrite > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite must be <= Length.");
			}
			if (readStartIndex >= bufferLength)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be < bufferLength.");
			}
			if (readStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
			}
			if (writeStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be < Length.");
			}
			if (writeStartIndex < 0)
			{
				throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
			}
			if (readStartIndex + numBytesToWrite > bufferLength)
			{
				throw new ArgumentOutOfRangeException("readStartIndex + numBytesToWrite must be < bufferLength.");
			}
			if (numBytesToWrite + writeStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
			}
			NativeTools.CopyMemory(bytes, KzrASGBQzkPHeGbPjgHVGIzDFaCpc, readStartIndex, writeStartIndex, numBytesToWrite);
		}

		public int TryWriteBytes(byte[] bytes, int numBytesToWrite, int writeStartIndex = 0, int readStartIndex = 0)
		{
			if (bytes == null)
			{
				return 0;
			}
			int num = bytes.Length;
			if (num == 0 || numBytesToWrite <= 0 || readStartIndex >= num || writeStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				return 0;
			}
			if (readStartIndex < 0)
			{
				readStartIndex = 0;
			}
			if (writeStartIndex < 0)
			{
				writeStartIndex = 0;
			}
			if (readStartIndex + numBytesToWrite > num)
			{
				numBytesToWrite = num - readStartIndex;
			}
			if (numBytesToWrite + writeStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				numBytesToWrite = cctKyWiMFHTAaSecxwWfwWAoByWD - writeStartIndex;
			}
			if (!NativeTools.CopyMemory(bytes, KzrASGBQzkPHeGbPjgHVGIzDFaCpc, readStartIndex, writeStartIndex, numBytesToWrite, throwOnError: false))
			{
				return 0;
			}
			return numBytesToWrite;
		}

		public int TryWriteBytes(IntPtr bytes, int bufferLength, int numBytesToWrite, int writeStartIndex = 0, int readStartIndex = 0)
		{
			if (bytes == IntPtr.Zero || bufferLength <= 0 || numBytesToWrite <= 0 || readStartIndex >= bufferLength || writeStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				return 0;
			}
			if (readStartIndex < 0)
			{
				readStartIndex = 0;
			}
			if (writeStartIndex < 0)
			{
				writeStartIndex = 0;
			}
			if (readStartIndex + numBytesToWrite > bufferLength)
			{
				numBytesToWrite = bufferLength - readStartIndex;
			}
			if (numBytesToWrite + writeStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				numBytesToWrite = cctKyWiMFHTAaSecxwWfwWAoByWD - writeStartIndex;
			}
			if (!NativeTools.CopyMemory(bytes, KzrASGBQzkPHeGbPjgHVGIzDFaCpc, readStartIndex, writeStartIndex, numBytesToWrite, throwOnError: false))
			{
				return 0;
			}
			return numBytesToWrite;
		}

		public int TryFill(byte value, int numBytesToWrite, int writeStartIndex = 0)
		{
			if (numBytesToWrite <= 0 || writeStartIndex >= cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				return 0;
			}
			if (writeStartIndex < 0)
			{
				writeStartIndex = 0;
			}
			if (numBytesToWrite + writeStartIndex > cctKyWiMFHTAaSecxwWfwWAoByWD)
			{
				numBytesToWrite = cctKyWiMFHTAaSecxwWfwWAoByWD - writeStartIndex;
			}
			if (!NativeTools.FillMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, writeStartIndex, numBytesToWrite, value, throwOnError: false))
			{
				return 0;
			}
			return numBytesToWrite;
		}

		public bool Resize(int size, bool preserveData)
		{
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (cctKyWiMFHTAaSecxwWfwWAoByWD == size)
			{
				return true;
			}
			if (size == 0)
			{
				Release();
				return true;
			}
			IntPtr intPtr;
			if (preserveData)
			{
				try
				{
					intPtr = Marshal.AllocHGlobal(size);
					if (intPtr == IntPtr.Zero)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				int bytesToCopy = MathTools.Min(size, cctKyWiMFHTAaSecxwWfwWAoByWD);
				if (!NativeTools.CopyMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, intPtr, 0, 0, bytesToCopy, throwOnError: false))
				{
					Marshal.FreeHGlobal(intPtr);
					return false;
				}
				if (size > cctKyWiMFHTAaSecxwWfwWAoByWD)
				{
					NativeTools.FillMemory(intPtr, cctKyWiMFHTAaSecxwWfwWAoByWD, size - cctKyWiMFHTAaSecxwWfwWAoByWD, 0, throwOnError: false);
				}
				Release();
			}
			else
			{
				Release();
				try
				{
					intPtr = Marshal.AllocHGlobal(size);
					if (intPtr == IntPtr.Zero)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				NativeTools.ZeroFillMemory(intPtr, size);
			}
			KzrASGBQzkPHeGbPjgHVGIzDFaCpc = intPtr;
			cctKyWiMFHTAaSecxwWfwWAoByWD = size;
			return true;
		}

		public void Clear()
		{
			if (cctKyWiMFHTAaSecxwWfwWAoByWD != 0)
			{
				NativeTools.ZeroFillMemory(KzrASGBQzkPHeGbPjgHVGIzDFaCpc, cctKyWiMFHTAaSecxwWfwWAoByWD);
			}
		}

		public void Release()
		{
			if (KzrASGBQzkPHeGbPjgHVGIzDFaCpc != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(Pointer);
				KzrASGBQzkPHeGbPjgHVGIzDFaCpc = IntPtr.Zero;
			}
			cctKyWiMFHTAaSecxwWfwWAoByWD = 0;
		}

		public void CopyFrom(NativeBuffer other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (!(KzrASGBQzkPHeGbPjgHVGIzDFaCpc == IntPtr.Zero) && !(other.Pointer == IntPtr.Zero))
			{
				int bytesToCopy = MathTools.Min(cctKyWiMFHTAaSecxwWfwWAoByWD, other.cctKyWiMFHTAaSecxwWfwWAoByWD);
				NativeTools.CopyMemory(other.KzrASGBQzkPHeGbPjgHVGIzDFaCpc, KzrASGBQzkPHeGbPjgHVGIzDFaCpc, 0, 0, bytesToCopy);
			}
		}

		public override string ToString()
		{
			return "Length = " + cctKyWiMFHTAaSecxwWfwWAoByWD + "\nPointer = " + KzrASGBQzkPHeGbPjgHVGIzDFaCpc + "\n";
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~NativeBuffer()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!tPHmrINQAjowvjjuWfMImNKobHBO)
			{
				Release();
				tPHmrINQAjowvjjuWfMImNKobHBO = true;
			}
		}

		public static implicit operator IntPtr(NativeBuffer buffer)
		{
			return buffer?.KzrASGBQzkPHeGbPjgHVGIzDFaCpc ?? IntPtr.Zero;
		}

		public static bool Copy(NativeBuffer source, NativeBuffer destination)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (source.cctKyWiMFHTAaSecxwWfwWAoByWD == 0)
			{
				destination.Release();
				return true;
			}
			if (destination.Resize(source.cctKyWiMFHTAaSecxwWfwWAoByWD, preserveData: false))
			{
				return NativeTools.CopyMemory(source.KzrASGBQzkPHeGbPjgHVGIzDFaCpc, destination.KzrASGBQzkPHeGbPjgHVGIzDFaCpc, 0, 0, source.cctKyWiMFHTAaSecxwWfwWAoByWD, throwOnError: false);
			}
			return false;
		}
	}
}
