using System;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	[Serializable]
	public class LoadingScreenPreset
	{
		[SerializeField]
		private LoadingScreenShowType _showType;

		[SerializeField]
		private bool _isAvailable = true;

		[SerializeField]
		private float _fadeInDuration;

		[SerializeField]
		private float _fadeOutDuration;

		[SerializeField]
		private float _minDisplayDuration;

		[SerializeField]
		private float _targetAlpha = 1f;

		[SerializeField]
		private bool _useUnscaledTime = true;

		[Header("Content")]
		[SerializeField]
		private LoadingScreenScreenType _screenType;

		[Header("Behaviour")]
		[Tooltip("Spawn screen prefab (ScreensPresenter) and reveal it after fade-in.")]
		[SerializeField]
		private bool _hasScreenContent;

		[Tooltip("Session loading overlay: tracks IsContentLoadingActive, allows replacement by another content-loading Show, and uses HideAsync instead of FadeOutAsync on dismiss.")]
		[SerializeField]
		private bool _isContentLoadingMode;

		[Tooltip("Show BlackScreenWindow at full opacity while this preset is visible (fade-only and fade+content modes).")]
		[SerializeField]
		private bool _usesBlackScreenWindow;

		[Tooltip("While another show type is active (or held by the black-screen bridge), this preset may replace it only if the active type is listed here.")]
		[SerializeField]
		private LoadingScreenShowType[] _overrideActiveShowTypes = Array.Empty<LoadingScreenShowType>();

		public LoadingScreenShowType ShowType => _showType;

		public bool IsAvailable => _isAvailable;

		public float FadeInDuration => _fadeInDuration;

		public float FadeOutDuration => _fadeOutDuration;

		public float MinDisplayDuration => _minDisplayDuration;

		public float TargetAlpha => _targetAlpha;

		public bool UseUnscaledTime => _useUnscaledTime;

		public LoadingScreenScreenType ScreenType => _screenType;

		public bool HasScreenContent => _hasScreenContent;

		public bool IsContentLoadingMode => _isContentLoadingMode;

		public bool UsesBlackScreenWindow => _usesBlackScreenWindow;

		public LoadingScreenShowType[] OverrideActiveShowTypes => _overrideActiveShowTypes;
	}
}
