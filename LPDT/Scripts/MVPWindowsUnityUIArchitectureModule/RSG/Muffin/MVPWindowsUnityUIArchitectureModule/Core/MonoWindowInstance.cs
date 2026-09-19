using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class MonoWindowInstance : MonoBehaviour, IWindowInstance
	{
		public void Show()
		{
			EnableInstance();
		}

		public void Hide()
		{
			DisableInstance();
		}

		public void Destroy()
		{
			if (IsValid())
			{
				DestroyInstance();
			}
		}

		private void EnableInstance()
		{
			base.gameObject.SetActive(value: true);
		}

		private void DisableInstance()
		{
			base.gameObject.SetActive(value: false);
		}

		private void DestroyInstance()
		{
			if (IsValid())
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private bool IsValid()
		{
			return this != null;
		}

		public IEnumerable<ViewBehaviour> GetAllViews()
		{
			if (this == null || base.gameObject == null)
			{
				return Array.Empty<ViewBehaviour>();
			}
			return GetComponentsInChildren<ViewBehaviour>(includeInactive: true);
		}

		public IEnumerable<IFocusableElement> GetAllFocusables(bool includeInactive)
		{
			return GetComponentsInChildren<IFocusableElement>(includeInactive);
		}

		public void AddChild(Transform childRoot, bool worldPositionStays)
		{
			childRoot.SetParent(base.gameObject.transform, worldPositionStays);
		}

		public void RemoveChild(Transform childRoot)
		{
			if (childRoot.parent == base.gameObject.transform)
			{
				childRoot.SetParent(null);
			}
		}
	}
}
