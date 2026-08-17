using System;
using VContainer;

namespace EvilCore.DI.Core
{
	public class LazyLoader<T> where T : class
	{
		private readonly IObjectResolver _container;

		private T _value;

		private bool _isValueCreated;

		public bool IsValueCreated => _isValueCreated;

		public T Value
		{
			get
			{
				if (!_isValueCreated)
				{
					_value = _container.Resolve<T>();
					_isValueCreated = true;
				}
				return _value;
			}
		}

		[Inject]
		public LazyLoader(IObjectResolver container)
		{
			_container = container;
		}

		public T GetValueIfValid(Func<bool> precondition)
		{
			if (precondition())
			{
				return Value;
			}
			return null;
		}

		public void Initialize()
		{
			if (!_isValueCreated)
			{
				_ = Value;
			}
		}

		public void Reset()
		{
			_value = null;
			_isValueCreated = false;
		}
	}
}
