using System;

namespace Features.InteractModule.Scripts
{
	public interface IToggleableInteractable
	{
		bool IsToggledOn { get; }

		event Action OnToggleChanged;
	}
}
