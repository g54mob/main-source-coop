using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class LowLevelInputEventQueue : IDisposable
	{
		private LowLevelInputEvent ouRXjdclMtfQOoenGumFHGVfGGTK;

		private readonly NativeRingBuffer GPGNZTqomGaIGGudasKhtoUoPNbT;

		private readonly int FKDkCeKoLjHmDDQhOEMEIntbsMXAB;

		private readonly int IRjsUFWNGJFvPgJhLzbJGXTQbnlfA;

		private readonly int OJPoWdBcGOyRpYreXNzmfTiKyNEL;

		private readonly int dANqSgkPwHFMOiCoTuiWTvvpoQcxA;

		private readonly int fihreOeoMZajZJvHUcpmirixFhXC;

		private uint IRkQLeTeDSkAWChAWaOBBrkxpOg;

		private bool wukaVaAElsllZioWwUOfbIAKOmqE;

		public int Count => GPGNZTqomGaIGGudasKhtoUoPNbT.BytesInBuffer / dANqSgkPwHFMOiCoTuiWTvvpoQcxA;

		public int Capacity => fihreOeoMZajZJvHUcpmirixFhXC;

		public LowLevelInputEvent this[int index] => new LowLevelInputEvent(GPGNZTqomGaIGGudasKhtoUoPNbT.GetPointerFromReadPosition(index * dANqSgkPwHFMOiCoTuiWTvvpoQcxA), FKDkCeKoLjHmDDQhOEMEIntbsMXAB, IRjsUFWNGJFvPgJhLzbJGXTQbnlfA, OJPoWdBcGOyRpYreXNzmfTiKyNEL);

		public LowLevelInputEventQueue(int P_0, int P_1, int P_2, int P_3)
		{
			fihreOeoMZajZJvHUcpmirixFhXC = P_0;
			FKDkCeKoLjHmDDQhOEMEIntbsMXAB = P_1;
			IRjsUFWNGJFvPgJhLzbJGXTQbnlfA = P_2;
			OJPoWdBcGOyRpYreXNzmfTiKyNEL = P_3;
			dANqSgkPwHFMOiCoTuiWTvvpoQcxA = LowLevelInputEvent.GetReportSize(P_1, P_2, P_3);
			GPGNZTqomGaIGGudasKhtoUoPNbT = new NativeRingBuffer(fihreOeoMZajZJvHUcpmirixFhXC * dANqSgkPwHFMOiCoTuiWTvvpoQcxA);
			ouRXjdclMtfQOoenGumFHGVfGGTK = new LowLevelInputEvent(IntPtr.Zero, FKDkCeKoLjHmDDQhOEMEIntbsMXAB, IRjsUFWNGJFvPgJhLzbJGXTQbnlfA, P_3);
		}

		public LowLevelInputEvent CreateEvent()
		{
			uint passId;
			IntPtr intPtr = GPGNZTqomGaIGGudasKhtoUoPNbT.Allocate(dANqSgkPwHFMOiCoTuiWTvvpoQcxA, zeroFill: false, out passId);
			LowLevelInputEvent result = new LowLevelInputEvent(intPtr, FKDkCeKoLjHmDDQhOEMEIntbsMXAB, IRjsUFWNGJFvPgJhLzbJGXTQbnlfA, OJPoWdBcGOyRpYreXNzmfTiKyNEL);
			result.SetId(IRkQLeTeDSkAWChAWaOBBrkxpOg = MiscTools.Tick(IRkQLeTeDSkAWChAWaOBBrkxpOg));
			return result;
		}

		public int FindNextIndex(uint id)
		{
			int num = GPGNZTqomGaIGGudasKhtoUoPNbT.BytesInBuffer / dANqSgkPwHFMOiCoTuiWTvvpoQcxA;
			if (num == 0)
			{
				return -1;
			}
			ouRXjdclMtfQOoenGumFHGVfGGTK._buffer = GPGNZTqomGaIGGudasKhtoUoPNbT.GetPointerFromReadPosition(0);
			uint num2 = ouRXjdclMtfQOoenGumFHGVfGGTK.GetId();
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
			if (index < 0 || index >= GPGNZTqomGaIGGudasKhtoUoPNbT.BytesInBuffer / dANqSgkPwHFMOiCoTuiWTvvpoQcxA)
			{
				@event = default(LowLevelInputEvent);
				return false;
			}
			@event = new LowLevelInputEvent(GPGNZTqomGaIGGudasKhtoUoPNbT.GetPointerFromReadPosition(index * dANqSgkPwHFMOiCoTuiWTvvpoQcxA), FKDkCeKoLjHmDDQhOEMEIntbsMXAB, IRjsUFWNGJFvPgJhLzbJGXTQbnlfA, OJPoWdBcGOyRpYreXNzmfTiKyNEL);
			return true;
		}

		public void Clear()
		{
			GPGNZTqomGaIGGudasKhtoUoPNbT.Reset();
		}

		public void CopyAllFrom(LowLevelInputEventQueue other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			GPGNZTqomGaIGGudasKhtoUoPNbT.CopyFrom(other.GPGNZTqomGaIGGudasKhtoUoPNbT);
			IRkQLeTeDSkAWChAWaOBBrkxpOg = other.IRkQLeTeDSkAWChAWaOBBrkxpOg;
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
			uint id = new LowLevelInputEvent(GPGNZTqomGaIGGudasKhtoUoPNbT.GetPointerFromReadPosition((count - 1) * dANqSgkPwHFMOiCoTuiWTvvpoQcxA), FKDkCeKoLjHmDDQhOEMEIntbsMXAB, IRjsUFWNGJFvPgJhLzbJGXTQbnlfA, OJPoWdBcGOyRpYreXNzmfTiKyNEL).GetId();
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
					IntPtr buffer = GPGNZTqomGaIGGudasKhtoUoPNbT.Allocate(dANqSgkPwHFMOiCoTuiWTvvpoQcxA, zeroFill: false, out passId);
					other.GPGNZTqomGaIGGudasKhtoUoPNbT.RandomRead(buffer, dANqSgkPwHFMOiCoTuiWTvvpoQcxA, dANqSgkPwHFMOiCoTuiWTvvpoQcxA, other.GPGNZTqomGaIGGudasKhtoUoPNbT.GetOffsetFromReadPosition((num + i) * dANqSgkPwHFMOiCoTuiWTvvpoQcxA));
				}
				IRkQLeTeDSkAWChAWaOBBrkxpOg = other.IRkQLeTeDSkAWChAWaOBBrkxpOg;
			}
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

		~LowLevelInputEventQueue()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!wukaVaAElsllZioWwUOfbIAKOmqE)
			{
				if (disposing)
				{
					GPGNZTqomGaIGGudasKhtoUoPNbT.Dispose();
				}
				wukaVaAElsllZioWwUOfbIAKOmqE = true;
			}
		}
	}
}
