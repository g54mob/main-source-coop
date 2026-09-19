using System;
using System.Collections.Generic;
using System.Linq;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class WindowsComponentsFinderService : IWindowsComponentsFinderService
	{
		private IPresenterFactory _presenterFactory;

		private Dictionary<IWindowInstance, ViewPresenterMapper> _viewPresenterMapsByWindowDictionary = new Dictionary<IWindowInstance, ViewPresenterMapper>();

		public WindowsComponentsFinderService(IPresenterFactory presenterFactory)
		{
			_presenterFactory = presenterFactory ?? throw new ArgumentNullException("presenterFactory");
		}

		public void CollectPresentersInWindow(IWindowInstance windowInstance)
		{
			if (windowInstance == null)
			{
				throw new ArgumentNullException("windowInstance");
			}
			ViewPresenterMapper value;
			ViewPresenterMapper viewPresenterMapper = (_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out value) ? value : new ViewPresenterMapper());
			_viewPresenterMapsByWindowDictionary[windowInstance] = viewPresenterMapper;
			foreach (ViewBehaviour allView in windowInstance.GetAllViews())
			{
				if (!viewPresenterMapper.TryGetPresenterForView(allView, out var _))
				{
					viewPresenterMapper[allView] = _presenterFactory.GetPresenter(allView);
					viewPresenterMapper.RefreshMaps();
				}
			}
		}

		public void ClearPresentersInWindow(IWindowInstance windowInstance)
		{
			if (windowInstance == null)
			{
				throw new ArgumentNullException("windowInstance");
			}
			_viewPresenterMapsByWindowDictionary.Remove(windowInstance);
		}

		public TPresenterType GetPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			return GetPresenterInWindow(typeof(TPresenterType), windowInstance, collectIfNotFound) as TPresenterType;
		}

		public IEnumerable<TPresenterType> GetPresentersInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			return GetPresentersInWindow(typeof(TPresenterType), windowInstance, collectIfNotFound).Cast<TPresenterType>();
		}

		public bool ContainsPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			return ContainsPresenterInWindow(typeof(TPresenterType), windowInstance, collectIfNotFound);
		}

		public bool TryGetPresentersInWindow<TPresenterType>(IWindowInstance windowInstance, out IEnumerable<TPresenterType> presenters, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			IEnumerable<PresenterBehaviour> presenterBases;
			bool result = TryGetPresentersInWindow(typeof(TPresenterType), windowInstance, out presenterBases, collectIfNotFound);
			presenters = presenterBases.Cast<TPresenterType>();
			return result;
		}

		public bool TryGetPresenterInWindow<TPresenterType>(IWindowInstance windowInstance, out TPresenterType presenter, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			PresenterBehaviour presenter2;
			bool result = TryGetPresenterInWindow(typeof(TPresenterType), windowInstance, out presenter2, collectIfNotFound);
			presenter = presenter2 as TPresenterType;
			return result;
		}

		public TPresenterType GetPresenterInWindowForView<TPresenterType>(IWindowInstance windowInstance, ViewBehaviour view, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			return GetPresenterInWindowForView(windowInstance, view, collectIfNotFound) as TPresenterType;
		}

		public bool TryGetPresenterInWindowForView<TPresenterType>(IWindowInstance windowInstance, ViewBehaviour view, out TPresenterType presenter, bool collectIfNotFound = false) where TPresenterType : PresenterBehaviour
		{
			PresenterBehaviour presenter2;
			bool result = TryGetPresenterInWindowForView(windowInstance, view, out presenter2, collectIfNotFound);
			presenter = presenter2 as TPresenterType;
			return result;
		}

		public PresenterBehaviour GetPresenterInWindow(Type presenterType, IWindowInstance windowInstance, bool collectIfNotFound = false)
		{
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						throw new PresentersNotCollectedException(windowInstance.GetType());
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenter(presenterType, out var presenter))
				{
					return presenter;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			throw new PresenterNotFoundException(windowInstance.GetType(), presenterType);
		}

		public IEnumerable<PresenterBehaviour> GetPresentersInWindow(Type presenterType, IWindowInstance windowInstance, bool collectIfNotFound = false)
		{
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						throw new PresentersNotCollectedException(windowInstance.GetType());
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenters(presenterType, out var presenters))
				{
					return presenters;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			throw new PresenterNotFoundException(windowInstance.GetType(), presenterType);
		}

		public bool ContainsPresenterInWindow(Type presenterType, IWindowInstance windowInstance, bool collectIfNotFound = false)
		{
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						return false;
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenter(presenterType, out var _))
				{
					return true;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			return false;
		}

		public bool TryGetPresenterInWindow(Type presenterType, IWindowInstance windowInstance, out PresenterBehaviour presenter, bool collectIfNotFound = false)
		{
			presenter = null;
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						return false;
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenter(presenterType, out presenter))
				{
					return true;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			return false;
		}

		public bool TryGetPresentersInWindow(Type presentersType, IWindowInstance windowInstance, out IEnumerable<PresenterBehaviour> presenterBases, bool collectIfNotFound = false)
		{
			presenterBases = null;
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						return false;
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenters(presentersType, out presenterBases))
				{
					return true;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			return false;
		}

		public IEnumerable<IFocusableElement> GetAllFocusablesInWindow(IWindowInstance windowInstance, bool includeInactive = false)
		{
			return windowInstance.GetAllFocusables(includeInactive);
		}

		public PresenterBehaviour GetPresenterInWindowForView(IWindowInstance windowInstance, ViewBehaviour view, bool collectIfNotFound = false)
		{
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						throw new PresentersNotCollectedException(windowInstance.GetType());
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenterForView(view, out var presenterBehaviour))
				{
					return presenterBehaviour;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			throw new PresenterNotFoundException(windowInstance.GetType());
		}

		public bool TryGetPresenterInWindowForView(IWindowInstance windowInstance, ViewBehaviour view, out PresenterBehaviour presenter, bool collectIfNotFound = false)
		{
			presenter = null;
			while (true)
			{
				if (!_viewPresenterMapsByWindowDictionary.TryGetValue(windowInstance, out var value))
				{
					if (!collectIfNotFound)
					{
						return false;
					}
					CollectPresentersInWindow(windowInstance);
					collectIfNotFound = false;
					continue;
				}
				if (value.TryGetPresenterForView(view, out presenter))
				{
					return true;
				}
				if (!collectIfNotFound)
				{
					break;
				}
				CollectPresentersInWindow(windowInstance);
				collectIfNotFound = false;
			}
			return false;
		}
	}
}
