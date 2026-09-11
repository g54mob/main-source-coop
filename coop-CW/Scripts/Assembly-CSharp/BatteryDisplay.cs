using UnityEngine;

public class BatteryDisplay : MonoBehaviour
{
	public Material m_offMaterial;

	public Material m_greenMaterial;

	public Material m_yellowMaterial;

	public Material m_redMaterial;

	public Renderer[] m_renderer;

	public void SetCharge(float charge)
	{
		float step = 1f / (float)m_renderer.Length;
		Material material = GetOnMaterial();
		for (int i = 0; i < m_renderer.Length; i++)
		{
			m_renderer[i].material = (((float)i * step < charge && charge > 0f) ? material : m_offMaterial);
		}
		Material GetOnMaterial()
		{
			if (charge < step)
			{
				return m_redMaterial;
			}
			if (charge < step * 2f)
			{
				return m_yellowMaterial;
			}
			return m_greenMaterial;
		}
	}
}
