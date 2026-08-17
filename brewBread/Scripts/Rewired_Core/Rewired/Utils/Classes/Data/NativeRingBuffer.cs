using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class NativeRingBuffer : IDisposable
	{
		private readonly NativeBuffer aKSeOTDNXYxsQuOgzngtqCfdfsFw;

		private readonly int eAZTOidCgblheDmZfgTMJcXnhJLfb;

		private long XldBLbkkAZakFBtmqDFjiseBmqMx;

		private long ScfrQOHyoEwnRHhxWoFyNILgZBtv;

		private int UOGKxoMRGgcubTBwukEYwwJFlUFy;

		private bool wlNJfBCXlbWKgWhEQESXKrAtnFcs;

		private uint pgXmrFSfUhmDgLltNdBHkpktLiBy;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public int Capacity => eAZTOidCgblheDmZfgTMJcXnhJLfb;

		public int BytesInBuffer => UOGKxoMRGgcubTBwukEYwwJFlUFy;

		public bool BufferOverrun => wlNJfBCXlbWKgWhEQESXKrAtnFcs;

		public int ReadPosition => (int)ScfrQOHyoEwnRHhxWoFyNILgZBtv;

		public long WritePosition => XldBLbkkAZakFBtmqDFjiseBmqMx;

		public NativeRingBuffer(int P_0)
		{
			eAZTOidCgblheDmZfgTMJcXnhJLfb = P_0;
			if (P_0 <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeInBytes");
			}
			aKSeOTDNXYxsQuOgzngtqCfdfsFw = new NativeBuffer(P_0);
		}

		public IntPtr Allocate(int bufferLength, bool zeroFill, out uint passId)
		{
			IntPtr pointer = aKSeOTDNXYxsQuOgzngtqCfdfsFw.GetPointer((int)XldBLbkkAZakFBtmqDFjiseBmqMx);
			passId = pgXmrFSfUhmDgLltNdBHkpktLiBy;
			if (zeroFill)
			{
				int num = 0;
				aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryFill(0, bufferLength, (int)XldBLbkkAZakFBtmqDFjiseBmqMx);
				if (num == 0)
				{
					return IntPtr.Zero;
				}
				if (num < bufferLength)
				{
					num += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryFill(0, bufferLength - num, num);
				}
			}
			eTFMjptIaDuIHIcWCmserVeOCyEe(bufferLength);
			return pointer;
		}

		public int Write(IntPtr buffer, int bufferLength, int numBytesToWrite, out int startOffset, out uint passId)
		{
			startOffset = (int)XldBLbkkAZakFBtmqDFjiseBmqMx;
			passId = pgXmrFSfUhmDgLltNdBHkpktLiBy;
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToWrite <= 0)
			{
				return 0;
			}
			if (numBytesToWrite > bufferLength)
			{
				numBytesToWrite = bufferLength;
			}
			int num = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryWriteBytes(buffer, bufferLength, numBytesToWrite, (int)XldBLbkkAZakFBtmqDFjiseBmqMx);
			if (num == 0)
			{
				return 0;
			}
			if (num < numBytesToWrite)
			{
				num += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryWriteBytes(buffer, bufferLength, numBytesToWrite - num, 0, num);
			}
			eTFMjptIaDuIHIcWCmserVeOCyEe(num);
			return num;
		}

		public int Write(byte[] buffer, int numBytesToWrite, out int startOffset, out uint passId)
		{
			startOffset = (int)XldBLbkkAZakFBtmqDFjiseBmqMx;
			passId = pgXmrFSfUhmDgLltNdBHkpktLiBy;
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
			int num2 = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryWriteBytes(buffer, numBytesToWrite, (int)XldBLbkkAZakFBtmqDFjiseBmqMx);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 < numBytesToWrite)
			{
				num2 += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryWriteBytes(buffer, numBytesToWrite - num2, 0, num2);
			}
			eTFMjptIaDuIHIcWCmserVeOCyEe(num2);
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
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToRead <= 0 || UOGKxoMRGgcubTBwukEYwwJFlUFy == 0)
			{
				return 0;
			}
			if (numBytesToRead > bufferLength)
			{
				numBytesToRead = bufferLength;
			}
			if (numBytesToRead > UOGKxoMRGgcubTBwukEYwwJFlUFy)
			{
				numBytesToRead = UOGKxoMRGgcubTBwukEYwwJFlUFy;
			}
			int num = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, bufferLength, numBytesToRead, (int)ScfrQOHyoEwnRHhxWoFyNILgZBtv);
			if (num <= 0)
			{
				return 0;
			}
			if (num < numBytesToRead)
			{
				num += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, bufferLength, numBytesToRead - num, 0, num);
			}
			MLRGFQCAhrdofHFRgYLAImwKZABib(num);
			return num;
		}

		public int Read(byte[] buffer, int numBytesToRead)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (num <= 0 || numBytesToRead <= 0 || UOGKxoMRGgcubTBwukEYwwJFlUFy == 0)
			{
				return 0;
			}
			if (numBytesToRead > num)
			{
				numBytesToRead = num;
			}
			if (numBytesToRead > UOGKxoMRGgcubTBwukEYwwJFlUFy)
			{
				numBytesToRead = UOGKxoMRGgcubTBwukEYwwJFlUFy;
			}
			int num2 = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, numBytesToRead, (int)ScfrQOHyoEwnRHhxWoFyNILgZBtv);
			if (num2 <= 0)
			{
				return 0;
			}
			if (num2 < numBytesToRead)
			{
				num2 += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, numBytesToRead - num2, 0, num2);
			}
			MLRGFQCAhrdofHFRgYLAImwKZABib(num2);
			return num2;
		}

		public int RandomRead(IntPtr buffer, int bufferLength, int numBytesToRead, int readStartIndex)
		{
			if (buffer == IntPtr.Zero || bufferLength <= 0 || numBytesToRead <= 0 || UOGKxoMRGgcubTBwukEYwwJFlUFy == 0 || readStartIndex < 0 || readStartIndex >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				return 0;
			}
			if (numBytesToRead > bufferLength)
			{
				numBytesToRead = bufferLength;
			}
			if (numBytesToRead > UOGKxoMRGgcubTBwukEYwwJFlUFy)
			{
				numBytesToRead = UOGKxoMRGgcubTBwukEYwwJFlUFy;
			}
			int num = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, bufferLength, numBytesToRead, readStartIndex);
			if (num <= 0)
			{
				return 0;
			}
			if (num < numBytesToRead)
			{
				num += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, bufferLength, numBytesToRead - num, 0, num);
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
			if (num <= 0 || numBytesToRead <= 0 || UOGKxoMRGgcubTBwukEYwwJFlUFy == 0 || readStartIndex < 0 || readStartIndex >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				return 0;
			}
			if (numBytesToRead > num)
			{
				numBytesToRead = num;
			}
			if (numBytesToRead > UOGKxoMRGgcubTBwukEYwwJFlUFy)
			{
				numBytesToRead = UOGKxoMRGgcubTBwukEYwwJFlUFy;
			}
			int num2 = aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, numBytesToRead, readStartIndex);
			if (num2 <= 0)
			{
				return 0;
			}
			if (num2 < numBytesToRead)
			{
				num2 += aKSeOTDNXYxsQuOgzngtqCfdfsFw.TryReadBytes(buffer, numBytesToRead - num2, 0, num2);
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
			return aKSeOTDNXYxsQuOgzngtqCfdfsFw.GetPointer(offsetFromReadPosition);
		}

		public int GetOffsetFromReadPosition(int offset)
		{
			int num = (int)ScfrQOHyoEwnRHhxWoFyNILgZBtv + offset;
			if (num >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				num -= eAZTOidCgblheDmZfgTMJcXnhJLfb;
			}
			else if (num < 0)
			{
				num += eAZTOidCgblheDmZfgTMJcXnhJLfb;
			}
			if (num < 0 || num >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				return -1;
			}
			return num;
		}

		public bool IsValid(int startIndex, uint passId)
		{
			if (startIndex < 0 || startIndex >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				return false;
			}
			if (startIndex < XldBLbkkAZakFBtmqDFjiseBmqMx)
			{
				if (passId == pgXmrFSfUhmDgLltNdBHkpktLiBy)
				{
					return true;
				}
			}
			else if (startIndex >= XldBLbkkAZakFBtmqDFjiseBmqMx)
			{
				if (pgXmrFSfUhmDgLltNdBHkpktLiBy == 0)
				{
					return false;
				}
				if (pgXmrFSfUhmDgLltNdBHkpktLiBy - 1 == passId)
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
			if (eAZTOidCgblheDmZfgTMJcXnhJLfb != other.eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				throw new Exception("Buffer does not have the same capacity. Cannot copy.");
			}
			XldBLbkkAZakFBtmqDFjiseBmqMx = other.XldBLbkkAZakFBtmqDFjiseBmqMx;
			ScfrQOHyoEwnRHhxWoFyNILgZBtv = other.ScfrQOHyoEwnRHhxWoFyNILgZBtv;
			UOGKxoMRGgcubTBwukEYwwJFlUFy = other.UOGKxoMRGgcubTBwukEYwwJFlUFy;
			wlNJfBCXlbWKgWhEQESXKrAtnFcs = other.wlNJfBCXlbWKgWhEQESXKrAtnFcs;
			pgXmrFSfUhmDgLltNdBHkpktLiBy = other.pgXmrFSfUhmDgLltNdBHkpktLiBy;
			aKSeOTDNXYxsQuOgzngtqCfdfsFw.CopyFrom(other.aKSeOTDNXYxsQuOgzngtqCfdfsFw);
		}

		public void Reset()
		{
			XldBLbkkAZakFBtmqDFjiseBmqMx = 0L;
			ScfrQOHyoEwnRHhxWoFyNILgZBtv = 0L;
			UOGKxoMRGgcubTBwukEYwwJFlUFy = 0;
			wlNJfBCXlbWKgWhEQESXKrAtnFcs = false;
			pgXmrFSfUhmDgLltNdBHkpktLiBy = 0u;
		}

		private void eTFMjptIaDuIHIcWCmserVeOCyEe(int P_0)
		{
			if (P_0 <= 0)
			{
				return;
			}
			int num = (int)XldBLbkkAZakFBtmqDFjiseBmqMx;
			XldBLbkkAZakFBtmqDFjiseBmqMx += P_0;
			bool flag = false;
			if (num < ScfrQOHyoEwnRHhxWoFyNILgZBtv)
			{
				if (XldBLbkkAZakFBtmqDFjiseBmqMx > ScfrQOHyoEwnRHhxWoFyNILgZBtv)
				{
					flag = true;
				}
			}
			else if (num > ScfrQOHyoEwnRHhxWoFyNILgZBtv)
			{
				if (XldBLbkkAZakFBtmqDFjiseBmqMx - eAZTOidCgblheDmZfgTMJcXnhJLfb > ScfrQOHyoEwnRHhxWoFyNILgZBtv)
				{
					flag = true;
				}
			}
			else if (UOGKxoMRGgcubTBwukEYwwJFlUFy > 0)
			{
				flag = true;
			}
			if (flag)
			{
				wlNJfBCXlbWKgWhEQESXKrAtnFcs = true;
				ScfrQOHyoEwnRHhxWoFyNILgZBtv = XldBLbkkAZakFBtmqDFjiseBmqMx;
				if (ScfrQOHyoEwnRHhxWoFyNILgZBtv >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
				{
					ScfrQOHyoEwnRHhxWoFyNILgZBtv -= eAZTOidCgblheDmZfgTMJcXnhJLfb;
				}
			}
			if (XldBLbkkAZakFBtmqDFjiseBmqMx >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				XldBLbkkAZakFBtmqDFjiseBmqMx -= eAZTOidCgblheDmZfgTMJcXnhJLfb;
				CPRBLSQIAqmWjJiDNrITVLZCisCn();
			}
			UOGKxoMRGgcubTBwukEYwwJFlUFy = (int)MathTools.Clamp((long)UOGKxoMRGgcubTBwukEYwwJFlUFy + (long)P_0, 0L, eAZTOidCgblheDmZfgTMJcXnhJLfb);
		}

		private void MLRGFQCAhrdofHFRgYLAImwKZABib(int P_0)
		{
			if (P_0 > 0)
			{
				if (wlNJfBCXlbWKgWhEQESXKrAtnFcs)
				{
					wlNJfBCXlbWKgWhEQESXKrAtnFcs = false;
				}
				ScfrQOHyoEwnRHhxWoFyNILgZBtv += P_0;
				if (ScfrQOHyoEwnRHhxWoFyNILgZBtv >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
				{
					ScfrQOHyoEwnRHhxWoFyNILgZBtv -= eAZTOidCgblheDmZfgTMJcXnhJLfb;
				}
				long num = (long)UOGKxoMRGgcubTBwukEYwwJFlUFy - (long)P_0;
				UOGKxoMRGgcubTBwukEYwwJFlUFy = (int)((num >= 0) ? num : 0);
			}
		}

		private void CPRBLSQIAqmWjJiDNrITVLZCisCn()
		{
			if (pgXmrFSfUhmDgLltNdBHkpktLiBy == uint.MaxValue)
			{
				pgXmrFSfUhmDgLltNdBHkpktLiBy = 0u;
			}
			else
			{
				pgXmrFSfUhmDgLltNdBHkpktLiBy++;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~NativeRingBuffer()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing && aKSeOTDNXYxsQuOgzngtqCfdfsFw != null)
				{
					aKSeOTDNXYxsQuOgzngtqCfdfsFw.Dispose();
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
