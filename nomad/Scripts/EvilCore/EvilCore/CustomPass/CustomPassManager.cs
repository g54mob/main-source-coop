using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.HighDefinition;

namespace EvilCore.CustomPass
{
	public class CustomPassManager : SerializedMonoBehaviour, ICustomPassManager
	{
		private Dictionary<string, CustomPassVolume> _discoveredPassVolumes = new Dictionary<string, CustomPassVolume>();

		private readonly Dictionary<string, CustomPassHandle> _passes = new Dictionary<string, CustomPassHandle>();

		private readonly CustomPassPropertyAccessor _nullAccessor;

		public bool IsInitialized { get; private set; }

		public CustomPassManager()
		{
			_nullAccessor = new CustomPassPropertyAccessor(null);
		}

		private void Awake()
		{
			DiscoverPasses();
			IsInitialized = true;
		}

		private void DiscoverPasses()
		{
			_passes.Clear();
			_discoveredPassVolumes.Clear();
			CustomPassVolume[] componentsInChildren = GetComponentsInChildren<CustomPassVolume>(includeInactive: true);
			foreach (CustomPassVolume customPassVolume in componentsInChildren)
			{
				string text = StripCustomPassSuffix(customPassVolume.gameObject.name);
				if (!_passes.ContainsKey(text))
				{
					CustomPassHandle value = new CustomPassHandle(text, customPassVolume);
					_passes[text] = value;
					_discoveredPassVolumes[text] = customPassVolume;
				}
			}
		}

		private static string StripCustomPassSuffix(string name)
		{
			if (name.EndsWith("CustomPass"))
			{
				int length = "CustomPass".Length;
				return name.Substring(0, name.Length - length);
			}
			return name;
		}

		public void EnablePass(string passId)
		{
			if (TryGetHandle(passId, out var handle))
			{
				handle.IsEnabled = true;
			}
		}

		public void DisablePass(string passId)
		{
			if (TryGetHandle(passId, out var handle))
			{
				handle.IsEnabled = false;
			}
		}

		public void SetPassEnabled(string passId, bool enabled)
		{
			if (TryGetHandle(passId, out var handle))
			{
				handle.IsEnabled = enabled;
			}
		}

		public bool IsPassEnabled(string passId)
		{
			if (TryGetHandle(passId, out var handle))
			{
				return handle.IsEnabled;
			}
			return false;
		}

		public bool HasPass(string passId)
		{
			return _passes.ContainsKey(passId);
		}

		public CustomPassPropertyAccessor GetProperties(string passId)
		{
			if (_passes.TryGetValue(passId, out var value))
			{
				return new CustomPassPropertyAccessor(value);
			}
			return _nullAccessor;
		}

		public void DisableAll()
		{
			foreach (CustomPassHandle value in _passes.Values)
			{
				value.IsEnabled = false;
			}
		}

		public void EnableAll()
		{
			foreach (CustomPassHandle value in _passes.Values)
			{
				value.IsEnabled = true;
			}
		}

		private bool TryGetHandle(string passId, out CustomPassHandle handle)
		{
			if (_passes.TryGetValue(passId, out handle))
			{
				return true;
			}
			return false;
		}
	}
}
