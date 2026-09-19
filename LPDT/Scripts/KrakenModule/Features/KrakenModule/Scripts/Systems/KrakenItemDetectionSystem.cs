using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenItemDetectionSystem : IInitializable, IDisposable, ITickable
	{
		private readonly KrakenRuntimeModel _runtimeModel;

		private readonly KrakenAppearTriggerWindowModel _triggerWindowModel;

		private readonly KrakenBehaviourConfiguration _configuration;

		private readonly IKrakenThrowAssignmentService _assignmentService;

		private readonly List<IPointGrabable> _itemsSnapshot = new List<IPointGrabable>();

		private KrakenController _controller;

		public KrakenItemDetectionSystem(KrakenRuntimeModel runtimeModel, KrakenAppearTriggerWindowModel triggerWindowModel, KrakenBehaviourConfiguration configuration, IKrakenThrowAssignmentService assignmentService)
		{
			_runtimeModel = runtimeModel;
			_triggerWindowModel = triggerWindowModel;
			_configuration = configuration;
			_assignmentService = assignmentService;
		}

		public void Initialize()
		{
			_runtimeModel.OnControllerChanged += OnControllerChanged;
			if (_runtimeModel.TryGetController(out var controller))
			{
				OnControllerChanged(controller);
			}
		}

		public void Dispose()
		{
			_runtimeModel.OnControllerChanged -= OnControllerChanged;
			Unsubscribe();
		}

		private void OnControllerChanged(KrakenController controller)
		{
			Unsubscribe();
			_controller = controller;
			if (_controller?.ItemDetector != null)
			{
				_controller.ItemDetector.OnGrabbableDetected += OnGrabbableDetected;
			}
		}

		private void Unsubscribe()
		{
			if (_controller?.ItemDetector != null)
			{
				_controller.ItemDetector.OnGrabbableDetected -= OnGrabbableDetected;
			}
			_controller = null;
		}

		private void OnGrabbableDetected(IPointGrabable pointGrabable)
		{
			if (!(_controller == null) && _controller.HasStateAuthority && _assignmentService.IsGrabbableValid(pointGrabable))
			{
				float time = Time.time;
				_triggerWindowModel.RecordItemTriggered(time, _configuration.ItemAppearTriggerWindowSeconds);
				if (!_controller.IsVisible && _triggerWindowModel.GetItemTriggerCount(time, _configuration.ItemAppearTriggerWindowSeconds) >= _configuration.ItemAppearTriggerCount)
				{
					_controller.RequestAppear(KrakenAppearReason.ItemTriggerWindow);
				}
				else
				{
					_controller.ProcessItemDetected(pointGrabable);
				}
			}
		}

		public void Tick()
		{
			if (_controller == null || !_controller.HasStateAuthority || !_controller.IsVisible || !_controller.CanAcceptItemThrow || _controller.ItemDetector == null)
			{
				return;
			}
			_controller.ItemDetector.PruneInvalidItems();
			_itemsSnapshot.Clear();
			foreach (IPointGrabable item in _controller.ItemDetector.ItemsInZone)
			{
				_itemsSnapshot.Add(item);
			}
			foreach (IPointGrabable item2 in _itemsSnapshot)
			{
				if (!_controller.CanAcceptItemThrow)
				{
					break;
				}
				if (_assignmentService.IsGrabbableValid(item2))
				{
					_controller.ProcessItemDetected(item2);
				}
			}
		}
	}
}
