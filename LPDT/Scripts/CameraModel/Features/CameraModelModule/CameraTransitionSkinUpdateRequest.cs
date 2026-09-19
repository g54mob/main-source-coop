using System;

namespace Features.CameraModelModule
{
	public class CameraTransitionSkinUpdateRequest
	{
		public event Action<bool> OnCameraTransitionSkinUpdateRequested;

		public void Request(bool playerVisible)
		{
			this.OnCameraTransitionSkinUpdateRequested?.Invoke(playerVisible);
		}
	}
}
