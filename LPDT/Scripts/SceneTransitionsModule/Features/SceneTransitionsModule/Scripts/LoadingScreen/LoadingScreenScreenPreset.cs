using System;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	[Serializable]
	public class LoadingScreenScreenPreset
	{
		[SerializeField]
		private LoadingScreenScreenType _screenType;

		[SerializeField]
		private GameObject _prefab;

		[Header("Tips")]
		[SerializeField]
		private bool _isTipsEnabled;

		[SerializeField]
		private bool _isTipsSequence;

		[SerializeField]
		private bool _isAnimatedLoadingTip;

		[SerializeField]
		private LoadingScreenTipEntry[] _entries = Array.Empty<LoadingScreenTipEntry>();

		public LoadingScreenScreenType ScreenType => _screenType;

		public GameObject Prefab => _prefab;

		public bool IsTipsEnabled => _isTipsEnabled;

		public bool IsTipsSequence => _isTipsSequence;

		public bool IsAnimatedLoadingTip => _isAnimatedLoadingTip;

		public LoadingScreenTipEntry[] Entries => _entries ?? Array.Empty<LoadingScreenTipEntry>();
	}
}
