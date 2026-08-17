using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Utils;

internal class dRxYZKovikdvFiOlZLmFiKpaWUdu
{
	private byte RIUppuPjAtxguNIPTIHtyKfbSGQc;

	private byte POojPmaViDIujhNfCVFxdPMBBQcV;

	private byte zCbrYekcGvsrNfMfeJViHALovsLt;

	[CompilerGenerated]
	private Action m_KhdFqLHnkQpyjokVAndSadBMcFSRA;

	public float mQPaVmdRqYozYExrtihYfahdYhPF
	{
		get
		{
			return (float)(int)RIUppuPjAtxguNIPTIHtyKfbSGQc / 255f;
		}
		set
		{
			dzlPvBalHRSfegtkxkAECZRZUliD = (byte)MathTools.Clamp((int)(num * 255f), 0, 255);
		}
	}

	public float ffwrCveDusqznMzxjnoAkStTGZyeA
	{
		get
		{
			return (float)(int)POojPmaViDIujhNfCVFxdPMBBQcV / 255f;
		}
		set
		{
			pUNnpXbqlHMdMbFBrwAbNRJiZxKR = (byte)MathTools.Clamp((int)(num * 255f), 0, 255);
		}
	}

	public float DnwgdpganiBQCDljGtGXWAzaoHBmB
	{
		get
		{
			return (float)(int)zCbrYekcGvsrNfMfeJViHALovsLt / 255f;
		}
		set
		{
			ZfhhhzCONloJjmcuIfFhItNGYTyBc = (byte)MathTools.Clamp((int)(num * 255f), 0, 255);
		}
	}

	public byte dzlPvBalHRSfegtkxkAECZRZUliD
	{
		get
		{
			return RIUppuPjAtxguNIPTIHtyKfbSGQc;
		}
		set
		{
			RIUppuPjAtxguNIPTIHtyKfbSGQc = rIUppuPjAtxguNIPTIHtyKfbSGQc;
			if (this.KhdFqLHnkQpyjokVAndSadBMcFSRA != null)
			{
				this.KhdFqLHnkQpyjokVAndSadBMcFSRA();
			}
		}
	}

	public byte pUNnpXbqlHMdMbFBrwAbNRJiZxKR
	{
		get
		{
			return POojPmaViDIujhNfCVFxdPMBBQcV;
		}
		set
		{
			POojPmaViDIujhNfCVFxdPMBBQcV = pOojPmaViDIujhNfCVFxdPMBBQcV;
			if (this.KhdFqLHnkQpyjokVAndSadBMcFSRA != null)
			{
				this.KhdFqLHnkQpyjokVAndSadBMcFSRA();
			}
		}
	}

	public byte ZfhhhzCONloJjmcuIfFhItNGYTyBc
	{
		get
		{
			return zCbrYekcGvsrNfMfeJViHALovsLt;
		}
		set
		{
			zCbrYekcGvsrNfMfeJViHALovsLt = b;
			if (this.KhdFqLHnkQpyjokVAndSadBMcFSRA != null)
			{
				this.KhdFqLHnkQpyjokVAndSadBMcFSRA();
			}
		}
	}

	public event Action KhdFqLHnkQpyjokVAndSadBMcFSRA
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m_KhdFqLHnkQpyjokVAndSadBMcFSRA;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref this.m_KhdFqLHnkQpyjokVAndSadBMcFSRA, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m_KhdFqLHnkQpyjokVAndSadBMcFSRA;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref this.m_KhdFqLHnkQpyjokVAndSadBMcFSRA, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public dRxYZKovikdvFiOlZLmFiKpaWUdu(byte P_0, byte P_1, byte P_2)
	{
		RIUppuPjAtxguNIPTIHtyKfbSGQc = P_0;
		POojPmaViDIujhNfCVFxdPMBBQcV = P_1;
		zCbrYekcGvsrNfMfeJViHALovsLt = P_2;
	}
}
