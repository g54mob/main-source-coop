using System;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IFocusablesService
	{
		event Action<Type> OnContainerBecomeFocusable;

		event Action<Type> OnContainerBecomeUnFocusable;

		void MakeContainerFocusable<TFocusableContainerType>() where TFocusableContainerType : IFocusableContainer;

		void MakeContainerUnFocusable<TFocusableContainerType>() where TFocusableContainerType : IFocusableContainer;

		void RegisterFocusableContainer<TFocusableContainerType>(TFocusableContainerType container) where TFocusableContainerType : IFocusableContainer;

		void UnRegisterFocusableContainer<TFocusableContainerType>() where TFocusableContainerType : IFocusableContainer;

		void MakeContainerFocusable(Type focusableContainerType);

		void MakeContainerUnFocusable(Type focusableContainerType);

		void RegisterFocusableContainer(Type focusableContainerType, IFocusableContainer container);

		void UnRegisterFocusableContainer(Type focusableContainerType);
	}
}
