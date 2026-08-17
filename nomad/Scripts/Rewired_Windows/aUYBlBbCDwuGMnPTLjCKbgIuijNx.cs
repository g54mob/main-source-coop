using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

internal class aUYBlBbCDwuGMnPTLjCKbgIuijNx
{
	[CompilerGenerated]
	private DateTime LhABbRlhwoHUJOdUMdedJtCIJQwGA;

	[CompilerGenerated]
	private WeakReference camzVvnLzqBVuoqBHCoMEysqdxyeA;

	[CompilerGenerated]
	private string jzXFeODOTUdfHaNCDVxnBVwRbAREb;

	public DateTime fYQRPLJumleUusAttlYMtYutDFSM
	{
		[CompilerGenerated]
		get
		{
			return LhABbRlhwoHUJOdUMdedJtCIJQwGA;
		}
		[CompilerGenerated]
		private set
		{
			LhABbRlhwoHUJOdUMdedJtCIJQwGA = lhABbRlhwoHUJOdUMdedJtCIJQwGA;
		}
	}

	public WeakReference qvPUClfDzGUJuusgyTYzibMYwTIT
	{
		[CompilerGenerated]
		get
		{
			return camzVvnLzqBVuoqBHCoMEysqdxyeA;
		}
		[CompilerGenerated]
		private set
		{
			camzVvnLzqBVuoqBHCoMEysqdxyeA = weakReference;
		}
	}

	public string sEZbLzRbTkWAgUdSSnfUJlLohtedA
	{
		[CompilerGenerated]
		get
		{
			return jzXFeODOTUdfHaNCDVxnBVwRbAREb;
		}
		[CompilerGenerated]
		private set
		{
			jzXFeODOTUdfHaNCDVxnBVwRbAREb = text;
		}
	}

	public bool lppcPjFSwqGKUpHxIGIiRJRZodcN => qvPUClfDzGUJuusgyTYzibMYwTIT.IsAlive;

	public aUYBlBbCDwuGMnPTLjCKbgIuijNx(DateTime P_0, TjLvFIATAwjKUDtcUGvSPgBzGvgS P_1, string P_2)
	{
		fYQRPLJumleUusAttlYMtYutDFSM = P_0;
		qvPUClfDzGUJuusgyTYzibMYwTIT = new WeakReference(P_1, trackResurrection: true);
		sEZbLzRbTkWAgUdSSnfUJlLohtedA = P_2;
	}

	public virtual string FuxXVNryaRGDOGnbYuMGpLQahtSDA()
	{
		if (!(qvPUClfDzGUJuusgyTYzibMYwTIT.Target is TjLvFIATAwjKUDtcUGvSPgBzGvgS tjLvFIATAwjKUDtcUGvSPgBzGvgS))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Active COM Object: [0x{0:X}] Class: [{1}] Time [{2}] Stack:\r\n{3}", tjLvFIATAwjKUDtcUGvSPgBzGvgS.fREGeAsscSanGSwlvHwWDQIMIYWO.ToInt64(), tjLvFIATAwjKUDtcUGvSPgBzGvgS.GetType().FullName, fYQRPLJumleUusAttlYMtYutDFSM, sEZbLzRbTkWAgUdSSnfUJlLohtedA).AppendLine();
		return stringBuilder.ToString();
	}
}
