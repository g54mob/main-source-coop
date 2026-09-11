using System.Collections.Generic;
using System.Text;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Advanced
{
	public class G_AdvancedData : MonoBehaviour, IMovable, IModifiableState
	{
		[SerializeField]
		private List<Image> m_backgroundImages = new List<Image>();

		[SerializeField]
		private Text m_graphicsDeviceVersionText;

		[SerializeField]
		private Text m_processorTypeText;

		[SerializeField]
		private Text m_operatingSystemText;

		[SerializeField]
		private Text m_systemMemoryText;

		[SerializeField]
		private Text m_graphicsDeviceNameText;

		[SerializeField]
		private Text m_graphicsMemorySizeText;

		[SerializeField]
		private Text m_screenResolutionText;

		[SerializeField]
		private Text m_gameWindowResolutionText;

		[SerializeField]
		private Text m_gameVRResolutionText;

		[Range(1f, 60f)]
		[SerializeField]
		private float m_updateRate = 1f;

		private GraphyManager m_graphyManager;

		private RectTransform m_rectTransform;

		private Vector2 m_origPosition = Vector2.zero;

		private float m_deltaTime;

		private StringBuilder m_sb;

		private GraphyManager.ModuleState m_previousModuleState;

		private GraphyManager.ModuleState m_currentModuleState;

		private readonly string[] m_windowStrings = new string[6] { "Window: ", "x", "@", "Hz", "[", "dpi]" };

		private readonly string[] m_vrStrings = new string[4] { "VR: (", "*2)x", "@", "Hz" };

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			m_deltaTime += Time.unscaledDeltaTime;
			if (m_deltaTime > 1f / m_updateRate)
			{
				m_sb.Length = 0;
				m_sb.Append(m_windowStrings[0]).Append(Screen.width.ToStringNonAlloc()).Append(m_windowStrings[1])
					.Append(Screen.height.ToStringNonAlloc())
					.Append(m_windowStrings[2])
					.Append(((int)Screen.currentResolution.refreshRateRatio.value).ToStringNonAlloc())
					.Append(m_windowStrings[3])
					.Append(m_windowStrings[4])
					.Append(((int)Screen.dpi).ToStringNonAlloc())
					.Append(m_windowStrings[5]);
				m_gameWindowResolutionText.text = m_sb.ToString();
				m_deltaTime = 0f;
			}
		}

		public void SetPosition(GraphyManager.ModulePosition newModulePosition, Vector2 offset)
		{
			if (newModulePosition != GraphyManager.ModulePosition.FREE)
			{
				m_rectTransform.anchoredPosition = m_origPosition;
				float num = Mathf.Abs(m_rectTransform.anchoredPosition.x) + offset.x;
				float num2 = Mathf.Abs(m_rectTransform.anchoredPosition.y) + offset.y;
				switch (newModulePosition)
				{
				case GraphyManager.ModulePosition.TOP_LEFT:
					m_rectTransform.anchorMax = Vector2.up;
					m_rectTransform.anchorMin = Vector2.up;
					m_rectTransform.anchoredPosition = new Vector2(num, 0f - num2);
					break;
				case GraphyManager.ModulePosition.TOP_RIGHT:
					m_rectTransform.anchorMax = Vector2.one;
					m_rectTransform.anchorMin = Vector2.one;
					m_rectTransform.anchoredPosition = new Vector2(0f - num, 0f - num2);
					break;
				case GraphyManager.ModulePosition.BOTTOM_LEFT:
					m_rectTransform.anchorMax = Vector2.zero;
					m_rectTransform.anchorMin = Vector2.zero;
					m_rectTransform.anchoredPosition = new Vector2(num, num2);
					break;
				case GraphyManager.ModulePosition.BOTTOM_RIGHT:
					m_rectTransform.anchorMax = Vector2.right;
					m_rectTransform.anchorMin = Vector2.right;
					m_rectTransform.anchoredPosition = new Vector2(0f - num, num2);
					break;
				}
				switch (newModulePosition)
				{
				case GraphyManager.ModulePosition.TOP_LEFT:
				case GraphyManager.ModulePosition.BOTTOM_LEFT:
					m_processorTypeText.alignment = TextAnchor.UpperLeft;
					m_systemMemoryText.alignment = TextAnchor.UpperLeft;
					m_graphicsDeviceNameText.alignment = TextAnchor.UpperLeft;
					m_graphicsDeviceVersionText.alignment = TextAnchor.UpperLeft;
					m_graphicsMemorySizeText.alignment = TextAnchor.UpperLeft;
					m_screenResolutionText.alignment = TextAnchor.UpperLeft;
					m_gameWindowResolutionText.alignment = TextAnchor.UpperLeft;
					m_gameVRResolutionText.alignment = TextAnchor.UpperLeft;
					m_operatingSystemText.alignment = TextAnchor.UpperLeft;
					break;
				case GraphyManager.ModulePosition.TOP_RIGHT:
				case GraphyManager.ModulePosition.BOTTOM_RIGHT:
					m_processorTypeText.alignment = TextAnchor.UpperRight;
					m_systemMemoryText.alignment = TextAnchor.UpperRight;
					m_graphicsDeviceNameText.alignment = TextAnchor.UpperRight;
					m_graphicsDeviceVersionText.alignment = TextAnchor.UpperRight;
					m_graphicsMemorySizeText.alignment = TextAnchor.UpperRight;
					m_screenResolutionText.alignment = TextAnchor.UpperRight;
					m_gameWindowResolutionText.alignment = TextAnchor.UpperRight;
					m_gameVRResolutionText.alignment = TextAnchor.UpperRight;
					m_operatingSystemText.alignment = TextAnchor.UpperRight;
					break;
				}
			}
		}

		public void SetState(GraphyManager.ModuleState state, bool silentUpdate = false)
		{
			if (!silentUpdate)
			{
				m_previousModuleState = m_currentModuleState;
			}
			m_currentModuleState = state;
			bool flag = state == GraphyManager.ModuleState.FULL || state == GraphyManager.ModuleState.TEXT || state == GraphyManager.ModuleState.BASIC;
			base.gameObject.SetActive(flag);
			m_backgroundImages.SetAllActive(flag && m_graphyManager.Background);
		}

		public void RestorePreviousState()
		{
			SetState(m_previousModuleState);
		}

		public void UpdateParameters()
		{
			foreach (Image backgroundImage in m_backgroundImages)
			{
				backgroundImage.color = m_graphyManager.BackgroundColor;
			}
			SetPosition(m_graphyManager.AdvancedModulePosition, Vector2.zero);
			SetState(m_graphyManager.AdvancedModuleState);
		}

		public void RefreshParameters()
		{
			foreach (Image backgroundImage in m_backgroundImages)
			{
				backgroundImage.color = m_graphyManager.BackgroundColor;
			}
			SetPosition(m_graphyManager.AdvancedModulePosition, Vector2.zero);
			SetState(m_currentModuleState, silentUpdate: true);
		}

		private void Init()
		{
			G_IntString.Init(0, 7680);
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_sb = new StringBuilder();
			m_rectTransform = GetComponent<RectTransform>();
			m_processorTypeText.text = "CPU: " + SystemInfo.processorType + " [" + SystemInfo.processorCount + " cores]";
			m_systemMemoryText.text = "RAM: " + SystemInfo.systemMemorySize + " MB";
			m_graphicsDeviceVersionText.text = "Graphics API: " + SystemInfo.graphicsDeviceVersion;
			m_graphicsDeviceNameText.text = "GPU: " + SystemInfo.graphicsDeviceName;
			m_graphicsMemorySizeText.text = "VRAM: " + SystemInfo.graphicsMemorySize + "MB. Max texture size: " + SystemInfo.maxTextureSize + "px. Shader level: " + SystemInfo.graphicsShaderLevel;
			Resolution currentResolution = Screen.currentResolution;
			m_screenResolutionText.text = "Screen: " + currentResolution.width + "x" + currentResolution.height + "@" + ((int)Screen.currentResolution.refreshRateRatio.value).ToStringNonAlloc() + "Hz";
			m_operatingSystemText.text = "OS: " + SystemInfo.operatingSystem + " [" + SystemInfo.deviceType.ToString() + "]";
			m_gameVRResolutionText.text = "VR: Not active";
			float num = 0f;
			foreach (Text item in new List<Text> { m_graphicsDeviceVersionText, m_processorTypeText, m_systemMemoryText, m_graphicsDeviceNameText, m_graphicsMemorySizeText, m_screenResolutionText, m_gameWindowResolutionText, m_gameVRResolutionText, m_operatingSystemText })
			{
				if (item.preferredWidth > num)
				{
					num = item.preferredWidth;
				}
			}
			m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num + 25f);
			m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x - m_rectTransform.rect.width / 2f + m_rectTransform.rect.width / 2f * Mathf.Sign(m_rectTransform.anchoredPosition.x), m_rectTransform.anchoredPosition.y);
			m_origPosition = m_rectTransform.anchoredPosition;
			UpdateParameters();
		}
	}
}
