using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

internal static class cZvAnzaFWesNtWzpLuYgPrBJAUUP
{
	private static Dictionary<IntPtr, List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>> uaXCMqnGPMOKjatAqnVSStetDjln;

	[ThreadStatic]
	private static Dictionary<IntPtr, List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>> DtSPeYpGqXewTqtBQFdJfOVuStRuA;

	[CompilerGenerated]
	private static EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> m_bCgDeHuaWETDqVFdIYJQduifTAGM;

	[CompilerGenerated]
	private static EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> m_sVXFdABpqRCWYOntmEpizDiVtoxBA;

	private static Dictionary<IntPtr, List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>> FrqpNMQpEmWryjGCnliNYFIpIiLaA
	{
		get
		{
			if (mvvxSJgPMrEzBfovLohwQSbHfdYFb.ajaEWlEaeCOpctWLXASBotpMHcgRA)
			{
				if (DtSPeYpGqXewTqtBQFdJfOVuStRuA == null)
				{
					DtSPeYpGqXewTqtBQFdJfOVuStRuA = new Dictionary<IntPtr, List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>>(ZGRqyIhhGkFaQrLFGTXkhOJhwvyg.mDAwMymNukaJeKyoeAnXmKeEZUyDA);
				}
				return DtSPeYpGqXewTqtBQFdJfOVuStRuA;
			}
			if (uaXCMqnGPMOKjatAqnVSStetDjln == null)
			{
				uaXCMqnGPMOKjatAqnVSStetDjln = new Dictionary<IntPtr, List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>>(ZGRqyIhhGkFaQrLFGTXkhOJhwvyg.mDAwMymNukaJeKyoeAnXmKeEZUyDA);
			}
			return uaXCMqnGPMOKjatAqnVSStetDjln;
		}
	}

	public static event EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> bCgDeHuaWETDqVFdIYJQduifTAGM
	{
		[CompilerGenerated]
		add
		{
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler = cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_bCgDeHuaWETDqVFdIYJQduifTAGM;
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> value2 = (EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT>)Delegate.Combine(eventHandler2, b);
				eventHandler = Interlocked.CompareExchange(ref cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_bCgDeHuaWETDqVFdIYJQduifTAGM, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler = cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_bCgDeHuaWETDqVFdIYJQduifTAGM;
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> value2 = (EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT>)Delegate.Remove(eventHandler2, value3);
				eventHandler = Interlocked.CompareExchange(ref cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_bCgDeHuaWETDqVFdIYJQduifTAGM, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public static event EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> sVXFdABpqRCWYOntmEpizDiVtoxBA
	{
		[CompilerGenerated]
		add
		{
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler = cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_sVXFdABpqRCWYOntmEpizDiVtoxBA;
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> value2 = (EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT>)Delegate.Combine(eventHandler2, b);
				eventHandler = Interlocked.CompareExchange(ref cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_sVXFdABpqRCWYOntmEpizDiVtoxBA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler = cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_sVXFdABpqRCWYOntmEpizDiVtoxBA;
			EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT> value2 = (EventHandler<BdMDQIikTDhmLxxlHebJrcvSzFTT>)Delegate.Remove(eventHandler2, value3);
				eventHandler = Interlocked.CompareExchange(ref cZvAnzaFWesNtWzpLuYgPrBJAUUP.m_sVXFdABpqRCWYOntmEpizDiVtoxBA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	static cZvAnzaFWesNtWzpLuYgPrBJAUUP()
	{
		AppDomain.CurrentDomain.DomainUnload += KquglYpJDYnnfgHlTtSYbXIeKhOj;
		AppDomain.CurrentDomain.ProcessExit += KquglYpJDYnnfgHlTtSYbXIeKhOj;
	}

	private static void KquglYpJDYnnfgHlTtSYbXIeKhOj(object P_0, EventArgs P_1)
	{
		if (mvvxSJgPMrEzBfovLohwQSbHfdYFb.mGvwGFvhvCUTLMZjpApyGtPpOCVm)
		{
			string value = ugmubyNtWXFoPpRIaKXLQjRXlgVe();
			if (!string.IsNullOrEmpty(value))
			{
				Console.WriteLine(value);
			}
		}
	}

	public static void cNjJMbpJCOJHdJIhsFyIfMqIkcey(UMBfGVQXygOTeNKYveUhyPWibjPb P_0)
	{
		if (P_0 == null || P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb == IntPtr.Zero)
		{
			return;
		}
		lock (FrqpNMQpEmWryjGCnliNYFIpIiLaA)
		{
			if (!FrqpNMQpEmWryjGCnliNYFIpIiLaA.TryGetValue(P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb, out var value))
			{
				value = new List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>();
				FrqpNMQpEmWryjGCnliNYFIpIiLaA.Add(P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb, value);
			}
			StringBuilder stringBuilder = new StringBuilder();
			StackFrame[] frames = new StackTrace(3, fNeedFileInfo: true).GetFrames();
			foreach (StackFrame stackFrame in frames)
			{
				if (stackFrame.GetFileLineNumber() != 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\t{0}({1},{2}) : {3}", stackFrame.GetFileName(), stackFrame.GetFileLineNumber(), stackFrame.GetFileColumnNumber(), stackFrame.GetMethod()).AppendLine();
				}
			}
			value.Add(new bGEdnRkfMmdsZAxxyHBUfUYaptKhB(DateTime.Now, P_0, stringBuilder.ToString()));
			hoFTnnqrAjfvdrzewCOSPcYGHljV(P_0);
		}
	}

	public static List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB> yavherKzVPpHjZgtkMuTlBNUIWcq(IntPtr P_0)
	{
		lock (FrqpNMQpEmWryjGCnliNYFIpIiLaA)
		{
			if (FrqpNMQpEmWryjGCnliNYFIpIiLaA.TryGetValue(P_0, out var value))
			{
				return new List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>(value);
			}
		}
		return new List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>();
	}

	public static bGEdnRkfMmdsZAxxyHBUfUYaptKhB yavherKzVPpHjZgtkMuTlBNUIWcq(UMBfGVQXygOTeNKYveUhyPWibjPb P_0)
	{
		lock (FrqpNMQpEmWryjGCnliNYFIpIiLaA)
		{
			if (FrqpNMQpEmWryjGCnliNYFIpIiLaA.TryGetValue(P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb, out var value))
			{
				foreach (bGEdnRkfMmdsZAxxyHBUfUYaptKhB item in value)
				{
					if (item.xlclGijLhuUnADiPEOnaeNLGlAzP.Target == P_0)
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	public static void bUHXdakfBXgDDBjdhWEHSulaDlMx(UMBfGVQXygOTeNKYveUhyPWibjPb P_0)
	{
		if (P_0 == null || P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb == IntPtr.Zero)
		{
			return;
		}
		lock (FrqpNMQpEmWryjGCnliNYFIpIiLaA)
		{
			if (!FrqpNMQpEmWryjGCnliNYFIpIiLaA.TryGetValue(P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb, out var value))
			{
				return;
			}
			for (int num = value.Count - 1; num >= 0; num--)
			{
				bGEdnRkfMmdsZAxxyHBUfUYaptKhB bGEdnRkfMmdsZAxxyHBUfUYaptKhB2 = value[num];
				if (bGEdnRkfMmdsZAxxyHBUfUYaptKhB2.xlclGijLhuUnADiPEOnaeNLGlAzP.Target == P_0)
				{
					value.RemoveAt(num);
				}
				else if (!bGEdnRkfMmdsZAxxyHBUfUYaptKhB2.vrjEfMEhSWNyHgIccyqabVcQxbdOA)
				{
					value.RemoveAt(num);
				}
			}
			if (value.Count == 0)
			{
				FrqpNMQpEmWryjGCnliNYFIpIiLaA.Remove(P_0.KUJrOBoduRcGKIkxgMoKicmgtCeb);
			}
			nMeAKQffuXiTAYhqCEkZKnjKSwrlA(P_0);
		}
	}

	public static List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB> tVkeAtBRXsqqdvkwbynZmclsQFXD()
	{
		List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB> list = new List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB>();
		lock (FrqpNMQpEmWryjGCnliNYFIpIiLaA)
		{
			foreach (List<bGEdnRkfMmdsZAxxyHBUfUYaptKhB> value in FrqpNMQpEmWryjGCnliNYFIpIiLaA.Values)
			{
				foreach (bGEdnRkfMmdsZAxxyHBUfUYaptKhB item in value)
				{
					if (item.vrjEfMEhSWNyHgIccyqabVcQxbdOA)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public static string ugmubyNtWXFoPpRIaKXLQjRXlgVe()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (bGEdnRkfMmdsZAxxyHBUfUYaptKhB item in tVkeAtBRXsqqdvkwbynZmclsQFXD())
		{
			string text = item.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.AppendFormat("[{0}]: {1}", num, text);
				object target = item.xlclGijLhuUnADiPEOnaeNLGlAzP.Target;
				if (target != null)
				{
					string name = target.GetType().Name;
					if (!dictionary.TryGetValue(name, out var value))
					{
						dictionary[name] = 0;
					}
					dictionary[name] = value + 1;
				}
			}
			num++;
		}
		List<string> list = new List<string>(dictionary.Keys);
		list.Sort();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Count per Type:");
		foreach (string item2 in list)
		{
			stringBuilder.AppendFormat("{0} : {1}", item2, dictionary[item2]);
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	private static void hoFTnnqrAjfvdrzewCOSPcYGHljV(UMBfGVQXygOTeNKYveUhyPWibjPb P_0)
	{
		cZvAnzaFWesNtWzpLuYgPrBJAUUP.bCgDeHuaWETDqVFdIYJQduifTAGM?.Invoke(null, new BdMDQIikTDhmLxxlHebJrcvSzFTT(P_0));
	}

	private static void nMeAKQffuXiTAYhqCEkZKnjKSwrlA(UMBfGVQXygOTeNKYveUhyPWibjPb P_0)
	{
		cZvAnzaFWesNtWzpLuYgPrBJAUUP.sVXFdABpqRCWYOntmEpizDiVtoxBA?.Invoke(null, new BdMDQIikTDhmLxxlHebJrcvSzFTT(P_0));
	}
}
