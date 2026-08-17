using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class CwZZDDJAajOcfPoBNojBofelrbNP : IDisposable
{
	private abstract class TeTnSJBMwSxSVkQXbAGDqylvdjym : IPoolableObject, IDisposable, IPoolableObject_Internal
	{
		[CompilerGenerated]
		private IObjectPool FJWApKzlISApBQukZTuCVlVcmyYT;

		IObjectPool IPoolableObject_Internal.pool
		{
			[CompilerGenerated]
			get
			{
				return FJWApKzlISApBQukZTuCVlVcmyYT;
			}
			[CompilerGenerated]
			set
			{
				FJWApKzlISApBQukZTuCVlVcmyYT = value;
			}
		}

		protected abstract void Clear();

		void IPoolableObject_Internal.Clear()
		{
			Clear();
		}

		void IDisposable.Dispose()
		{
			Return();
		}

		public void Return()
		{
			((IPoolableObject_Internal)this).pool?.Return(this);
		}

		void IPoolableObject.Return()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Return
			this.Return();
		}
	}

	private class wdoImNIKaVkHPlCevUPQJTNehiVN : TeTnSJBMwSxSVkQXbAGDqylvdjym
	{
		public RccfXdGJAhAfYhxuCPZPXoPjzrQkA TtFqinaGbyWJLDSaLIaBkNtKNNqg;

		public woIXEaxkDSRQKTJnvfWBciuaAYVB GlslCJAWUBTstMDkPxtPzNqUNEoe;

		public double iqzxCqeNahpbvTDHZENzasmolKSYA;

		protected virtual void aNKCenalluxoGhebAxRdDplNQNM()
		{
			TtFqinaGbyWJLDSaLIaBkNtKNNqg = null;
			GlslCJAWUBTstMDkPxtPzNqUNEoe = default(woIXEaxkDSRQKTJnvfWBciuaAYVB);
			iqzxCqeNahpbvTDHZENzasmolKSYA = 0.0;
		}
	}

	private sealed class ISZkCHojwwaHGNRNAOASpUgssFAO : TeTnSJBMwSxSVkQXbAGDqylvdjym
	{
		public RccfXdGJAhAfYhxuCPZPXoPjzrQkA ORMWhJuyproDNSOgpfgiYpYVEGlCA;

		public nqXEdeCznmdQnPGHYFzpCcMKdXyjb OmwcgPHbUMuKwasltpQQRxatSoQCA;

		protected void qHgqySxboiXvdwQjgYWnzzxbRVib()
		{
			ORMWhJuyproDNSOgpfgiYpYVEGlCA = null;
			OmwcgPHbUMuKwasltpQQRxatSoQCA = default(nqXEdeCznmdQnPGHYFzpCcMKdXyjb);
		}
	}

	[Serializable]
	private sealed class PwzfrGKqSAsEFcgDRdoXeZSeOWFBb
	{
		public static readonly PwzfrGKqSAsEFcgDRdoXeZSeOWFBb _003C_003E9 = new PwzfrGKqSAsEFcgDRdoXeZSeOWFBb();

		public static Func<wdoImNIKaVkHPlCevUPQJTNehiVN> _003C_003E9__19_0;

		public static Func<ISZkCHojwwaHGNRNAOASpUgssFAO> _003C_003E9__19_1;

		internal wdoImNIKaVkHPlCevUPQJTNehiVN wlSdNWnFHZgSrkWAzCvmaWbTXCIuA()
		{
			return new wdoImNIKaVkHPlCevUPQJTNehiVN();
		}

		internal ISZkCHojwwaHGNRNAOASpUgssFAO doIKHUWXhQzpijpEChsAxWeLfrED()
		{
			return new ISZkCHojwwaHGNRNAOASpUgssFAO();
		}
	}

	private readonly List<eofFKxEAMVnHHfOreCsQKlqDSWZM> YUdCCkhrapicZwuJCIYBFXQmsdGC;

	private readonly ReadOnlyCollection<eofFKxEAMVnHHfOreCsQKlqDSWZM> EuZqgSteKrSXLoQJrvktKRgHbrRd;

	private readonly List<RccfXdGJAhAfYhxuCPZPXoPjzrQkA> riWDbfBVIonfdWGiDQtEogKjBjog;

	private readonly Func<int> KSrTWPMBeXveracyjxNzxHQomPwx;

	private readonly Rewired.Utils.Classes.Utility.SpinLock hKvEgqkvbPJvgdNlcOuUGrTUDqjA = new Rewired.Utils.Classes.Utility.SpinLock();

	private readonly Rewired.Utils.Classes.Utility.SpinLock KyVJQVvktBJmfczkMrXOaltlcjod = new Rewired.Utils.Classes.Utility.SpinLock();

	private RingBuffer<wdoImNIKaVkHPlCevUPQJTNehiVN> CdZQjvkvguCMUlqEGKfSGQWtgXTs;

	private RingBuffer<ISZkCHojwwaHGNRNAOASpUgssFAO> tXpuMrjCFkoJLxEolHtMRLNAAfHM;

	private bool QqzvmkwhHrqLViCubXNkwzDJcipJ;

	private readonly ThreadSafeObjectPool<wdoImNIKaVkHPlCevUPQJTNehiVN> BxuugxZArKkNoKraWmiZsZURbkaV;

	private readonly ThreadSafeObjectPool<ISZkCHojwwaHGNRNAOASpUgssFAO> JaCyTrVcPNGgGVeehPQTYuzvZUjj;

	private readonly List<eofFKxEAMVnHHfOreCsQKlqDSWZM> XZESSLrWaPHzWiaSGuOwiNqaATVp;

	private RingBuffer<wdoImNIKaVkHPlCevUPQJTNehiVN> jVIEAsgSGQeKUveTEfntwwAmnXVH;

	private RingBuffer<ISZkCHojwwaHGNRNAOASpUgssFAO> EltecAbiSnhanjHWkNnWsuzqFjIkA;

	private bool UPCteEOVORAdZDIoqAXOPWHdDRuDA;

	private Action<RccfXdGJAhAfYhxuCPZPXoPjzrQkA, nqXEdeCznmdQnPGHYFzpCcMKdXyjb> BkxJtdaCZrDZJGJzMVKcbSRELAKgA;

	[CompilerGenerated]
	private Action m_ppnLzmkRVxigBCIQOItRTxFNKroo;

	private bool CpWDnjbGuzTywRQYWqXTjNHyLIVY;

	private static Guid[] iXqfRTBAeaOmwIbTFSfBSFzLwuYrB;

	private static string[] LVfgsnBHANraKqFGYEmcRAhZfSCrA;

	private static string[] NLibBzIUxLJQeYXDCCHGeGNJoThSb;

	public event Action ppnLzmkRVxigBCIQOItRTxFNKroo
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m_ppnLzmkRVxigBCIQOItRTxFNKroo;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref this.m_ppnLzmkRVxigBCIQOItRTxFNKroo, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m_ppnLzmkRVxigBCIQOItRTxFNKroo;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref this.m_ppnLzmkRVxigBCIQOItRTxFNKroo, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public CwZZDDJAajOcfPoBNojBofelrbNP(Func<int> P_0)
	{
		KSrTWPMBeXveracyjxNzxHQomPwx = P_0;
		BkxJtdaCZrDZJGJzMVKcbSRELAKgA = BJbGUBDiuEkzaKnePbUDLOUNNDUcA;
		YUdCCkhrapicZwuJCIYBFXQmsdGC = new List<eofFKxEAMVnHHfOreCsQKlqDSWZM>();
		XZESSLrWaPHzWiaSGuOwiNqaATVp = new List<eofFKxEAMVnHHfOreCsQKlqDSWZM>();
		EuZqgSteKrSXLoQJrvktKRgHbrRd = new ReadOnlyCollection<eofFKxEAMVnHHfOreCsQKlqDSWZM>(YUdCCkhrapicZwuJCIYBFXQmsdGC);
		riWDbfBVIonfdWGiDQtEogKjBjog = new List<RccfXdGJAhAfYhxuCPZPXoPjzrQkA>();
		QqzvmkwhHrqLViCubXNkwzDJcipJ = ReInput.IsInputAllowed(ControllerType.Joystick);
		int num = (int)(0.5f * (float)FwvuhjisMNfwRNPCnXxbQzkrWKy.VjeFpcjaFerIirGHKspqAQDAgjGxA * 32f) + 1;
		BxuugxZArKkNoKraWmiZsZURbkaV = new ThreadSafeObjectPool<wdoImNIKaVkHPlCevUPQJTNehiVN>(num, PwzfrGKqSAsEFcgDRdoXeZSeOWFBb._003C_003E9.wlSdNWnFHZgSrkWAzCvmaWbTXCIuA);
		JaCyTrVcPNGgGVeehPQTYuzvZUjj = new ThreadSafeObjectPool<ISZkCHojwwaHGNRNAOASpUgssFAO>(128, PwzfrGKqSAsEFcgDRdoXeZSeOWFBb._003C_003E9.doIKHUWXhQzpijpEChsAxWeLfrED);
		CdZQjvkvguCMUlqEGKfSGQWtgXTs = new RingBuffer<wdoImNIKaVkHPlCevUPQJTNehiVN>(num);
		tXpuMrjCFkoJLxEolHtMRLNAAfHM = new RingBuffer<ISZkCHojwwaHGNRNAOASpUgssFAO>(128);
		jVIEAsgSGQeKUveTEfntwwAmnXVH = new RingBuffer<wdoImNIKaVkHPlCevUPQJTNehiVN>(num);
		EltecAbiSnhanjHWkNnWsuzqFjIkA = new RingBuffer<ISZkCHojwwaHGNRNAOASpUgssFAO>(128);
		RccfXdGJAhAfYhxuCPZPXoPjzrQkA.blVCZWUurzSDKuUeMTcYfBBtZoJQ += kCIyAcgWNPsKSzzsxptgjvHaGdJQ;
		RccfXdGJAhAfYhxuCPZPXoPjzrQkA.NoTiPNhErejLQKbryCvQdfPOoJRGb += ugPdmmnvgTcMphUaFmypsqHpjQZo;
		FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent += ZlDnKRCnWSZEwBiVNEoRgvDaugMb;
		FwvuhjisMNfwRNPCnXxbQzkrWKy.apYJZWeiHEkMNcOtWVjXVlAhaguy.ThreadUpdateEvent += IzXKknbMmrenjgakZrVAUymhoKLi;
		ReInput.ApplicationFocusChangedEvent += PzQBLnjjEeADqBgzSXaxqPJypVbH;
		ReInput.ApplicationPauseChangedEvent += TcuAugwrOsaNCiAJjGMjDqxenCuac;
		RccfXdGJAhAfYhxuCPZPXoPjzrQkA.EQQpAcjAYrhOezYzDawxzyqEocwh();
		ygCuaGLYtGKkFEVMrKagNwPmlSuP();
	}

	public void VanLwmRcqlYjzJbzvbJFbhuIMbacb()
	{
		bool flag = false;
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			if (UPCteEOVORAdZDIoqAXOPWHdDRuDA)
			{
				UPCteEOVORAdZDIoqAXOPWHdDRuDA = false;
				flag = true;
			}
		}
		if (flag)
		{
			ygCuaGLYtGKkFEVMrKagNwPmlSuP();
		}
	}

	public void LWgASUhbznQNWFouJaygETEGJJsJb()
	{
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			MiscTools.Swap(ref CdZQjvkvguCMUlqEGKfSGQWtgXTs, ref jVIEAsgSGQeKUveTEfntwwAmnXVH);
		}
		while (CdZQjvkvguCMUlqEGKfSGQWtgXTs.Count > 0)
		{
			wdoImNIKaVkHPlCevUPQJTNehiVN wdoImNIKaVkHPlCevUPQJTNehiVN2 = CdZQjvkvguCMUlqEGKfSGQWtgXTs.Dequeue();
			int num = HztIGwKxPMGotSSgHrwfBoILinnb(YUdCCkhrapicZwuJCIYBFXQmsdGC, wdoImNIKaVkHPlCevUPQJTNehiVN2.TtFqinaGbyWJLDSaLIaBkNtKNNqg);
			if (num >= 0)
			{
				YUdCCkhrapicZwuJCIYBFXQmsdGC[num].PjPYtzFmmOSWAMFUxcCTKmOdhgMq(wdoImNIKaVkHPlCevUPQJTNehiVN2.GlslCJAWUBTstMDkPxtPzNqUNEoe, wdoImNIKaVkHPlCevUPQJTNehiVN2.iqzxCqeNahpbvTDHZENzasmolKSYA);
			}
			wdoImNIKaVkHPlCevUPQJTNehiVN2.Return();
		}
	}

	private void BJbGUBDiuEkzaKnePbUDLOUNNDUcA(RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_0, nqXEdeCznmdQnPGHYFzpCcMKdXyjb P_1)
	{
		if (!QqzvmkwhHrqLViCubXNkwzDJcipJ)
		{
			return;
		}
		using (KyVJQVvktBJmfczkMrXOaltlcjod.Lock())
		{
			ISZkCHojwwaHGNRNAOASpUgssFAO iSZkCHojwwaHGNRNAOASpUgssFAO = JaCyTrVcPNGgGVeehPQTYuzvZUjj.Get();
			iSZkCHojwwaHGNRNAOASpUgssFAO.ORMWhJuyproDNSOgpfgiYpYVEGlCA = P_0;
			iSZkCHojwwaHGNRNAOASpUgssFAO.OmwcgPHbUMuKwasltpQQRxatSoQCA = P_1;
			tXpuMrjCFkoJLxEolHtMRLNAAfHM.Enqueue(iSZkCHojwwaHGNRNAOASpUgssFAO);
		}
	}

	public IList<eofFKxEAMVnHHfOreCsQKlqDSWZM> GgcbqobGjgRcEuiIXUJADxqshbBL()
	{
		return EuZqgSteKrSXLoQJrvktKRgHbrRd;
	}

	private void ygCuaGLYtGKkFEVMrKagNwPmlSuP()
	{
		bool flag = false;
		List<RccfXdGJAhAfYhxuCPZPXoPjzrQkA> list = riWDbfBVIonfdWGiDQtEogKjBjog;
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			RccfXdGJAhAfYhxuCPZPXoPjzrQkA.BgUjDfaRDdxxqKLxVmMxiFztFQHX(list);
			for (int num = XZESSLrWaPHzWiaSGuOwiNqaATVp.Count - 1; num >= 0; num--)
			{
				if (!lCXIOlRvzzhZtzjtwAglzaCaQkev(list, XZESSLrWaPHzWiaSGuOwiNqaATVp[num].pjoPTqajYDsooDcWrUNdKuKQUGMg))
				{
					XZESSLrWaPHzWiaSGuOwiNqaATVp[num].pjoPTqajYDsooDcWrUNdKuKQUGMg.kxWZtZeCLFJBGPBhigTOedpPNXxzA();
					XZESSLrWaPHzWiaSGuOwiNqaATVp[num].Dispose();
					XZESSLrWaPHzWiaSGuOwiNqaATVp.RemoveAt(num);
					flag = true;
				}
			}
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				RccfXdGJAhAfYhxuCPZPXoPjzrQkA rccfXdGJAhAfYhxuCPZPXoPjzrQkA = list[num2];
				if (RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(rccfXdGJAhAfYhxuCPZPXoPjzrQkA, null))
				{
					list.RemoveAt(num2);
				}
				else
				{
					int num3 = HztIGwKxPMGotSSgHrwfBoILinnb(XZESSLrWaPHzWiaSGuOwiNqaATVp, rccfXdGJAhAfYhxuCPZPXoPjzrQkA);
					if (num3 >= 0)
					{
						list[num2].kxWZtZeCLFJBGPBhigTOedpPNXxzA();
						list[num2] = XZESSLrWaPHzWiaSGuOwiNqaATVp[num3].pjoPTqajYDsooDcWrUNdKuKQUGMg;
					}
					else
					{
						XZESSLrWaPHzWiaSGuOwiNqaATVp.Add(new eofFKxEAMVnHHfOreCsQKlqDSWZM(rccfXdGJAhAfYhxuCPZPXoPjzrQkA, KSrTWPMBeXveracyjxNzxHQomPwx(), BkxJtdaCZrDZJGJzMVKcbSRELAKgA));
						flag = true;
					}
				}
			}
			for (int num4 = list.Count - 1; num4 >= 0; num4--)
			{
				RccfXdGJAhAfYhxuCPZPXoPjzrQkA rccfXdGJAhAfYhxuCPZPXoPjzrQkA2 = list[num4];
				int num5 = HztIGwKxPMGotSSgHrwfBoILinnb(XZESSLrWaPHzWiaSGuOwiNqaATVp, rccfXdGJAhAfYhxuCPZPXoPjzrQkA2);
				if (num5 >= 0)
				{
					eofFKxEAMVnHHfOreCsQKlqDSWZM item = XZESSLrWaPHzWiaSGuOwiNqaATVp[num5];
					XZESSLrWaPHzWiaSGuOwiNqaATVp.RemoveAt(num5);
					XZESSLrWaPHzWiaSGuOwiNqaATVp.Insert(0, item);
				}
			}
			YUdCCkhrapicZwuJCIYBFXQmsdGC.Clear();
			for (int i = 0; i < XZESSLrWaPHzWiaSGuOwiNqaATVp.Count; i++)
			{
				YUdCCkhrapicZwuJCIYBFXQmsdGC.Add(XZESSLrWaPHzWiaSGuOwiNqaATVp[i]);
			}
		}
		list.Clear();
		if (flag)
		{
			this.ppnLzmkRVxigBCIQOItRTxFNKroo?.Invoke();
		}
	}

	private void PzQBLnjjEeADqBgzSXaxqPJypVbH(bool P_0)
	{
		QqzvmkwhHrqLViCubXNkwzDJcipJ = ReInput.IsInputAllowed(ControllerType.Joystick);
		if (!QqzvmkwhHrqLViCubXNkwzDJcipJ)
		{
			using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
			{
				CdZQjvkvguCMUlqEGKfSGQWtgXTs.Clear();
			}
		}
	}

	private void TcuAugwrOsaNCiAJjGMjDqxenCuac(bool P_0)
	{
		QqzvmkwhHrqLViCubXNkwzDJcipJ = ReInput.IsInputAllowed(ControllerType.Joystick);
		if (!QqzvmkwhHrqLViCubXNkwzDJcipJ)
		{
			using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
			{
				CdZQjvkvguCMUlqEGKfSGQWtgXTs.Clear();
			}
		}
	}

	private void ZlDnKRCnWSZEwBiVNEoRgvDaugMb()
	{
		if (CpWDnjbGuzTywRQYWqXTjNHyLIVY || !QqzvmkwhHrqLViCubXNkwzDJcipJ)
		{
			return;
		}
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			int count = XZESSLrWaPHzWiaSGuOwiNqaATVp.Count;
			for (int i = 0; i < count; i++)
			{
				wdoImNIKaVkHPlCevUPQJTNehiVN wdoImNIKaVkHPlCevUPQJTNehiVN2 = BxuugxZArKkNoKraWmiZsZURbkaV.Get();
				wdoImNIKaVkHPlCevUPQJTNehiVN2.TtFqinaGbyWJLDSaLIaBkNtKNNqg = XZESSLrWaPHzWiaSGuOwiNqaATVp[i].pjoPTqajYDsooDcWrUNdKuKQUGMg;
				wdoImNIKaVkHPlCevUPQJTNehiVN2.GlslCJAWUBTstMDkPxtPzNqUNEoe = wdoImNIKaVkHPlCevUPQJTNehiVN2.TtFqinaGbyWJLDSaLIaBkNtKNNqg.UHStfjZjlamrWiLhGdIfhzlbGQdx();
				wdoImNIKaVkHPlCevUPQJTNehiVN2.iqzxCqeNahpbvTDHZENzasmolKSYA = ReInput.realTime;
				jVIEAsgSGQeKUveTEfntwwAmnXVH.Enqueue(wdoImNIKaVkHPlCevUPQJTNehiVN2);
			}
		}
	}

	private void IzXKknbMmrenjgakZrVAUymhoKLi()
	{
		if (CpWDnjbGuzTywRQYWqXTjNHyLIVY)
		{
			return;
		}
		using (KyVJQVvktBJmfczkMrXOaltlcjod.Lock())
		{
			MiscTools.Swap(ref tXpuMrjCFkoJLxEolHtMRLNAAfHM, ref EltecAbiSnhanjHWkNnWsuzqFjIkA);
		}
		while (EltecAbiSnhanjHWkNnWsuzqFjIkA.Count > 0)
		{
			ISZkCHojwwaHGNRNAOASpUgssFAO iSZkCHojwwaHGNRNAOASpUgssFAO = EltecAbiSnhanjHWkNnWsuzqFjIkA.Dequeue();
			try
			{
				iSZkCHojwwaHGNRNAOASpUgssFAO.ORMWhJuyproDNSOgpfgiYpYVEGlCA.pESTISaTVyafAzkAzGSrFwRauLCRA = iSZkCHojwwaHGNRNAOASpUgssFAO.OmwcgPHbUMuKwasltpQQRxatSoQCA;
			}
			catch
			{
			}
			iSZkCHojwwaHGNRNAOASpUgssFAO.Return();
		}
	}

	private void kCIyAcgWNPsKSzzsxptgjvHaGdJQ(RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_0)
	{
		P_0.kxWZtZeCLFJBGPBhigTOedpPNXxzA();
		if (CpWDnjbGuzTywRQYWqXTjNHyLIVY)
		{
			return;
		}
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			UPCteEOVORAdZDIoqAXOPWHdDRuDA = true;
		}
	}

	private void ugPdmmnvgTcMphUaFmypsqHpjQZo(RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_0)
	{
		P_0.kxWZtZeCLFJBGPBhigTOedpPNXxzA();
		if (CpWDnjbGuzTywRQYWqXTjNHyLIVY)
		{
			return;
		}
		using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
		{
			UPCteEOVORAdZDIoqAXOPWHdDRuDA = true;
		}
	}

	public void Dispose()
	{
		yXkTfBWftVdKUehNnmGheBlyJLzbA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void SQcanUALUgEQgMjFFDGYjshkgohKb()
	{
		try
		{
			yXkTfBWftVdKUehNnmGheBlyJLzbA(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void yXkTfBWftVdKUehNnmGheBlyJLzbA(bool P_0)
	{
		if (CpWDnjbGuzTywRQYWqXTjNHyLIVY)
		{
			return;
		}
		if (P_0)
		{
			ReInput.ApplicationFocusChangedEvent -= PzQBLnjjEeADqBgzSXaxqPJypVbH;
			ReInput.ApplicationPauseChangedEvent -= TcuAugwrOsaNCiAJjGMjDqxenCuac;
			RccfXdGJAhAfYhxuCPZPXoPjzrQkA.blVCZWUurzSDKuUeMTcYfBBtZoJQ -= kCIyAcgWNPsKSzzsxptgjvHaGdJQ;
			RccfXdGJAhAfYhxuCPZPXoPjzrQkA.NoTiPNhErejLQKbryCvQdfPOoJRGb -= ugPdmmnvgTcMphUaFmypsqHpjQZo;
			FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent -= ZlDnKRCnWSZEwBiVNEoRgvDaugMb;
			FwvuhjisMNfwRNPCnXxbQzkrWKy.apYJZWeiHEkMNcOtWVjXVlAhaguy.ThreadUpdateEvent -= IzXKknbMmrenjgakZrVAUymhoKLi;
			using (hKvEgqkvbPJvgdNlcOuUGrTUDqjA.Lock())
			{
				for (int i = 0; i < XZESSLrWaPHzWiaSGuOwiNqaATVp.Count; i++)
				{
					try
					{
						XZESSLrWaPHzWiaSGuOwiNqaATVp[i].Dispose();
						XZESSLrWaPHzWiaSGuOwiNqaATVp[i].pjoPTqajYDsooDcWrUNdKuKQUGMg.kxWZtZeCLFJBGPBhigTOedpPNXxzA();
					}
					catch
					{
					}
				}
				XZESSLrWaPHzWiaSGuOwiNqaATVp.Clear();
				YUdCCkhrapicZwuJCIYBFXQmsdGC.Clear();
			}
			try
			{
				RccfXdGJAhAfYhxuCPZPXoPjzrQkA.kURehxXfCPiZPVWDOkVGJdEnpFKM();
			}
			catch
			{
			}
		}
		CpWDnjbGuzTywRQYWqXTjNHyLIVY = true;
	}

	private static bool lCXIOlRvzzhZtzjtwAglzaCaQkev(IList<RccfXdGJAhAfYhxuCPZPXoPjzrQkA> P_0, RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_1)
	{
		return NMcdUwsucWdCAHLpxrBgAUNanZmC(P_0, P_1) >= 0;
	}

	private static int HztIGwKxPMGotSSgHrwfBoILinnb(IList<eofFKxEAMVnHHfOreCsQKlqDSWZM> P_0, RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_1)
	{
		if (P_0 == null || RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(P_1, null))
		{
			return -1;
		}
		for (int i = 0; i < P_0.Count; i++)
		{
			if (P_0[i] != null && RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(P_0[i].pjoPTqajYDsooDcWrUNdKuKQUGMg, P_1))
			{
				return i;
			}
		}
		return -1;
	}

	private static int NMcdUwsucWdCAHLpxrBgAUNanZmC(IList<RccfXdGJAhAfYhxuCPZPXoPjzrQkA> P_0, RccfXdGJAhAfYhxuCPZPXoPjzrQkA P_1)
	{
		if (P_0 == null || RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(P_1, null))
		{
			return -1;
		}
		for (int i = 0; i < P_0.Count; i++)
		{
			if (!RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(P_0[i], null) && RccfXdGJAhAfYhxuCPZPXoPjzrQkA.ZgTwfIWcHxcbKEZSWcVVaMKBNCpDc(P_0[i], P_1))
			{
				return i;
			}
		}
		return -1;
	}

	static CwZZDDJAajOcfPoBNojBofelrbNP()
	{
		iXqfRTBAeaOmwIbTFSfBSFzLwuYrB = new Guid[1]
		{
			new Guid("02e0045e-0000-0000-0000-504944564944")
		};
		LVfgsnBHANraKqFGYEmcRAhZfSCrA = new string[1] { "Xbox Bluetooth Gamepad" };
		NLibBzIUxLJQeYXDCCHGeGNJoThSb = new string[1] { "Xbox Wireless Controller.*" };
	}
}
