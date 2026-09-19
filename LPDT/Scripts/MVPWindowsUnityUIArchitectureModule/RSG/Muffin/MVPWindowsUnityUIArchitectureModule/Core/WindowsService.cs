using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using RSG.Muffin.EventBusModule.Interfaces;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public sealed class WindowsService : IWindowsService, IDisposable
	{
		private readonly IGenericEventBus<WindowsEventData> _windowsEventBus;

		private readonly Dictionary<Type, IWindow> _windowByTypeDictionary = new Dictionary<Type, IWindow>();

		public event Action<Type> OnWindowOpened;

		public event Action<Type> OnWindowClosed;

		public event Action<Type> OnWindowShowed;

		public event Action<Type> OnWindowHidden;

		public WindowsService(IGenericEventBus<WindowsEventData> windowsEventBus)
		{
			_windowsEventBus = windowsEventBus ?? throw new ArgumentNullException("windowsEventBus");
		}

		public void Dispose()
		{
			foreach (KeyValuePair<Type, IWindow> item in _windowByTypeDictionary)
			{
				item.Value.OnWindowOpened -= OnWindowOpenedHandler;
				item.Value.OnWindowClosed -= OnWindowClosedHandler;
				item.Value.OnWindowShowed -= OnWindowShowedHandler;
				item.Value.OnWindowHidden -= OnWindowHiddenHandler;
			}
			_windowByTypeDictionary.Clear();
		}

		public void OpenWindow<TWindowType>() where TWindowType : IWindow
		{
			OpenWindow(typeof(TWindowType));
		}

		public void OpenPreloadWindow<TWindowType>() where TWindowType : IWindow
		{
			OpenPreloadWindow(typeof(TWindowType));
		}

		public async UniTask OpenWindowAsync<TWindowType>() where TWindowType : IWindow
		{
			await OpenWindowAsync(typeof(TWindowType));
		}

		public void CloseWindow<TWindowType>() where TWindowType : IWindow
		{
			CloseWindow(typeof(TWindowType));
		}

		public void ShowWindow<TWindowType>() where TWindowType : IWindow
		{
			ShowWindow(typeof(TWindowType));
		}

		public void HideWindow<TWindowType>() where TWindowType : IWindow
		{
			HideWindow(typeof(TWindowType));
		}

		public void RegisterWindow<TWindowType>(TWindowType window) where TWindowType : IWindow
		{
			RegisterWindow(typeof(TWindowType), window);
		}

		public void UnRegisterWindow<TWindowType>() where TWindowType : IWindow
		{
			UnRegisterWindow(typeof(TWindowType));
		}

		public void OpenWindow(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "OpenWindow");
			}
			value.Open();
		}

		public void OpenPreloadWindow(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "OpenPreloadWindow");
			}
			value.OpenPreloaded();
		}

		public async UniTask OpenWindowAsync(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "OpenWindow");
			}
			await value.OpenAsync();
		}

		public void CloseWindow(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "CloseWindow");
			}
			value.Close();
		}

		public void ShowWindow(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "ShowWindow");
			}
			value.Show();
		}

		public void HideWindow(Type windowType)
		{
			if (!_windowByTypeDictionary.TryGetValue(windowType, out var value))
			{
				throw new WindowTypeNotFoundException(windowType, "HideWindow");
			}
			value.Hide();
		}

		public void RegisterWindow(Type windowType, IWindow window)
		{
			_windowByTypeDictionary.Add(windowType, window);
			window.OnWindowOpened += OnWindowOpenedHandler;
			window.OnWindowClosed += OnWindowClosedHandler;
			window.OnWindowShowed += OnWindowShowedHandler;
			window.OnWindowHidden += OnWindowHiddenHandler;
		}

		public void UnRegisterWindow(Type windowType)
		{
			if (_windowByTypeDictionary.Any())
			{
				_windowByTypeDictionary[windowType].OnWindowOpened -= OnWindowOpenedHandler;
				_windowByTypeDictionary[windowType].OnWindowClosed -= OnWindowClosedHandler;
				_windowByTypeDictionary[windowType].OnWindowShowed -= OnWindowShowedHandler;
				_windowByTypeDictionary[windowType].OnWindowHidden -= OnWindowHiddenHandler;
				_windowByTypeDictionary.Remove(windowType);
			}
		}

		public IWindow GetWindow(Type type)
		{
			if (_windowByTypeDictionary.Any())
			{
				return _windowByTypeDictionary.FirstOrDefault((KeyValuePair<Type, IWindow> pair) => pair.Key == type).Value;
			}
			return null;
		}

		public IEnumerable<IWindow> GetAllWindowsWithStatus(WindowStatus status)
		{
			IEnumerable<KeyValuePair<Type, IWindow>> source = _windowByTypeDictionary.Where((KeyValuePair<Type, IWindow> pair) => pair.Value.WindowStatus == status);
			if (source.Any())
			{
				return source.Select((KeyValuePair<Type, IWindow> pair) => pair.Value);
			}
			return new List<IWindow>();
		}

		public IEnumerable<IWindow> GetAllWindows()
		{
			if (_windowByTypeDictionary.Any())
			{
				return _windowByTypeDictionary.Select((KeyValuePair<Type, IWindow> pair) => pair.Value);
			}
			return new List<IWindow>();
		}

		private void OnWindowOpenedHandler(Type windowType)
		{
			_windowsEventBus.Publish(new OnWindowOpenedEventData
			{
				WindowType = windowType
			});
			this.OnWindowOpened?.Invoke(windowType);
		}

		private void OnWindowClosedHandler(Type windowType)
		{
			_windowsEventBus.Publish(new OnWindowClosedEventData
			{
				WindowType = windowType
			});
			this.OnWindowClosed?.Invoke(windowType);
		}

		private void OnWindowShowedHandler(Type windowType)
		{
			_windowsEventBus.Publish(new OnWindowShowedEventData
			{
				WindowType = windowType
			});
			this.OnWindowShowed?.Invoke(windowType);
		}

		private void OnWindowHiddenHandler(Type windowType)
		{
			_windowsEventBus.Publish(new OnWindowHiddenEventData
			{
				WindowType = windowType
			});
			this.OnWindowHidden?.Invoke(windowType);
		}
	}
}
