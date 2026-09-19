using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenUIService : ILoadingScreenUIService
	{
		private readonly LoadingScreenWindow _loadingScreenWindow;

		private readonly HashSet<int> _registeredViewIds = new HashSet<int>();

		public LoadingScreenUIService(LoadingScreenWindow loadingScreenWindow)
		{
			_loadingScreenWindow = loadingScreenWindow;
		}

		public TPresenter BindPresenter<TPresenter>(ViewBehaviour view) where TPresenter : PresenterBehaviour
		{
			if (_loadingScreenWindow.WindowStatus == WindowStatus.Closed)
			{
				_loadingScreenWindow.Open();
			}
			if (_registeredViewIds.Add(view.GetInstanceID()))
			{
				Transform parent = view.transform.parent;
				int siblingIndex = ((parent != null) ? view.transform.GetSiblingIndex() : 0);
				_loadingScreenWindow.AddView(view.transform, worldPositionStays: false);
				if (parent != null)
				{
					view.transform.SetParent(parent, worldPositionStays: false);
					view.transform.SetSiblingIndex(siblingIndex);
				}
			}
			return _loadingScreenWindow.GetPresenterForView<TPresenter>(view);
		}
	}
}
