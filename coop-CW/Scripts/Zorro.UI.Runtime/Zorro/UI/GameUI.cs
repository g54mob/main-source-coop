using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;

namespace Zorro.UI
{
	public class GameUI : Singleton<GameUI>
	{
		public OverlayUIHandler overlayUI;

		private Dictionary<Type, GameUISystem> m_gameUISystems = new Dictionary<Type, GameUISystem>();

		private Dictionary<Type, GameUISystem> m_nonCustomGameUISystems = new Dictionary<Type, GameUISystem>();

		public List<GameUISystem> OpenSystems = new List<GameUISystem>();

		private bool m_shouldShowCursor;

		private CanvasGroup m_canvasGroup;

		private bool m_show = true;

		protected override void Awake()
		{
			base.Awake();
			SetFadeVisibility(show: true);
			m_canvasGroup = GetComponent<CanvasGroup>();
			GameUISystem[] componentsInChildren = GetComponentsInChildren<GameUISystem>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				RegisterSystem(componentsInChildren[i]);
			}
		}

		public static void RegisterSystem(GameUISystem system)
		{
			Type type = system.GetType();
			if (!Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(type))
			{
				Singleton<GameUI>.Instance.m_gameUISystems.Add(type, system);
				if (!type.GetInterfaces().Contains(typeof(IManualUISystemVisability)))
				{
					Singleton<GameUI>.Instance.m_nonCustomGameUISystems.Add(type, system);
				}
			}
			else
			{
				Debug.LogError("Failed to register game UI  system of type: " + type.ToString() + " becuse one is already registed");
			}
		}

		public static void UnregisterSystem(GameUISystem system)
		{
			Type type = system.GetType();
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(type))
			{
				Singleton<GameUI>.Instance.m_gameUISystems.Remove(type);
				if (Singleton<GameUI>.Instance.m_nonCustomGameUISystems.ContainsKey(type))
				{
					Singleton<GameUI>.Instance.m_nonCustomGameUISystems.Remove(type);
				}
			}
			else
			{
				Debug.LogError("Failed to unregister game UI  system of type: " + type.ToString() + " becuse no system with type is registed");
			}
		}

		public static void RunUICode<T>(Action<T> codeAction) where T : GameUISystem
		{
			Type typeFromHandle = typeof(T);
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(typeFromHandle))
			{
				codeAction?.Invoke(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle] as T);
			}
			else
			{
				Debug.LogError("Failed to run UI code on Systems of type: " + typeFromHandle.ToString() + ", no system registered");
			}
		}

		public static bool NeedsCursor()
		{
			if (Singleton<GameUI>.Instance == null)
			{
				return false;
			}
			if (Singleton<GameUI>.Instance.overlayUI == null)
			{
				return Singleton<GameUI>.Instance.m_shouldShowCursor;
			}
			if (!Singleton<GameUI>.Instance.m_shouldShowCursor)
			{
				return Singleton<GameUI>.Instance.overlayUI.IsOpen;
			}
			return true;
		}

		public static void SetFadeVisibility(bool show)
		{
			Singleton<GameUI>.Instance.m_show = show;
		}

		public static void ShowUI<T>() where T : GameUISystem
		{
			ShowUI<T>(null);
		}

		public static void ShowUI<T>(Action<T> codeAction) where T : GameUISystem
		{
			Type typeFromHandle = typeof(T);
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(typeFromHandle))
			{
				if (!Singleton<GameUI>.Instance.OpenSystems.Contains(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle]))
				{
					Debug.Log($"Showing {typeFromHandle} UI");
					Singleton<GameUI>.Instance.OpenSystems.Add(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle]);
					Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle].Show();
					RecalculateShouldShowCursor();
				}
				codeAction?.Invoke(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle] as T);
			}
			else
			{
				Debug.LogError("Failed to show  UI System of type: " + typeFromHandle.ToString() + ", no system registered");
			}
		}

		public static void ShowUI(Type systemType)
		{
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(systemType))
			{
				if (!Singleton<GameUI>.Instance.OpenSystems.Contains(Singleton<GameUI>.Instance.m_gameUISystems[systemType]))
				{
					Debug.Log($"Showing {systemType} UI");
					Singleton<GameUI>.Instance.OpenSystems.Add(Singleton<GameUI>.Instance.m_gameUISystems[systemType]);
					Singleton<GameUI>.Instance.m_gameUISystems[systemType].Show();
					RecalculateShouldShowCursor();
				}
			}
			else
			{
				Debug.LogError("Failed to show  UI System of type: " + systemType.ToString() + ", no system registered");
			}
		}

		public static void Hide(Type type)
		{
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(type))
			{
				if (Singleton<GameUI>.Instance.OpenSystems.Contains(Singleton<GameUI>.Instance.m_gameUISystems[type]))
				{
					Singleton<GameUI>.Instance.OpenSystems.Remove(Singleton<GameUI>.Instance.m_gameUISystems[type]);
					Singleton<GameUI>.Instance.m_gameUISystems[type].Hide();
					RecalculateShouldShowCursor();
				}
			}
			else
			{
				Debug.LogError("Failed to hide  UI System of type: " + type.ToString() + ", no system registered");
			}
		}

		public static void Hide<T>() where T : GameUISystem
		{
			Type typeFromHandle = typeof(T);
			if (Singleton<GameUI>.Instance.m_gameUISystems.ContainsKey(typeFromHandle))
			{
				if (Singleton<GameUI>.Instance.OpenSystems.Contains(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle]))
				{
					Singleton<GameUI>.Instance.OpenSystems.Remove(Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle]);
					Singleton<GameUI>.Instance.m_gameUISystems[typeFromHandle].Hide();
					RecalculateShouldShowCursor();
				}
			}
			else
			{
				Debug.LogError("Failed to hide  UI System of type: " + typeFromHandle.ToString() + ", no system registered");
			}
		}

		private static void RecalculateShouldShowCursor()
		{
			Singleton<GameUI>.Instance.m_shouldShowCursor = Singleton<GameUI>.Instance.OpenSystems.Where((GameUISystem system) => system.NeedsCursor()).ToArray().Length != 0;
		}

		private void Update()
		{
			if (ShouldAttemptOpenMenu() && !overlayUI.IsOpen)
			{
				overlayUI.Open();
			}
			m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, m_show ? 1 : 0, Time.unscaledDeltaTime * 10f);
		}

		protected virtual bool ShouldAttemptOpenMenu()
		{
			return Input.GetKeyDown(KeyCode.Escape);
		}

		public static bool EscapeMenuOpen()
		{
			if (Singleton<GameUI>.Instance != null)
			{
				return Singleton<GameUI>.Instance.overlayUI.IsOpen;
			}
			return false;
		}

		public Dictionary<Type, GameUISystem> GetUISystems()
		{
			return m_nonCustomGameUISystems;
		}
	}
}
