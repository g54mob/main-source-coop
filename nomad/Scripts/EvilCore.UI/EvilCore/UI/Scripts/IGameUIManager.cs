using System;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public interface IGameUIManager
	{
		bool IsGameMenuOpen { get; }

		event Action OnMenuOpened;

		event Action OnMenuClosed;

		void SetCanvasVisibilityForGameStart();

		void SetCustomCanvasVisibilitiesForPlayerLeave();

		void ShowCanvasGroup(GameCanvasGroupName name, bool interactable, bool blockRaycast);

		void HideCanvasGroup(GameCanvasGroupName name);

		Vector3 GetScreenPosition(Vector3 worldPosition);
	}
}
