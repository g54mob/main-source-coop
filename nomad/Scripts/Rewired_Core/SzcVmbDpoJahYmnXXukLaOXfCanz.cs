using System;
using Rewired;
using Rewired.Interfaces;
using Rewired.Utils.Classes.Utility;

internal sealed class SzcVmbDpoJahYmnXXukLaOXfCanz : IControllerElementTarget, IPoolableObject_Internal, IPoolableObject, IDisposable
{
	[Serializable]
	private sealed class uxqZEdPzXkVjwWpmMujsKfrkDjop
	{
		public static readonly uxqZEdPzXkVjwWpmMujsKfrkDjop _003C_003E9 = new uxqZEdPzXkVjwWpmMujsKfrkDjop();

		public static Func<SzcVmbDpoJahYmnXXukLaOXfCanz> _003C_003E9__30_0;

		internal SzcVmbDpoJahYmnXXukLaOXfCanz ukgYxTWDNVaBiPgAyALDCeyMstsi()
		{
			return MoUHBwpcMangYCquFpcvJDNGaBMD();
		}
	}

	private static ObjectPool<SzcVmbDpoJahYmnXXukLaOXfCanz> JjkyTazycZbGbfcALMNtAarXEiIeA;

	private Controller vankmZjCwBtIIcLjXrfmaFhsbOJO;

	private int mJbbVoxOYKmAGpeSEJbDfynVOdjl;

	private AxisRange EFOYxRHNKbJMsxLqwUvpLgiSJUjS;

	private IObjectPool FhqMQMqANXmuKtUAxSDcYnBxapFIA;

	private bool aCWqAinnVZAiyUKFezTQVgddIKev;

	int IControllerElementTarget.elementIdentifierId => mJbbVoxOYKmAGpeSEJbDfynVOdjl;

	AxisRange IControllerElementTarget.axisRange => EFOYxRHNKbJMsxLqwUvpLgiSJUjS;

	bool IControllerElementTarget.hasTarget => ((IControllerElementTarget)this).element != null;

	ControllerElementType IControllerElementTarget.elementType
	{
		get
		{
			if (((IControllerElementTarget)this).element == null)
			{
				return ControllerElementType.Axis;
			}
			return ((IControllerElementTarget)this).element.type;
		}
	}

	string IControllerElementTarget.descriptiveName
	{
		get
		{
			if (vankmZjCwBtIIcLjXrfmaFhsbOJO == null)
			{
				return string.Empty;
			}
			ControllerElementIdentifier elementIdentifierById = vankmZjCwBtIIcLjXrfmaFhsbOJO.GetElementIdentifierById(mJbbVoxOYKmAGpeSEJbDfynVOdjl);
			if (elementIdentifierById == null)
			{
				return string.Empty;
			}
			Controller.Element elementById = vankmZjCwBtIIcLjXrfmaFhsbOJO.GetElementById(mJbbVoxOYKmAGpeSEJbDfynVOdjl);
			if (elementById == null)
			{
				return string.Empty;
			}
			return elementIdentifierById.GetDisplayName(elementById.type, EFOYxRHNKbJMsxLqwUvpLgiSJUjS);
		}
	}

	Controller IControllerElementTarget.controller => vankmZjCwBtIIcLjXrfmaFhsbOJO;

	Controller.Element IControllerElementTarget.element
	{
		get
		{
			if (vankmZjCwBtIIcLjXrfmaFhsbOJO == null)
			{
				return null;
			}
			if (vankmZjCwBtIIcLjXrfmaFhsbOJO.GetElementIdentifierById(mJbbVoxOYKmAGpeSEJbDfynVOdjl) == null)
			{
				return null;
			}
			return vankmZjCwBtIIcLjXrfmaFhsbOJO.GetElementById(mJbbVoxOYKmAGpeSEJbDfynVOdjl);
		}
	}

	public ControllerElementIdentifier CDEsJYkwjwjoMCFxzTvwSCTRcFgDA
	{
		get
		{
			if (vankmZjCwBtIIcLjXrfmaFhsbOJO == null)
			{
				return null;
			}
			return vankmZjCwBtIIcLjXrfmaFhsbOJO.GetElementIdentifierById(mJbbVoxOYKmAGpeSEJbDfynVOdjl);
		}
	}

	IObjectPool IPoolableObject_Internal.pool
	{
		get
		{
			return FhqMQMqANXmuKtUAxSDcYnBxapFIA;
		}
		set
		{
			FhqMQMqANXmuKtUAxSDcYnBxapFIA = value;
		}
	}

	internal SzcVmbDpoJahYmnXXukLaOXfCanz(Controller P_0, int P_1, AxisRange P_2)
	{
		vankmZjCwBtIIcLjXrfmaFhsbOJO = P_0;
		mJbbVoxOYKmAGpeSEJbDfynVOdjl = P_1;
		EFOYxRHNKbJMsxLqwUvpLgiSJUjS = P_2;
	}

	internal void QtffntythKoQiujtHSmsStXjkEbK(ControllerElementTarget P_0)
	{
		vankmZjCwBtIIcLjXrfmaFhsbOJO = P_0.controller;
		mJbbVoxOYKmAGpeSEJbDfynVOdjl = P_0.elementIdentifierId;
		EFOYxRHNKbJMsxLqwUvpLgiSJUjS = P_0.axisRange;
	}

	internal void UEWfotDVqdJLJbZsNuqGspJGBIIE(IControllerElementTarget P_0)
	{
		vankmZjCwBtIIcLjXrfmaFhsbOJO = P_0.controller;
		mJbbVoxOYKmAGpeSEJbDfynVOdjl = P_0.elementIdentifierId;
		EFOYxRHNKbJMsxLqwUvpLgiSJUjS = P_0.axisRange;
	}

	internal void yvBjPnyABUjdxQrdFioDuSxebTpS(SzcVmbDpoJahYmnXXukLaOXfCanz P_0)
	{
		UEWfotDVqdJLJbZsNuqGspJGBIIE(P_0);
	}

	void IPoolableObject_Internal.Clear()
	{
		vankmZjCwBtIIcLjXrfmaFhsbOJO = null;
		mJbbVoxOYKmAGpeSEJbDfynVOdjl = -1;
		EFOYxRHNKbJMsxLqwUvpLgiSJUjS = AxisRange.Full;
	}

	void IPoolableObject.Return()
	{
		if (FhqMQMqANXmuKtUAxSDcYnBxapFIA != null)
		{
			FhqMQMqANXmuKtUAxSDcYnBxapFIA.Return(this);
		}
	}

	internal static SzcVmbDpoJahYmnXXukLaOXfCanz FSxRdHZmKwuJoxnUKvLclPUsyiZj()
	{
		if (JjkyTazycZbGbfcALMNtAarXEiIeA == null)
		{
			JjkyTazycZbGbfcALMNtAarXEiIeA = new ObjectPool<SzcVmbDpoJahYmnXXukLaOXfCanz>(uxqZEdPzXkVjwWpmMujsKfrkDjop._003C_003E9.ukgYxTWDNVaBiPgAyALDCeyMstsi);
		}
		return JjkyTazycZbGbfcALMNtAarXEiIeA.Get();
	}

	internal static SzcVmbDpoJahYmnXXukLaOXfCanz iUalUWqSTahvFebVilfnXrVIAQbf(ControllerElementTarget P_0)
	{
		SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = FSxRdHZmKwuJoxnUKvLclPUsyiZj();
		szcVmbDpoJahYmnXXukLaOXfCanz.QtffntythKoQiujtHSmsStXjkEbK(P_0);
		return szcVmbDpoJahYmnXXukLaOXfCanz;
	}

	internal static void jzhYiOZYDeArdmkyDczZrxvFgLDbA(SzcVmbDpoJahYmnXXukLaOXfCanz P_0)
	{
		if (P_0 != null && JjkyTazycZbGbfcALMNtAarXEiIeA != null)
		{
			JjkyTazycZbGbfcALMNtAarXEiIeA.Return(P_0);
		}
	}

	internal static SzcVmbDpoJahYmnXXukLaOXfCanz MoUHBwpcMangYCquFpcvJDNGaBMD()
	{
		return new SzcVmbDpoJahYmnXXukLaOXfCanz(null, -1, AxisRange.Full);
	}

	void IDisposable.Dispose()
	{
		qoYWJAYczJMGUwQmHBTlQrlewmdB(true);
		GC.SuppressFinalize(this);
	}

	protected void AehDuegENarntSosHoNzfpvirwlfB()
	{
		try
		{
			qoYWJAYczJMGUwQmHBTlQrlewmdB(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void qoYWJAYczJMGUwQmHBTlQrlewmdB(bool P_0)
	{
		if (!aCWqAinnVZAiyUKFezTQVgddIKev)
		{
			if (P_0)
			{
				((IPoolableObject)this).Return();
			}
			aCWqAinnVZAiyUKFezTQVgddIKev = true;
		}
	}
}
