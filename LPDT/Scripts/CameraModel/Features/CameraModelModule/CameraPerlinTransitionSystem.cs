using System;
using System.Collections;
using Features.CoroutineUtils.Scripts;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.CameraModelModule
{
	public class CameraPerlinTransitionSystem : IInitializable, IDisposable
	{
		private readonly CameraModel _cameraModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private Coroutine _perlinTransitionRoutine;

		public CameraPerlinTransitionSystem(CameraModel cameraModel, ICoroutineRunner coroutineRunner)
		{
			_cameraModel = cameraModel;
			_coroutineRunner = coroutineRunner;
		}

		public void Initialize()
		{
			_cameraModel.OnCameraTransitionEnqueued += ProcessCameraTransition;
		}

		public void Dispose()
		{
			_cameraModel.OnCameraTransitionEnqueued -= ProcessCameraTransition;
			if (_perlinTransitionRoutine != null)
			{
				_coroutineRunner.StopCoroutine(_perlinTransitionRoutine);
				_perlinTransitionRoutine = null;
			}
			_cameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.CameraTransition);
		}

		private void ProcessCameraTransition(CameraType currentType, CameraType transitedType)
		{
			if (currentType != transitedType)
			{
				if (_perlinTransitionRoutine != null)
				{
					_coroutineRunner.StopCoroutine(_perlinTransitionRoutine);
				}
				_cameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.CameraTransition);
				_perlinTransitionRoutine = _coroutineRunner.StartCoroutine(WaitForBlendFinishedRoutine(currentType, transitedType));
			}
		}

		private IEnumerator WaitForBlendFinishedRoutine(CameraType fromType, CameraType toType)
		{
			try
			{
				CinemachineBrain brain = _cameraModel.CameraBrain;
				if (!(brain == null))
				{
					yield return new WaitForEndOfFrame();
					float blendTime = GetTransitionBlendTime(brain, fromType, toType);
					float timer = 0f;
					while (brain.IsBlending || timer < blendTime)
					{
						timer += Time.deltaTime;
						yield return null;
					}
				}
			}
			finally
			{
				CameraPerlinTransitionSystem cameraPerlinTransitionSystem = this;
				cameraPerlinTransitionSystem._perlinTransitionRoutine = null;
				cameraPerlinTransitionSystem._cameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.CameraTransition);
			}
		}

		private float GetTransitionBlendTime(CinemachineBrain brain, CameraType fromType, CameraType toType)
		{
			CinemachineBlend activeBlend = brain.ActiveBlend;
			if (activeBlend != null && activeBlend.IsValid)
			{
				return activeBlend.Duration;
			}
			if (brain.CustomBlends != null && _cameraModel.Cameras.TryGetValue(fromType, out var value) && _cameraModel.Cameras.TryGetValue(toType, out var value2))
			{
				return brain.CustomBlends.GetBlendForVirtualCameras(value.name, value2.name, brain.DefaultBlend).BlendTime;
			}
			return brain.DefaultBlend.BlendTime;
		}
	}
}
