using System;

namespace EvilCore.UI.Scripts
{
	public interface IAllDownedPopup
	{
		event Action OnConfirmed;

		void Show();
	}
}
