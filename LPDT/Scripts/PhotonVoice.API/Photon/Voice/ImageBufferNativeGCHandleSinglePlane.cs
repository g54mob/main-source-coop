using System;
using System.Runtime.InteropServices;

namespace Photon.Voice
{
	[Obsolete("Requres regular allocations of byte[]. May leak if used w/o pool. Use ImageBufferNativeGCHandleBytes with Texture2D.GetRawTextureData<byte>().CopyTo(b.PlaneBytes)) instead")]
	public class ImageBufferNativeGCHandleSinglePlane : ImageBufferNative, IDisposable
	{
		private ImageBufferNativePool<ImageBufferNativeGCHandleSinglePlane> pool;

		private GCHandle planeHandle;

		public ImageBufferNativeGCHandleSinglePlane(ImageBufferNativePool<ImageBufferNativeGCHandleSinglePlane> pool, ImageBufferInfo info)
			: base(info)
		{
			if (info.Stride.Length != 1)
			{
				throw new Exception("ImageBufferNativeGCHandleSinglePlane wrong plane count " + info.Stride.Length);
			}
			this.pool = pool;
		}

		public void PinPlane(byte[] plane)
		{
			planeHandle = GCHandle.Alloc(plane, GCHandleType.Pinned);
			Planes[0] = planeHandle.AddrOfPinnedObject();
		}

		protected override void Free()
		{
			if (pool != null)
			{
				Reset();
				pool.Free(this);
			}
			else
			{
				Dispose();
			}
		}

		public override void Dispose()
		{
		}
	}
}
