using System;
using Features.MultiplayerSessionServices.Scripts;
using NetworkServices.ObjectsProvider;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public class SessionCleanupSystem : IInitializable, IDisposable
	{
		private readonly ISessionCleanup[] _sessionCleanups;

		private readonly ContextDependentCleanupModel _contextDependentCleanupModel;

		private readonly ISessionCleanupWindow[] _sessionCleanupWindows;

		private readonly SessionCleanupEvent _sessionCleanupEvent;

		private readonly IWindowsService _windowsService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SessionCleanUpStartedEvent _sessionCleanUpStartedEvent;

		public SessionCleanupSystem(ISessionCleanup[] sessionCleanups, ContextDependentCleanupModel contextDependentCleanupModel, ISessionCleanupWindow[] sessionCleanupWindows, SessionCleanupEvent sessionCleanupEvent, IWindowsService windowsService, MultiplayerModel multiplayerModel, SessionCleanUpStartedEvent sessionCleanUpStartedEvent)
		{
			_sessionCleanups = sessionCleanups;
			_contextDependentCleanupModel = contextDependentCleanupModel;
			_sessionCleanupWindows = sessionCleanupWindows;
			_sessionCleanupEvent = sessionCleanupEvent;
			_windowsService = windowsService;
			_multiplayerModel = multiplayerModel;
			_sessionCleanUpStartedEvent = sessionCleanUpStartedEvent;
		}

		public void Initialize()
		{
			_sessionCleanupEvent.OnSessionCleanup += SessionCleanup;
		}

		public void Dispose()
		{
			_sessionCleanupEvent.OnSessionCleanup -= SessionCleanup;
		}

		private void SessionCleanup()
		{
			_sessionCleanUpStartedEvent.Publish();
			CleanupObjectProvider();
			DefaultSessionCleanup();
			SessionWindowsCleanup();
		}

		private void CleanupObjectProvider()
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				PoolObjectProvider poolObjectProvider = _multiplayerModel.NetworkRunner.ObjectProvider as PoolObjectProvider;
				if ((UnityEngine.Object)(object)poolObjectProvider != null)
				{
					poolObjectProvider.Cleanup();
				}
			}
		}

		private void DefaultSessionCleanup()
		{
			foreach (ContextDependentCleanupBase contextCleanup in _contextDependentCleanupModel.ContextCleanups)
			{
				contextCleanup.Cleanup();
			}
			ISessionCleanup[] sessionCleanups = _sessionCleanups;
			for (int i = 0; i < sessionCleanups.Length; i++)
			{
				sessionCleanups[i].Cleanup();
			}
		}

		private void SessionWindowsCleanup()
		{
			ISessionCleanupWindow[] sessionCleanupWindows = _sessionCleanupWindows;
			foreach (ISessionCleanupWindow sessionCleanupWindow in sessionCleanupWindows)
			{
				CloseIfWindowIsOpen(sessionCleanupWindow.WindowBehaviour);
			}
		}

		private void CloseIfWindowIsOpen(WindowBehaviour windowBehaviour)
		{
			Type type = windowBehaviour.GetType();
			if (_windowsService.GetWindow(type) != null && _windowsService.GetWindow(type).WindowStatus != WindowStatus.Closed)
			{
				_windowsService.CloseWindow(type);
			}
		}
	}
}
