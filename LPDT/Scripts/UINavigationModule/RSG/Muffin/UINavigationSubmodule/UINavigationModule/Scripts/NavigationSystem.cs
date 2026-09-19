using System;
using Features.GameUpdaterModule;
using UnityEngine.EventSystems;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts
{
	public class NavigationSystem : IInitializable, IDisposable
	{
		private readonly NavigationModel _navigationModel;

		private readonly IGameUpdater _gameUpdater;

		public NavigationSystem(NavigationModel navigationModel, IGameUpdater gameUpdater)
		{
			_navigationModel = navigationModel;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_navigationModel.EventSystem = EventSystem.current;
			_gameUpdater.OnUpdate += HandleSelectionQuery;
			_gameUpdater.OnUpdate += SetSelectedObjectToModel;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= HandleSelectionQuery;
			_gameUpdater.OnUpdate -= SetSelectedObjectToModel;
		}

		private void HandleSelectionQuery()
		{
			if (_navigationModel.NavigationQuery.Count != 0 && !_navigationModel.EventSystem.alreadySelecting)
			{
				_navigationModel.EventSystem.SetSelectedGameObject(_navigationModel.NavigationQuery[0]);
				_navigationModel.NavigationQuery.RemoveAt(0);
			}
		}

		private void SetSelectedObjectToModel()
		{
			if (!(EventSystem.current == null) && !(EventSystem.current.currentSelectedGameObject == null))
			{
				_navigationModel.CurrentSelectedObject = EventSystem.current.currentSelectedGameObject;
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					_navigationModel.LastSelectedObject = EventSystem.current.currentSelectedGameObject;
				}
			}
		}
	}
}
