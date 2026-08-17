using UnityEngine;

namespace QFSW.QC.Actions
{
	public class WaitKey : WaitUntil
	{
		public WaitKey(KeyCode key)
			: base(() => InputHelper.GetKeyDown(key))
		{
		}
	}
}
