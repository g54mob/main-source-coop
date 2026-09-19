using Features.PlayerItemViewModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayerStatesModule.Scripts.Views
{
	public abstract class SpectatorViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _container;

		public Animator LeftArrowAnimator;

		public Animator RightArrowAnimator;

		public TMP_Text Nickname;

		[field: SerializeField]
		public LeftRightButtonHolder LeftRightButtonHolder { get; private set; }

		[field: SerializeField]
		public PlayerItemViewBase PlayerItemViewBase { get; private set; }

		[field: SerializeField]
		public Selectable FirstButtonToSelect { get; private set; }

		public Transform GetPlayerItemsContainer()
		{
			return _container;
		}

		public abstract void StartFade();

		public abstract void DisableFade();

		public abstract void ClearFade();
	}
}
