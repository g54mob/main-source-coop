using System;
using System.Runtime.InteropServices;

internal struct aMZqdyjJERTAUbjSZWzzHWVxTEnF
{
	public IntPtr wgtCCiLXCvFMKPuxittDTUaHJTEk;

	public int qgutIQhPnXIiQtsjnVldiGCCNOUR;

	public int CxyjjQvHRbbDygUbIyJDqcXAgiJJA;

	public cearvUhOhIGFiMrovHXSAoxpvgdP jWyxbwpyOBwigomFzcjATaXrYEzP;

	public bool PhDXVugvBTbgzVslaZheBDCyjcNL
	{
		get
		{
			if (wgtCCiLXCvFMKPuxittDTUaHJTEk != IntPtr.Zero && qgutIQhPnXIiQtsjnVldiGCCNOUR > 0)
			{
				return CxyjjQvHRbbDygUbIyJDqcXAgiJJA > 0;
			}
			return false;
		}
	}

	public aMZqdyjJERTAUbjSZWzzHWVxTEnF(IntPtr P_0, int P_1, int P_2)
	{
		wgtCCiLXCvFMKPuxittDTUaHJTEk = P_0;
		qgutIQhPnXIiQtsjnVldiGCCNOUR = P_1;
		CxyjjQvHRbbDygUbIyJDqcXAgiJJA = P_2;
		jWyxbwpyOBwigomFzcjATaXrYEzP = cearvUhOhIGFiMrovHXSAoxpvgdP.None;
	}

	public string qDGIUedAITovkOLYqihZloohdQAIA()
	{
		string text = "OutputReport:\n";
		text = text + "buffer = " + ((wgtCCiLXCvFMKPuxittDTUaHJTEk == IntPtr.Zero) ? "NULL" : ("Is Valid (" + wgtCCiLXCvFMKPuxittDTUaHJTEk + ")")) + "\n";
		text = text + "bufferLength = " + qgutIQhPnXIiQtsjnVldiGCCNOUR + "\n";
		text = text + "reportLength = " + CxyjjQvHRbbDygUbIyJDqcXAgiJJA + "\n";
		string text2 = text;
		int num = (int)jWyxbwpyOBwigomFzcjATaXrYEzP;
		text = text2 + "options = " + num + "\n";
		if (wgtCCiLXCvFMKPuxittDTUaHJTEk != IntPtr.Zero)
		{
			text += "Buffer data:\n";
			for (int i = 0; i < CxyjjQvHRbbDygUbIyJDqcXAgiJJA; i++)
			{
				text += Marshal.ReadByte(wgtCCiLXCvFMKPuxittDTUaHJTEk, i).ToString("X2");
				if (i < CxyjjQvHRbbDygUbIyJDqcXAgiJJA - 1)
				{
					text += ", ";
				}
			}
		}
		return text;
	}
}
