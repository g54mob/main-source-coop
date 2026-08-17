using System;
using System.Runtime.InteropServices;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class NativeMemoryBlock : IDisposable
	{
		private int giFqQhidptVHBWdspiuPoqiXuDYk;

		private uint opEvdwVhKIryXTdsJCiicWZuAdfj;

		private IntPtr BGvNVutlPNenmSkGpbjeAalXSQWhA;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public uint size => opEvdwVhKIryXTdsJCiicWZuAdfj;

		public NativeMemoryBlock(uint P_0)
		{
			if (P_0 == 0)
			{
				throw new Exception("size must be > 0!");
			}
			opEvdwVhKIryXTdsJCiicWZuAdfj = P_0;
			giFqQhidptVHBWdspiuPoqiXuDYk = 0;
			try
			{
				BGvNVutlPNenmSkGpbjeAalXSQWhA = Marshal.AllocHGlobal((int)P_0);
				if (BGvNVutlPNenmSkGpbjeAalXSQWhA == IntPtr.Zero)
				{
					throw new Exception("Could not allocate native memory.");
				}
			}
			catch
			{
				throw;
			}
		}

		public IntPtr Allocate(uint bytes, IntPtr ptrToData)
		{
			if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				return IntPtr.Zero;
			}
			if (bytes == 0)
			{
				return IntPtr.Zero;
			}
			if (bytes > opEvdwVhKIryXTdsJCiicWZuAdfj)
			{
				return IntPtr.Zero;
			}
			if (giFqQhidptVHBWdspiuPoqiXuDYk + bytes >= opEvdwVhKIryXTdsJCiicWZuAdfj)
			{
				giFqQhidptVHBWdspiuPoqiXuDYk = 0;
			}
			IntPtr intPtr = new IntPtr(BGvNVutlPNenmSkGpbjeAalXSQWhA.ToInt64() + giFqQhidptVHBWdspiuPoqiXuDYk);
			if (ptrToData != IntPtr.Zero)
			{
				NativeTools.CopyMemory(ptrToData, intPtr, 0, 0, (int)bytes);
			}
			giFqQhidptVHBWdspiuPoqiXuDYk += (int)bytes;
			return intPtr;
		}

		public IntPtr Allocate(uint bytes)
		{
			return Allocate(bytes, IntPtr.Zero);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~NativeMemoryBlock()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
				if (BGvNVutlPNenmSkGpbjeAalXSQWhA != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(BGvNVutlPNenmSkGpbjeAalXSQWhA);
					BGvNVutlPNenmSkGpbjeAalXSQWhA = IntPtr.Zero;
				}
			}
		}
	}
}
