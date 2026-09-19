using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IWindowsService
	{
		event Action<Type> OnWindowOpened;

		event Action<Type> OnWindowClosed;

		event Action<Type> OnWindowShowed;

		event Action<Type> OnWindowHidden;

		void OpenWindow<TWindowType>() where TWindowType : IWindow;

		void OpenPreloadWindow<TWindowType>() where TWindowType : IWindow;

		UniTask OpenWindowAsync<TWindowType>() where TWindowType : IWindow;

		void CloseWindow<TWindowType>() where TWindowType : IWindow;

		void ShowWindow<TWindowType>() where TWindowType : IWindow;

		void HideWindow<TWindowType>() where TWindowType : IWindow;

		void RegisterWindow<TWindowType>(TWindowType window) where TWindowType : IWindow;

		void UnRegisterWindow<TWindowType>() where TWindowType : IWindow;

		void OpenWindow(Type windowType);

		void OpenPreloadWindow(Type windowType);

		UniTask OpenWindowAsync(Type windowType);

		void CloseWindow(Type windowType);

		void ShowWindow(Type windowType);

		void HideWindow(Type windowType);

		void RegisterWindow(Type windowType, IWindow window);

		void UnRegisterWindow(Type windowType);

		IEnumerable<IWindow> GetAllWindows();

		IWindow GetWindow(Type type);

		IEnumerable<IWindow> GetAllWindowsWithStatus(WindowStatus status);
	}
}
