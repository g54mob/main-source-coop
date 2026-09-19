using System;
using System.Collections.Generic;
using Zenject;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class WindowFocusSystem : IInitializable, IDisposable
	{
		private readonly IFocusablesService _focusablesService;

		private readonly IWindowsService _windowsService;

		private Type _currentFocusedWindow;

		private Stack<Type> _focusWindowQueue = new Stack<Type>();

		private HashSet<Type> _hiddenWindows = new HashSet<Type>();

		public WindowFocusSystem(IFocusablesService focusablesService, IWindowsService windowsService)
		{
			_focusablesService = focusablesService;
			_windowsService = windowsService;
		}

		public void Initialize()
		{
			_windowsService.OnWindowOpened += OnWindowOpened;
			_windowsService.OnWindowClosed += OnWindowClosed;
			_windowsService.OnWindowShowed += OnWindowShowed;
			_windowsService.OnWindowHidden += OnWindowHidden;
		}

		public void Dispose()
		{
			_windowsService.OnWindowOpened -= OnWindowOpened;
			_windowsService.OnWindowClosed -= OnWindowClosed;
			_windowsService.OnWindowShowed -= OnWindowShowed;
			_windowsService.OnWindowHidden -= OnWindowHidden;
			_focusWindowQueue.Clear();
		}

		private void OnWindowOpened(Type openedWindowType)
		{
			if (IsWindowFocusable(openedWindowType))
			{
				if (_currentFocusedWindow != null)
				{
					_focusWindowQueue.Push(_currentFocusedWindow);
					_focusablesService.MakeContainerUnFocusable(_currentFocusedWindow);
				}
				_currentFocusedWindow = openedWindowType;
				_focusablesService.MakeContainerFocusable(_currentFocusedWindow);
			}
		}

		private void OnWindowClosed(Type closedWindowType)
		{
			if (!IsWindowFocusable(closedWindowType))
			{
				return;
			}
			if (_hiddenWindows.Contains(closedWindowType))
			{
				_hiddenWindows.Remove(closedWindowType);
				return;
			}
			if (!_focusWindowQueue.Contains(closedWindowType) && _currentFocusedWindow != closedWindowType)
			{
				throw new WindowIsNotAddedToFocusableCollectionException(closedWindowType);
			}
			if (_focusWindowQueue.Contains(closedWindowType))
			{
				RemoveWindowTypeFromStack(closedWindowType);
				return;
			}
			if (_currentFocusedWindow == closedWindowType)
			{
				if (_focusWindowQueue.TryPop(out var result))
				{
					_currentFocusedWindow = result;
					_focusablesService.MakeContainerFocusable(_currentFocusedWindow);
				}
				else
				{
					_currentFocusedWindow = null;
				}
				return;
			}
			throw new WindowIsNotAddedToFocusableCollectionException(closedWindowType);
		}

		private void OnWindowShowed(Type showedWindowType)
		{
			if (IsWindowFocusable(showedWindowType))
			{
				if (_currentFocusedWindow != null)
				{
					_focusWindowQueue.Push(_currentFocusedWindow);
					_focusablesService.MakeContainerUnFocusable(_currentFocusedWindow);
				}
				_hiddenWindows.Remove(showedWindowType);
				_currentFocusedWindow = showedWindowType;
				_focusablesService.MakeContainerFocusable(_currentFocusedWindow);
			}
		}

		private void OnWindowHidden(Type hiddenWindowType)
		{
			if (!IsWindowFocusable(hiddenWindowType))
			{
				return;
			}
			if (!_focusWindowQueue.Contains(hiddenWindowType) && _currentFocusedWindow != hiddenWindowType)
			{
				throw new WindowIsNotAddedToFocusableCollectionException(hiddenWindowType);
			}
			_hiddenWindows.Add(hiddenWindowType);
			if (_focusWindowQueue.Contains(hiddenWindowType))
			{
				RemoveWindowTypeFromStack(hiddenWindowType);
				return;
			}
			if (_currentFocusedWindow == hiddenWindowType)
			{
				_focusablesService.MakeContainerUnFocusable(_currentFocusedWindow);
				if (_focusWindowQueue.TryPop(out var result))
				{
					_currentFocusedWindow = result;
					_focusablesService.MakeContainerFocusable(_currentFocusedWindow);
				}
				else
				{
					_currentFocusedWindow = null;
				}
				return;
			}
			throw new WindowIsNotAddedToFocusableCollectionException(hiddenWindowType);
		}

		private void RemoveWindowTypeFromStack(Type closedWindowType)
		{
			Stack<Type> stack = new Stack<Type>();
			while (_focusWindowQueue.Count > 0)
			{
				Type type = _focusWindowQueue.Pop();
				if (type == closedWindowType)
				{
					break;
				}
				stack.Push(type);
			}
			while (stack.Count > 0)
			{
				_focusWindowQueue.Push(stack.Pop());
			}
		}

		private bool IsWindowFocusable(Type closedWindowType)
		{
			return typeof(IFocusableContainer).IsAssignableFrom(closedWindowType);
		}
	}
}
