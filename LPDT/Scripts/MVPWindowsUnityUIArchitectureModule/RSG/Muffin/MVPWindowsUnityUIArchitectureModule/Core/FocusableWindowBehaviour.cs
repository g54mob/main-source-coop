using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RSG.Muffin.AssetLoaderModule.Core;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	[PublicAPI]
	public abstract class FocusableWindowBehaviour : WindowBehaviour, IFocusableContainer
	{
		protected List<IFocusableElement> _focusableElements = new List<IFocusableElement>();

		private IFocusablesService _focusablesService;

		public bool IsContainerFocusable { get; protected set; }

		public IEnumerable<IFocusableElement> FocusableElements => _focusableElements;

		public event Action OnContainerBecomeFocusable;

		public event Action OnContainerBecomeUnFocusable;

		protected FocusableWindowBehaviour(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService)
		{
			_focusablesService = focusablesService ?? throw new ArgumentNullException("focusablesService");
		}

		public override void Initialize()
		{
			_windowsService.RegisterWindow(GetType(), this);
			_focusablesService.RegisterFocusableContainer(GetType(), this);
		}

		public override void Dispose()
		{
			if (base.WindowStatus != WindowStatus.Closed)
			{
				Close();
			}
			_windowsService.UnRegisterWindow(GetType());
			_focusablesService.UnRegisterFocusableContainer(GetType());
		}

		public void MakeFocusable()
		{
			if (IsContainerFocusable)
			{
				throw new FocusableContainerOperationFailureException(GetType(), "MakeFocusable", FocusableContainerOperationFailureCause.OperationIsAlreadyPerformed);
			}
			WindowStatus windowStatus = base.WindowStatus;
			if (windowStatus == WindowStatus.Closed || windowStatus == WindowStatus.Hidden)
			{
				throw new FocusableContainerOperationFailureException(GetType(), "MakeFocusable", FocusableContainerOperationFailureCause.ContainerIsInactive);
			}
			MakeAllElementsFocusable();
			IsContainerFocusable = true;
			this.OnContainerBecomeFocusable?.Invoke();
			OnBecomeFocusable();
		}

		public void MakeUnFocusable()
		{
			if (!IsContainerFocusable)
			{
				throw new FocusableContainerOperationFailureException(GetType(), "MakeUnFocusable", FocusableContainerOperationFailureCause.OperationIsAlreadyPerformed);
			}
			MakeAllElementsUnFocusable();
			IsContainerFocusable = false;
			this.OnContainerBecomeUnFocusable?.Invoke();
			OnBecomeUnFocusable();
		}

		public void SetFocusableElements(IEnumerable<IFocusableElement> focusableElements)
		{
			_focusableElements = focusableElements.ToList();
			if (IsContainerFocusable)
			{
				MakeAllElementsFocusable();
			}
			else
			{
				MakeAllElementsUnFocusable();
			}
		}

		public void RegisterFocusableElement(IFocusableElement focusableElement)
		{
			_focusableElements.Add(focusableElement);
			if (IsContainerFocusable)
			{
				focusableElement.MakeFocusable();
			}
			else
			{
				focusableElement.MakeUnFocusable();
			}
		}

		public void RegisterFocusableElementRange(IEnumerable<IFocusableElement> focusableElements)
		{
			IEnumerable<IFocusableElement> enumerable = focusableElements.ToList();
			_focusableElements.AddRange(enumerable);
			if (IsContainerFocusable)
			{
				foreach (IFocusableElement item in enumerable)
				{
					item.MakeFocusable();
				}
				return;
			}
			foreach (IFocusableElement item2 in enumerable)
			{
				item2.MakeUnFocusable();
			}
		}

		public void UnRegisterFocusableElement(IFocusableElement focusableElement)
		{
			_focusableElements.Remove(focusableElement);
			focusableElement.MakeUnFocusable();
		}

		public void UnRegisterFocusableElementRange(IEnumerable<IFocusableElement> focusableElements)
		{
			_focusableElements = _focusableElements.Except(focusableElements).ToList();
			foreach (IFocusableElement focusableElement in focusableElements)
			{
				focusableElement.MakeUnFocusable();
			}
		}

		private void InitializeWindow()
		{
			_windowInstance = _windowsFactory.GetWindowInstanceForWindowType(GetType(), AssetLoadSource.Addressables);
			UpdateWindow();
		}

		public override void UpdateWindow()
		{
			SetFocusableElements(_windowsComponentsFinderService.GetAllFocusablesInWindow(_windowInstance, includeInactive: true));
			_windowsComponentsFinderService.CollectPresentersInWindow(_windowInstance);
		}

		protected override void DisposeWindow()
		{
			foreach (IFocusableElement focusableElement in _focusableElements)
			{
				focusableElement.MakeUnFocusable();
			}
			_focusableElements.Clear();
			foreach (ViewBehaviour allView in _windowInstance.GetAllViews())
			{
				allView.DisableView();
				allView.DisposeView();
			}
			_windowsComponentsFinderService.ClearPresentersInWindow(_windowInstance);
			_windowInstance?.Destroy();
			_windowInstance = null;
			IsContainerFocusable = false;
		}

		private void MakeAllElementsFocusable()
		{
			_focusableElements.ForEach(delegate(IFocusableElement element)
			{
				element.MakeFocusable();
			});
		}

		private void MakeAllElementsUnFocusable()
		{
			_focusableElements.ForEach(delegate(IFocusableElement element)
			{
				element.MakeUnFocusable();
			});
		}
	}
}
