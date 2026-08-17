using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ThreadedMessageQueue<T> : IDisposable
	{
		private readonly int SrSKwNYIWTDkPXJWZGHujOGbdppW;

		private readonly int tNhRGLPEaAktluORaOnqIZCTvtFM;

		private readonly int XqUxmYboKBlrGCstpNtvbpRqzwaQ;

		private readonly bool kiPnqVwMiZpmRlqBOTfdXlwaaUnR;

		private ThreadHelper SDWRoAydnweorhdNaUaMNqumjHBn;

		private Queue<T> oBzavpHkLVIRuOXlDYOkfvQzKSMdA;

		private Queue<T> RVmAeqbavyjyfBwHStgoutgqidxi;

		private bool aiIRBlALweaIqrbgVAVJhcAgFdSG;

		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		private Action<T> WliraZuJRSDHlBnzWcRjryowYjXeA;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public ThreadedMessageQueue(int P_0, int P_1, int P_2, bool P_3, Action<T> P_4)
		{
			if (P_4 == null)
			{
				throw new ArgumentNullException("messageReceiverDelegate");
			}
			if (P_0 < 0)
			{
				P_0 = 0;
			}
			if (P_1 < 0)
			{
				P_1 = 0;
			}
			if (P_2 < 0)
			{
				P_2 = 0;
			}
			SrSKwNYIWTDkPXJWZGHujOGbdppW = P_0;
			tNhRGLPEaAktluORaOnqIZCTvtFM = P_1;
			XqUxmYboKBlrGCstpNtvbpRqzwaQ = P_2;
			kiPnqVwMiZpmRlqBOTfdXlwaaUnR = P_3;
			WliraZuJRSDHlBnzWcRjryowYjXeA = P_4;
			oBzavpHkLVIRuOXlDYOkfvQzKSMdA = new Queue<T>(P_0);
			RVmAeqbavyjyfBwHStgoutgqidxi = new Queue<T>(P_0);
		}

		public void Enqueue(T message)
		{
			if (!qhuNDonLyhwaOUJVhxsLRGkQPgDT())
			{
				return;
			}
			lock (oBzavpHkLVIRuOXlDYOkfvQzKSMdA)
			{
				if (SrSKwNYIWTDkPXJWZGHujOGbdppW > 0)
				{
					while (oBzavpHkLVIRuOXlDYOkfvQzKSMdA.Count >= SrSKwNYIWTDkPXJWZGHujOGbdppW)
					{
						oBzavpHkLVIRuOXlDYOkfvQzKSMdA.Dequeue();
					}
				}
				oBzavpHkLVIRuOXlDYOkfvQzKSMdA.Enqueue(message);
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
					SDWRoAydnweorhdNaUaMNqumjHBn.Start(kiPnqVwMiZpmRlqBOTfdXlwaaUnR);
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

		private void ymTTiHjCQeVwmKjCBWYWdpLPgdrM()
		{
			lock (oBzavpHkLVIRuOXlDYOkfvQzKSMdA)
			{
				lock (RVmAeqbavyjyfBwHStgoutgqidxi)
				{
					MiscTools.Swap(ref oBzavpHkLVIRuOXlDYOkfvQzKSMdA, ref RVmAeqbavyjyfBwHStgoutgqidxi);
				}
			}
		}

		private void mssMEguIMGMpxuckgaxvBWEALvxE()
		{
			ymTTiHjCQeVwmKjCBWYWdpLPgdrM();
			lock (RVmAeqbavyjyfBwHStgoutgqidxi)
			{
				while (RVmAeqbavyjyfBwHStgoutgqidxi.Count > 0)
				{
					try
					{
						WliraZuJRSDHlBnzWcRjryowYjXeA(RVmAeqbavyjyfBwHStgoutgqidxi.Dequeue());
					}
					catch (Exception ex)
					{
						Logger.LogError("An exception occurred while sending message.\nMessage: " + ex.Message, requiredThreadSafety: true);
					}
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~ThreadedMessageQueue()
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
				if (oBzavpHkLVIRuOXlDYOkfvQzKSMdA != null)
				{
					if (RVmAeqbavyjyfBwHStgoutgqidxi != null)
					{
						lock (oBzavpHkLVIRuOXlDYOkfvQzKSMdA)
						{
							lock (RVmAeqbavyjyfBwHStgoutgqidxi)
							{
								oBzavpHkLVIRuOXlDYOkfvQzKSMdA.Clear();
								RVmAeqbavyjyfBwHStgoutgqidxi.Clear();
							}
						}
					}
					else
					{
						lock (oBzavpHkLVIRuOXlDYOkfvQzKSMdA)
						{
							oBzavpHkLVIRuOXlDYOkfvQzKSMdA.Clear();
						}
					}
				}
				else if (RVmAeqbavyjyfBwHStgoutgqidxi != null)
				{
					lock (RVmAeqbavyjyfBwHStgoutgqidxi)
					{
						RVmAeqbavyjyfBwHStgoutgqidxi.Clear();
					}
				}
				if (SDWRoAydnweorhdNaUaMNqumjHBn != null)
				{
					SDWRoAydnweorhdNaUaMNqumjHBn.Dispose();
				}
			}
			AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
		}
	}
}
