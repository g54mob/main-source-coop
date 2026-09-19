using UnityEngine.UI;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts
{
	public interface INavigationService
	{
		void SetNavigationToObject(Selectable selectable);

		void RemoveNavigationFromAnyObject();

		void SetNavigationCurrentInputModuleEnabled(bool isEnabled);

		void SetLeftNavigation(Selectable fromSelectable, Selectable toSelectable);

		void SetRightNavigation(Selectable fromSelectable, Selectable toSelectable);

		void SetUpNavigation(Selectable fromSelectable, Selectable toSelectable);

		void SetDownNavigation(Selectable fromSelectable, Selectable toSelectable);

		Selectable GetCurrentSelectedObject();

		Selectable GetLastSelectedObject();

		bool IsCurrentSelectedObjectActive();

		void SetNavigationActive(bool isActive);

		bool IsNavigationActive();
	}
}
