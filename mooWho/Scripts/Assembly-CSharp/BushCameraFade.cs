using Mirror;
using UnityEngine;

public class BushCameraFade : MonoBehaviour
{
	[Tooltip("Kamera bu mesafenin altındayken çalı tamamen saydam (fade=1)")]
	public float fullyTransparentDistance = 1.5f;

	[Tooltip("Kamera bu mesafenin üstündeyken çalı normal (opak) görünür (fade=0)")]
	public float fullyOpaqueDistance = 4f;

	[Tooltip("Saydamlaşma/geri dönme hızı (fade birimi/sn)")]
	public float fadeSpeed = 8f;

	private const float FullyFadedValue = 1f;

	private const float NotFadedValue = 0f;

	private static readonly int FadeAmountId = Shader.PropertyToID("_Bush_Fade_Amount");

	private Renderer[] _renderers;

	private MaterialPropertyBlock _mpb;

	private Vector3 _boundsCenter;

	private float _currentFade;

	private float _lastAppliedFade = float.NaN;

	private static Camera _cachedCamera;

	private static int _cachedFrame = -1;

	private void Awake()
	{
		_renderers = GetComponentsInChildren<Renderer>();
		_mpb = new MaterialPropertyBlock();
		_currentFade = 0f;
		if (_renderers.Length != 0)
		{
			Bounds bounds = _renderers[0].bounds;
			for (int i = 1; i < _renderers.Length; i++)
			{
				bounds.Encapsulate(_renderers[i].bounds);
			}
			_boundsCenter = bounds.center;
		}
		else
		{
			_boundsCenter = base.transform.position;
		}
	}

	private static bool IsUsable(Camera c)
	{
		if (c != null && c.enabled)
		{
			return c.gameObject.activeInHierarchy;
		}
		return false;
	}

	private static Camera GetActiveCamera()
	{
		if (_cachedFrame == Time.frameCount)
		{
			return _cachedCamera;
		}
		_cachedFrame = Time.frameCount;
		if (FreeCamera.Instance != null && FreeCamera.Instance.IsActive)
		{
			Camera component = FreeCamera.Instance.GetComponent<Camera>();
			if (IsUsable(component))
			{
				_cachedCamera = component;
				return _cachedCamera;
			}
		}
		if (SpectatorController.IsSpectating && SpectatorController.Instance != null && IsUsable(SpectatorController.Instance.spectatorCamera))
		{
			_cachedCamera = SpectatorController.Instance.spectatorCamera;
			return _cachedCamera;
		}
		if (NetworkClient.localPlayer != null)
		{
			NetworkedCameraController component2 = NetworkClient.localPlayer.GetComponent<NetworkedCameraController>();
			if (component2 != null && IsUsable(component2.playerCamera))
			{
				_cachedCamera = component2.playerCamera;
				return _cachedCamera;
			}
		}
		_cachedCamera = (IsUsable(Camera.main) ? Camera.main : null);
		return _cachedCamera;
	}

	private void Update()
	{
		Camera activeCamera = GetActiveCamera();
		float target = 0f;
		if (activeCamera != null)
		{
			float value = Vector3.Distance(activeCamera.transform.position, _boundsCenter);
			float t = Mathf.Clamp01(Mathf.InverseLerp(fullyTransparentDistance, fullyOpaqueDistance, value));
			target = Mathf.Lerp(1f, 0f, t);
		}
		_currentFade = Mathf.MoveTowards(_currentFade, target, fadeSpeed * Time.deltaTime);
		if (Mathf.Approximately(_currentFade, _lastAppliedFade))
		{
			return;
		}
		_lastAppliedFade = _currentFade;
		Renderer[] renderers = _renderers;
		foreach (Renderer renderer in renderers)
		{
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetFloat(FadeAmountId, _currentFade);
				renderer.SetPropertyBlock(_mpb);
			}
		}
	}
}
