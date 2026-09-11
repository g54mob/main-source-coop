using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio
{
	public class G_AudioText : MonoBehaviour
	{
		[SerializeField]
		private Text m_DBText;

		private GraphyManager m_graphyManager;

		private G_AudioMonitor m_audioMonitor;

		private int m_updateRate = 4;

		private float m_deltaTimeOffset;

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			if (m_audioMonitor.SpectrumDataAvailable)
			{
				if (m_deltaTimeOffset > 1f / (float)m_updateRate)
				{
					m_deltaTimeOffset = 0f;
					m_DBText.text = Mathf.Clamp((int)m_audioMonitor.MaxDB, -80, 0).ToStringNonAlloc();
				}
				else
				{
					m_deltaTimeOffset += Time.deltaTime;
				}
			}
		}

		public void UpdateParameters()
		{
			m_updateRate = m_graphyManager.AudioTextUpdateRate;
		}

		private void Init()
		{
			G_IntString.Init(-80, 0);
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_audioMonitor = GetComponent<G_AudioMonitor>();
			UpdateParameters();
		}
	}
}
