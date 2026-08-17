using System;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class DualThreadLowLevelInputEventQueue : IDisposable
	{
		private class sPWoyDsJubpGaSXotFEaKCMYwjRb : LockedObject<LowLevelInputEvent>, IDisposable, INewEventWrapper
		{
			public LowLevelInputEvent Event
			{
				get
				{
					return item;
				}
				set
				{
					item = value;
				}
			}

			public sPWoyDsJubpGaSXotFEaKCMYwjRb(object P_0)
				: base(P_0)
			{
			}
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		public interface INewEventWrapper : IDisposable
		{
			LowLevelInputEvent Event { get; set; }
		}

		private readonly LowLevelInputEventQueue agxUisXUKeZDxJvbAqOHkHoqfgUo;

		private readonly LowLevelInputEventQueue RVmAeqbavyjyfBwHStgoutgqidxi;

		private readonly object zlZuvPRitzGmmDmtNYkHMNydhaK;

		private uint OtpMJaEZuqaaLzqagwgNNGIbmgUt;

		private bool uOfmLALaASMErtPWPGdodYAwAXrEA;

		private int KdqNkdCCchXcJApQbfJnWxeIimIn;

		private int PseYqTzYjVKKCtpsLILRsxePAwYu;

		private sPWoyDsJubpGaSXotFEaKCMYwjRb YeZGhIMncYHFwGgzwvCALuMQzXmC;

		public LowLevelInputEvent currentEvent;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public uint lastProcessedEventId => OtpMJaEZuqaaLzqagwgNNGIbmgUt;

		public int count
		{
			get
			{
				lock (zlZuvPRitzGmmDmtNYkHMNydhaK)
				{
					return agxUisXUKeZDxJvbAqOHkHoqfgUo.Count;
				}
			}
		}

		public DualThreadLowLevelInputEventQueue(int P_0, int P_1, int P_2, int P_3)
		{
			agxUisXUKeZDxJvbAqOHkHoqfgUo = new LowLevelInputEventQueue(P_0, P_1, P_2, P_3);
			RVmAeqbavyjyfBwHStgoutgqidxi = new LowLevelInputEventQueue(P_0, P_1, P_2, P_3);
			zlZuvPRitzGmmDmtNYkHMNydhaK = new object();
			YeZGhIMncYHFwGgzwvCALuMQzXmC = new sPWoyDsJubpGaSXotFEaKCMYwjRb(zlZuvPRitzGmmDmtNYkHMNydhaK);
		}

		public INewEventWrapper T_CreateEvent()
		{
			YeZGhIMncYHFwGgzwvCALuMQzXmC.Lock();
			YeZGhIMncYHFwGgzwvCALuMQzXmC.item = RVmAeqbavyjyfBwHStgoutgqidxi.CreateEvent();
			return YeZGhIMncYHFwGgzwvCALuMQzXmC;
		}

		public void Update()
		{
			lock (zlZuvPRitzGmmDmtNYkHMNydhaK)
			{
				agxUisXUKeZDxJvbAqOHkHoqfgUo.CopyNewEventsFrom(RVmAeqbavyjyfBwHStgoutgqidxi);
			}
		}

		public void Clear()
		{
			lock (zlZuvPRitzGmmDmtNYkHMNydhaK)
			{
				StopProcessingEvents();
				agxUisXUKeZDxJvbAqOHkHoqfgUo.Clear();
				RVmAeqbavyjyfBwHStgoutgqidxi.Clear();
			}
		}

		public bool ProcessNewEvents()
		{
			if (PseYqTzYjVKKCtpsLILRsxePAwYu == 0)
			{
				Update();
				int num = agxUisXUKeZDxJvbAqOHkHoqfgUo.FindNextIndex(OtpMJaEZuqaaLzqagwgNNGIbmgUt);
				if (num < 0)
				{
					currentEvent = default(LowLevelInputEvent);
					return false;
				}
				PseYqTzYjVKKCtpsLILRsxePAwYu = num;
				uOfmLALaASMErtPWPGdodYAwAXrEA = true;
				KdqNkdCCchXcJApQbfJnWxeIimIn = agxUisXUKeZDxJvbAqOHkHoqfgUo.Count;
			}
			if (PseYqTzYjVKKCtpsLILRsxePAwYu >= KdqNkdCCchXcJApQbfJnWxeIimIn)
			{
				currentEvent = default(LowLevelInputEvent);
				uOfmLALaASMErtPWPGdodYAwAXrEA = false;
				PseYqTzYjVKKCtpsLILRsxePAwYu = 0;
				return false;
			}
			if (agxUisXUKeZDxJvbAqOHkHoqfgUo.TryGetNext(PseYqTzYjVKKCtpsLILRsxePAwYu, out currentEvent))
			{
				OtpMJaEZuqaaLzqagwgNNGIbmgUt = currentEvent.GetId();
				PseYqTzYjVKKCtpsLILRsxePAwYu++;
				return true;
			}
			currentEvent = default(LowLevelInputEvent);
			uOfmLALaASMErtPWPGdodYAwAXrEA = false;
			PseYqTzYjVKKCtpsLILRsxePAwYu = 0;
			return false;
		}

		public void StopProcessingEvents()
		{
			uOfmLALaASMErtPWPGdodYAwAXrEA = false;
			PseYqTzYjVKKCtpsLILRsxePAwYu = 0;
		}

		public void ImportAll(DualThreadLowLevelInputEventQueue other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (other == this)
			{
				return;
			}
			lock (zlZuvPRitzGmmDmtNYkHMNydhaK)
			{
				lock (other.zlZuvPRitzGmmDmtNYkHMNydhaK)
				{
					agxUisXUKeZDxJvbAqOHkHoqfgUo.CopyAllFrom(other.agxUisXUKeZDxJvbAqOHkHoqfgUo);
					RVmAeqbavyjyfBwHStgoutgqidxi.CopyAllFrom(other.RVmAeqbavyjyfBwHStgoutgqidxi);
					OtpMJaEZuqaaLzqagwgNNGIbmgUt = other.OtpMJaEZuqaaLzqagwgNNGIbmgUt;
					uOfmLALaASMErtPWPGdodYAwAXrEA = other.uOfmLALaASMErtPWPGdodYAwAXrEA;
					KdqNkdCCchXcJApQbfJnWxeIimIn = other.KdqNkdCCchXcJApQbfJnWxeIimIn;
					PseYqTzYjVKKCtpsLILRsxePAwYu = other.PseYqTzYjVKKCtpsLILRsxePAwYu;
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~DualThreadLowLevelInputEventQueue()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				return;
			}
			if (disposing)
			{
				lock (zlZuvPRitzGmmDmtNYkHMNydhaK)
				{
					agxUisXUKeZDxJvbAqOHkHoqfgUo.Dispose();
					RVmAeqbavyjyfBwHStgoutgqidxi.Dispose();
				}
			}
			AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
		}
	}
}
