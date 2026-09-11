using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Post_Sleep : MonoBehaviour
{
	public ScriptableRendererFeature blurFeature;

	private Volume vol;

	private void Start()
	{
		vol = GetComponent<Volume>();
	}

	private void Update()
	{
		if (Player.localPlayer == null)
		{
			return;
		}
		vol.weight = Mathf.Pow(Player.localPlayer.data.sleepAmount, 2f);
		if (vol.weight > 0.01f)
		{
			if (!vol.enabled)
			{
				blurFeature.SetActive(active: true);
				vol.enabled = true;
			}
		}
		else if (vol.enabled)
		{
			blurFeature.SetActive(active: false);
			vol.enabled = false;
		}
	}

	private void OnDestroy()
	{
		blurFeature.SetActive(active: false);
	}
}
