using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

internal static class fjtMBbsXZkVKwAcLecwgmVFWrOLS
{
	private static Dictionary<IntPtr, List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>> JPYDDsiPsMetIDHsIDzRDceivVTFb;

	[ThreadStatic]
	private static Dictionary<IntPtr, List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>> VonCeLSddqgxaxJaJPnDITwtpyDj;

	[CompilerGenerated]
	private static EventHandler<EcSNkSweWHPgEzgDqmRJKZjNCFKr> TSnMJhhEVIfrqmxpYUabESMOCryo;

	[CompilerGenerated]
	private static EventHandler<EcSNkSweWHPgEzgDqmRJKZjNCFKr> AexgmHrCMwpzDfLaSbobJAVFNrlq;

	private static Dictionary<IntPtr, List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>> CrOrxgeNOQzHjcbPdAdBAHOnqxcU
	{
		get
		{
			if (tHzkVOaXGtzMNeVqcRulrzAOzBuc.pPTvBEVUzrkwPibEiprEtSFRpgIB)
			{
				if (VonCeLSddqgxaxJaJPnDITwtpyDj == null)
				{
					VonCeLSddqgxaxJaJPnDITwtpyDj = new Dictionary<IntPtr, List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>>(AAJjYYdWFiKHBxFxlzxgSPRgOcbP.hvaHzonQxRoBklZezoDdcJxqmkjA);
				}
				return VonCeLSddqgxaxJaJPnDITwtpyDj;
			}
			if (JPYDDsiPsMetIDHsIDzRDceivVTFb == null)
			{
				JPYDDsiPsMetIDHsIDzRDceivVTFb = new Dictionary<IntPtr, List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>>(AAJjYYdWFiKHBxFxlzxgSPRgOcbP.hvaHzonQxRoBklZezoDdcJxqmkjA);
			}
			return JPYDDsiPsMetIDHsIDzRDceivVTFb;
		}
	}

	static fjtMBbsXZkVKwAcLecwgmVFWrOLS()
	{
		AppDomain.CurrentDomain.DomainUnload += dqCaDLXPPGWupEjJOdxsIPZecXoL;
		AppDomain.CurrentDomain.ProcessExit += dqCaDLXPPGWupEjJOdxsIPZecXoL;
	}

	private static void dqCaDLXPPGWupEjJOdxsIPZecXoL(object P_0, EventArgs P_1)
	{
		if (tHzkVOaXGtzMNeVqcRulrzAOzBuc.WsELCweQPWDAjXxRcBVZcPTHHAsnA)
		{
			string value = nZxkLWoeXwLKMiBnbkdsefTzfxUF();
			if (!string.IsNullOrEmpty(value))
			{
				Console.WriteLine(value);
			}
		}
	}

	public static void CEbopSUlmjBrEKAwVAEzmedJokXmA(TjLvFIATAwjKUDtcUGvSPgBzGvgS P_0)
	{
		if (P_0 == null || P_0.fREGeAsscSanGSwlvHwWDQIMIYWO == IntPtr.Zero)
		{
			return;
		}
		lock (CrOrxgeNOQzHjcbPdAdBAHOnqxcU)
		{
			if (!CrOrxgeNOQzHjcbPdAdBAHOnqxcU.TryGetValue(P_0.fREGeAsscSanGSwlvHwWDQIMIYWO, out var value))
			{
				value = new List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>();
				CrOrxgeNOQzHjcbPdAdBAHOnqxcU.Add(P_0.fREGeAsscSanGSwlvHwWDQIMIYWO, value);
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
			value.Add(new aUYBlBbCDwuGMnPTLjCKbgIuijNx(DateTime.Now, P_0, stringBuilder.ToString()));
			hZhUEOfGipUhAkCvrWNKcTBTNsOl(P_0);
		}
	}

	public static aUYBlBbCDwuGMnPTLjCKbgIuijNx uMMTzTCcSnnTuiHmXxndikPypETp(TjLvFIATAwjKUDtcUGvSPgBzGvgS P_0)
	{
		lock (CrOrxgeNOQzHjcbPdAdBAHOnqxcU)
		{
			if (CrOrxgeNOQzHjcbPdAdBAHOnqxcU.TryGetValue(P_0.fREGeAsscSanGSwlvHwWDQIMIYWO, out var value))
			{
				foreach (aUYBlBbCDwuGMnPTLjCKbgIuijNx item in value)
				{
					if (item.qvPUClfDzGUJuusgyTYzibMYwTIT.Target == P_0)
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	public static void OCZiEWCfMtAkofgXbPlfMvJjbbmsb(TjLvFIATAwjKUDtcUGvSPgBzGvgS P_0)
	{
		if (P_0 == null || P_0.fREGeAsscSanGSwlvHwWDQIMIYWO == IntPtr.Zero)
		{
			return;
		}
		lock (CrOrxgeNOQzHjcbPdAdBAHOnqxcU)
		{
			if (!CrOrxgeNOQzHjcbPdAdBAHOnqxcU.TryGetValue(P_0.fREGeAsscSanGSwlvHwWDQIMIYWO, out var value))
			{
				return;
			}
			for (int num = value.Count - 1; num >= 0; num--)
			{
				aUYBlBbCDwuGMnPTLjCKbgIuijNx aUYBlBbCDwuGMnPTLjCKbgIuijNx2 = value[num];
				if (aUYBlBbCDwuGMnPTLjCKbgIuijNx2.qvPUClfDzGUJuusgyTYzibMYwTIT.Target == P_0)
				{
					value.RemoveAt(num);
				}
				else if (!aUYBlBbCDwuGMnPTLjCKbgIuijNx2.lppcPjFSwqGKUpHxIGIiRJRZodcN)
				{
					value.RemoveAt(num);
				}
			}
			if (value.Count == 0)
			{
				CrOrxgeNOQzHjcbPdAdBAHOnqxcU.Remove(P_0.fREGeAsscSanGSwlvHwWDQIMIYWO);
			}
			sDsOVOvHKrCiKjMwYjmKJmWbvVZZ(P_0);
		}
	}

	public static List<aUYBlBbCDwuGMnPTLjCKbgIuijNx> jhZHzkzsGUBCFCokXYBrGjkBtHTs()
	{
		List<aUYBlBbCDwuGMnPTLjCKbgIuijNx> list = new List<aUYBlBbCDwuGMnPTLjCKbgIuijNx>();
		lock (CrOrxgeNOQzHjcbPdAdBAHOnqxcU)
		{
			foreach (List<aUYBlBbCDwuGMnPTLjCKbgIuijNx> value in CrOrxgeNOQzHjcbPdAdBAHOnqxcU.Values)
			{
				foreach (aUYBlBbCDwuGMnPTLjCKbgIuijNx item in value)
				{
					if (item.lppcPjFSwqGKUpHxIGIiRJRZodcN)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public static string nZxkLWoeXwLKMiBnbkdsefTzfxUF()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (aUYBlBbCDwuGMnPTLjCKbgIuijNx item in jhZHzkzsGUBCFCokXYBrGjkBtHTs())
		{
			string text = item.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.AppendFormat("[{0}]: {1}", num, text);
				object target = item.qvPUClfDzGUJuusgyTYzibMYwTIT.Target;
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

	private static void hZhUEOfGipUhAkCvrWNKcTBTNsOl(TjLvFIATAwjKUDtcUGvSPgBzGvgS P_0)
	{
		TSnMJhhEVIfrqmxpYUabESMOCryo?.Invoke(null, new EcSNkSweWHPgEzgDqmRJKZjNCFKr(P_0));
	}

	private static void sDsOVOvHKrCiKjMwYjmKJmWbvVZZ(TjLvFIATAwjKUDtcUGvSPgBzGvgS P_0)
	{
		AexgmHrCMwpzDfLaSbobJAVFNrlq?.Invoke(null, new EcSNkSweWHPgEzgDqmRJKZjNCFKr(P_0));
	}
}
