using System;
using System.Runtime.InteropServices;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class NativeMemoryBlock : IDisposable
	{
		private int vWhiMTusVGIiNDwVoTZEAcLkkmLQ;

		private uint FWAxUdnYTQpAqYDCFaTHuCTZjtSt;

		private IntPtr dTqhIqjBlcnFeOUSxFdrYEUaxsMk;

		private bool dRcqMxRAYUZYlTCjlFXOJLjYCDVx;

		public uint size => FWAxUdnYTQpAqYDCFaTHuCTZjtSt;

		public NativeMemoryBlock(uint P_0)
		{
			if (P_0 == 0)
			{
				throw new Exception("size must be > 0!");
			}
			FWAxUdnYTQpAqYDCFaTHuCTZjtSt = P_0;
			vWhiMTusVGIiNDwVoTZEAcLkkmLQ = 0;
			try
			{
				dTqhIqjBlcnFeOUSxFdrYEUaxsMk = Marshal.AllocHGlobal((int)P_0);
				if (dTqhIqjBlcnFeOUSxFdrYEUaxsMk == IntPtr.Zero)
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
			if (dRcqMxRAYUZYlTCjlFXOJLjYCDVx)
			{
				return IntPtr.Zero;
			}
			if (bytes == 0)
			{
				return IntPtr.Zero;
			}
			if (bytes > FWAxUdnYTQpAqYDCFaTHuCTZjtSt)
			{
				return IntPtr.Zero;
			}
			if (vWhiMTusVGIiNDwVoTZEAcLkkmLQ + bytes >= FWAxUdnYTQpAqYDCFaTHuCTZjtSt)
			{
				vWhiMTusVGIiNDwVoTZEAcLkkmLQ = 0;
			}
			IntPtr intPtr = new IntPtr(dTqhIqjBlcnFeOUSxFdrYEUaxsMk.ToInt64() + vWhiMTusVGIiNDwVoTZEAcLkkmLQ);
			if (ptrToData != IntPtr.Zero)
			{
				NativeTools.CopyMemory(ptrToData, intPtr, 0, 0, (int)bytes);
			}
			vWhiMTusVGIiNDwVoTZEAcLkkmLQ += (int)bytes;
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

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~NativeMemoryBlock()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!dRcqMxRAYUZYlTCjlFXOJLjYCDVx)
			{
				dRcqMxRAYUZYlTCjlFXOJLjYCDVx = true;
				if (dTqhIqjBlcnFeOUSxFdrYEUaxsMk != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dTqhIqjBlcnFeOUSxFdrYEUaxsMk);
					dTqhIqjBlcnFeOUSxFdrYEUaxsMk = IntPtr.Zero;
				}
			}
		}
	}
}
