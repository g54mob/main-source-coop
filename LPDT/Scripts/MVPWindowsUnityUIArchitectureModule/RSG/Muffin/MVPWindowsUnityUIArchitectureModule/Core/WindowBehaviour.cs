using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RSG.Muffin.AssetLoaderModule.Core;
using UnityEngine;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	[PublicAPI]
	public abstract class WindowBehaviour : IWindow
	{
		protected IWindowInstance _windowInstance;

		protected readonly IWindowsFactory _windowsFactory;

		protected readonly IWindowsComponentsFinderService _windowsComponentsFinderService;

		protected readonly IWindowsService _windowsService;

		public WindowStatus WindowStatus { get; protected set; }

		public event Action<Type> OnWindowOpened;

		public event Action<Type> OnWindowClosed;

		public event Action<Type> OnWindowShowed;

		public event Action<Type> OnWindowHidden;

		protected WindowBehaviour(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService)
		{
			_windowsService = windowsService ?? throw new ArgumentNullException("windowsService");
			_windowsFactory = windowsFactory ?? throw new ArgumentNullException("windowsFactory");
			_windowsComponentsFinderService = windowsComponentsFinderService ?? throw new ArgumentNullException("windowsComponentsFinderService");
		}

		public virtual void Initialize()
		{
			_windowsService.RegisterWindow(GetType(), this);
		}

		public virtual void Dispose()
		{
			if (WindowStatus != WindowStatus.Closed)
			{
				Close();
			}
			_windowsService.UnRegisterWindow(GetType());
		}

		public async Task OpenAsync()
		{
			if (_windowInstance != null)
			{
				throw new WindowOperationFailureException(GetType(), "OpenAsync", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			await InitializeWindowAsync();
			WindowStatus = WindowStatus.Showed;
			this.OnWindowOpened?.Invoke(GetType());
			OnOpened();
		}

		public void Open()
		{
			if (_windowInstance != null)
			{
				throw new WindowOperationFailureException(GetType(), "Open", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			InitializeWindow();
			WindowStatus = WindowStatus.Showed;
			this.OnWindowOpened?.Invoke(GetType());
			OnOpened();
		}

		public void OpenPreloaded()
		{
			if (_windowInstance != null)
			{
				throw new WindowOperationFailureException(GetType(), "Open", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			InitializePreloadWindow();
			WindowStatus = WindowStatus.Showed;
			this.OnWindowOpened?.Invoke(GetType());
			OnOpened();
		}

		public void Close()
		{
			if (WindowStatus == WindowStatus.Closed)
			{
				throw new WindowOperationFailureException(GetType(), "Close", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			DisposeWindow();
			WindowStatus = WindowStatus.Closed;
			this.OnWindowClosed?.Invoke(GetType());
			OnClosed();
		}

		public void Show()
		{
			if (WindowStatus == WindowStatus.Showed)
			{
				throw new WindowOperationFailureException(GetType(), "Show", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			if (WindowStatus == WindowStatus.Closed)
			{
				throw new WindowOperationFailureException(GetType(), "Show", WindowOperationFailureCause.WindowIsNotOpened);
			}
			_windowInstance.Show();
			WindowStatus = WindowStatus.Showed;
			this.OnWindowShowed?.Invoke(GetType());
			OnShowed();
		}

		public void Hide()
		{
			if (WindowStatus == WindowStatus.Hidden)
			{
				throw new WindowOperationFailureException(GetType(), "Hide", WindowOperationFailureCause.OperationIsAlreadyPerformed);
			}
			if (WindowStatus == WindowStatus.Closed)
			{
				throw new WindowOperationFailureException(GetType(), "Hide", WindowOperationFailureCause.WindowIsNotOpened);
			}
			_windowInstance.Hide();
			WindowStatus = WindowStatus.Hidden;
			this.OnWindowHidden?.Invoke(GetType());
			OnHidden();
		}

		public void AddView(Transform viewRoot, bool worldPositionStays = true)
		{
			_windowInstance.AddChild(viewRoot, worldPositionStays);
			UpdateWindow();
		}

		public void RemoveView(Transform viewRoot)
		{
			_windowInstance.RemoveChild(viewRoot);
			UpdateWindow();
		}

		public TPresenterType GetPresenter<TPresenterType>() where TPresenterType : PresenterBehaviour
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenter");
			}
			return _windowsComponentsFinderService.GetPresenterInWindow<TPresenterType>(_windowInstance, collectIfNotFound: true);
		}

		public IEnumerable<TPresenterType> GetPresenters<TPresenterType>() where TPresenterType : PresenterBehaviour
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenters");
			}
			return _windowsComponentsFinderService.GetPresentersInWindow<TPresenterType>(_windowInstance, collectIfNotFound: true);
		}

		public bool TryGetPresenter<TPresenterType>(out TPresenterType presenter) where TPresenterType : PresenterBehaviour
		{
			presenter = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresenterInWindow<TPresenterType>(_windowInstance, out presenter, collectIfNotFound: true);
			}
			return false;
		}

		public bool TryGetPresenters<TPresenterType>(out IEnumerable<TPresenterType> presenters) where TPresenterType : PresenterBehaviour
		{
			presenters = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresentersInWindow(_windowInstance, out presenters, true);
			}
			return false;
		}

		public TPresenterType GetPresenterForView<TPresenterType>(ViewBehaviour view) where TPresenterType : PresenterBehaviour
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenter");
			}
			return _windowsComponentsFinderService.GetPresenterInWindowForView<TPresenterType>(_windowInstance, view, collectIfNotFound: true);
		}

		public bool TryGetPresenterForView<TPresenterType>(ViewBehaviour view, out TPresenterType presenter) where TPresenterType : PresenterBehaviour
		{
			presenter = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresenterInWindowForView(_windowInstance, view, out presenter, true);
			}
			return false;
		}

		public PresenterBehaviour GetPresenter(Type ofType)
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenter");
			}
			return _windowsComponentsFinderService.GetPresenterInWindow(ofType, _windowInstance, collectIfNotFound: true);
		}

		public IEnumerable<PresenterBehaviour> GetPresenters(Type presentersType)
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenters");
			}
			return _windowsComponentsFinderService.GetPresentersInWindow(presentersType, _windowInstance, collectIfNotFound: true);
		}

		public bool TryGetPresenter(Type ofType, out PresenterBehaviour presenter)
		{
			presenter = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresenterInWindow(ofType, _windowInstance, out presenter, collectIfNotFound: true);
			}
			return false;
		}

		public bool TryGetPresenters(Type presentersType, out IEnumerable<PresenterBehaviour> presenterBases)
		{
			presenterBases = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresentersInWindow(presentersType, _windowInstance, out presenterBases, collectIfNotFound: true);
			}
			return false;
		}

		public PresenterBehaviour GetPresenterForView(ViewBehaviour view)
		{
			if (_windowInstance == null)
			{
				throw new WindowIsNotOpenedException(GetType(), "GetPresenter");
			}
			return _windowsComponentsFinderService.GetPresenterInWindowForView(_windowInstance, view, collectIfNotFound: true);
		}

		public bool TryGetPresenterForView(ViewBehaviour view, out PresenterBehaviour presenter)
		{
			presenter = null;
			if (_windowInstance != null)
			{
				return _windowsComponentsFinderService.TryGetPresenterInWindowForView(_windowInstance, view, out presenter, collectIfNotFound: true);
			}
			return false;
		}

		public virtual void OnBack()
		{
		}

		protected virtual void OnOpened()
		{
		}

		protected virtual void OnClosed()
		{
		}

		protected virtual void OnShowed()
		{
		}

		protected virtual void OnHidden()
		{
		}

		protected virtual void OnBecomeFocusable()
		{
		}

		protected virtual void OnBecomeUnFocusable()
		{
		}

		private async Task InitializeWindowAsync()
		{
			_windowInstance = await _windowsFactory.GetWindowInstanceForWindowTypeAsync(GetType(), AssetLoadSource.Addressables);
			UpdateWindow();
		}

		private void InitializeWindow()
		{
			_windowInstance = _windowsFactory.GetWindowInstanceForWindowType(GetType(), AssetLoadSource.Addressables);
			UpdateWindow();
		}

		private void InitializePreloadWindow()
		{
			_windowInstance = _windowsFactory.GetPreloadWindowInstanceForWindowType(GetType());
			UpdateWindow();
		}

		public virtual void UpdateWindow()
		{
			_windowsComponentsFinderService.CollectPresentersInWindow(_windowInstance);
		}

		protected virtual void DisposeWindow()
		{
			foreach (ViewBehaviour allView in _windowInstance.GetAllViews())
			{
				allView.DisableView();
				allView.DisposeView();
			}
			_windowsComponentsFinderService.ClearPresentersInWindow(_windowInstance);
			_windowInstance?.Destroy();
			_windowInstance = null;
		}
	}
}
