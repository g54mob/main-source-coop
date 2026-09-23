using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffectInternal
{
	internal static class UIExtraCallbacks
	{
		private static bool s_IsInitializedAfterCanvasRebuild;

		private static readonly FastAction s_AfterCanvasRebuildAction = new FastAction();

		private static readonly FastAction s_LateAfterCanvasRebuildAction = new FastAction();

		private static readonly FastAction s_BeforeCanvasRebuildAction = new FastAction();

		private static readonly FastAction s_OnScreenSizeChangedAction = new FastAction();

		private static Vector2Int s_LastScreenSize;

		public static event Action onLateAfterCanvasRebuild
		{
			add
			{
				s_LateAfterCanvasRebuildAction.Add(value);
			}
			remove
			{
				s_LateAfterCanvasRebuildAction.Remove(value);
			}
		}

		public static event Action onBeforeCanvasRebuild
		{
			add
			{
				s_BeforeCanvasRebuildAction.Add(value);
			}
			remove
			{
				s_BeforeCanvasRebuildAction.Remove(value);
			}
		}

		public static event Action onAfterCanvasRebuild
		{
			add
			{
				s_AfterCanvasRebuildAction.Add(value);
			}
			remove
			{
				s_AfterCanvasRebuildAction.Remove(value);
			}
		}

		public static event Action onScreenSizeChanged
		{
			add
			{
				s_OnScreenSizeChangedAction.Add(value);
			}
			remove
			{
				s_OnScreenSizeChangedAction.Remove(value);
			}
		}

		private static void InitializeAfterCanvasRebuild()
		{
			if (!s_IsInitializedAfterCanvasRebuild)
			{
				s_IsInitializedAfterCanvasRebuild = true;
				CanvasUpdateRegistry.IsRebuildingLayout();
				typeof(TMP_UpdateManager).GetProperty("instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
				Canvas.willRenderCanvases -= OnAfterCanvasRebuild;
				Canvas.willRenderCanvases += OnAfterCanvasRebuild;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad()
		{
			Canvas.preWillRenderCanvases -= OnBeforeCanvasRebuild;
			Canvas.preWillRenderCanvases += OnBeforeCanvasRebuild;
			Canvas.willRenderCanvases -= OnAfterCanvasRebuild;
			s_IsInitializedAfterCanvasRebuild = false;
			s_LastScreenSize = default(Vector2Int);
		}

		private static void OnBeforeCanvasRebuild()
		{
			Vector2Int vector2Int = new Vector2Int(Screen.width, Screen.height);
			if (s_LastScreenSize != vector2Int)
			{
				if (s_LastScreenSize != default(Vector2Int))
				{
					s_OnScreenSizeChangedAction.Invoke();
				}
				s_LastScreenSize = vector2Int;
			}
			s_BeforeCanvasRebuildAction.Invoke();
			InitializeAfterCanvasRebuild();
		}

		private static void OnAfterCanvasRebuild()
		{
			s_AfterCanvasRebuildAction.Invoke();
			s_LateAfterCanvasRebuildAction.Invoke();
		}
	}
}
