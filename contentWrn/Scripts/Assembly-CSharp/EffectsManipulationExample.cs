using UnityEngine;
using UnityEngine.Rendering;

public class EffectsManipulationExample : MonoBehaviour
{
	public Volume volume;

	private LimitlessGlitch1 m_Glitch;

	private void Start()
	{
		if (!(volume == null))
		{
			volume.profile.TryGet<LimitlessGlitch1>(out m_Glitch);
			if ((object)m_Glitch == null)
			{
				Debug.Log("Add Glitch1 effect to your Volume component to make Manipulation Example work");
			}
			else
			{
				m_Glitch.active = true;
			}
		}
	}

	private void FixedUpdate()
	{
		if (!(volume == null) && (object)m_Glitch != null)
		{
			m_Glitch.bMultiplier.value = Random.Range(-2f, 2f);
		}
	}
}
