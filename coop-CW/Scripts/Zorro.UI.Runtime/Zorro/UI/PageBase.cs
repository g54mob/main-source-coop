using Sirenix.Utilities;
using UnityEngine;

namespace Zorro.UI
{
	public abstract class PageBase : MonoBehaviour
	{
		public virtual void OnPageEnter()
		{
		}

		public virtual void OnPageExit()
		{
		}

		public void OnPageEntered()
		{
			GetComponentsInChildren<IOnPageEntered>().ForEach(delegate(IOnPageEntered entered)
			{
				entered.OnEntered();
			});
		}
	}
}
