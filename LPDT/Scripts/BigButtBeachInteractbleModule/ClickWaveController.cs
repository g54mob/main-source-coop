using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ClickWaveController : MonoBehaviour
{
	private const int MaxWaves = 16;

	[Header("Інтенсивність хвилі від кліку (амплітуда в метрах)")]
	public float clickIntensity = 0.25f;

	[Tooltip("Якщо порожньо — береться Camera.main")]
	public Camera raycastCamera;

	private Renderer rend;

	private MaterialPropertyBlock mpb;

	private readonly Vector4[] wavePositions = new Vector4[16];

	private readonly float[] waveAmplitudes = new float[16];

	private int nextIndex;

	private static readonly int PositionsId = Shader.PropertyToID("_WavePositions");

	private static readonly int AmplitudesId = Shader.PropertyToID("_WaveAmplitudes");

	private void Awake()
	{
		rend = GetComponent<Renderer>();
		mpb = new MaterialPropertyBlock();
		if (!raycastCamera)
		{
			raycastCamera = Camera.main;
		}
		for (int i = 0; i < 16; i++)
		{
			wavePositions[i] = new Vector4(0f, 0f, 0f, -10000f);
		}
		Apply();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(raycastCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo) && hitInfo.collider.gameObject == base.gameObject)
		{
			AddWave(hitInfo.point, clickIntensity);
		}
	}

	public void AddWave(Vector3 worldPoint, float intensity)
	{
		wavePositions[nextIndex] = new Vector4(worldPoint.x, worldPoint.y, worldPoint.z, Time.timeSinceLevelLoad);
		waveAmplitudes[nextIndex] = intensity;
		nextIndex = (nextIndex + 1) % 16;
		Apply();
	}

	private void Apply()
	{
		rend.GetPropertyBlock(mpb);
		mpb.SetVectorArray(PositionsId, wavePositions);
		mpb.SetFloatArray(AmplitudesId, waveAmplitudes);
		rend.SetPropertyBlock(mpb);
	}
}
