using System;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearSelfRemovalSystem : ITickable, IDisposable
	{
		private const float LOOK_UP_DOT = 0.7f;

		private readonly CameraModel _cameraModel;

		private readonly HeadwearModel _headwearModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly InteractModel _interactModel;

		private IPointGrabable _forcedTarget;

		public HeadwearSelfRemovalSystem(CameraModel cameraModel, HeadwearModel headwearModel, MultiplayerModel multiplayerModel, InteractModel interactModel)
		{
			_cameraModel = cameraModel;
			_headwearModel = headwearModel;
			_multiplayerModel = multiplayerModel;
			_interactModel = interactModel;
		}

		public void Tick()
		{
			IPointGrabable pointGrabable = ResolveRemovableHeadwear();
			if (pointGrabable != _forcedTarget)
			{
				if (_forcedTarget != null && _interactModel.ForcedFallbackTarget == _forcedTarget)
				{
					_interactModel.ForcedFallbackTarget = null;
				}
				_forcedTarget = pointGrabable;
				if (_forcedTarget != null)
				{
					_interactModel.ForcedFallbackTarget = _forcedTarget;
				}
			}
		}

		public void Dispose()
		{
			if (_forcedTarget != null && _interactModel.ForcedFallbackTarget == _forcedTarget)
			{
				_interactModel.ForcedFallbackTarget = null;
			}
			_forcedTarget = null;
		}

		private IPointGrabable ResolveRemovableHeadwear()
		{
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject == null)
			{
				return null;
			}
			if (Vector3.Dot(cameraObject.transform.forward, Vector3.up) < 0.7f)
			{
				return null;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_headwearModel.TryGetHeadwear(playerId, out var headwear))
			{
				return null;
			}
			if (headwear.IsWearerUnderCover)
			{
				return null;
			}
			return headwear.Grabable;
		}
	}
}
