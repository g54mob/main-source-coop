using System;
using System.Runtime.InteropServices;

namespace Photon.Voice
{
	public struct FrameBuffer
	{
		private readonly byte[] array;

		private readonly int offset;

		private readonly int count;

		private readonly IDisposable disposer;

		private bool disposed;

		private int refCnt;

		private GCHandle gcHandle;

		private IntPtr ptr;

		private bool pinned;

		internal static int statDisposerCreated = int.MaxValue;

		internal static int statDisposerDisposed = int.MaxValue;

		internal static int statPinned = int.MaxValue;

		internal static int statUnpinned = int.MaxValue;

		public IntPtr Ptr
		{
			get
			{
				if (!pinned && array != null)
				{
					gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
					ptr = IntPtr.Add(gcHandle.AddrOfPinnedObject(), offset);
					pinned = true;
				}
				return ptr;
			}
		}

		public byte[] Array => array;

		public int Length => count;

		public int Offset => offset;

		public FrameFlags Flags { get; }

		public byte FrameNum { get; }

		public bool IsFEC => (Flags & FrameFlags.FEC) != 0;

		public bool IsConfig => (Flags & FrameFlags.Config) != 0;

		public bool IsKeyframe => (Flags & FrameFlags.KeyFrame) != 0;

		public FrameBuffer(byte[] array, int offset, int count, FrameFlags flags, byte frameNum, IDisposable disposer)
		{
			this.array = array;
			this.offset = offset;
			this.count = count;
			Flags = flags;
			FrameNum = frameNum;
			this.disposer = disposer;
			disposed = false;
			refCnt = 1;
			gcHandle = default(GCHandle);
			ptr = IntPtr.Zero;
			pinned = false;
		}

		public FrameBuffer(byte[] array, FrameFlags flags, byte frameNum)
		{
			this.array = array;
			offset = 0;
			count = ((array != null) ? array.Length : 0);
			Flags = flags;
			FrameNum = frameNum;
			disposer = null;
			disposed = false;
			refCnt = 1;
			gcHandle = default(GCHandle);
			ptr = IntPtr.Zero;
			pinned = false;
		}

		public FrameBuffer(FrameBuffer from, int offset, int count, FrameFlags flags, byte frameNum)
		{
			this = from;
			this.offset = offset;
			this.count = count;
			Flags = flags;
			FrameNum = frameNum;
		}

		public void Retain()
		{
			refCnt++;
		}

		public void Release()
		{
			refCnt--;
			if (refCnt <= 0)
			{
				Dispose();
			}
		}

		private void Dispose()
		{
			if (pinned)
			{
				gcHandle.Free();
				pinned = false;
			}
			if (disposer != null && !disposed)
			{
				disposer.Dispose();
				disposed = true;
			}
		}

		public override string ToString()
		{
			if (array == null)
			{
				return "null";
			}
			FrameFlags frameFlags = Flags & FrameFlags.MaskFrag;
			return "#" + FrameNum + " " + frameFlags.ToString() + ((frameFlags == FrameFlags.FragNotEnd) ? (" c#" + array[offset + count - 1]) : "") + (IsFEC ? " FEC" : "") + " r#" + refCnt;
		}
	}
}
