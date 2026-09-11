using System;
using System.Runtime.InteropServices;

namespace Photon.Voice
{
	public class ImageBufferNativeGCHandleBytes : ImageBufferNative, IDisposable
	{
		private ImageBufferNativePool<ImageBufferNativeGCHandleBytes> pool;

		private readonly GCHandle[] planeHandle;

		private readonly byte[][] planeBytes;

		public byte[][] PlaneBytes => planeBytes;

		public ImageBufferNativeGCHandleBytes(ImageBufferNativePool<ImageBufferNativeGCHandleBytes> pool, ImageBufferInfo info)
			: base(info)
		{
			this.pool = pool;
			planeBytes = new byte[info.Stride.Length][];
			planeHandle = new GCHandle[info.Stride.Length];
			for (int i = 0; i < info.Stride.Length; i++)
			{
				planeBytes[i] = new byte[info.Stride[i] * info.Height];
				planeHandle[i] = GCHandle.Alloc(planeBytes[i], GCHandleType.Pinned);
				Planes[i] = planeHandle[i].AddrOfPinnedObject();
			}
		}

		public override void Release()
		{
			if (pool != null)
			{
				pool.Release(this);
			}
		}

		public override void Dispose()
		{
			if (planeHandle != null)
			{
				for (int i = 0; i < planeHandle.Length; i++)
				{
					planeHandle[i].Free();
				}
			}
		}
	}
}
