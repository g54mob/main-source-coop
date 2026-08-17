using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fred : StaticInstance<Fred>
{
	public Light2D PointLight;

	public float LightIntensity;

	public Transform RopePoint;

	[field: SerializeField]
	public PenguinActionsController Penguin { get; private set; }

	public Transform PenguinTransform => base.transform.parent.transform;

	public virtual void Start()
	{
		PointLight.intensity = PlayerPrefs.GetFloat("LightIntensity", LightIntensity);
		StaticInstance<Pause>.Instance.onGamePaused.AddListener(DisableInput);
		StaticInstance<Pause>.Instance.onGameResumed.AddListener(EnableInput);
	}

	public void DisableInput()
	{
		SetInput(value: false);
	}

	public void EnableInput()
	{
		SetInput(value: true);
	}

	private void SetInput(bool value)
	{
		Penguin.EnableInput(value);
	}

	public void SetSpriteRendererActive(bool value)
	{
		Penguin.SetSpriteEnabled(value);
	}

	public virtual void OnDestroy()
	{
		PlayerPrefs.SetFloat("LightIntensity", PointLight.intensity);
	}
}
