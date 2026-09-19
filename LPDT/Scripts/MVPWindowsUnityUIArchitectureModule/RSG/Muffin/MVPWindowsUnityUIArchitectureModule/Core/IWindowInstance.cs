using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IWindowInstance
	{
		void Show();

		void Hide();

		void Destroy();

		IEnumerable<ViewBehaviour> GetAllViews();

		IEnumerable<IFocusableElement> GetAllFocusables(bool includeInactive);

		void AddChild(Transform childRoot, bool worldPositionStays);

		void RemoveChild(Transform childRoot);
	}
}
