using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CW.Common
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwDemoButton")]
	[AddComponentMenu("Common/CW Demo Button")]
	public class CwDemoButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		public enum LinkType
		{
			PreviousScene = 0,
			NextScene = 1,
			Publisher = 2,
			URL = 3,
			Isolate = 4
		}

		public enum ToggleType
		{
			KeepSelected = 0,
			ToggleSelection = 1,
			SelectPrevious = 2
		}

		[SerializeField]
		private LinkType link;

		[SerializeField]
		private string urlTarget;

		[SerializeField]
		private Transform isolateTarget;

		[SerializeField]
		private ToggleType isolateToggle;

		[NonSerialized]
		private CanvasGroup cachedCanvasGroup;

		[NonSerialized]
		private Transform previousChild;

		public LinkType Link
		{
			get
			{
				return link;
			}
			set
			{
				link = value;
			}
		}

		public string UrlTarget
		{
			get
			{
				return urlTarget;
			}
			set
			{
				urlTarget = value;
			}
		}

		public Transform IsolateTarget
		{
			get
			{
				return isolateTarget;
			}
			set
			{
				isolateTarget = value;
			}
		}

		public ToggleType IsolateToggle
		{
			get
			{
				return isolateToggle;
			}
			set
			{
				isolateToggle = value;
			}
		}

		protected virtual void OnEnable()
		{
			cachedCanvasGroup = GetComponent<CanvasGroup>();
		}

		protected virtual void Update()
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (!(component != null))
			{
				return;
			}
			float num = 1f;
			switch (link)
			{
			case LinkType.PreviousScene:
			case LinkType.NextScene:
				num = ((GetCurrentLevel() >= 0 && GetLevelCount() > 1) ? 1f : 0f);
				break;
			case LinkType.Isolate:
				if (isolateTarget != null)
				{
					num = (isolateTarget.gameObject.activeInHierarchy ? 1f : 0.5f);
				}
				break;
			}
			component.alpha = num;
			component.blocksRaycasts = num > 0f;
			component.interactable = num > 0f;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			switch (link)
			{
			case LinkType.PreviousScene:
			{
				int currentLevel2 = GetCurrentLevel();
				if (currentLevel2 >= 0)
				{
					if (--currentLevel2 < 0)
					{
						currentLevel2 = GetLevelCount() - 1;
					}
					LoadLevel(currentLevel2);
				}
				break;
			}
			case LinkType.NextScene:
			{
				int currentLevel = GetCurrentLevel();
				if (currentLevel >= 0)
				{
					if (++currentLevel >= GetLevelCount())
					{
						currentLevel = 0;
					}
					LoadLevel(currentLevel);
				}
				break;
			}
			case LinkType.Publisher:
				Application.OpenURL("https://carloswilkes.com");
				break;
			case LinkType.URL:
				if (!string.IsNullOrEmpty(urlTarget))
				{
					Application.OpenURL(urlTarget);
				}
				break;
			case LinkType.Isolate:
			{
				if (!(isolateTarget != null))
				{
					break;
				}
				Transform parent = isolateTarget.transform.parent;
				bool activeSelf = isolateTarget.gameObject.activeSelf;
				foreach (Transform item in parent.transform)
				{
					if (item.gameObject.activeSelf)
					{
						if (item != isolateTarget)
						{
							previousChild = item;
						}
						item.gameObject.SetActive(value: false);
					}
				}
				switch (isolateToggle)
				{
				case ToggleType.KeepSelected:
					isolateTarget.gameObject.SetActive(value: true);
					break;
				case ToggleType.ToggleSelection:
					isolateTarget.gameObject.SetActive(!activeSelf);
					break;
				case ToggleType.SelectPrevious:
					if (activeSelf && previousChild != null)
					{
						previousChild.gameObject.SetActive(value: true);
					}
					else
					{
						isolateTarget.gameObject.SetActive(value: true);
					}
					break;
				}
				break;
			}
			}
		}

		private static int GetCurrentLevel()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			int buildIndex = activeScene.buildIndex;
			if (buildIndex >= 0 && SceneManager.GetSceneByBuildIndex(buildIndex).handle != activeScene.handle)
			{
				return -1;
			}
			return buildIndex;
		}

		private static int GetLevelCount()
		{
			return SceneManager.sceneCountInBuildSettings;
		}

		private static void LoadLevel(int index)
		{
			SceneManager.LoadScene(index);
		}
	}
}
