using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class LowLevelInputEventQueue : IDisposable
	{
		private LowLevelInputEvent jQnDCEiGrPvTInHPXOutqkHTcrREA;

		private readonly NativeRingBuffer itHdidhKeNhyxSMdpUhPbOWjMIjcA;

		private readonly int QJpgCvdDxIIaukCWckDkuElRUIGE;

		private readonly int gsGmLwrXoBgMTRxYcaAlQzdPHlrW;

		private readonly int nGzbpVDglcowGGmUjmoNUvXQietbA;

		private readonly int LurXhdJARCjJKhyRgUrsNxaPIdqh;

		private readonly int eAZTOidCgblheDmZfgTMJcXnhJLfb;

		private uint qFfbxrIaBJhimcEZDXJspkjRkDIkA;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public int Count => itHdidhKeNhyxSMdpUhPbOWjMIjcA.BytesInBuffer / LurXhdJARCjJKhyRgUrsNxaPIdqh;

		public int Capacity => eAZTOidCgblheDmZfgTMJcXnhJLfb;

		public LowLevelInputEvent this[int index] => new LowLevelInputEvent(itHdidhKeNhyxSMdpUhPbOWjMIjcA.GetPointerFromReadPosition(index * LurXhdJARCjJKhyRgUrsNxaPIdqh), QJpgCvdDxIIaukCWckDkuElRUIGE, gsGmLwrXoBgMTRxYcaAlQzdPHlrW, nGzbpVDglcowGGmUjmoNUvXQietbA);

		public LowLevelInputEventQueue(int P_0, int P_1, int P_2, int P_3)
		{
			eAZTOidCgblheDmZfgTMJcXnhJLfb = P_0;
			QJpgCvdDxIIaukCWckDkuElRUIGE = P_1;
			gsGmLwrXoBgMTRxYcaAlQzdPHlrW = P_2;
			nGzbpVDglcowGGmUjmoNUvXQietbA = P_3;
			LurXhdJARCjJKhyRgUrsNxaPIdqh = LowLevelInputEvent.GetReportSize(P_1, P_2, P_3);
			itHdidhKeNhyxSMdpUhPbOWjMIjcA = new NativeRingBuffer(eAZTOidCgblheDmZfgTMJcXnhJLfb * LurXhdJARCjJKhyRgUrsNxaPIdqh);
			jQnDCEiGrPvTInHPXOutqkHTcrREA = new LowLevelInputEvent(IntPtr.Zero, QJpgCvdDxIIaukCWckDkuElRUIGE, gsGmLwrXoBgMTRxYcaAlQzdPHlrW, P_3);
		}

		public LowLevelInputEvent CreateEvent()
		{
			uint passId;
			IntPtr intPtr = itHdidhKeNhyxSMdpUhPbOWjMIjcA.Allocate(LurXhdJARCjJKhyRgUrsNxaPIdqh, zeroFill: false, out passId);
			LowLevelInputEvent result = new LowLevelInputEvent(intPtr, QJpgCvdDxIIaukCWckDkuElRUIGE, gsGmLwrXoBgMTRxYcaAlQzdPHlrW, nGzbpVDglcowGGmUjmoNUvXQietbA);
			result.SetId(qFfbxrIaBJhimcEZDXJspkjRkDIkA = MiscTools.Tick(qFfbxrIaBJhimcEZDXJspkjRkDIkA));
			return result;
		}

		public int FindNextIndex(uint id)
		{
			int num = itHdidhKeNhyxSMdpUhPbOWjMIjcA.BytesInBuffer / LurXhdJARCjJKhyRgUrsNxaPIdqh;
			if (num == 0)
			{
				return -1;
			}
			jQnDCEiGrPvTInHPXOutqkHTcrREA._buffer = itHdidhKeNhyxSMdpUhPbOWjMIjcA.GetPointerFromReadPosition(0);
			uint num2 = jQnDCEiGrPvTInHPXOutqkHTcrREA.GetId();
			int num3 = 0;
			if (MiscTools.IsTickNewer(id, num2))
			{
				num3 = (int)MiscTools.TickDifference(id, num2) + 1;
				num2 = MiscTools.Tick(id);
			}
			for (int i = num3; i < num; i++)
			{
				if (!MiscTools.IsTickNewer(num2, id))
				{
					num2 = MiscTools.Tick(num2);
					continue;
				}
				return i;
			}
			return -1;
		}

		public bool TryGetNext(int index, out LowLevelInputEvent @event)
		{
			if (index < 0 || index >= itHdidhKeNhyxSMdpUhPbOWjMIjcA.BytesInBuffer / LurXhdJARCjJKhyRgUrsNxaPIdqh)
			{
				@event = default(LowLevelInputEvent);
				return false;
			}
			@event = new LowLevelInputEvent(itHdidhKeNhyxSMdpUhPbOWjMIjcA.GetPointerFromReadPosition(index * LurXhdJARCjJKhyRgUrsNxaPIdqh), QJpgCvdDxIIaukCWckDkuElRUIGE, gsGmLwrXoBgMTRxYcaAlQzdPHlrW, nGzbpVDglcowGGmUjmoNUvXQietbA);
			return true;
		}

		public void Clear()
		{
			itHdidhKeNhyxSMdpUhPbOWjMIjcA.Reset();
		}

		public void CopyAllFrom(LowLevelInputEventQueue other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			itHdidhKeNhyxSMdpUhPbOWjMIjcA.CopyFrom(other.itHdidhKeNhyxSMdpUhPbOWjMIjcA);
			qFfbxrIaBJhimcEZDXJspkjRkDIkA = other.qFfbxrIaBJhimcEZDXJspkjRkDIkA;
		}

		public void CopyNewEventsFrom(LowLevelInputEventQueue other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			int count = Count;
			int count2 = other.Count;
			if (count2 == 0)
			{
				return;
			}
			if (count == 0)
			{
				CopyAllFrom(other);
				return;
			}
			uint id = new LowLevelInputEvent(itHdidhKeNhyxSMdpUhPbOWjMIjcA.GetPointerFromReadPosition((count - 1) * LurXhdJARCjJKhyRgUrsNxaPIdqh), QJpgCvdDxIIaukCWckDkuElRUIGE, gsGmLwrXoBgMTRxYcaAlQzdPHlrW, nGzbpVDglcowGGmUjmoNUvXQietbA).GetId();
			int num = other.FindNextIndex(id);
			if (num < 0)
			{
				return;
			}
			int num2 = count2 - num;
			if (num2 != 0)
			{
				for (int i = 0; i < num2; i++)
				{
					uint passId;
					IntPtr buffer = itHdidhKeNhyxSMdpUhPbOWjMIjcA.Allocate(LurXhdJARCjJKhyRgUrsNxaPIdqh, zeroFill: false, out passId);
					other.itHdidhKeNhyxSMdpUhPbOWjMIjcA.RandomRead(buffer, LurXhdJARCjJKhyRgUrsNxaPIdqh, LurXhdJARCjJKhyRgUrsNxaPIdqh, other.itHdidhKeNhyxSMdpUhPbOWjMIjcA.GetOffsetFromReadPosition((num + i) * LurXhdJARCjJKhyRgUrsNxaPIdqh));
				}
				qFfbxrIaBJhimcEZDXJspkjRkDIkA = other.qFfbxrIaBJhimcEZDXJspkjRkDIkA;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~LowLevelInputEventQueue()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing)
				{
					itHdidhKeNhyxSMdpUhPbOWjMIjcA.Dispose();
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
