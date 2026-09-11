using UnityEngine;

namespace Zorro.UI
{
	public abstract class GameUISystem : MonoBehaviour
	{
		public abstract bool NeedsCursor();

		public virtual void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public virtual void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		public virtual bool ShouldShow()
		{
			return false;
		}
	}
}
