using CurvedUI;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class UI_Health : MonoBehaviour
{
	public ProceduralImage fill;

	[SerializeField]
	private CurvedUIVertexEffect m_curvedEffect;

	private void Update()
	{
		if (!(Player.localPlayer == null))
		{
			fill.fillAmount = Player.localPlayer.data.health * 0.01f;
			if ((bool)m_curvedEffect)
			{
				m_curvedEffect.TryUpdateCurvedVertex();
			}
		}
	}
}
