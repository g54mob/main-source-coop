using System;
using System.Collections.Generic;
using System.Linq;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class ViewPresenterMapper
	{
		private Dictionary<ViewBehaviour, PresenterBehaviour> _presenterByViewDictionary = new Dictionary<ViewBehaviour, PresenterBehaviour>();

		public PresenterBehaviour this[ViewBehaviour viewBehaviour]
		{
			get
			{
				return _presenterByViewDictionary[viewBehaviour];
			}
			set
			{
				MapViewAndPresenter(viewBehaviour, value);
			}
		}

		public TPresenterType GetPresenterForView<TPresenterType>(ViewBehaviour viewBehaviour) where TPresenterType : PresenterBehaviour
		{
			return GetPresenterForView(viewBehaviour) as TPresenterType;
		}

		public bool TryGetPresenterForView<TPresenterType>(ViewBehaviour viewBehaviour, out TPresenterType presenter) where TPresenterType : PresenterBehaviour
		{
			PresenterBehaviour presenterBehaviour;
			bool result = TryGetPresenterForView(viewBehaviour, out presenterBehaviour);
			presenter = presenterBehaviour as TPresenterType;
			return result;
		}

		public PresenterBehaviour GetPresenterForView(ViewBehaviour viewBehaviour)
		{
			return _presenterByViewDictionary[viewBehaviour];
		}

		public bool TryGetPresenterForView(ViewBehaviour viewBehaviour, out PresenterBehaviour presenterBehaviour)
		{
			return _presenterByViewDictionary.TryGetValue(viewBehaviour, out presenterBehaviour);
		}

		public TPresenterType GetPresenter<TPresenterType>() where TPresenterType : PresenterBehaviour
		{
			return GetPresenter(typeof(TPresenterType)) as TPresenterType;
		}

		public IEnumerable<TPresenterType> GetPresenters<TPresenterType>() where TPresenterType : PresenterBehaviour
		{
			return GetPresenters(typeof(TPresenterType)).Cast<TPresenterType>();
		}

		public bool TryGetPresenter<TPresenterType>(out TPresenterType presenter) where TPresenterType : PresenterBehaviour
		{
			PresenterBehaviour presenter2;
			bool result = TryGetPresenter(typeof(TPresenterType), out presenter2);
			presenter = presenter2 as TPresenterType;
			return result;
		}

		public bool TryGetPresenters<TPresenterType>(out IEnumerable<TPresenterType> presenters) where TPresenterType : PresenterBehaviour
		{
			IEnumerable<PresenterBehaviour> presenters2;
			bool result = TryGetPresenters(typeof(TPresenterType), out presenters2);
			presenters = presenters2.Cast<TPresenterType>();
			return result;
		}

		public PresenterBehaviour GetPresenter(Type typeOf)
		{
			foreach (KeyValuePair<ViewBehaviour, PresenterBehaviour> item in _presenterByViewDictionary)
			{
				if (item.Value.GetType() == typeOf)
				{
					return item.Value;
				}
			}
			throw new PresenterNotFoundException(typeOf);
		}

		private IEnumerable<PresenterBehaviour> GetPresenters(Type presentersType)
		{
			List<PresenterBehaviour> list = new List<PresenterBehaviour>();
			foreach (KeyValuePair<ViewBehaviour, PresenterBehaviour> item in _presenterByViewDictionary)
			{
				if (item.Value.GetType() == presentersType)
				{
					list.Add(item.Value);
				}
			}
			return list;
		}

		public bool TryGetPresenter(Type typeOf, out PresenterBehaviour presenter)
		{
			presenter = null;
			foreach (KeyValuePair<ViewBehaviour, PresenterBehaviour> item in _presenterByViewDictionary)
			{
				if (!(item.Value.GetType() != typeOf))
				{
					presenter = item.Value;
					break;
				}
			}
			return presenter != null;
		}

		public bool TryGetPresenters(Type presentersType, out IEnumerable<PresenterBehaviour> presenters)
		{
			List<PresenterBehaviour> list = new List<PresenterBehaviour>();
			foreach (KeyValuePair<ViewBehaviour, PresenterBehaviour> item in _presenterByViewDictionary)
			{
				if (!(item.Value.GetType() != presentersType))
				{
					list.Add(item.Value);
				}
			}
			presenters = list;
			return presenters.Any();
		}

		public void AddMap(ViewBehaviour viewBehaviour, PresenterBehaviour presenterBehaviour)
		{
			_presenterByViewDictionary[viewBehaviour] = presenterBehaviour;
		}

		public void RemoveMap(ViewBehaviour viewBehaviour)
		{
			_presenterByViewDictionary.Remove(viewBehaviour);
		}

		public void ClearMaps()
		{
			_presenterByViewDictionary.Clear();
		}

		public void RefreshMaps()
		{
			List<ViewBehaviour> list = new List<ViewBehaviour>();
			foreach (KeyValuePair<ViewBehaviour, PresenterBehaviour> item in _presenterByViewDictionary)
			{
				if (item.Key == null || item.Key.gameObject == null)
				{
					list.Add(item.Key);
				}
			}
			foreach (ViewBehaviour item2 in list)
			{
				_presenterByViewDictionary.Remove(item2);
			}
		}

		private void MapViewAndPresenter(ViewBehaviour viewBehaviour, PresenterBehaviour presenterBehaviour)
		{
			_presenterByViewDictionary[viewBehaviour] = presenterBehaviour;
			presenterBehaviour.SetView(viewBehaviour);
		}
	}
}
