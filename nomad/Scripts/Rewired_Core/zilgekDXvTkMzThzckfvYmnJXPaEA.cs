using System;
using Rewired.Internal.Localization;
using Rewired.Utils.Classes.Data;

internal sealed class zilgekDXvTkMzThzckfvYmnJXPaEA : cAwfhgIDGfMqIqwFGxVCNiWfViqT, IDisposable
{
	private Action eqfNyiSXeUaUgGDjHQYXmOhLzIeH;

	private Id HRyZVUCMdNdQgVyuBmbAtgLLIeUW;

	private bool iinSpLmGWHITBuczVzrkPqLSZkMl;

	public zilgekDXvTkMzThzckfvYmnJXPaEA(Action P_0)
	{
		eqfNyiSXeUaUgGDjHQYXmOhLzIeH = P_0;
		HRyZVUCMdNdQgVyuBmbAtgLLIeUW = 0u;
		LocalizationManager.Add(this, ref HRyZVUCMdNdQgVyuBmbAtgLLIeUW);
	}

	void cAwfhgIDGfMqIqwFGxVCNiWfViqT.Localize()
	{
		eqfNyiSXeUaUgGDjHQYXmOhLzIeH();
	}

	private void YclxCJvqEEgihiBtEjrahvUjqiyK(bool P_0)
	{
		if (!iinSpLmGWHITBuczVzrkPqLSZkMl)
		{
			if (P_0)
			{
				LocalizationManager.Remove(ref HRyZVUCMdNdQgVyuBmbAtgLLIeUW);
			}
			iinSpLmGWHITBuczVzrkPqLSZkMl = true;
		}
	}

	public void Dispose()
	{
		YclxCJvqEEgihiBtEjrahvUjqiyK(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}
}
