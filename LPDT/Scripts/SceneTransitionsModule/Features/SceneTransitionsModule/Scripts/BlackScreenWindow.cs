using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	public class BlackScreenWindow : MonoBehaviour
	{
		[SerializeField]
		private GameObject _windowPrefab;

		private CanvasGroup _canvasGroup;

		private bool _hasPendingBridge;

		private LoadingScreenShowType _pendingBridgeShowType;

		public static BlackScreenWindow Instance { get; private set; }

		public bool IsShown
		{
			get
			{
				if (_canvasGroup != null && _canvasGroup.gameObject.activeSelf)
				{
					return _canvasGroup.blocksRaycasts;
				}
				return false;
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			SpawnWindow();
		}

		private void SpawnWindow()
		{
			if (_windowPrefab == null)
			{
				Debug.LogError("BlackScreenWindow: windowPrefab is not assigned!");
				return;
			}
			GameObject gameObject = Object.Instantiate(_windowPrefab);
			Object.DontDestroyOnLoad(gameObject);
			_canvasGroup = gameObject.GetComponent<CanvasGroup>();
			if (_canvasGroup == null)
			{
				Debug.LogError("BlackScreenWindow: windowPrefab root must have CanvasGroup!");
				Object.Destroy(gameObject);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}

		public void RetainAsBridge(LoadingScreenShowType showType)
		{
			_pendingBridgeShowType = showType;
			_hasPendingBridge = true;
			Show();
		}

		public bool TryPeekBridge(out LoadingScreenShowType showType)
		{
			if (!_hasPendingBridge)
			{
				showType = LoadingScreenShowType.ShowFade;
				return false;
			}
			showType = _pendingBridgeShowType;
			return true;
		}

		public bool TryConsumeBridge(out LoadingScreenShowType showType)
		{
			if (!TryPeekBridge(out showType))
			{
				return false;
			}
			_hasPendingBridge = false;
			return true;
		}

		public void Show()
		{
			_canvasGroup.alpha = 1f;
			_canvasGroup.blocksRaycasts = true;
			_canvasGroup.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			_canvasGroup.alpha = 0f;
			_canvasGroup.blocksRaycasts = false;
			_canvasGroup.gameObject.SetActive(value: false);
		}
	}
}
