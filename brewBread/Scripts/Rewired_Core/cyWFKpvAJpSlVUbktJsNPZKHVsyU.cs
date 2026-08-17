using System;
using Rewired;
using Rewired.Interfaces;
using Rewired.Utils.Classes.Utility;

internal sealed class cyWFKpvAJpSlVUbktJsNPZKHVsyU : IDisposable, IControllerElementTarget, IPoolableObject, IPoolableObject_Internal
{
	[Serializable]
	private sealed class QpCDyddmoOdqdcsZeGcsnboOtndT
	{
		public static readonly QpCDyddmoOdqdcsZeGcsnboOtndT _003C_003E9 = new QpCDyddmoOdqdcsZeGcsnboOtndT();

		public static Func<cyWFKpvAJpSlVUbktJsNPZKHVsyU> _003C_003E9__30_0;

		internal cyWFKpvAJpSlVUbktJsNPZKHVsyU uCFJVGswFxDuMMDmfiecwnyNCNFw()
		{
			return jqAWqFpdLUDQRsdZuZzXWwxrjFMg();
		}
	}

	private static ObjectPool<cyWFKpvAJpSlVUbktJsNPZKHVsyU> VzDKcvZrFeMarzILzqwIYaNEvYEr;

	private Controller oBPMdftKtJLWDwKjOKgLuimxBfUKA;

	private int NPuaUGjYHfmjnFZazhpMoRbThWzG;

	private AxisRange dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

	private IObjectPool JftDeHNOJqfZFYZHBIaYCZsYUfJI;

	private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

	public int elementIdentifierId => NPuaUGjYHfmjnFZazhpMoRbThWzG;

	public AxisRange axisRange => dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

	public bool hasTarget => element != null;

	public ControllerElementType elementType
	{
		get
		{
			if (element == null)
			{
				return ControllerElementType.Axis;
			}
			return element.type;
		}
	}

	public string descriptiveName
	{
		get
		{
			if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
			{
				return string.Empty;
			}
			ControllerElementIdentifier elementIdentifierById = oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
			if (elementIdentifierById == null)
			{
				return string.Empty;
			}
			Controller.Element elementById = oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
			if (elementById == null)
			{
				return string.Empty;
			}
			return elementIdentifierById.GetDisplayName(elementById.type, dnqcDaKuDzTeZpkjWrulGAfmNtHmA);
		}
	}

	public Controller controller => oBPMdftKtJLWDwKjOKgLuimxBfUKA;

	public Controller.Element element
	{
		get
		{
			if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
			{
				return null;
			}
			if (oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG) == null)
			{
				return null;
			}
			return oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
		}
	}

	public ControllerElementIdentifier xeqGegiWiHTAOWixzgszmrvFhMdA
	{
		get
		{
			if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
			{
				return null;
			}
			return oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
		}
	}

	IObjectPool IPoolableObject_Internal.pool
	{
		get
		{
			return JftDeHNOJqfZFYZHBIaYCZsYUfJI;
		}
		set
		{
			JftDeHNOJqfZFYZHBIaYCZsYUfJI = jftDeHNOJqfZFYZHBIaYCZsYUfJI;
		}
	}

	internal cyWFKpvAJpSlVUbktJsNPZKHVsyU(Controller P_0, int P_1, AxisRange P_2)
	{
		oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0;
		NPuaUGjYHfmjnFZazhpMoRbThWzG = P_1;
		dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_2;
	}

	internal void ckPgJNiFQjrLdKfwClUsdsySXFyIb(ControllerElementTarget P_0)
	{
		oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0.controller;
		NPuaUGjYHfmjnFZazhpMoRbThWzG = P_0.elementIdentifierId;
		dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0.axisRange;
	}

	internal void ckPgJNiFQjrLdKfwClUsdsySXFyIb(IControllerElementTarget P_0)
	{
		oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0.controller;
		NPuaUGjYHfmjnFZazhpMoRbThWzG = P_0.elementIdentifierId;
		dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0.axisRange;
	}

	internal void ckPgJNiFQjrLdKfwClUsdsySXFyIb(cyWFKpvAJpSlVUbktJsNPZKHVsyU P_0)
	{
		ckPgJNiFQjrLdKfwClUsdsySXFyIb((IControllerElementTarget)P_0);
	}

	private void ptdtUmrjHSjpGHNKpRzJPMdkiGsbb()
	{
		oBPMdftKtJLWDwKjOKgLuimxBfUKA = null;
		NPuaUGjYHfmjnFZazhpMoRbThWzG = -1;
		dnqcDaKuDzTeZpkjWrulGAfmNtHmA = AxisRange.Full;
	}

	void IPoolableObject_Internal.Clear()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ptdtUmrjHSjpGHNKpRzJPMdkiGsbb
		this.ptdtUmrjHSjpGHNKpRzJPMdkiGsbb();
	}

	void IPoolableObject.Return()
	{
		if (JftDeHNOJqfZFYZHBIaYCZsYUfJI != null)
		{
			JftDeHNOJqfZFYZHBIaYCZsYUfJI.Return(this);
		}
	}

	internal static cyWFKpvAJpSlVUbktJsNPZKHVsyU LVEUXoDfRdrEgyPvOtkseSGcUXvL()
	{
		if (VzDKcvZrFeMarzILzqwIYaNEvYEr == null)
		{
			VzDKcvZrFeMarzILzqwIYaNEvYEr = new ObjectPool<cyWFKpvAJpSlVUbktJsNPZKHVsyU>(QpCDyddmoOdqdcsZeGcsnboOtndT._003C_003E9.uCFJVGswFxDuMMDmfiecwnyNCNFw);
		}
		return VzDKcvZrFeMarzILzqwIYaNEvYEr.Get();
	}

	internal static cyWFKpvAJpSlVUbktJsNPZKHVsyU LVEUXoDfRdrEgyPvOtkseSGcUXvL(ControllerElementTarget P_0)
	{
		cyWFKpvAJpSlVUbktJsNPZKHVsyU obj = LVEUXoDfRdrEgyPvOtkseSGcUXvL();
		obj.ckPgJNiFQjrLdKfwClUsdsySXFyIb(P_0);
		return obj;
	}

	internal static void GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU P_0)
	{
		if (P_0 != null && VzDKcvZrFeMarzILzqwIYaNEvYEr != null)
		{
			VzDKcvZrFeMarzILzqwIYaNEvYEr.Return(P_0);
		}
	}

	internal static cyWFKpvAJpSlVUbktJsNPZKHVsyU jqAWqFpdLUDQRsdZuZzXWwxrjFMg()
	{
		return new cyWFKpvAJpSlVUbktJsNPZKHVsyU(null, -1, AxisRange.Full);
	}

	void IDisposable.Dispose()
	{
		oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(true);
		GC.SuppressFinalize(this);
	}

	protected void iMcWPzJbivbQRVFtjrscsWpjuUvv()
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

	private void oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(bool P_0)
	{
		if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
		{
			if (P_0)
			{
				((IPoolableObject)this).Return();
			}
			AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
		}
	}
}
