using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IWindow
	{
		WindowStatus WindowStatus { get; }

		event Action<Type> OnWindowOpened;

		event Action<Type> OnWindowClosed;

		event Action<Type> OnWindowShowed;

		event Action<Type> OnWindowHidden;

		Task OpenAsync();

		void Close();

		void Show();

		void Hide();

		void UpdateWindow();

		void AddView(Transform viewRoot, bool worldPositionStays = true);

		void RemoveView(Transform viewRoot);

		TPresenterType GetPresenter<TPresenterType>() where TPresenterType : PresenterBehaviour;

		IEnumerable<TPresenterType> GetPresenters<TPresenterType>() where TPresenterType : PresenterBehaviour;

		bool TryGetPresenter<TPresenterType>(out TPresenterType presenter) where TPresenterType : PresenterBehaviour;

		bool TryGetPresenters<TPresenterType>(out IEnumerable<TPresenterType> presenters) where TPresenterType : PresenterBehaviour;

		TPresenterType GetPresenterForView<TPresenterType>(ViewBehaviour view) where TPresenterType : PresenterBehaviour;

		bool TryGetPresenterForView<TPresenterType>(ViewBehaviour view, out TPresenterType presenter) where TPresenterType : PresenterBehaviour;

		PresenterBehaviour GetPresenter(Type ofType);

		IEnumerable<PresenterBehaviour> GetPresenters(Type presentersType);

		bool TryGetPresenter(Type ofType, out PresenterBehaviour presenter);

		bool TryGetPresenters(Type presentersType, out IEnumerable<PresenterBehaviour> presenterBases);

		PresenterBehaviour GetPresenterForView(ViewBehaviour view);

		bool TryGetPresenterForView(ViewBehaviour view, out PresenterBehaviour presenter);

		void Open();

		void OpenPreloaded();

		void OnBack();
	}
}
