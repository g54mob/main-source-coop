using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ThreadedRingReportBuffer : IDisposable
	{
		private readonly int qjsQbJmfxxcBfQqYjyRjfoSmXkFy;

		private readonly int scVmzlioQnSrTjLjHBhQtMcpCfSh;

		private readonly int vCAoGebhdoyMGQXAKQFUIDnWJuoX;

		private readonly int tNhRGLPEaAktluORaOnqIZCTvtFM;

		private readonly int XqUxmYboKBlrGCstpNtvbpRqzwaQ;

		private readonly bool kiPnqVwMiZpmRlqBOTfdXlwaaUnR;

		private ThreadHelper SDWRoAydnweorhdNaUaMNqumjHBn;

		private NativeRingBuffer jnWgiUSndTqvXcnVtFcajjpLbNYSA;

		private NativeRingBuffer IaxIPKjOsEcplbAIhIFxgGyErLjnd;

		private Action<byte[]> SmqbYSXppFyuiQPrQaxHXiHmXIUh;

		private byte[] URwphKyYrcrEbEJSUuMGsxQjTmaH;

		private byte[] JxJFlPgugeLBtXhBKeFpHlUgjHWbc;

		private bool aiIRBlALweaIqrbgVAVJhcAgFdSG;

		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		private int wTWtPXmkEIasvDkUynqmdvrdcVjMc;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public bool IsRunning => SDWRoAydnweorhdNaUaMNqumjHBn.isRunning;

		public int BufferLength => qjsQbJmfxxcBfQqYjyRjfoSmXkFy;

		public int BytesInBuffer => IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer;

		public int EntriesInBuffer => IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer / scVmzlioQnSrTjLjHBhQtMcpCfSh;

		public byte[] ReadBuffer => JxJFlPgugeLBtXhBKeFpHlUgjHWbc;

		public int LastNumBytesRead => wTWtPXmkEIasvDkUynqmdvrdcVjMc;

		public ThreadedRingReportBuffer(int P_0, int P_1, int P_2, int P_3, bool P_4, Action<byte[]> P_5)
		{
			if (P_0 <= 0)
			{
				throw new ArgumentOutOfRangeException("entryByteLength must be > 0.");
			}
			if (P_1 < 1)
			{
				throw new ArgumentOutOfRangeException("entryCapacity must be >= 1.");
			}
			if (P_2 < 0)
			{
				P_2 = 0;
			}
			if (P_3 < 0)
			{
				P_3 = 0;
			}
			if (P_5 == null)
			{
				throw new ArgumentNullException("threadRetrieveDataDelegate");
			}
			scVmzlioQnSrTjLjHBhQtMcpCfSh = P_0;
			vCAoGebhdoyMGQXAKQFUIDnWJuoX = P_1;
			qjsQbJmfxxcBfQqYjyRjfoSmXkFy = P_0 * P_1;
			tNhRGLPEaAktluORaOnqIZCTvtFM = P_2;
			XqUxmYboKBlrGCstpNtvbpRqzwaQ = P_3;
			kiPnqVwMiZpmRlqBOTfdXlwaaUnR = P_4;
			SmqbYSXppFyuiQPrQaxHXiHmXIUh = P_5;
			jnWgiUSndTqvXcnVtFcajjpLbNYSA = new NativeRingBuffer(qjsQbJmfxxcBfQqYjyRjfoSmXkFy);
			IaxIPKjOsEcplbAIhIFxgGyErLjnd = new NativeRingBuffer(qjsQbJmfxxcBfQqYjyRjfoSmXkFy);
			URwphKyYrcrEbEJSUuMGsxQjTmaH = new byte[P_0];
			JxJFlPgugeLBtXhBKeFpHlUgjHWbc = new byte[P_0];
			if (!qhuNDonLyhwaOUJVhxsLRGkQPgDT())
			{
				throw new Exception("Could not initialize thread.");
			}
		}

		public int Read()
		{
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(JxJFlPgugeLBtXhBKeFpHlUgjHWbc, scVmzlioQnSrTjLjHBhQtMcpCfSh);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int Read(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(buffer, buffer.Length);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int Read(IntPtr buffer, int bufferLength)
		{
			if (buffer == IntPtr.Zero)
			{
				throw new ArgumentNullException("buffer");
			}
			if (bufferLength <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferLength");
			}
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(buffer, bufferLength, bufferLength);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int StartRead()
		{
			dymWjsjBfCEIrESusydeCnGdzfEW();
			return IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer;
		}

		public void StartThread()
		{
			if (SDWRoAydnweorhdNaUaMNqumjHBn.isRunning)
			{
				return;
			}
			try
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.Start(kiPnqVwMiZpmRlqBOTfdXlwaaUnR);
			}
			catch
			{
			}
		}

		public void StopThread()
		{
			if (SDWRoAydnweorhdNaUaMNqumjHBn.isStopped)
			{
				return;
			}
			try
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.Stop(kiPnqVwMiZpmRlqBOTfdXlwaaUnR);
			}
			catch
			{
			}
		}

		private bool qhuNDonLyhwaOUJVhxsLRGkQPgDT()
		{
			if (aiIRBlALweaIqrbgVAVJhcAgFdSG)
			{
				return false;
			}
			if (!ceMUJrromJgcosGWFwBmZaGsLGyG())
			{
				return false;
			}
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return true;
			}
			obpzIVquVRQseulcTcTZaHvizTjRA = true;
			return true;
		}

		private bool ceMUJrromJgcosGWFwBmZaGsLGyG()
		{
			if (aiIRBlALweaIqrbgVAVJhcAgFdSG)
			{
				return false;
			}
			if (SDWRoAydnweorhdNaUaMNqumjHBn == null)
			{
				try
				{
					SDWRoAydnweorhdNaUaMNqumjHBn = ThreadHelper.CreateFixedTimeStep(tNhRGLPEaAktluORaOnqIZCTvtFM, XqUxmYboKBlrGCstpNtvbpRqzwaQ);
					SDWRoAydnweorhdNaUaMNqumjHBn.ThreadUpdateEvent += mssMEguIMGMpxuckgaxvBWEALvxE;
					return true;
				}
				catch (Exception ex)
				{
					Logger.LogError("Exception occurred while creating thread!\n" + ex, requiredThreadSafety: true);
					if (SDWRoAydnweorhdNaUaMNqumjHBn != null)
					{
						SDWRoAydnweorhdNaUaMNqumjHBn.Stop(kiPnqVwMiZpmRlqBOTfdXlwaaUnR);
					}
					aiIRBlALweaIqrbgVAVJhcAgFdSG = true;
					return false;
				}
			}
			if (!SDWRoAydnweorhdNaUaMNqumjHBn.isRunning)
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.Start(kiPnqVwMiZpmRlqBOTfdXlwaaUnR);
			}
			else if (XqUxmYboKBlrGCstpNtvbpRqzwaQ > 0)
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.ResetTimeout();
			}
			return true;
		}

		private void dymWjsjBfCEIrESusydeCnGdzfEW()
		{
			lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
			{
				lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
				{
					MiscTools.Swap(ref jnWgiUSndTqvXcnVtFcajjpLbNYSA, ref IaxIPKjOsEcplbAIhIFxgGyErLjnd);
				}
			}
		}

		private void mssMEguIMGMpxuckgaxvBWEALvxE()
		{
			try
			{
				lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
				{
					SmqbYSXppFyuiQPrQaxHXiHmXIUh(URwphKyYrcrEbEJSUuMGsxQjTmaH);
					jnWgiUSndTqvXcnVtFcajjpLbNYSA.Write(URwphKyYrcrEbEJSUuMGsxQjTmaH, scVmzlioQnSrTjLjHBhQtMcpCfSh);
				}
			}
			catch
			{
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~ThreadedRingReportBuffer()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing && SDWRoAydnweorhdNaUaMNqumjHBn != null)
				{
					SDWRoAydnweorhdNaUaMNqumjHBn.Dispose();
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
