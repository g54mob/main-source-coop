using System;
using VContainer;

namespace EvilCore.DI.Core
{
	public class ScopedContainer : IDisposable
	{
		private readonly IObjectResolver _parentContainer;

		private IObjectResolver _scopedContainer;

		private bool _isDisposed;

		public bool IsReady
		{
			get
			{
				if (_scopedContainer != null)
				{
					return !_isDisposed;
				}
				return false;
			}
		}

		public IObjectResolver Container
		{
			get
			{
				if (_isDisposed)
				{
					throw new ObjectDisposedException("ScopedContainer");
				}
				return _scopedContainer ?? _parentContainer;
			}
		}

		[Inject]
		public ScopedContainer(IObjectResolver parentContainer)
		{
			_parentContainer = parentContainer;
		}

		public void BeginScope(Action<IContainerBuilder> builder)
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException("ScopedContainer");
			}
			DisposeScope();
			ContainerBuilder containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterInstance(_parentContainer).AsImplementedInterfaces();
			builder?.Invoke(containerBuilder);
			_scopedContainer = containerBuilder.Build();
		}

		public T Resolve<T>()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException("ScopedContainer");
			}
			return Container.Resolve<T>();
		}

		public void DisposeScope()
		{
			_scopedContainer?.Dispose();
			_scopedContainer = null;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				DisposeScope();
				_isDisposed = true;
			}
		}
	}
}
