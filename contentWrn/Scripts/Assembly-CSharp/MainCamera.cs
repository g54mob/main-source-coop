using EPOOutline;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zorro.Core;
using Zorro.Core.SmallShadows;
using Zorro.Recorder;

public class MainCamera : MonoBehaviour
{
	public RecorderAudioListener m_audioListener;

	public static MainCamera instance;

	public Outliner outliner;

	private float baseFOV = 70f;

	private Camera cam;

	public Camera Cam => cam;

	private void Awake()
	{
		instance = this;
		cam = GetComponent<Camera>();
		cam.GetComponent<UniversalAdditionalCameraData>();
		baseFOV = cam.fieldOfView;
	}

	private void Update()
	{
		Singleton<SmallShadowHandler>.Instance?.ScheduleShadowDistanceChecks(base.transform.position);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, GetFov(), Time.deltaTime * 10f);
	}

	private void LateUpdate()
	{
		Singleton<SmallShadowHandler>.Instance?.Process();
	}

	private float GetFov()
	{
		if (!Player.localPlayer)
		{
			return baseFOV;
		}
		float num = 1f;
		if (Player.localPlayer == null)
		{
			return baseFOV;
		}
		if (Player.localPlayer.data.hookedIntoTerminal && num > 0.5f)
		{
			num = 0.5f;
		}
		return baseFOV * num;
	}

	public void Flick()
	{
		AudioListener component = GetComponent<AudioListener>();
		component.enabled = false;
		component.enabled = true;
	}

	public Vector3 GetDebugItemSpawnPos()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out var hitInfo, 10f, HelperFunctions.GetMask(HelperFunctions.LayerType.All)))
		{
			return hitInfo.point + hitInfo.normal * 0.25f;
		}
		return ray.GetPoint(10f);
	}
}
