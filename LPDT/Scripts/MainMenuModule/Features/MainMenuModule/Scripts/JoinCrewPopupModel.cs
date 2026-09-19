using System;

namespace Features.MainMenuModule.Scripts
{
	public class JoinCrewPopupModel
	{
		public bool IsOpen { get; private set; }

		public bool JoinViaCodeInteractable { get; private set; }

		public event Action<bool> OnOpenChanged;

		public event Action<bool> OnJoinViaCodeInteractableChanged;

		public event Action OnJoinViaCodeRequested;

		public event Action<string> OnRoomIdentifierChanged;

		public event Action<string> OnRoomIdentifierDisplayChanged;

		public void SetOpen(bool isOpen)
		{
			if (IsOpen != isOpen)
			{
				IsOpen = isOpen;
				this.OnOpenChanged?.Invoke(IsOpen);
			}
		}

		public void SetJoinViaCodeInteractable(bool interactable)
		{
			if (JoinViaCodeInteractable != interactable)
			{
				JoinViaCodeInteractable = interactable;
				this.OnJoinViaCodeInteractableChanged?.Invoke(interactable);
			}
		}

		public void RequestJoinViaCode()
		{
			this.OnJoinViaCodeRequested?.Invoke();
		}

		public void NotifyRoomIdentifierChanged(string roomIdentifier)
		{
			this.OnRoomIdentifierChanged?.Invoke(roomIdentifier);
		}

		public void SetRoomIdentifierDisplay(string roomIdentifier)
		{
			this.OnRoomIdentifierDisplayChanged?.Invoke(roomIdentifier);
		}
	}
}
