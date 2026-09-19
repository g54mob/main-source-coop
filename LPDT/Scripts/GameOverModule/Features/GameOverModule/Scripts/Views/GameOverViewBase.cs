using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameOverModule.Scripts.Views
{
	public abstract class GameOverViewBase : ViewBehaviour, IFocusableElement
	{
		[field: SerializeField]
		public float ReturnToLobbyDelay { get; private set; } = 5f;

		[field: SerializeField]
		public Selectable FirstButtonToSelect { get; private set; }

		public bool IsFocused { get; private set; }

		public bool IsFocusable { get; private set; }

		public event Action OnFocused;

		public event Action OnUnFocused;

		public void Focus(Action onInteract)
		{
			IsFocused = true;
		}

		public void UnFocus()
		{
			IsFocused = false;
		}

		public void MakeFocusable()
		{
			IsFocusable = true;
			this.OnFocused?.Invoke();
		}

		public void MakeUnFocusable()
		{
			IsFocusable = false;
			this.OnUnFocused?.Invoke();
		}
	}
}
