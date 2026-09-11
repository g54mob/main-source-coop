using UnityEngine;

public class SetTheme : MonoBehaviour
{
	private static readonly int Evening = Shader.PropertyToID("Evening");

	[ColorUsage(false, true)]
	public Color fog;

	public Material skyMat;

	[SerializeField]
	private MeshRenderer m_fakeSky;

	public float evening;

	public void Start()
	{
		RenderSettings.fogColor = fog;
		Shader.SetGlobalFloat(Evening, evening);
		m_fakeSky.material = skyMat;
	}
}
