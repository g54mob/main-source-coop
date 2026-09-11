using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
	public class G_RamText : MonoBehaviour
	{
		[SerializeField]
		private Text m_allocatedSystemMemorySizeText;

		[SerializeField]
		private Text m_reservedSystemMemorySizeText;

		[SerializeField]
		private Text m_monoSystemMemorySizeText;

		private GraphyManager m_graphyManager;

		private G_RamMonitor m_ramMonitor;

		private float m_updateRate = 4f;

		private float m_deltaTime;

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			m_deltaTime += Time.unscaledDeltaTime;
			if (m_deltaTime > 1f / m_updateRate)
			{
				m_allocatedSystemMemorySizeText.text = ((int)m_ramMonitor.AllocatedRam).ToStringNonAlloc();
				m_reservedSystemMemorySizeText.text = ((int)m_ramMonitor.ReservedRam).ToStringNonAlloc();
				m_monoSystemMemorySizeText.text = ((int)m_ramMonitor.MonoRam).ToStringNonAlloc();
				m_deltaTime = 0f;
			}
		}

		public void UpdateParameters()
		{
			m_allocatedSystemMemorySizeText.color = m_graphyManager.AllocatedRamColor;
			m_reservedSystemMemorySizeText.color = m_graphyManager.ReservedRamColor;
			m_monoSystemMemorySizeText.color = m_graphyManager.MonoRamColor;
			m_updateRate = m_graphyManager.RamTextUpdateRate;
		}

		private void Init()
		{
			G_IntString.Init(0, 16386);
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_ramMonitor = GetComponent<G_RamMonitor>();
			UpdateParameters();
		}
	}
}
