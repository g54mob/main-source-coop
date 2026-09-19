using System;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	[PublicAPI]
	public class UITipPresenter : PresenterBehaviour<UITipViewBase>
	{
		public void SetParent(Transform parent)
		{
			base.View.SetParent(parent);
		}

		public void SetAnchoredPosition(Vector2 anchoredPosition)
		{
			base.View.SetAnchoredPosition(anchoredPosition);
		}

		public void SetVisibleInstant(bool visible)
		{
			base.View.SetVisibleInstant(visible);
		}

		public void Appear(Action onComplete = null)
		{
			base.View.Appear(onComplete);
		}

		public void Disappear(Action onComplete = null)
		{
			base.View.Disappear(onComplete);
		}

		public void DestroyView()
		{
			UnityEngine.Object.Destroy(base.View.gameObject);
		}
	}
}
