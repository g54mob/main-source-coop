using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
public class BarnCinematicCamera : MonoBehaviour
{
	private Camera _cam;

	private AudioListener _listener;

	public static BarnCinematicCamera Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		_cam = GetComponent<Camera>();
		_listener = GetComponent<AudioListener>();
		_cam.enabled = false;
		_listener.enabled = false;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void Activate()
	{
		_cam.enabled = true;
		_listener.enabled = true;
	}

	public void Deactivate()
	{
		_cam.enabled = false;
		_listener.enabled = false;
	}
}
