using System;

namespace QFSW.QC.Actions
{
	public class WaitUntil : WaitWhile
	{
		public WaitUntil(Func<bool> condition)
			: base(() => !condition())
		{
		}
	}
}
