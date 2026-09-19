using UnityEngine;

namespace Features.CameraModelModule
{
	public interface ICameraSpectatorFollowService
	{
		bool IsActive { get; }

		Transform Begin(Transform subject);

		void End();
	}
}
