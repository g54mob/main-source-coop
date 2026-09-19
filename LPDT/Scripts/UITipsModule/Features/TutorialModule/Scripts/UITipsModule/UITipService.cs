using System.Collections.Generic;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	public class UITipService : IUITipService
	{
		private class ActiveTip
		{
			public UITipViewBase View { get; }

			public PresenterBehaviour Presenter { get; }

			public ActiveTip(UITipViewBase view, PresenterBehaviour presenter)
			{
				View = view;
				Presenter = presenter;
			}
		}

		private readonly UITipConfiguration _configuration;

		private readonly DiContainer _container;

		private readonly TutorialWindow _tutorialWindow;

		private readonly Dictionary<int, ActiveTip> _activeTips = new Dictionary<int, ActiveTip>();

		private int _nextHandleId = 1;

		public bool IsReady => _tutorialWindow.WindowStatus != WindowStatus.Closed;

		public UITipService(UITipConfiguration configuration, DiContainer container, TutorialWindow tutorialWindow)
		{
			_configuration = configuration;
			_container = container;
			_tutorialWindow = tutorialWindow;
		}

		public UITipHandle CreateTip(UITipType tipType, Vector2? anchoredPosition = null, bool animated = true)
		{
			return CreateTip<UITipPresenter>(tipType, anchoredPosition, animated);
		}

		public UITipHandle CreateTip<TPresenter>(UITipType tipType, Vector2? anchoredPosition = null, bool animated = true) where TPresenter : PresenterBehaviour
		{
			_configuration.Prefabs.TryGetValue(tipType, out var value);
			UITipViewBase component = _container.InstantiatePrefab(value.gameObject).GetComponent<UITipViewBase>();
			_tutorialWindow.AddView(component.transform, worldPositionStays: false);
			if (anchoredPosition.HasValue)
			{
				component.SetAnchoredPosition(anchoredPosition.Value);
			}
			TPresenter presenterForView = _tutorialWindow.GetPresenterForView<TPresenter>(component);
			if (animated)
			{
				component.Appear();
			}
			else
			{
				component.SetVisibleInstant(visible: true);
			}
			int num = _nextHandleId++;
			_activeTips[num] = new ActiveTip(component, presenterForView);
			return new UITipHandle(num);
		}

		public TPresenter GetPresenter<TPresenter>(UITipHandle handle) where TPresenter : PresenterBehaviour
		{
			if (!handle.IsValid || !_activeTips.TryGetValue(handle.Id, out var value))
			{
				return null;
			}
			return value.Presenter as TPresenter;
		}

		public void KillTip(UITipHandle handle, bool animated = true)
		{
			if (handle.IsValid && _activeTips.TryGetValue(handle.Id, out var value))
			{
				_activeTips.Remove(handle.Id);
				KillTip(value, animated);
			}
		}

		public void KillAll(bool animated = true)
		{
			List<ActiveTip> list = new List<ActiveTip>(_activeTips.Values);
			_activeTips.Clear();
			foreach (ActiveTip item in list)
			{
				KillTip(item, animated);
			}
		}

		private void KillTip(ActiveTip activeTip, bool animated)
		{
			if (animated && activeTip.View != null)
			{
				activeTip.View.Disappear(delegate
				{
					DestroyTip(activeTip);
				});
			}
			else
			{
				DestroyTip(activeTip);
			}
		}

		private void DestroyTip(ActiveTip activeTip)
		{
			if (activeTip.View != null)
			{
				Object.Destroy(activeTip.View.gameObject);
			}
		}
	}
}
