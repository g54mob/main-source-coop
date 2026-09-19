using System;
using UnityEngine.EventSystems;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class ButtonWithNavigationCallBackWithSubmit : ButtonWithNavigationCallBackBehaviour, ISubmittableWithNavigationCallbacks, IPointerClickHandler, IEventSystemHandler
	{
		public event Action<ISubmittableWithNavigationCallbacks> OnSubmitEvent;

		public event Action<ISelectableWithNavigationCallbacks> OnClickEvent;

		void ISubmittableWithNavigationCallbacks.SetNavigatedFrom(MoveDirection moveDirection)
		{
			MoveDirectionFrom = moveDirection;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			this.OnClickEvent?.Invoke(this);
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			this.OnSubmitEvent?.Invoke(this);
		}
	}
}
