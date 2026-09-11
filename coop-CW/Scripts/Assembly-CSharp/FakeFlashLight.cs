using UnityEngine;

public class FakeFlashLight : MonoBehaviour
{
	public Light m_light;

	public MeshRenderer lightBeam;

	public MeshRenderer brightPart;

	public bool isOn;

	public ItemInstance itemInstance;

	public Color angryBrightEmission;

	private Color defaultLightColor;

	private Color defaultBeamColor;

	private Color defaultBrightColor;

	private Color defaultBrightEmission;

	private void Awake()
	{
		itemInstance = GetComponent<ItemInstance>();
	}

	private void Start()
	{
		defaultLightColor = new Color(1f, 1f, 1f, 1f);
		defaultBeamColor = new Color(1f, 1f, 1f, 1f);
		defaultBrightColor = new Color(1f, 1f, 1f, 1f);
		defaultBrightEmission = new Color(16f, 16f, 16f, 1f);
		Debug.Log($"Default bright emission: {defaultBrightEmission}");
		ColorDefault();
	}

	public void ColorRed()
	{
		m_light.color = Color.red;
		lightBeam.material.color = Color.red;
		brightPart.material.color = Color.red;
		brightPart.material.SetColor("_EmissionColor", angryBrightEmission);
	}

	public void ColorDefault()
	{
		m_light.color = defaultLightColor;
		lightBeam.material.color = defaultBeamColor;
		brightPart.material.color = defaultBrightColor;
		brightPart.material.SetColor("_EmissionColor", defaultBrightEmission);
	}

	public void Toggle(bool on)
	{
		if (isOn != on)
		{
			isOn = on;
			m_light.enabled = on;
			lightBeam.enabled = on;
			brightPart.enabled = on;
		}
	}
}
