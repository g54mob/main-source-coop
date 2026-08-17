using System;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

namespace Rewired.HID
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class HidOutputReportHandler : IDisposable
	{
		[CustomObfuscation(rename = false)]
		public delegate bool WriteReportDelegate(OutputReport report);

		private class mrsxmPeniIAlDsIZostwdvTXCbYO : IDisposable
		{
			private bool jFnBtyhaVtvfQmxdadHWIGrnxmurA;

			private OutputReport HbiqUOVIpQGATNoGPGqUEuiOJwkpA;

			private NativeBuffer aKSeOTDNXYxsQuOgzngtqCfdfsFw;

			private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

			public bool xxlDHgMFNSsjsaoGzxMovcxuvRWl => jFnBtyhaVtvfQmxdadHWIGrnxmurA;

			public mrsxmPeniIAlDsIZostwdvTXCbYO()
			{
				aKSeOTDNXYxsQuOgzngtqCfdfsFw = new NativeBuffer(0);
			}

			public void WfdDXzQiHTaPqIKPBANGYaSeZhAC(ref OutputReport P_0)
			{
				jFnBtyhaVtvfQmxdadHWIGrnxmurA = false;
				if (!P_0.IsValid)
				{
					return;
				}
				HbiqUOVIpQGATNoGPGqUEuiOJwkpA = P_0;
				if (aKSeOTDNXYxsQuOgzngtqCfdfsFw.Length >= P_0.bufferLength || aKSeOTDNXYxsQuOgzngtqCfdfsFw.Resize(P_0.bufferLength, preserveData: false))
				{
					try
					{
						aKSeOTDNXYxsQuOgzngtqCfdfsFw.Write(P_0.buffer, P_0.bufferLength, P_0.bufferLength);
					}
					catch
					{
						return;
					}
					HbiqUOVIpQGATNoGPGqUEuiOJwkpA.buffer = aKSeOTDNXYxsQuOgzngtqCfdfsFw.Pointer;
					HbiqUOVIpQGATNoGPGqUEuiOJwkpA.bufferLength = aKSeOTDNXYxsQuOgzngtqCfdfsFw.Length;
					jFnBtyhaVtvfQmxdadHWIGrnxmurA = true;
				}
			}

			public OutputReport HikbhTgnRxKSIdGkYcXgbiShdgwFA()
			{
				if (!jFnBtyhaVtvfQmxdadHWIGrnxmurA)
				{
					return default(OutputReport);
				}
				jFnBtyhaVtvfQmxdadHWIGrnxmurA = false;
				return HbiqUOVIpQGATNoGPGqUEuiOJwkpA;
			}

			public OutputReport IfgKpcarSMYMXDlivxHOXAreSLvn()
			{
				if (!jFnBtyhaVtvfQmxdadHWIGrnxmurA)
				{
					return default(OutputReport);
				}
				return HbiqUOVIpQGATNoGPGqUEuiOJwkpA;
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				HbiqUOVIpQGATNoGPGqUEuiOJwkpA.Clear();
				jFnBtyhaVtvfQmxdadHWIGrnxmurA = false;
			}

			public void Dispose()
			{
				oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void iMcWPzJbivbQRVFtjrscsWpjuUvv()
			{
				try
				{
					oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(false);
				}
				finally
				{
					base.Finalize();
				}
			}

			protected virtual void oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(bool P_0)
			{
				if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
				{
					if (P_0 && aKSeOTDNXYxsQuOgzngtqCfdfsFw != null)
					{
						aKSeOTDNXYxsQuOgzngtqCfdfsFw.Dispose();
					}
					AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
				}
			}
		}

		private const bool OOVfpZgriMyBAOWSMRHDVfSMByTJA = false;

		private const int vuJZNhnnCgBKMeOkrUnFECZhGmSEb = 100;

		private const int FOCLbaZMoxfWDLMPLtUkNdJjiZIl = 10000;

		private ThreadHelper SDWRoAydnweorhdNaUaMNqumjHBn;

		private mrsxmPeniIAlDsIZostwdvTXCbYO aKSeOTDNXYxsQuOgzngtqCfdfsFw;

		private mrsxmPeniIAlDsIZostwdvTXCbYO UUwqAbUowhgBYLOyXXnIpHhuHqVHA;

		private bool aiIRBlALweaIqrbgVAVJhcAgFdSG;

		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		private readonly object wgbuEDDCpQVjjpJmqvtkIRfvdEQw;

		private WriteReportDelegate mYBgGVUBLwISqhExkglUxfLqZUES;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public HidOutputReportHandler(WriteReportDelegate P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("writeReportDelegate");
			}
			mYBgGVUBLwISqhExkglUxfLqZUES = P_0;
			aKSeOTDNXYxsQuOgzngtqCfdfsFw = new mrsxmPeniIAlDsIZostwdvTXCbYO();
			UUwqAbUowhgBYLOyXXnIpHhuHqVHA = new mrsxmPeniIAlDsIZostwdvTXCbYO();
			wgbuEDDCpQVjjpJmqvtkIRfvdEQw = new object();
		}

		public void WriteReport(OutputReport report)
		{
			lock (wgbuEDDCpQVjjpJmqvtkIRfvdEQw)
			{
				if (AeWaeWamxRrERkciQkpFDWbRfZMkA || !report.IsValid || !qhuNDonLyhwaOUJVhxsLRGkQPgDT())
				{
					return;
				}
				lock (aKSeOTDNXYxsQuOgzngtqCfdfsFw)
				{
					aKSeOTDNXYxsQuOgzngtqCfdfsFw.WfdDXzQiHTaPqIKPBANGYaSeZhAC(ref report);
				}
			}
		}

		public void Clear()
		{
			if (aKSeOTDNXYxsQuOgzngtqCfdfsFw != null)
			{
				if (UUwqAbUowhgBYLOyXXnIpHhuHqVHA != null)
				{
					lock (aKSeOTDNXYxsQuOgzngtqCfdfsFw)
					{
						lock (UUwqAbUowhgBYLOyXXnIpHhuHqVHA)
						{
							aKSeOTDNXYxsQuOgzngtqCfdfsFw.SPGTRPyvIslcMdbPTItsewSLRPxx();
							UUwqAbUowhgBYLOyXXnIpHhuHqVHA.SPGTRPyvIslcMdbPTItsewSLRPxx();
							return;
						}
					}
				}
				lock (aKSeOTDNXYxsQuOgzngtqCfdfsFw)
				{
					aKSeOTDNXYxsQuOgzngtqCfdfsFw.SPGTRPyvIslcMdbPTItsewSLRPxx();
					return;
				}
			}
			if (UUwqAbUowhgBYLOyXXnIpHhuHqVHA != null)
			{
				lock (UUwqAbUowhgBYLOyXXnIpHhuHqVHA)
				{
					UUwqAbUowhgBYLOyXXnIpHhuHqVHA.SPGTRPyvIslcMdbPTItsewSLRPxx();
				}
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
					SDWRoAydnweorhdNaUaMNqumjHBn = ThreadHelper.CreateFixedTimeStep(100, 10000);
					SDWRoAydnweorhdNaUaMNqumjHBn.ThreadUpdateEvent += mssMEguIMGMpxuckgaxvBWEALvxE;
					SDWRoAydnweorhdNaUaMNqumjHBn.ThreadStartedEvent += dPQIyPCcKKNWViKLLVSLNIMxKsrv;
					SDWRoAydnweorhdNaUaMNqumjHBn.ThreadPreStopEvent += vnQPDGjLJINhHLqKIgKrtbKcIgXZ;
					SDWRoAydnweorhdNaUaMNqumjHBn.Start(wait: false);
					return true;
				}
				catch (Exception ex)
				{
					Logger.LogError("Exception occurred while creating thread!\n" + ex, requiredThreadSafety: true);
					if (SDWRoAydnweorhdNaUaMNqumjHBn != null)
					{
						SDWRoAydnweorhdNaUaMNqumjHBn.Stop(wait: false);
					}
					aiIRBlALweaIqrbgVAVJhcAgFdSG = true;
					return false;
				}
			}
			if (!SDWRoAydnweorhdNaUaMNqumjHBn.isRunning)
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.Start(wait: false);
			}
			else
			{
				SDWRoAydnweorhdNaUaMNqumjHBn.ResetTimeout();
			}
			return true;
		}

		private void BitcJsIMXoMWIxsFQhXZewBqUJtjA()
		{
			lock (aKSeOTDNXYxsQuOgzngtqCfdfsFw)
			{
				lock (UUwqAbUowhgBYLOyXXnIpHhuHqVHA)
				{
					MiscTools.Swap(ref aKSeOTDNXYxsQuOgzngtqCfdfsFw, ref UUwqAbUowhgBYLOyXXnIpHhuHqVHA);
				}
			}
		}

		private void dPQIyPCcKKNWViKLLVSLNIMxKsrv()
		{
		}

		private void vnQPDGjLJINhHLqKIgKrtbKcIgXZ()
		{
		}

		private void mssMEguIMGMpxuckgaxvBWEALvxE()
		{
			BitcJsIMXoMWIxsFQhXZewBqUJtjA();
			lock (UUwqAbUowhgBYLOyXXnIpHhuHqVHA)
			{
				if (!UUwqAbUowhgBYLOyXXnIpHhuHqVHA.xxlDHgMFNSsjsaoGzxMovcxuvRWl)
				{
					return;
				}
				try
				{
					mYBgGVUBLwISqhExkglUxfLqZUES(UUwqAbUowhgBYLOyXXnIpHhuHqVHA.HikbhTgnRxKSIdGkYcXgbiShdgwFA());
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred while sending HID output report.\nMessage: " + ex.Message, requiredThreadSafety: true);
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~HidOutputReportHandler()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				return;
			}
			lock (wgbuEDDCpQVjjpJmqvtkIRfvdEQw)
			{
				if (disposing)
				{
					Clear();
					if (SDWRoAydnweorhdNaUaMNqumjHBn != null)
					{
						SDWRoAydnweorhdNaUaMNqumjHBn.Dispose();
					}
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
