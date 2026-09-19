using Features.UINavigationModuleRealization.Scripts.BackButton;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Features.UINavigationModuleRealization.Scripts
{
	public abstract class UIBackNavigationCallback : MonoBehaviour, IBackButtonProcessor
	{
		private IUIBackButtonRegistrationService _backButtonRegistrationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.UIElement;

		public virtual bool CanHandleBack()
		{
			return true;
		}

		[Inject]
		private void InjectDependencies(IUIBackButtonRegistrationService backButtonRegistrationService)
		{
			_backButtonRegistrationService = backButtonRegistrationService;
		}

		protected virtual void OnEnable()
		{
			_backButtonRegistrationService.Register(this);
		}

		protected virtual void OnDisable()
		{
			_backButtonRegistrationService.Unregister(this);
		}

		protected virtual bool IsSelected()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null && currentSelectedGameObject.transform.IsChildOf(base.transform))
			{
				return true;
			}
			return false;
		}

		public abstract void OnBack();
	}
}
