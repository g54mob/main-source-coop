using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public abstract class JoinCrewPopupViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Selectable FirstButtonToSelect { get; private set; }

		[field: SerializeField]
		public Selectable ButtonToSelectOnExit { get; private set; }

		[field: SerializeField]
		public RectTransform AnimationTarget { get; private set; }

		public event Action OnCloseClicked;

		public event Action OnJoinViaCodeClicked;

		public event Action OnJoinViaSteamClicked;

		public event Action<string> OnRoomIdentifierChanged;

		public abstract void SetVisible(bool isVisible);

		public abstract void SetSteamJoinVisible(bool isVisible);

		public abstract void SetJoinViaCodeInteractable(bool interactable);

		public abstract void SetRoomIdentifier(string roomIdentifier);

		protected void InvokeCloseClicked()
		{
			this.OnCloseClicked?.Invoke();
		}

		protected void InvokeJoinViaCodeClicked()
		{
			this.OnJoinViaCodeClicked?.Invoke();
		}

		protected void InvokeJoinViaSteamClicked()
		{
			this.OnJoinViaSteamClicked?.Invoke();
		}

		protected void InvokeRoomIdentifierChanged(string value)
		{
			this.OnRoomIdentifierChanged?.Invoke(value);
		}

		public abstract void ClearInputField();
	}
}
