using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Utils;

internal class iwnZquMFWHwhZjzckYkHRPdcqkIc
{
	private int nkNLuBfLPdeSdJpcZXwiIvJdRYzmA;

	private int ieMbJYWXZIRSYaZPPTPCJelaBWOX;

	private int VuFSNGbSAgfmtCUvHGfMchoxyiPl;

	[CompilerGenerated]
	private Action m_JbLUwmUKfnDCvYnjJuByJLLCsxze;

	public float IzilEZFnKKPoEpcKyoPmGolsUlOt
	{
		get
		{
			return NSPbLVBSpYgZYmXYImnNuOZsRRskA(nkNLuBfLPdeSdJpcZXwiIvJdRYzmA);
		}
		set
		{
			nkNLuBfLPdeSdJpcZXwiIvJdRYzmA = HfuyDYtcwcKGAfwQPHNrsSpJVgkF(num);
			if (this.JbLUwmUKfnDCvYnjJuByJLLCsxze != null)
			{
				this.JbLUwmUKfnDCvYnjJuByJLLCsxze();
			}
		}
	}

	public int ZcjoZwbIDbbFlaWQFjFKWrESBVuu
	{
		get
		{
			return nkNLuBfLPdeSdJpcZXwiIvJdRYzmA;
		}
		set
		{
			nkNLuBfLPdeSdJpcZXwiIvJdRYzmA = num;
			if (this.JbLUwmUKfnDCvYnjJuByJLLCsxze != null)
			{
				this.JbLUwmUKfnDCvYnjJuByJLLCsxze();
			}
		}
	}

	public event Action JbLUwmUKfnDCvYnjJuByJLLCsxze
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m_JbLUwmUKfnDCvYnjJuByJLLCsxze;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref this.m_JbLUwmUKfnDCvYnjJuByJLLCsxze, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m_JbLUwmUKfnDCvYnjJuByJLLCsxze;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref this.m_JbLUwmUKfnDCvYnjJuByJLLCsxze, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public iwnZquMFWHwhZjzckYkHRPdcqkIc(int P_0, int P_1)
	{
		ieMbJYWXZIRSYaZPPTPCJelaBWOX = P_0;
		VuFSNGbSAgfmtCUvHGfMchoxyiPl = P_1;
	}

	private float NSPbLVBSpYgZYmXYImnNuOZsRRskA(int P_0)
	{
		return MathTools.Clamp((float)P_0 / (float)VuFSNGbSAgfmtCUvHGfMchoxyiPl, 0f, 1f);
	}

	private int HfuyDYtcwcKGAfwQPHNrsSpJVgkF(float P_0)
	{
		return MathTools.Clamp((int)(P_0 * (float)VuFSNGbSAgfmtCUvHGfMchoxyiPl), ieMbJYWXZIRSYaZPPTPCJelaBWOX, VuFSNGbSAgfmtCUvHGfMchoxyiPl);
	}
}
