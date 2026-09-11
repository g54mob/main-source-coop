using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps
{
	public class G_FpsText : MonoBehaviour
	{
		[SerializeField]
		private Text m_fpsText;

		[SerializeField]
		private Text m_msText;

		[SerializeField]
		private Text m_avgFpsText;

		[SerializeField]
		private Text m_onePercentFpsText;

		[SerializeField]
		private Text m_zero1PercentFpsText;

		private GraphyManager m_graphyManager;

		private G_FpsMonitor m_fpsMonitor;

		private int m_updateRate = 4;

		private int m_frameCount;

		private float m_deltaTime;

		private float m_fps;

		private float m_ms;

		private const string m_msStringFormat = "0.0";

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			m_deltaTime += Time.unscaledDeltaTime;
			m_frameCount++;
			if (m_deltaTime > 1f / (float)m_updateRate)
			{
				m_fps = (float)m_frameCount / m_deltaTime;
				m_ms = m_deltaTime / (float)m_frameCount * 1000f;
				m_fpsText.text = Mathf.RoundToInt(m_fps).ToStringNonAlloc();
				SetFpsRelatedTextColor(m_fpsText, m_fps);
				m_msText.text = m_ms.ToStringNonAlloc("0.0");
				SetFpsRelatedTextColor(m_msText, m_fps);
				m_onePercentFpsText.text = G_IntString.ToStringNonAlloc(m_fpsMonitor.OnePercentFPS);
				SetFpsRelatedTextColor(m_onePercentFpsText, m_fpsMonitor.OnePercentFPS);
				m_zero1PercentFpsText.text = G_IntString.ToStringNonAlloc(m_fpsMonitor.Zero1PercentFps);
				SetFpsRelatedTextColor(m_zero1PercentFpsText, m_fpsMonitor.Zero1PercentFps);
				m_avgFpsText.text = G_IntString.ToStringNonAlloc(m_fpsMonitor.AverageFPS);
				SetFpsRelatedTextColor(m_avgFpsText, m_fpsMonitor.AverageFPS);
				m_deltaTime = 0f;
				m_frameCount = 0;
			}
		}

		public void UpdateParameters()
		{
			m_updateRate = m_graphyManager.FpsTextUpdateRate;
		}

		private void SetFpsRelatedTextColor(Text text, float fps)
		{
			int num = Mathf.RoundToInt(fps);
			if (num >= m_graphyManager.GoodFPSThreshold)
			{
				text.color = m_graphyManager.GoodFPSColor;
			}
			else if (num >= m_graphyManager.CautionFPSThreshold)
			{
				text.color = m_graphyManager.CautionFPSColor;
			}
			else
			{
				text.color = m_graphyManager.CriticalFPSColor;
			}
		}

		private void Init()
		{
			G_IntString.Init(0, 2000);
			G_FloatString.Init(0f, 100f);
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_fpsMonitor = GetComponent<G_FpsMonitor>();
			UpdateParameters();
		}
	}
}
