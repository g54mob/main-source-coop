using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired;
using Rewired.Data;
using Rewired.Interfaces;

internal class mZoUefHoHQZMVPydZmJcbUbNJKUP : IInputSource, IDisposable
{
	private static CwZZDDJAajOcfPoBNojBofelrbNP YQFWfgaboYVqAxcXNfAoXrbDisFs;

	private List<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> qeEYvEcMDoEtWjyHQbikcCgdGBqsB;

	private ReadOnlyCollection<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> KBCENPHzdjTSnbIBTbRyGKqYVCZw;

	private ConfigVars fsFxdrLKqQhrrswhcFuqVydPJMYI;

	private readonly bool YLBBwqirzKQemQhUnACWmXMDveBsA;

	private readonly bool TFqAhUKovjysXWULQQcVrCsWuzKR;

	private readonly bool JGkDbJAStSgnTFVNeEbzyqijymbJB;

	private bool zEUcswfJbvpMRdYuAKggEKBipPcmb;

	[CompilerGenerated]
	private Action m_PplzXFAhrrkLAemHJMuZpWgFUDDe;

	private readonly bool ZAuoqjxachlXlJrVWeZumEgzNjED;

	private readonly bool LLPLPhJZTnQuSTLJgXVGicYmYkvV;

	private readonly bool QIDILQKnCZumBfOTMRkJnNKlDzMk;

	private int AehFyEVrgfuwGQurWIhWGUOFDrTh;

	private bool mwUQVSTjNUEbiIZiGmjNNwrIeWzkA;

	private static readonly string xIHeHjBvPJVhyFlzJEPHLYDyvAYAA = "Rewired Windows Gaming Input support is not available on this system.";

	private bool NkLCzgVmOFrHFIMwZosLNKqBytpP;

	public IUnifiedKeyboardSource ARMePJTasZBcDwrzdQmdOtCaFxsK => null;

	public IUnifiedMouseSource JLhKAeUIShmziyIEjDNrHdwEXpuD => null;

	private event Action PplzXFAhrrkLAemHJMuZpWgFUDDe
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m_PplzXFAhrrkLAemHJMuZpWgFUDDe;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref this.m_PplzXFAhrrkLAemHJMuZpWgFUDDe, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m_PplzXFAhrrkLAemHJMuZpWgFUDDe;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref this.m_PplzXFAhrrkLAemHJMuZpWgFUDDe, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	event Action IInputSource.DeviceChangedEvent
	{
		add
		{
			PplzXFAhrrkLAemHJMuZpWgFUDDe += value;
		}
		remove
		{
			PplzXFAhrrkLAemHJMuZpWgFUDDe -= value;
		}
	}

	public mZoUefHoHQZMVPydZmJcbUbNJKUP(ConfigVars P_0, bool P_1, bool P_2, bool P_3)
	{
		try
		{
			fsFxdrLKqQhrrswhcFuqVydPJMYI = P_0;
			YLBBwqirzKQemQhUnACWmXMDveBsA = P_1;
			TFqAhUKovjysXWULQQcVrCsWuzKR = P_2;
			JGkDbJAStSgnTFVNeEbzyqijymbJB = P_3;
			if (P_2)
			{
				throw new NotImplementedException("WGI mouse input not implemented.");
			}
			if (P_3)
			{
				throw new NotImplementedException("WGI keyboard input not implemented.");
			}
			try
			{
				if (!yvgWAgmxKiqEtmrAqGIhPEEXIuLEA.bIzEGGQGRfdgxHvZhtnVEjYEjUai())
				{
					Logger.LogWarning(xIHeHjBvPJVhyFlzJEPHLYDyvAYAA + " Requires " + yvgWAgmxKiqEtmrAqGIhPEEXIuLEA.FCMJrYAfhPrQiVXaBlwrGCPyAMrjA() + " or greater.");
					throw new Exception();
				}
			}
			catch (DllNotFoundException)
			{
				Logger.LogWarning(xIHeHjBvPJVhyFlzJEPHLYDyvAYAA + " Either Rewired_WindowsGamingInput.dll is missing or this version of Windows does not meet the minimum version requirements for Windows Gaming Input support.");
				throw new Exception();
			}
			catch
			{
				Logger.LogWarning(xIHeHjBvPJVhyFlzJEPHLYDyvAYAA);
				throw new Exception();
			}
			ZAuoqjxachlXlJrVWeZumEgzNjED = true;
			if (QIDILQKnCZumBfOTMRkJnNKlDzMk)
			{
				LLPLPhJZTnQuSTLJgXVGicYmYkvV = false;
			}
			if (ZAuoqjxachlXlJrVWeZumEgzNjED)
			{
				YQFWfgaboYVqAxcXNfAoXrbDisFs = new CwZZDDJAajOcfPoBNojBofelrbNP(gWVQMOlBGiZoeqmduicEARNMqpaI);
			}
			qeEYvEcMDoEtWjyHQbikcCgdGBqsB = new List<MeGehmGvtoXRlfGQhxxMoBPtYUNiA>();
			KBCENPHzdjTSnbIBTbRyGKqYVCZw = new ReadOnlyCollection<MeGehmGvtoXRlfGQhxxMoBPtYUNiA>(qeEYvEcMDoEtWjyHQbikcCgdGBqsB);
			if (ZAuoqjxachlXlJrVWeZumEgzNjED)
			{
				YQFWfgaboYVqAxcXNfAoXrbDisFs.ppnLzmkRVxigBCIQOItRTxFNKroo += gDBZKnMuGvhIWWEmYrmrlzrsJVUN;
			}
			if (P_1)
			{
				QEYIxaeRKPKiuWrUILRKuNwecQMJ(true);
			}
			ReInput.ApplicationFocusChangedEvent += xGLmkdbHWYdyjUJPREYIRnzQAXdo;
		}
		catch (Exception)
		{
			Dispose();
			throw;
		}
	}

	public void lYwicCWmrcxLkPlEtaxPFNHGISUg()
	{
		zEUcswfJbvpMRdYuAKggEKBipPcmb = false;
		QEYIxaeRKPKiuWrUILRKuNwecQMJ(false);
	}

	public bool oIcjcADJDbWNBaFukqovrkIfAdGiA(PidVid P_0)
	{
		if (ZAuoqjxachlXlJrVWeZumEgzNjED && RccfXdGJAhAfYhxuCPZPXoPjzrQkA.VcHsRkDGVdfFkKIPZmxvlfjApkJq(P_0.vendorId, P_0.productId))
		{
			return true;
		}
		return false;
	}

	public void SystemDeviceDisconnected()
	{
		gDBZKnMuGvhIWWEmYrmrlzrsJVUN();
	}

	void IInputSource.SystemDeviceDisconnected()
	{
		//ILSpy generated this explicit interface implementation from .override directive in SystemDeviceDisconnected
		this.SystemDeviceDisconnected();
	}

	public void SystemDeviceConnected()
	{
		gDBZKnMuGvhIWWEmYrmrlzrsJVUN();
	}

	void IInputSource.SystemDeviceConnected()
	{
		//ILSpy generated this explicit interface implementation from .override directive in SystemDeviceConnected
		this.SystemDeviceConnected();
	}

	public void Update()
	{
		if (mwUQVSTjNUEbiIZiGmjNNwrIeWzkA)
		{
			gDBZKnMuGvhIWWEmYrmrlzrsJVUN();
		}
		if (ZAuoqjxachlXlJrVWeZumEgzNjED)
		{
			YQFWfgaboYVqAxcXNfAoXrbDisFs.VanLwmRcqlYjzJbzvbJFbhuIMbacb();
		}
	}

	void IInputSource.Update()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Update
		this.Update();
	}

	public void UpdateDevices(UpdateLoopType updateLoop)
	{
		if (YLBBwqirzKQemQhUnACWmXMDveBsA)
		{
			for (int i = 0; i < qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Count; i++)
			{
				qeEYvEcMDoEtWjyHQbikcCgdGBqsB[i]?.niIUqdWORRLlpUmelcLkFwKfbdBk(updateLoop);
			}
			if (ZAuoqjxachlXlJrVWeZumEgzNjED)
			{
				YQFWfgaboYVqAxcXNfAoXrbDisFs.LWgASUhbznQNWFouJaygETEGJJsJb();
			}
		}
	}

	void IInputSource.UpdateDevices(UpdateLoopType updateLoop)
	{
		//ILSpy generated this explicit interface implementation from .override directive in UpdateDevices
		this.UpdateDevices(updateLoop);
	}

	public void UpdateFinished()
	{
		for (int i = 0; i < qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Count; i++)
		{
			qeEYvEcMDoEtWjyHQbikcCgdGBqsB[i]?.jyNyijgtqXmTGKuggchcsjVOzGwi();
		}
	}

	void IInputSource.UpdateFinished()
	{
		//ILSpy generated this explicit interface implementation from .override directive in UpdateFinished
		this.UpdateFinished();
	}

	public IList<T> GetJoysticks<T>() where T : class
	{
		return KBCENPHzdjTSnbIBTbRyGKqYVCZw as IList<T>;
	}

	IList<T> IInputSource.GetJoysticks<T>()
	{
		//ILSpy generated this explicit interface implementation from .override directive in GetJoysticks
		return this.GetJoysticks<T>();
	}

	private void QEYIxaeRKPKiuWrUILRKuNwecQMJ(bool P_0)
	{
		if (mwUQVSTjNUEbiIZiGmjNNwrIeWzkA)
		{
			mwUQVSTjNUEbiIZiGmjNNwrIeWzkA = false;
		}
		List<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> list = new List<MeGehmGvtoXRlfGQhxxMoBPtYUNiA>();
		int num = 0;
		if (ZAuoqjxachlXlJrVWeZumEgzNjED)
		{
			IList<eofFKxEAMVnHHfOreCsQKlqDSWZM> list2 = YQFWfgaboYVqAxcXNfAoXrbDisFs.GgcbqobGjgRcEuiIXUJADxqshbBL();
			for (int i = 0; i < list2.Count; i++)
			{
				eofFKxEAMVnHHfOreCsQKlqDSWZM eofFKxEAMVnHHfOreCsQKlqDSWZM2 = list2[i];
				if (eofFKxEAMVnHHfOreCsQKlqDSWZM2 != null)
				{
					list.Add(eofFKxEAMVnHHfOreCsQKlqDSWZM2);
					num++;
				}
			}
		}
		if (list.Count == 0)
		{
			qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Clear();
			return;
		}
		int count = list.Count;
		int count2 = qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Count;
		MeGehmGvtoXRlfGQhxxMoBPtYUNiA[] array = new MeGehmGvtoXRlfGQhxxMoBPtYUNiA[count];
		for (int j = 0; j < count; j++)
		{
			bool flag = false;
			for (int k = 0; k < count2; k++)
			{
				if (list[j] != null && qeEYvEcMDoEtWjyHQbikcCgdGBqsB[k] != null && list[j].WGnHpWtoqiFLkATqqfJNSpjJutdUA == qeEYvEcMDoEtWjyHQbikcCgdGBqsB[k].WGnHpWtoqiFLkATqqfJNSpjJutdUA)
				{
					array[j] = qeEYvEcMDoEtWjyHQbikcCgdGBqsB[k];
					array[j].WMXCxOkMJimRdaBWgsmfAAvzflyeA(list[j]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				array[j] = list[j];
			}
		}
		qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Clear();
		for (int l = 0; l < count; l++)
		{
			if (array[l] != null)
			{
				qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Add(array[l]);
			}
		}
	}

	private void gDBZKnMuGvhIWWEmYrmrlzrsJVUN()
	{
		if (YLBBwqirzKQemQhUnACWmXMDveBsA)
		{
			zEUcswfJbvpMRdYuAKggEKBipPcmb = true;
		}
		if (this.PplzXFAhrrkLAemHJMuZpWgFUDDe != null)
		{
			this.PplzXFAhrrkLAemHJMuZpWgFUDDe();
		}
	}

	private int gWVQMOlBGiZoeqmduicEARNMqpaI()
	{
		int aehFyEVrgfuwGQurWIhWGUOFDrTh = AehFyEVrgfuwGQurWIhWGUOFDrTh;
		if (AehFyEVrgfuwGQurWIhWGUOFDrTh == 2147483647)
		{
			AehFyEVrgfuwGQurWIhWGUOFDrTh = 0;
			return aehFyEVrgfuwGQurWIhWGUOFDrTh;
		}
		AehFyEVrgfuwGQurWIhWGUOFDrTh++;
		return aehFyEVrgfuwGQurWIhWGUOFDrTh;
	}

	private void xGLmkdbHWYdyjUJPREYIRnzQAXdo(bool P_0)
	{
		if (YLBBwqirzKQemQhUnACWmXMDveBsA && P_0)
		{
			mwUQVSTjNUEbiIZiGmjNNwrIeWzkA = true;
		}
	}

	public void Dispose()
	{
		zWuJCEojwaGMMuVZVlIsKdotFGHR(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void NfeKSdlgQhasiEnOnFLfeFkFzSaGB()
	{
		try
		{
			zWuJCEojwaGMMuVZVlIsKdotFGHR(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void zWuJCEojwaGMMuVZVlIsKdotFGHR(bool P_0)
	{
		if (NkLCzgVmOFrHFIMwZosLNKqBytpP)
		{
			return;
		}
		if (P_0)
		{
			ReInput.ApplicationFocusChangedEvent -= xGLmkdbHWYdyjUJPREYIRnzQAXdo;
			if (YQFWfgaboYVqAxcXNfAoXrbDisFs != null)
			{
				YQFWfgaboYVqAxcXNfAoXrbDisFs.Dispose();
			}
			if (qeEYvEcMDoEtWjyHQbikcCgdGBqsB != null)
			{
				for (int i = 0; i < qeEYvEcMDoEtWjyHQbikcCgdGBqsB.Count; i++)
				{
					if (qeEYvEcMDoEtWjyHQbikcCgdGBqsB[i] != null)
					{
						qeEYvEcMDoEtWjyHQbikcCgdGBqsB[i].Dispose();
					}
				}
			}
		}
		NkLCzgVmOFrHFIMwZosLNKqBytpP = true;
	}
}
