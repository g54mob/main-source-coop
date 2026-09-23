using System.Collections.Generic;
using Mimicraft.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	[RequireComponent(typeof(Button))]
	public class QuickPlayButton : MonoBehaviour
	{
		private sealed class Poller : MonoBehaviour
		{
			private static Poller instance;

			private float nextCheckTime;

			public static void Ensure()
			{
				if (!(instance != null))
				{
					GameObject obj = new GameObject("QuickPlayButtonPoller")
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					Object.DontDestroyOnLoad(obj);
					instance = obj.AddComponent<Poller>();
				}
			}

			private void Update()
			{
				if (Time.unscaledTime < nextCheckTime)
				{
					return;
				}
				nextCheckTime = Time.unscaledTime + 1f;
				for (int num = buttons.Count - 1; num >= 0; num--)
				{
					if (buttons[num] == null)
					{
						buttons.RemoveAt(num);
					}
					else
					{
						buttons[num].Apply();
					}
				}
			}

			private void OnDestroy()
			{
				if (instance == this)
				{
					instance = null;
				}
			}
		}

		[Tooltip("Steam yokken gizlenecek obje. Boş bırakılırsa bu objenin kendisi kullanılır - butonun etrafında bir çerçeve ya da etiket varsa onların kökünü ver.")]
		[SerializeField]
		private GameObject root;

		private const float CheckIntervalSeconds = 1f;

		private Button button;

		private bool? appliedVisible;

		private static readonly List<QuickPlayButton> buttons = new List<QuickPlayButton>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			buttons.Clear();
		}

		private void Awake()
		{
			if (root == null)
			{
				root = base.gameObject;
			}
			button = GetComponent<Button>();
			button.onClick.RemoveListener(Press);
			button.onClick.AddListener(Press);
			buttons.Add(this);
			Poller.Ensure();
			Apply();
		}

		private void OnDestroy()
		{
			buttons.Remove(this);
		}

		private void Apply()
		{
			bool isAvailable = QuickPlay.IsAvailable;
			if (appliedVisible != isAvailable)
			{
				appliedVisible = isAvailable;
				if (root != null)
				{
					root.SetActive(isAvailable);
				}
			}
		}

		private void Press()
		{
			if (!QuickPlay.IsSearching)
			{
				QuickPlay.Start(base.gameObject.scene);
			}
		}
	}
}
