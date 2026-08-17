using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : StaticInstance<CameraController>
{
	[SerializeField]
	private float _shakeIntensity;

	[SerializeField]
	private float _shakeTime;

	[SerializeField]
	private CameraFollow _cameraFollow;

	[SerializeField]
	private CinemachineTargetGroup _targetGroup;

	[SerializeField]
	private GameObject _depthOfField;

	[SerializeField]
	private Transform _camera;

	private CinemachineVirtualCamera _vcam;

	private CinemachineTransposer _transposer;

	private float _size;

	protected override void Awake()
	{
		base.Awake();
		_vcam = GetComponent<CinemachineVirtualCamera>();
		_transposer = _vcam.GetCinemachineComponent<CinemachineTransposer>();
		_size = _vcam.m_Lens.OrthographicSize;
	}

	public Vector3 GetCameraPosition()
	{
		return _camera.position;
	}

	public Transform GetCameraTransform()
	{
		return _camera;
	}

	public void SetBlur(bool value)
	{
		if (_depthOfField != null)
		{
			_depthOfField.SetActive(value);
		}
	}

	public Vector2 GetDamping()
	{
		return new Vector2(_transposer.m_XDamping, _transposer.m_YawDamping);
	}

	public void SetDamping(Vector2 value)
	{
		_transposer.m_XDamping = value.x;
		_transposer.m_YDamping = value.y;
	}

	public void ShakeCamera()
	{
		StartCoroutine(CameraShakeDuringTime(_shakeIntensity, _shakeTime));
	}

	public void SetFollow(bool value)
	{
		_transposer.FollowTargetGroup.enabled = value;
	}

	public void SetFollowOffset(Vector3 value)
	{
		value.z = _transposer.m_FollowOffset.z;
		_transposer.m_FollowOffset = value;
	}

	public void ChangeCameraFollowTarget(Transform newTarget)
	{
		_targetGroup.m_Targets[0].target = newTarget;
		_targetGroup.m_Targets[1].target = newTarget;
	}

	public void RestoreCameraFollowTarget()
	{
		_targetGroup.m_Targets[0].target = _cameraFollow._penguins[0].transform;
		_targetGroup.m_Targets[1].target = _cameraFollow._penguins[1].transform;
	}

	public void CameraZoom(float zoom, float time)
	{
		StartCoroutine(DoZoom(zoom, time));
	}

	public void RestoreCameraZoom(float time)
	{
		StopCoroutine("DoZoom");
		StartCoroutine(RestoreZoom(time));
	}

	private IEnumerator DoZoom(float zoom, float time)
	{
		float t = 0f;
		float currentSize = _vcam.m_Lens.OrthographicSize;
		while (true)
		{
			_vcam.m_Lens.OrthographicSize = Mathf.Lerp(currentSize, zoom, t / time);
			t += Time.deltaTime;
			if (t >= time)
			{
				break;
			}
			yield return null;
		}
		_vcam.m_Lens.OrthographicSize = Mathf.Lerp(currentSize, zoom, 1f);
	}

	private IEnumerator RestoreZoom(float time)
	{
		float t = 0f;
		float currentSize = _vcam.m_Lens.OrthographicSize;
		while (true)
		{
			_vcam.m_Lens.OrthographicSize = Mathf.SmoothStep(currentSize, _size, t / time);
			t += Time.deltaTime;
			if (t >= time)
			{
				break;
			}
			yield return null;
		}
		_vcam.m_Lens.OrthographicSize = Mathf.SmoothStep(currentSize, _size, 1f);
	}

	private IEnumerator CameraShakeDuringTime(float intensity, float time)
	{
		_transposer.FollowTargetGroup.enabled = false;
		float t = 0f;
		while (t < time)
		{
			float x = Random.Range(0f - intensity, intensity);
			float y = Random.Range(0f - intensity, intensity);
			base.transform.position += new Vector3(x, y, 0f);
			t += Time.deltaTime;
			yield return null;
		}
		_transposer.FollowTargetGroup.enabled = true;
	}
}
