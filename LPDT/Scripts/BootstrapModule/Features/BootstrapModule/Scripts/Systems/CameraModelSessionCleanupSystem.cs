using System;
using Features.CameraModelModule;
using Features.GameCycle.Scripts.SessionCleanup;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class CameraModelSessionCleanupSystem : IInitializable, IDisposable
	{
		private readonly SessionCleanupEvent _sessionCleanupEvent;

		private readonly CameraModel _cameraModel;

		public CameraModelSessionCleanupSystem(SessionCleanupEvent sessionCleanupEvent, CameraModel cameraModel)
		{
			_sessionCleanupEvent = sessionCleanupEvent;
			_cameraModel = cameraModel;
		}

		public void Initialize()
		{
			_sessionCleanupEvent.OnSessionCleanup += _cameraModel.ResetSessionState;
		}

		public void Dispose()
		{
			_sessionCleanupEvent.OnSessionCleanup -= _cameraModel.ResetSessionState;
		}
	}
}
