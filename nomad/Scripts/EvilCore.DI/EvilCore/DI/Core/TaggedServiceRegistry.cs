using System;
using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace EvilCore.DI.Core
{
	public class TaggedServiceRegistry<TService> where TService : class
	{
		private readonly IObjectResolver _container;

		private readonly Dictionary<string, List<TService>> _taggedServices = new Dictionary<string, List<TService>>();

		private readonly Dictionary<Type, List<string>> _serviceTypeTags = new Dictionary<Type, List<string>>();

		private bool _isInitialized;

		[Inject]
		public TaggedServiceRegistry(IObjectResolver container)
		{
			_container = container;
		}

		public void Initialize()
		{
			if (!_isInitialized)
			{
				ScanForTaggedServices();
				_isInitialized = true;
			}
		}

		public IEnumerable<TService> GetServicesWithTag(string tag)
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			if (!_taggedServices.TryGetValue(tag, out var value))
			{
				return Enumerable.Empty<TService>();
			}
			return value;
		}

		public TSpecificService GetServiceWithTag<TSpecificService>(string tag) where TSpecificService : TService
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			List<TSpecificService> list = GetServicesWithTag(tag).OfType<TSpecificService>().ToList();
			if (list.Count == 0)
			{
				return default(TSpecificService);
			}
			_ = list.Count;
			_ = 1;
			return list.First();
		}

		public IEnumerable<string> GetTagsForServiceType(Type serviceType)
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			if (!_serviceTypeTags.TryGetValue(serviceType, out var value))
			{
				return Enumerable.Empty<string>();
			}
			return value;
		}

		public IEnumerable<string> GetAllTags()
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			return _taggedServices.Keys;
		}

		public IEnumerable<Type> GetAllServiceTypes()
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			return _serviceTypeTags.Keys;
		}

		public void RegisterService(TService service, string tag)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			Type type = service.GetType();
			if (!_taggedServices.TryGetValue(tag, out var value))
			{
				value = new List<TService>();
				_taggedServices[tag] = value;
			}
			if (!value.Contains(service))
			{
				value.Add(service);
			}
			if (!_serviceTypeTags.TryGetValue(type, out var value2))
			{
				value2 = new List<string>();
				_serviceTypeTags[type] = value2;
			}
			if (!value2.Contains(tag))
			{
				value2.Add(tag);
			}
		}

		private void ScanForTaggedServices()
		{
			_taggedServices.Clear();
			_serviceTypeTags.Clear();
			IEnumerable<TService> enumerable = _container.Resolve<IEnumerable<TService>>();
			if (enumerable == null)
			{
				return;
			}
			foreach (TService item in enumerable)
			{
				if (item == null)
				{
					continue;
				}
				List<ServiceTagAttribute> list = item.GetType().GetCustomAttributes(typeof(ServiceTagAttribute), inherit: false).Cast<ServiceTagAttribute>()
					.ToList();
				if (list.Count == 0)
				{
					continue;
				}
				foreach (ServiceTagAttribute item2 in list)
				{
					RegisterService(item, item2.Tag);
				}
			}
		}
	}
}
