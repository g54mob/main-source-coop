using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class NativeRingBuffer : IDisposable
	{
		private readonly NativeBuffer UKEjECAhMtgmJtBVqBuXensyBcceA;

		private readonly int rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;

		private long hmVQNijybKGCtqVPUkufWfEdBPFU;

		private long fExhciCWXGsRcUvykcfLIPTFqzxOA;

		private int FitqbpzJfnFPvIzAUlboXPJQjAlcA;

		private bool npJAcdgVMudZabpdcknYYQDWtUxIb;

		private uint QmAHeocwfLNDNwQjQfbKrZOVHIny;

		private bool DQKGZgELVQBaJNstmVpFLzsRbVEPA;

		public int Capacity => rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;

		public int BytesInBuffer => FitqbpzJfnFPvIzAUlboXPJQjAlcA;

		public bool BufferOverrun => npJAcdgVMudZabpdcknYYQDWtUxIb;

		public int ReadPosition => (int)fExhciCWXGsRcUvykcfLIPTFqzxOA;

		public long WritePosition => hmVQNijybKGCtqVPUkufWfEdBPFU;

		public NativeRingBuffer(int P_0)
		{
			rbpDMpKpIqoTmcRPEoZdRXqrjXsFc = P_0;
			if (P_0 <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeInBytes");
			}
			UKEjECAhMtgmJtBVqBuXensyBcceA = new NativeBuffer(P_0);
		}

		public IntPtr Allocate(int bufferLength, bool zeroFill, out uint passId)
		{
			IntPtr pointer = UKEjECAhMtgmJtBVqBuXensyBcceA.GetPointer((int)hmVQNijybKGCtqVPUkufWfEdBPFU);
			passId = QmAHeocwfLNDNwQjQfbKrZOVHIny;
			if (zeroFill)
			{
				int num = 0;
				UKEjECAhMtgmJtBVqBuXensyBcceA.TryFill(0, bufferLength, (int)hmVQNijybKGCtqVPUkufWfEdBPFU);
				if (num == 0)
				{
					return IntPtr.Zero;
				}
				if (num < bufferLength)
				{
					num += UKEjECAhMtgmJtBVqBuXensyBcceA.TryFill(0, bufferLength - num, num);
				}
			}
			kYUImGYXwiNspQAlHbJYaxTDJgyqA(bufferLength);
			return pointer;
		}

		public int Write(IntPtr buffer, int bufferLength, int numBytesToWrite, out int startOffset, out uint passId)
		{
			startOffset = (int)hmVQNijybKGCtqVPUkufWfEdBPFU;
			passId = QmAHeocwfLNDNwQjQfbKrZOVHIny;
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToWrite <= 0)
			{
				return 0;
			}
			if (numBytesToWrite > bufferLength)
			{
				numBytesToWrite = bufferLength;
			}
			int num = UKEjECAhMtgmJtBVqBuXensyBcceA.TryWriteBytes(buffer, bufferLength, numBytesToWrite, (int)hmVQNijybKGCtqVPUkufWfEdBPFU);
			if (num == 0)
			{
				return 0;
			}
			if (num < numBytesToWrite)
			{
				num += UKEjECAhMtgmJtBVqBuXensyBcceA.TryWriteBytes(buffer, bufferLength, numBytesToWrite - num, 0, num);
			}
			kYUImGYXwiNspQAlHbJYaxTDJgyqA(num);
			return num;
		}

		public int Write(byte[] buffer, int numBytesToWrite, out int startOffset, out uint passId)
		{
			startOffset = (int)hmVQNijybKGCtqVPUkufWfEdBPFU;
			passId = QmAHeocwfLNDNwQjQfbKrZOVHIny;
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (num <= 0 || numBytesToWrite <= 0)
			{
				return 0;
			}
			if (numBytesToWrite > num)
			{
				numBytesToWrite = num;
			}
			int num2 = UKEjECAhMtgmJtBVqBuXensyBcceA.TryWriteBytes(buffer, numBytesToWrite, (int)hmVQNijybKGCtqVPUkufWfEdBPFU);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 < numBytesToWrite)
			{
				num2 += UKEjECAhMtgmJtBVqBuXensyBcceA.TryWriteBytes(buffer, numBytesToWrite - num2, 0, num2);
			}
			kYUImGYXwiNspQAlHbJYaxTDJgyqA(num2);
			return num2;
		}

		public int Write(IntPtr buffer, int bufferLength, int numBytesToWrite)
		{
			int startOffset;
			uint passId;
			return Write(buffer, bufferLength, numBytesToWrite, out startOffset, out passId);
		}

		public int Write(byte[] buffer, int numBytesToWrite)
		{
			int startOffset;
			uint passId;
			return Write(buffer, numBytesToWrite, out startOffset, out passId);
		}

		public int Read(IntPtr buffer, int bufferLength, int numBytesToRead)
		{
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToRead <= 0 || FitqbpzJfnFPvIzAUlboXPJQjAlcA == 0)
			{
				return 0;
			}
			if (numBytesToRead > bufferLength)
			{
				numBytesToRead = bufferLength;
			}
			if (numBytesToRead > FitqbpzJfnFPvIzAUlboXPJQjAlcA)
			{
				numBytesToRead = FitqbpzJfnFPvIzAUlboXPJQjAlcA;
			}
			int num = UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, bufferLength, numBytesToRead, (int)fExhciCWXGsRcUvykcfLIPTFqzxOA);
			if (num <= 0)
			{
				return 0;
			}
			if (num < numBytesToRead)
			{
				num += UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, bufferLength, numBytesToRead - num, 0, num);
			}
			CLpqIGblaetLrFwtuhBjmNFlChsT(num);
			return num;
		}

		public int Read(byte[] buffer, int numBytesToRead)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (num <= 0 || numBytesToRead <= 0 || FitqbpzJfnFPvIzAUlboXPJQjAlcA == 0)
			{
				return 0;
			}
			if (numBytesToRead > num)
			{
				numBytesToRead = num;
			}
			if (numBytesToRead > FitqbpzJfnFPvIzAUlboXPJQjAlcA)
			{
				numBytesToRead = FitqbpzJfnFPvIzAUlboXPJQjAlcA;
			}
			int num2 = UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, numBytesToRead, (int)fExhciCWXGsRcUvykcfLIPTFqzxOA);
			if (num2 <= 0)
			{
				return 0;
			}
			if (num2 < numBytesToRead)
			{
				num2 += UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, numBytesToRead - num2, 0, num2);
			}
			CLpqIGblaetLrFwtuhBjmNFlChsT(num2);
			return num2;
		}

		public int RandomRead(IntPtr buffer, int bufferLength, int numBytesToRead, int readStartIndex)
		{
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToRead <= 0 || FitqbpzJfnFPvIzAUlboXPJQjAlcA == 0 || readStartIndex < 0 || readStartIndex >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				return 0;
			}
			if (numBytesToRead > bufferLength)
			{
				numBytesToRead = bufferLength;
			}
			if (numBytesToRead > FitqbpzJfnFPvIzAUlboXPJQjAlcA)
			{
				numBytesToRead = FitqbpzJfnFPvIzAUlboXPJQjAlcA;
			}
			int num = UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, bufferLength, numBytesToRead, readStartIndex);
			if (num <= 0)
			{
				return 0;
			}
			if (num < numBytesToRead)
			{
				num += UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, bufferLength, numBytesToRead - num, 0, num);
			}
			return num;
		}

		public int RandomRead(byte[] buffer, int numBytesToRead, int readStartIndex)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (num <= 0 || numBytesToRead <= 0 || FitqbpzJfnFPvIzAUlboXPJQjAlcA == 0 || readStartIndex < 0 || readStartIndex >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				return 0;
			}
			if (numBytesToRead > num)
			{
				numBytesToRead = num;
			}
			if (numBytesToRead > FitqbpzJfnFPvIzAUlboXPJQjAlcA)
			{
				numBytesToRead = FitqbpzJfnFPvIzAUlboXPJQjAlcA;
			}
			int num2 = UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, numBytesToRead, readStartIndex);
			if (num2 <= 0)
			{
				return 0;
			}
			if (num2 < numBytesToRead)
			{
				num2 += UKEjECAhMtgmJtBVqBuXensyBcceA.TryReadBytes(buffer, numBytesToRead - num2, 0, num2);
			}
			return num2;
		}

		public IntPtr GetPointerFromReadPosition(int offset)
		{
			int offsetFromReadPosition = GetOffsetFromReadPosition(offset);
			if (offsetFromReadPosition < 0)
			{
				return IntPtr.Zero;
			}
			return UKEjECAhMtgmJtBVqBuXensyBcceA.GetPointer(offsetFromReadPosition);
		}

		public int GetOffsetFromReadPosition(int offset)
		{
			int num = (int)fExhciCWXGsRcUvykcfLIPTFqzxOA + offset;
			if (num >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				num -= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;
			}
			else if (num < 0)
			{
				num += rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;
			}
			if (num < 0 || num >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				return -1;
			}
			return num;
		}

		public bool IsValid(int startIndex, uint passId)
		{
			if (startIndex < 0 || startIndex >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				return false;
			}
			if (startIndex < hmVQNijybKGCtqVPUkufWfEdBPFU)
			{
				if (passId == QmAHeocwfLNDNwQjQfbKrZOVHIny)
				{
					return true;
				}
			}
			else if (startIndex >= hmVQNijybKGCtqVPUkufWfEdBPFU)
			{
				if (QmAHeocwfLNDNwQjQfbKrZOVHIny == 0)
				{
					return false;
				}
				if (QmAHeocwfLNDNwQjQfbKrZOVHIny - 1 == passId)
				{
					return true;
				}
			}
			return false;
		}

		public void CopyFrom(NativeRingBuffer other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (rbpDMpKpIqoTmcRPEoZdRXqrjXsFc != other.rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				throw new Exception("Buffer does not have the same capacity. Cannot copy.");
			}
			hmVQNijybKGCtqVPUkufWfEdBPFU = other.hmVQNijybKGCtqVPUkufWfEdBPFU;
			fExhciCWXGsRcUvykcfLIPTFqzxOA = other.fExhciCWXGsRcUvykcfLIPTFqzxOA;
			FitqbpzJfnFPvIzAUlboXPJQjAlcA = other.FitqbpzJfnFPvIzAUlboXPJQjAlcA;
			npJAcdgVMudZabpdcknYYQDWtUxIb = other.npJAcdgVMudZabpdcknYYQDWtUxIb;
			QmAHeocwfLNDNwQjQfbKrZOVHIny = other.QmAHeocwfLNDNwQjQfbKrZOVHIny;
			UKEjECAhMtgmJtBVqBuXensyBcceA.CopyFrom(other.UKEjECAhMtgmJtBVqBuXensyBcceA);
		}

		public void Reset()
		{
			hmVQNijybKGCtqVPUkufWfEdBPFU = 0L;
			fExhciCWXGsRcUvykcfLIPTFqzxOA = 0L;
			FitqbpzJfnFPvIzAUlboXPJQjAlcA = 0;
			npJAcdgVMudZabpdcknYYQDWtUxIb = false;
			QmAHeocwfLNDNwQjQfbKrZOVHIny = 0u;
		}

		private void kYUImGYXwiNspQAlHbJYaxTDJgyqA(int P_0)
		{
			if (P_0 <= 0)
			{
				return;
			}
			int num = (int)hmVQNijybKGCtqVPUkufWfEdBPFU;
			hmVQNijybKGCtqVPUkufWfEdBPFU += P_0;
			bool flag = false;
			if (num < fExhciCWXGsRcUvykcfLIPTFqzxOA)
			{
				if (hmVQNijybKGCtqVPUkufWfEdBPFU > fExhciCWXGsRcUvykcfLIPTFqzxOA)
				{
					flag = true;
				}
			}
			else if (num > fExhciCWXGsRcUvykcfLIPTFqzxOA)
			{
				if (hmVQNijybKGCtqVPUkufWfEdBPFU - rbpDMpKpIqoTmcRPEoZdRXqrjXsFc > fExhciCWXGsRcUvykcfLIPTFqzxOA)
				{
					flag = true;
				}
			}
			else if (FitqbpzJfnFPvIzAUlboXPJQjAlcA > 0)
			{
				flag = true;
			}
			if (flag)
			{
				npJAcdgVMudZabpdcknYYQDWtUxIb = true;
				fExhciCWXGsRcUvykcfLIPTFqzxOA = hmVQNijybKGCtqVPUkufWfEdBPFU;
				if (fExhciCWXGsRcUvykcfLIPTFqzxOA >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
				{
					fExhciCWXGsRcUvykcfLIPTFqzxOA -= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;
				}
			}
			if (hmVQNijybKGCtqVPUkufWfEdBPFU >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
			{
				hmVQNijybKGCtqVPUkufWfEdBPFU -= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;
				hUNfkVAYGWfmDDWfblTTyYTNldevb();
			}
			FitqbpzJfnFPvIzAUlboXPJQjAlcA = (int)MathTools.Clamp((long)FitqbpzJfnFPvIzAUlboXPJQjAlcA + (long)P_0, 0L, rbpDMpKpIqoTmcRPEoZdRXqrjXsFc);
		}

		private void CLpqIGblaetLrFwtuhBjmNFlChsT(int P_0)
		{
			if (P_0 > 0)
			{
				if (npJAcdgVMudZabpdcknYYQDWtUxIb)
				{
					npJAcdgVMudZabpdcknYYQDWtUxIb = false;
				}
				fExhciCWXGsRcUvykcfLIPTFqzxOA += P_0;
				if (fExhciCWXGsRcUvykcfLIPTFqzxOA >= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc)
				{
					fExhciCWXGsRcUvykcfLIPTFqzxOA -= rbpDMpKpIqoTmcRPEoZdRXqrjXsFc;
				}
				long num = (long)FitqbpzJfnFPvIzAUlboXPJQjAlcA - (long)P_0;
				FitqbpzJfnFPvIzAUlboXPJQjAlcA = (int)((num >= 0) ? num : 0);
			}
		}

		private void hUNfkVAYGWfmDDWfblTTyYTNldevb()
		{
			if (QmAHeocwfLNDNwQjQfbKrZOVHIny == 4294967295u)
			{
				QmAHeocwfLNDNwQjQfbKrZOVHIny = 0u;
			}
			else
			{
				QmAHeocwfLNDNwQjQfbKrZOVHIny++;
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

		~NativeRingBuffer()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!DQKGZgELVQBaJNstmVpFLzsRbVEPA)
			{
				if (disposing && UKEjECAhMtgmJtBVqBuXensyBcceA != null)
				{
					UKEjECAhMtgmJtBVqBuXensyBcceA.Dispose();
				}
				DQKGZgELVQBaJNstmVpFLzsRbVEPA = true;
			}
		}
	}
}
