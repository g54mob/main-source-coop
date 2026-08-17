using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.Localization;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class LoadingGamePanel : MonoBehaviour
	{
		[Header("State Text")]
		[SerializeField]
		private TextMeshProUGUI stateText;

		[SerializeField]
		private float dotAnimationSpeed = 0.5f;

		[SerializeField]
		private int maxDots = 3;

		[Header("Tips")]
		[SerializeField]
		private bool enableTips = true;

		[SerializeField]
		private TextMeshProUGUI tipsText;

		[SerializeField]
		private float tipDisplayDuration = 5f;

		[Tooltip("Localization keys (@tips.*). Add a new tip in the Tips table + LK, then pick it here.")]
		[LocalizationKey("tips")]
		[SerializeField]
		private List<string> tipKeys = new List<string>();

		[Header("Loading Icon")]
		[SerializeField]
		private bool enableLoadingIcon = true;

		[SerializeField]
		private RectTransform loadingIcon;

		[SerializeField]
		private float rotationSpeed = 200f;

		[Inject]
		private IGameLoadingManager _loadingManager;

		[Inject]
		private ISceneFlowManager _sceneFlowManager;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private ILocalizationService _localizationService;

		private string _baseText = "";

		private string _currentStateKey = "";

		private string _currentTipKey = "";

		private int _currentDotCount;

		private Coroutine _dotAnimationCoroutine;

		private Coroutine _tipsCoroutine;

		private void Start()
		{
			if (_loadingManager != null)
			{
				_loadingManager.OnStateChanged.AddListener(OnStateChanged);
				_loadingManager.OnLoadingComplete.AddListener(OnLoadingComplete);
				_loadingManager.OnTimeout.AddListener(OnTimeout);
			}
			if (_lobbyManager != null)
			{
				_lobbyManager.OnLobbyLeft += OnLobbyLeft;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += OnLocaleChanged;
			}
		}

		private void OnDestroy()
		{
			if (_loadingManager != null)
			{
				_loadingManager.OnStateChanged.RemoveListener(OnStateChanged);
				_loadingManager.OnLoadingComplete.RemoveListener(OnLoadingComplete);
				_loadingManager.OnTimeout.RemoveListener(OnTimeout);
			}
			if (_lobbyManager != null)
			{
				_lobbyManager.OnLobbyLeft -= OnLobbyLeft;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= OnLocaleChanged;
			}
			StopAllAnimations();
		}

		private void Update()
		{
			if (enableLoadingIcon && loadingIcon != null)
			{
				loadingIcon.Rotate(0f, 0f, (0f - rotationSpeed) * Time.deltaTime);
			}
		}

		public void OnPanelShown()
		{
			_loadingManager?.Reset();
			_currentStateKey = "@loading.initializing";
			_baseText = Localize(_currentStateKey);
			_currentDotCount = 0;
			UpdateStateText();
			if (stateText != null)
			{
				stateText.color = Color.white;
			}
			StartTipsCoroutine();
			StartDotAnimation();
		}

		private void OnStateChanged(int newState)
		{
			_currentStateKey = _loadingManager.GetStateDescription(newState);
			_baseText = Localize(_currentStateKey);
			if (_dotAnimationCoroutine != null)
			{
				StopCoroutine(_dotAnimationCoroutine);
			}
			if (newState != 100)
			{
				_currentDotCount = 0;
				StartDotAnimation();
			}
			else
			{
				UpdateStateText();
			}
		}

		private void OnLoadingComplete()
		{
			StopAllAnimations();
			_sceneFlowManager?.UnloadMainMenuSceneAsync().Forget();
		}

		private void OnTimeout()
		{
			StopAllAnimations();
			_currentStateKey = "@loading.timeout";
			_baseText = Localize(_currentStateKey);
			if (stateText != null)
			{
				stateText.text = _baseText;
				stateText.color = Color.red;
			}
		}

		private void OnLobbyLeft()
		{
			if (_loadingManager == null || !_loadingManager.IsLoadingComplete())
			{
				StopAllAnimations();
				_uiManager?.ShowMainPanel();
			}
		}

		private void StartDotAnimation()
		{
			_dotAnimationCoroutine = StartCoroutine(DotAnimationCoroutine());
		}

		private void StartTipsCoroutine()
		{
			if (_tipsCoroutine != null)
			{
				StopCoroutine(_tipsCoroutine);
			}
			if (enableTips && tipKeys.Count > 0 && tipsText != null)
			{
				_tipsCoroutine = StartCoroutine(TipsCoroutine());
			}
		}

		private void StopAllAnimations()
		{
			if (_dotAnimationCoroutine != null)
			{
				StopCoroutine(_dotAnimationCoroutine);
				_dotAnimationCoroutine = null;
			}
			if (_tipsCoroutine != null)
			{
				StopCoroutine(_tipsCoroutine);
				_tipsCoroutine = null;
			}
		}

		private IEnumerator DotAnimationCoroutine()
		{
			while (true)
			{
				UpdateStateText();
				_currentDotCount = (_currentDotCount + 1) % (maxDots + 1);
				yield return new WaitForSeconds(dotAnimationSpeed);
			}
		}

		private IEnumerator TipsCoroutine()
		{
			int index = 0;
			while (true)
			{
				_currentTipKey = tipKeys[index];
				tipsText.text = Localize(_currentTipKey);
				index = (index + 1) % tipKeys.Count;
				yield return new WaitForSeconds(tipDisplayDuration);
			}
		}

		private void UpdateStateText()
		{
			if (!(stateText == null))
			{
				string text = new string('.', _currentDotCount);
				stateText.text = _baseText + text;
			}
		}

		private void OnLocaleChanged()
		{
			if (!string.IsNullOrEmpty(_currentStateKey))
			{
				_baseText = Localize(_currentStateKey);
				UpdateStateText();
			}
			if (!string.IsNullOrEmpty(_currentTipKey) && tipsText != null)
			{
				tipsText.text = Localize(_currentTipKey);
			}
		}

		private string Localize(string key)
		{
			if (_localizationService == null)
			{
				return key;
			}
			return _localizationService.Localize(key);
		}
	}
}
