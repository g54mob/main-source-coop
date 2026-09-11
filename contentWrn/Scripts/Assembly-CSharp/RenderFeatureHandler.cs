using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RenderFeatureHandler : MonoBehaviour
{
	[SerializeField]
	private ScriptableRendererFeature m_HighFidelity;

	[SerializeField]
	private ScriptableRendererFeature m_MediumFidelity1;

	[SerializeField]
	private ScriptableRendererFeature m_MediumFidelity2;

	[SerializeField]
	private ScriptableRendererFeature m_LowFidelity;

	public ScriptableRendererFeature jelly;

	private bool thingsAreActive;

	private bool jelloGreaterThanZero;

	private void Start()
	{
		jelly = m_HighFidelity;
		jelly.SetActive(active: false);
	}

	private void OnDisable()
	{
		jelly.SetActive(active: false);
	}

	private void Update()
	{
		if (Player.localPlayer == null || Player.localPlayer.data.dead)
		{
			if (thingsAreActive)
			{
				thingsAreActive = false;
				jelly.SetActive(active: false);
			}
			return;
		}
		thingsAreActive = true;
		jelloGreaterThanZero = Player.localPlayer.data.jelloTime > 0f;
		if (jelly.isActive != jelloGreaterThanZero)
		{
			Debug.Log("Set jello to active: " + jelloGreaterThanZero);
			jelly.SetActive(jelloGreaterThanZero);
		}
	}
}
