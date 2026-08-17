using UnityEngine;

namespace CW.Common
{
	public abstract class CwChild : MonoBehaviour
	{
		public interface IHasChildren
		{
			bool HasChild(CwChild child);
		}

		[ContextMenu("Destroy GameObject If Invalid All")]
		public void DestroyGameObjectIfInvalidAll()
		{
			if (base.transform.parent != null)
			{
				CwChild[] componentsInChildren = base.transform.parent.GetComponentsInChildren<CwChild>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].DestroyGameObjectIfInvalid();
				}
			}
		}

		[ContextMenu("Destroy GameObject If Invalid")]
		public void DestroyGameObjectIfInvalid()
		{
			IHasChildren parent = GetParent();
			if (parent == null || !parent.HasChild(this))
			{
				Object.DestroyImmediate(base.gameObject);
			}
		}

		protected abstract IHasChildren GetParent();

		protected virtual void Start()
		{
		}
	}
}
