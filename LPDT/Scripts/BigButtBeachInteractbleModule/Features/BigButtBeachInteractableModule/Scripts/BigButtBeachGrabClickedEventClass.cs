using System;
using Features.GrabModule.Scripts;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachGrabClickedEventClass
	{
		public event Action<BigButtBeachInteractable, SimplePointGrabable, int> OnGrabClicked;

		internal void InvokeGrabClicked(BigButtBeachInteractable source, SimplePointGrabable grabable, int playerId)
		{
			this.OnGrabClicked?.Invoke(source, grabable, playerId);
		}
	}
}
