using CurvedUI;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class UI_Oxygen : MonoBehaviour
{
	public ProceduralImage fill;

	[SerializeField]
	private CurvedUIVertexEffect m_curvedEffect;

	public AnimationCurve displayCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	private void Update()
	{
		if ((bool)Player.localPlayer && Time.frameCount % 8 == 0)
		{
			fill.fillAmount = Player.localPlayer.data.OxygenDisplayPercentage();
			if ((bool)m_curvedEffect)
			{
				m_curvedEffect.TryUpdateCurvedVertex();
			}
		}
	}
}
