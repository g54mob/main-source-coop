using System;
using Features.GrabModule.Scripts;

namespace Features.LineArmModule.Scripts.Data
{
	public class InteractModel
	{
		private IPointGrabable _currentInteractable;

		public IPointGrabable CurrentInteractable
		{
			get
			{
				return _currentInteractable;
			}
			set
			{
				_currentInteractable = value;
				this.OnCurrentInteractableChanged?.Invoke();
			}
		}

		public IPointGrabable ForcedFallbackTarget { get; set; }

		public event Action OnCurrentInteractableChanged;

		public event Action<bool> OnCursorVisibilityChanged;

		public void ChangeCursorVisibility(bool visible)
		{
			this.OnCursorVisibilityChanged?.Invoke(visible);
		}
	}
}
