using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using Zenject;

namespace Features.CameraModelModule
{
	public class CameraLateResolveService : IInitializable, IDisposable
	{
		private readonly CameraModel _cameraModel;

		private readonly List<ICameraLateResolveStep> _steps;

		private CancellationTokenSource _cancellation;

		private CinemachineBrain _managedBrain;

		private CinemachineBrain.UpdateMethods _restoreUpdateMethod;

		public CameraLateResolveService(CameraModel cameraModel, List<ICameraLateResolveStep> steps)
		{
			_cameraModel = cameraModel;
			_steps = steps;
		}

		public void Initialize()
		{
			_cancellation = new CancellationTokenSource();
			ResolveLoopAsync(_cancellation.Token).Forget();
		}

		public void Dispose()
		{
			_cancellation?.Cancel();
			_cancellation?.Dispose();
			_cancellation = null;
			ReleaseBrain();
		}

		private async UniTaskVoid ResolveLoopAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, token);
				Resolve();
			}
		}

		private void Resolve()
		{
			CinemachineBrain cameraBrain = _cameraModel.CameraBrain;
			if (cameraBrain == null)
			{
				ReleaseBrain();
				return;
			}
			if (cameraBrain != _managedBrain)
			{
				ReleaseBrain();
				_managedBrain = cameraBrain;
				_restoreUpdateMethod = cameraBrain.UpdateMethod;
				cameraBrain.UpdateMethod = CinemachineBrain.UpdateMethods.ManualUpdate;
			}
			for (int i = 0; i < _steps.Count; i++)
			{
				_steps[i].ResolveLate();
			}
			_managedBrain.ManualUpdate();
		}

		private void ReleaseBrain()
		{
			if (_managedBrain == null)
			{
				_managedBrain = null;
				return;
			}
			_managedBrain.UpdateMethod = _restoreUpdateMethod;
			_managedBrain = null;
		}
	}
}
