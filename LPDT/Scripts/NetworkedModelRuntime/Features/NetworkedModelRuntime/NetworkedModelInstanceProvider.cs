using System;
using System.Collections.Generic;
using Fusion;

namespace Features.NetworkedModelRuntime
{
	public class NetworkedModelInstanceProvider : INetworkedModelInstanceProvider
	{
		private static readonly IReadOnlyCollection<NetworkBehaviour> _empty = Array.Empty<NetworkBehaviour>();

		private readonly Dictionary<Type, HashSet<NetworkBehaviour>> _instancesByType = new Dictionary<Type, HashSet<NetworkBehaviour>>();

		public event Action<NetworkBehaviour> Added;

		public event Action<NetworkBehaviour> Removed;

		public void Register(NetworkBehaviour instance)
		{
			if (!(instance == null))
			{
				Type type = instance.GetType();
				if (!_instancesByType.TryGetValue(type, out var value))
				{
					value = new HashSet<NetworkBehaviour>();
					_instancesByType[type] = value;
				}
				if (value.Add(instance))
				{
					this.Added?.Invoke(instance);
				}
			}
		}

		public void Unregister(NetworkBehaviour instance)
		{
			if (!(instance == null) && _instancesByType.TryGetValue(instance.GetType(), out var value) && value.Remove(instance))
			{
				this.Removed?.Invoke(instance);
			}
		}

		public IReadOnlyCollection<NetworkBehaviour> GetAll(Type transportType)
		{
			if (!_instancesByType.TryGetValue(transportType, out var value))
			{
				return _empty;
			}
			return value;
		}

		public IEnumerable<T> GetAll<T>() where T : NetworkBehaviour
		{
			if (!_instancesByType.TryGetValue(typeof(T), out var value))
			{
				yield break;
			}
			foreach (NetworkBehaviour item in value)
			{
				yield return (T)item;
			}
		}
	}
}
