using System;

namespace Features.MainMenuModule.Scripts
{
	public class MatchmakingPreviewPopupModel
	{
		public bool IsOpen { get; private set; }

		public QuickJoinResult Result { get; private set; }

		public event Action<bool> OnOpenChanged;

		public event Action<QuickJoinResult> OnResultChanged;

		public event Action OnJoinRequested;

		public event Action OnSearchAgainRequested;

		public void SetResult(QuickJoinResult result)
		{
			Result = result;
			this.OnResultChanged?.Invoke(result);
		}

		public void SetOpen(bool isOpen)
		{
			if (IsOpen != isOpen)
			{
				IsOpen = isOpen;
				this.OnOpenChanged?.Invoke(isOpen);
			}
		}

		public void RequestJoin()
		{
			this.OnJoinRequested?.Invoke();
		}

		public void RequestSearchAgain()
		{
			this.OnSearchAgainRequested?.Invoke();
		}
	}
}
