using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ClickWaveControllerOptimized : MonoBehaviour
{
	private struct Wave
	{
		public Vector3 point;

		public float startTime;

		public float amplitude;
	}

	private const int MaxWaves = 16;

	[Header("Інтенсивність хвилі від кліку (амплітуда в метрах)")]
	public float clickIntensity = 0.25f;

	[Tooltip("Час життя хвилі в секундах — після цього вона видаляється з буфера. Підберіть так, щоб візуально хвиля вже встигала згаснути (залежить від Time Decay).")]
	public float waveLifetime = 6f;

	[Tooltip("Якщо порожньо — береться Camera.main")]
	public Camera raycastCamera;

	private Renderer rend;

	private MaterialPropertyBlock mpb;

	private readonly List<Wave> waves = new List<Wave>(16);

	private readonly Vector4[] positionsBuffer = new Vector4[16];

	private readonly float[] amplitudesBuffer = new float[16];

	private static readonly int PositionsId = Shader.PropertyToID("_WavePositions");

	private static readonly int AmplitudesId = Shader.PropertyToID("_WaveAmplitudes");

	private static readonly int CountId = Shader.PropertyToID("_WaveCount");

	private void Awake()
	{
		rend = GetComponent<Renderer>();
		mpb = new MaterialPropertyBlock();
		if (!raycastCamera)
		{
			raycastCamera = Camera.main;
		}
		Apply();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(raycastCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo) && hitInfo.collider.gameObject == base.gameObject)
		{
			AddWave(hitInfo.point, clickIntensity);
		}
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		bool flag = false;
		for (int num = waves.Count - 1; num >= 0; num--)
		{
			if (timeSinceLevelLoad - waves[num].startTime > waveLifetime)
			{
				waves.RemoveAt(num);
				flag = true;
			}
		}
		if (flag)
		{
			Apply();
		}
	}

	public void AddWave(Vector3 worldPoint, float intensity)
	{
		if (waves.Count >= 16)
		{
			waves.RemoveAt(0);
		}
		waves.Add(new Wave
		{
			point = worldPoint,
			startTime = Time.timeSinceLevelLoad,
			amplitude = intensity
		});
		Apply();
	}

	private void Apply()
	{
		for (int i = 0; i < waves.Count; i++)
		{
			Wave wave = waves[i];
			positionsBuffer[i] = new Vector4(wave.point.x, wave.point.y, wave.point.z, wave.startTime);
			amplitudesBuffer[i] = wave.amplitude;
		}
		rend.GetPropertyBlock(mpb);
		mpb.SetVectorArray(PositionsId, positionsBuffer);
		mpb.SetFloatArray(AmplitudesId, amplitudesBuffer);
		mpb.SetInt(CountId, waves.Count);
		rend.SetPropertyBlock(mpb);
	}
}
