using CurvedUI;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class UI_Stamina : MonoBehaviour
{
	public ProceduralImage fill;

	[SerializeField]
	private CurvedUIVertexEffect m_curvedEffect;

	private void LateUpdate()
	{
		if (!(Player.localPlayer == null))
		{
			fill.fillAmount = Player.localPlayer.data.currentStamina * (Player.localPlayer.refs.controller.maxStamina * 0.01f);
			if ((bool)m_curvedEffect)
			{
				m_curvedEffect.TryUpdateCurvedVertex();
			}
		}
	}
}
