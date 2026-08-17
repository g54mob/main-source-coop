using System;
using System.Collections.Generic;
using System.Linq;
using EvilCore.Extensions;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class GameUIManager : MonoBehaviour, IGameUIManager
	{
		[SerializeField]
		private List<GameCanvasGroup> canvasGroups;

		private ISceneFlowManager _sceneFlowManager;

		private IGameLoadingManager _loadingManager;

		private Canvas _canvas;

		private bool _isLoadingGateOpen;

		private readonly List<GameCanvasGroup> _hiddenForMenu = new List<GameCanvasGroup>();

		public bool IsGameMenuOpen { get; private set; }

		public event Action OnMenuOpened;

		public event Action OnMenuClosed;

		[Inject]
		private void Construct(ISceneFlowManager sceneFlowManager, IGameLoadingManager loadingManager)
		{
			_sceneFlowManager = sceneFlowManager;
			_loadingManager = loadingManager;
		}

		private void Awake()
		{
			InitializeAllCanvasGroups();
			HideAllCanvasGroups();
		}

		private void Start()
		{
			if (_loadingManager != null && !_loadingManager.IsLoadingComplete())
			{
				_loadingManager.OnLoadingComplete.AddListener(OpenLoadingGate);
				_loadingManager.OnTimeout.AddListener(OpenLoadingGate);
			}
			else
			{
				OpenLoadingGate();
			}
		}

		private void OpenLoadingGate()
		{
			_isLoadingGateOpen = true;
			_loadingManager?.OnLoadingComplete.RemoveListener(OpenLoadingGate);
			_loadingManager?.OnTimeout.RemoveListener(OpenLoadingGate);
			SetCanvasVisibilityForGameStart();
		}

		private void OnEnable()
		{
			if (_sceneFlowManager != null)
			{
				_sceneFlowManager.OnGameMenuSceneUnloaded += OnGameMenuSceneClosed;
			}
		}

		private void OnDisable()
		{
			if (_sceneFlowManager != null)
			{
				_sceneFlowManager.OnGameMenuSceneUnloaded -= OnGameMenuSceneClosed;
			}
			_loadingManager?.OnLoadingComplete.RemoveListener(OpenLoadingGate);
			_loadingManager?.OnTimeout.RemoveListener(OpenLoadingGate);
		}

		private void Update()
		{
			if (_isLoadingGateOpen && Input.GetKeyDown(KeyCode.Escape) && !_sceneFlowManager.IsGameMenuSceneLoaded)
			{
				OpenGameMenu();
			}
		}

		private void OpenGameMenu()
		{
			_sceneFlowManager.LoadGameMenuSceneAsync();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			IsGameMenuOpen = true;
			HideActiveHudForMenu();
			this.OnMenuOpened?.Invoke();
		}

		private void OnGameMenuSceneClosed()
		{
			IsGameMenuOpen = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			RestoreHudAfterMenu();
			this.OnMenuClosed?.Invoke();
		}

		private void HideActiveHudForMenu()
		{
			_hiddenForMenu.Clear();
			foreach (GameCanvasGroup canvasGroup in canvasGroups)
			{
				if (canvasGroup != null && canvasGroup.IsVisible)
				{
					_hiddenForMenu.Add(canvasGroup);
					canvasGroup.Hide();
				}
			}
		}

		private void RestoreHudAfterMenu()
		{
			foreach (GameCanvasGroup item in _hiddenForMenu)
			{
				if (item != null)
				{
					item.Show(interactable: false, blockRaycast: false);
				}
			}
			_hiddenForMenu.Clear();
		}

		public void InjectCanvasGroups()
		{
			canvasGroups.Clear();
			canvasGroups = GetComponentsInChildren<GameCanvasGroup>().ToList();
		}

		public Vector3 GetScreenPosition(Vector3 worldPosition)
		{
			return GetComponent<Canvas>().WorldToCanvasPosition(worldPosition);
		}

		private void InitializeAllCanvasGroups()
		{
			canvasGroups = GetComponentsInChildren<GameCanvasGroup>().ToList();
			foreach (GameCanvasGroup canvasGroup in canvasGroups)
			{
				canvasGroup.Initialize();
			}
		}

		public void SetCanvasVisibilityForGameStart()
		{
			ShowCanvasGroup(GameCanvasGroupName.Game, interactable: false, blockRaycast: false);
			ShowCanvasGroup(GameCanvasGroupName.Crosshair, interactable: false, blockRaycast: false);
			ShowCanvasGroup(GameCanvasGroupName.Interaction, interactable: false, blockRaycast: false);
			ShowCanvasGroup(GameCanvasGroupName.InfoMessage, interactable: false, blockRaycast: false);
		}

		public void SetCustomCanvasVisibilitiesForPlayerLeave()
		{
			HideAllCanvasGroups();
		}

		public void ShowCanvasGroup(GameCanvasGroupName name, bool interactable, bool blockRaycast)
		{
			foreach (GameCanvasGroup item in canvasGroups.Where((GameCanvasGroup cg) => cg.GameCanvasGroupName == name))
			{
				item.Show(interactable, blockRaycast);
			}
		}

		public void ShowAllCanvasGroups()
		{
			foreach (GameCanvasGroup item in canvasGroups.Where((GameCanvasGroup cg) => !cg.IsVisible))
			{
				item.Show(interactable: false, blockRaycast: false);
			}
		}

		public void HideCanvasGroup(GameCanvasGroupName name)
		{
			foreach (GameCanvasGroup item in canvasGroups.Where((GameCanvasGroup cg) => cg.GameCanvasGroupName == name))
			{
				item.Hide();
			}
		}

		private void HideAllCanvasGroups()
		{
			foreach (GameCanvasGroup item in canvasGroups.Where((GameCanvasGroup cg) => cg.IsVisible))
			{
				item.Hide();
			}
		}
	}
}
