using Features.DeviceModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts
{
	public class NavigationService : INavigationService
	{
		private readonly NavigationModel _navigationModel;

		private readonly IDeviceService _deviceService;

		public NavigationService(NavigationModel navigationModel, IDeviceService deviceService)
		{
			_navigationModel = navigationModel;
			_deviceService = deviceService;
		}

		public void SetNavigationToObject(Selectable selectable)
		{
			SetSelectedGameObject(selectable.gameObject);
		}

		public void RemoveNavigationFromAnyObject()
		{
			SetSelectedGameObject(null);
		}

		public void SetNavigationCurrentInputModuleEnabled(bool isEnabled)
		{
			EventSystem.current.currentInputModule.enabled = isEnabled;
		}

		public void SetLeftNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnLeft = toSelectable;
			fromSelectable.navigation = navigation;
		}

		public void SetRightNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnRight = toSelectable;
			fromSelectable.navigation = navigation;
		}

		public void SetUpNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnUp = toSelectable;
			fromSelectable.navigation = navigation;
		}

		public void SetDownNavigation(Selectable fromSelectable, Selectable toSelectable)
		{
			Navigation navigation = fromSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = toSelectable;
			fromSelectable.navigation = navigation;
		}

		public Selectable GetCurrentSelectedObject()
		{
			if (!(_navigationModel.CurrentSelectedObject != null))
			{
				return null;
			}
			return _navigationModel.CurrentSelectedObject.GetComponent<Selectable>();
		}

		public Selectable GetLastSelectedObject()
		{
			if (!(_navigationModel.LastSelectedObject != null))
			{
				return null;
			}
			return _navigationModel.LastSelectedObject.GetComponent<Selectable>();
		}

		public bool IsCurrentSelectedObjectActive()
		{
			if (_navigationModel.CurrentSelectedObject != null)
			{
				return _navigationModel.CurrentSelectedObject.activeSelf;
			}
			return false;
		}

		private void SetSelectedGameObject(GameObject gameObject)
		{
			AddSelectableToQuery(gameObject);
		}

		private void AddSelectableToQuery(GameObject gameObject)
		{
			if (_navigationModel.NavigationQuery.Count == 0)
			{
				_navigationModel.CurrentSelectedObject = gameObject;
			}
			_navigationModel.NavigationQuery.Add(gameObject);
		}

		public void SetNavigationActive(bool isActive)
		{
			_navigationModel.EventSystem.enabled = isActive;
		}

		public bool IsNavigationActive()
		{
			return _navigationModel.EventSystem.enabled;
		}
	}
}
