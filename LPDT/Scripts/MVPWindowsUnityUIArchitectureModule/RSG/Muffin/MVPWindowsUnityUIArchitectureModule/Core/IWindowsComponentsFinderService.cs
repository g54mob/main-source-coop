using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	[PublicAPI]
	public interface IWindowsComponentsFinderService
	{
		void CollectPresentersInWindow(IWindowInstance windowInstance);

		void ClearPresentersInWindow(IWindowInstance windowInstance);

		TPresenterType GetPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		IEnumerable<TPresenterType> GetPresentersInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		bool ContainsPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		bool TryGetPresentersInWindow<TPresenterType>(IWindowInstance windowInstance, out IEnumerable<TPresenterType> presenters, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		bool TryGetPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, out TPresenterType presenter, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		TPresenterType GetPresenterInWindowForView<TPresenterType>(IWindowInstance windowInstance, ViewBehaviour view, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		bool TryGetPresenterInWindowForView<TPresenterType>(IWindowInstance windowInstance, ViewBehaviour view, out TPresenterType presenter, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour;

		PresenterBehaviour GetPresenterInWindow(Type presenterType, IWindowInstance windowInstance, bool collectIfNotFound = false);

		IEnumerable<PresenterBehaviour> GetPresentersInWindow(Type presenterType, IWindowInstance windowInstance, bool collectIfNotFound = false);

		bool ContainsPresenterInWindow(Type presentersType, IWindowInstance windowInstance, bool collectIfNotFound = false);

		bool TryGetPresenterInWindow(Type presenterType, IWindowInstance windowInstance, out PresenterBehaviour presenter, bool collectIfNotFound = false);

		bool TryGetPresentersInWindow(Type presentersType, IWindowInstance windowInstance, out IEnumerable<PresenterBehaviour> presenterBases, bool collectIfNotFound = false);

		IEnumerable<IFocusableElement> GetAllFocusablesInWindow(IWindowInstance windowInstance, bool includeInactive = false);

		PresenterBehaviour GetPresenterInWindowForView(IWindowInstance windowInstance, ViewBehaviour view, bool collectIfNotFound = false);

		bool TryGetPresenterInWindowForView(IWindowInstance windowInstance, ViewBehaviour view, out PresenterBehaviour presenter, bool collectIfNotFound = false);
	}
}
