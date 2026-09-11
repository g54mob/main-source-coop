using UnityEngine;

public class VideoCameraSounds : MonoBehaviour
{
	private VideoCamera cam;

	public SFX_PlayOneShot recStart;

	public SFX_PlayOneShot recStop;

	public SFX_PlayOneShot zoomIn;

	public SFX_PlayOneShot zoomOut;

	private float prevZoom;

	private bool prevBool;

	private void Start()
	{
		cam = GetComponent<VideoCamera>();
		prevBool = cam.recording;
	}

	private void Update()
	{
		recStart.playOnClick = false;
		recStop.playOnClick = false;
		zoomOut.playOnClick = false;
		zoomIn.playOnClick = false;
		if (cam.recording && !prevBool)
		{
			recStart.playOnClick = true;
		}
		if (!cam.recording && prevBool)
		{
			recStop.playOnClick = true;
		}
		if (prevZoom > cam.m_camera.fieldOfView)
		{
			zoomOut.playOnClick = true;
		}
		if (prevZoom < cam.m_camera.fieldOfView)
		{
			zoomIn.playOnClick = true;
		}
		prevZoom = cam.m_camera.fieldOfView;
		prevBool = cam.recording;
	}
}
