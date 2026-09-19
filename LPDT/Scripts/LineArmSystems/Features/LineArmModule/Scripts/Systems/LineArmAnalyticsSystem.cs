using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.LineArmModule.Scripts.Systems
{
	public class LineArmAnalyticsSystem : IInitializable, IDisposable
	{
		private const LineArmType TARGET_ARM_TYPE = LineArmType.RightArmDefault;

		private const bool SEND_ONLY_FIST_TIME = true;

		private readonly GameAnalyticsEventSendService _gameAnalytics;

		private readonly LineArmsModel _lineArmsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private int _eventCount;

		private LineArmControllerBase _localLineArmController;

		public LineArmAnalyticsSystem(LineArmsModel lineArmsModel, MultiplayerModel multiplayerModel, GameAnalyticsEventSendService gameAnalytics)
		{
			_lineArmsModel = lineArmsModel;
			_multiplayerModel = multiplayerModel;
			_gameAnalytics = gameAnalytics;
		}

		public void Dispose()
		{
			LineArmsModel lineArmsModel = _lineArmsModel;
			lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Remove(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(OnLineArmPlayerRegistered));
			RemoveLocalLineArmListeners();
		}

		public void Initialize()
		{
			LineArmsModel lineArmsModel = _lineArmsModel;
			lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Combine(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(OnLineArmPlayerRegistered));
			FindLocalLineArm();
		}

		private void FindLocalLineArm()
		{
			if (_lineArmsModel.TryGetLineArmForPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var lineArm))
			{
				_localLineArmController = lineArm;
				AddLocalLineArmListeners();
			}
		}

		private void OnLineArmPlayerRegistered(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				_localLineArmController = _lineArmsModel.GetLineArmForPlayer(playerId, LineArmType.RightArmDefault);
				if (!(_localLineArmController == null))
				{
					AddLocalLineArmListeners();
				}
			}
		}

		private void AddLocalLineArmListeners()
		{
			LineArmControllerBase localLineArmController = _localLineArmController;
			localLineArmController.OnGrabbedChanged = (Action)Delegate.Combine(localLineArmController.OnGrabbedChanged, new Action(OnGrabbedChanged));
		}

		private void RemoveLocalLineArmListeners()
		{
			if (_localLineArmController != null)
			{
				LineArmControllerBase localLineArmController = _localLineArmController;
				localLineArmController.OnGrabbedChanged = (Action)Delegate.Remove(localLineArmController.OnGrabbedChanged, new Action(OnGrabbedChanged));
			}
		}

		private void OnGrabbedChanged()
		{
			if (_eventCount <= 0 && _localLineArmController.CurrentGrabbables.Count > 0)
			{
				IPointGrabable pointGrabable = _localLineArmController.CurrentGrabbables[0];
				if (pointGrabable != null && pointGrabable.NetworkObject.TryGetComponent<MonoItem>(out var component) && component.CurrencyValue > 1)
				{
					_gameAnalytics.TrackItemPickedUp();
					_eventCount++;
				}
			}
		}
	}
}
