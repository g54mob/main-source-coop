using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
using Zorro.Core;
using Zorro.Recorder;

public class VideoCamera : ItemInstanceBehaviour
{
	public float m_controllerScrollSpeed = 1f;

	public InputActionReference m_cameraZoomIn;

	public InputActionReference m_cameraZoomOut;

	public bool recordEvenWhenNotMine;

	public Camera m_camera;

	public Transform flipPos;

	private Vector3 startCameraLocalPos;

	[SerializeField]
	private Canvas m_cameraUI;

	[SerializeField]
	private GameObject m_recordingUI;

	[SerializeField]
	private MeshRenderer m_cameraScreen;

	[SerializeField]
	private float m_cameraFov = 40f;

	[SerializeField]
	private float m_zoom = 2f;

	private float m_zoomLevel;

	private float m_contentPollingTimer;

	private float m_contentPollingCooldown = 0.15f;

	public GameObject[] disableWhenRecordingNormal;

	public GameObject[] disableWhenRecordingFlipped;

	public GameObject[] disableWhenRecording;

	public GameObject[] enableWhenRecording;

	private bool m_lastCheckWasRecording;

	private bool m_lastCheckWasFlipped;

	private bool m_lastRecordEvenWhenNotMine;

	private VideoInfoEntry m_recorderInfoEntry;

	private ItemInstanceData m_instanceData;

	private PhotonView m_playerView;

	private Clip m_clip;

	private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

	private ContentBuffer contentBuffer;

	private bool m_blockCameraInput;

	private float timeSinceHeld;

	public SFX_Instance findCameraSounds;

	private float timeSinceBeep;

	public Light findMeLight;

	public float bleepInterval = 1f;

	public float dontBeepUntil = 2f;

	private Vector3 m_upOffset = new Vector3(0f, 0.2f, 0f);

	public bool recording => m_recorderInfoEntry?.isRecording ?? false;

	public bool HasFilmLeft => m_recorderInfoEntry.timeLeft > 0f;

	private void Start()
	{
		startCameraLocalPos = m_camera.transform.localPosition;
		GameObject[] array = disableWhenRecording;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
		array = enableWhenRecording;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		m_instanceData = data;
		m_playerView = playerView;
		CameraHandler.RegisterCamera(data.m_guid, this);
		if (isHeldByMe)
		{
			if (m_camera.targetTexture == null)
			{
				CreateRenderTexture();
			}
			m_camera.gameObject.SetActive(value: true);
		}
		if (data.TryGetEntry<VideoInfoEntry>(out m_recorderInfoEntry))
		{
			if (m_recorderInfoEntry.StartedRecording && m_recorderInfoEntry.isRecording)
			{
				bool isMasterClient = isHeldByMe;
				if (!isMasterClient && playerView == null)
				{
					isMasterClient = PhotonNetwork.IsMasterClient;
				}
				if (isMasterClient)
				{
					if (playerView != null)
					{
						RestartRecording(playerView);
						m_blockCameraInput = true;
						StartCoroutine(UnlockCamera());
					}
					else
					{
						StartCoroutine(RestartRecordingDelayed());
					}
				}
			}
		}
		else
		{
			m_recorderInfoEntry = new VideoInfoEntry
			{
				videoID = VideoHandle.Invalid,
				timeLeft = 90f,
				maxTime = 90f
			};
			data.AddDataEntry(m_recorderInfoEntry);
			m_recorderInfoEntry.SetDirty();
			VerboseDebug.Log("RecorderInfo entry not found, adding new entry with default info.");
		}
		m_recordingUI.gameObject.SetActive(recording);
	}

	private IEnumerator UnlockCamera()
	{
		yield return new WaitForSecondsRealtime(1f);
		m_blockCameraInput = false;
	}

	private IEnumerator RestartRecordingDelayed()
	{
		yield return new WaitForSeconds(0.25f);
		RestartRecording(null);
		yield return new WaitForSeconds(4f);
		if (m_recorderInfoEntry.isRecording)
		{
			RetrievableSingleton<RecordingsHandler>.Instance.StopRecording(m_instanceData);
			m_recordingUI.gameObject.SetActive(value: false);
		}
	}

	private void RestartRecording(PhotonView playerView)
	{
		if (HasFilmLeft)
		{
			RetrievableSingleton<RecordingsHandler>.Instance.StartRecording(m_instanceData, playerView);
		}
	}

	private void OnDestroy()
	{
		bool isMasterClient = isHeldByMe;
		if (!isMasterClient && m_playerView == null)
		{
			isMasterClient = PhotonNetwork.IsMasterClient;
		}
		if (isMasterClient)
		{
			VideoInfoEntry recorderInfoEntry = m_recorderInfoEntry;
			if (recorderInfoEntry != null && recorderInfoEntry.isRecording)
			{
				RetrievableSingleton<RecordingsHandler>.Instance.StopRecording(m_instanceData);
			}
		}
		if (m_instanceData != null)
		{
			CameraHandler.UnregisterCamera(m_instanceData.m_guid);
		}
		Clip clip = m_clip;
		if (clip != null && clip.local)
		{
			VideoRecorder recorder = m_clip.GetRecorder();
			if (recorder != null)
			{
				recorder.m_record = false;
				if ((bool)Singleton<RecorderAudioListener>.Instance)
				{
					Singleton<RecorderAudioListener>.Instance.RemoveRecorder(recorder);
				}
			}
		}
		if (m_camera.targetTexture != null)
		{
			m_camera.targetTexture.Release();
		}
	}

	private void DoFindMeBlipp()
	{
		timeSinceBeep += Time.deltaTime;
		findMeLight.transform.position = base.transform.position + m_upOffset;
		timeSinceHeld += Time.deltaTime;
		if (!(timeSinceHeld < dontBeepUntil) && timeSinceBeep > bleepInterval)
		{
			timeSinceBeep = 0f;
			findCameraSounds.Play(base.transform.position);
			findMeLight.enabled = !findMeLight.enabled;
		}
	}

	private void FixedUpdate()
	{
		if (PhotonNetwork.IsMasterClient && !isHeld && PhotonGameLobbyHandler.IsSurface && base.transform.position.y < -300f)
		{
			base.transform.position = Object.FindObjectOfType<CameraRespawnPoint>().transform.position;
			Rigidbody component = GetComponent<Rigidbody>();
			component.linearVelocity = Vector3.zero;
			component.angularVelocity = Vector3.zero;
		}
	}

	private void Update()
	{
		float b = m_cameraFov / m_zoom;
		if (!isHeld && SurfaceNetworkHandler.HasStarted)
		{
			DoFindMeBlipp();
		}
		if (isHeldByMe && GlobalInputHandler.CanTakeInput() && Player.localPlayer.input.toggleCameraFlipWasPressed)
		{
			m_recorderInfoEntry.isFlipped = !m_recorderInfoEntry.isFlipped;
			m_recorderInfoEntry.SetDirty();
		}
		if (isHeldByMe && GlobalInputHandler.CanTakeInput())
		{
			m_zoomLevel += Input.GetAxis("Mouse ScrollWheel");
			m_zoomLevel += (m_cameraZoomIn.action.IsPressed() ? (Time.deltaTime * m_controllerScrollSpeed) : 0f);
			m_zoomLevel -= (m_cameraZoomOut.action.IsPressed() ? (Time.deltaTime * m_controllerScrollSpeed) : 0f);
		}
		m_zoomLevel = math.saturate(m_zoomLevel);
		if (m_recorderInfoEntry.isFlipped)
		{
			m_camera.transform.SetPositionAndRotation(flipPos.position, flipPos.rotation);
			m_camera.fieldOfView = Mathf.Lerp(m_cameraFov, b, m_zoomLevel) * 2f;
		}
		else
		{
			m_camera.transform.SetLocalPositionAndRotation(startCameraLocalPos, Quaternion.identity);
			m_camera.fieldOfView = Mathf.Lerp(m_cameraFov, b, m_zoomLevel);
		}
		if (isHeldByMe && Player.localPlayer.input.clickWasPressed && !m_blockCameraInput)
		{
			if (!m_recorderInfoEntry.isRecording)
			{
				if (HasFilmLeft)
				{
					RetrievableSingleton<RecordingsHandler>.Instance.StartRecording(m_instanceData, m_playerView);
				}
			}
			else
			{
				RetrievableSingleton<RecordingsHandler>.Instance.StopRecording(m_instanceData);
			}
			m_recordingUI.gameObject.SetActive(recording);
		}
		if (isSimulatedByMe && recording)
		{
			m_recorderInfoEntry.timeLeft -= Time.unscaledDeltaTime;
			if (m_recorderInfoEntry.timeLeft <= 0f)
			{
				RetrievableSingleton<RecordingsHandler>.Instance.StopRecording(m_instanceData);
				m_recordingUI.gameObject.SetActive(recording);
			}
		}
		if (m_recorderInfoEntry.timeLeft <= 0f && PhotonNetwork.IsMasterClient && PhotonGameLobbyHandler.CurrentObjective is FilmSomethingScaryObjective)
		{
			PhotonGameLobbyHandler.Instance.SetCurrentObjective(new ReturnToTheDiveBellObjective());
		}
		bool isRecording = m_recorderInfoEntry.isRecording;
		bool isFlipped = m_recorderInfoEntry.isFlipped;
		recordEvenWhenNotMine = Vector3.SqrMagnitude(m_camera.transform.position - MainCamera.instance.transform.position) < 100f;
		if (isRecording != m_lastCheckWasRecording || isFlipped != m_lastCheckWasFlipped || recordEvenWhenNotMine != m_lastRecordEvenWhenNotMine)
		{
			m_lastCheckWasRecording = isRecording;
			m_lastCheckWasFlipped = isFlipped;
			m_lastRecordEvenWhenNotMine = recordEvenWhenNotMine;
			GameObject[] array = disableWhenRecording;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!isRecording);
			}
			array = disableWhenRecordingFlipped;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!isRecording || !m_recorderInfoEntry.isFlipped);
			}
			array = disableWhenRecordingNormal;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!isRecording || m_recorderInfoEntry.isFlipped);
			}
			array = enableWhenRecording;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(isRecording);
			}
			if (!isSimulatedByMe && recordEvenWhenNotMine)
			{
				if (m_camera.targetTexture == null)
				{
					CreateRenderTexture();
				}
				m_camera.gameObject.SetActive(value: true);
				m_cameraScreen.gameObject.SetActive(value: true);
			}
		}
		if (isSimulatedByMe && contentBuffer != null)
		{
			m_contentPollingTimer += Time.unscaledDeltaTime;
			if (m_contentPollingTimer > m_contentPollingCooldown)
			{
				m_contentPollingTimer = 0f;
				if (ContentPolling.CompletePolling(out var eventFrames))
				{
					List<ContentEventFrame> frames = ContentPolling.CombineAndGetBestContentEvent(eventFrames);
					contentBuffer.PushFrame(frames);
				}
				ContentPolling.StartPolling(m_camera, m_recorderInfoEntry.timeRecorded);
			}
		}
		if (isSimulatedByMe)
		{
			ContentPolling.TickPoll();
		}
		m_cameraScreen.gameObject.SetActive(m_camera.enabled && m_camera.gameObject.activeSelf);
	}

	public void StartRecording(Clip clip)
	{
		m_clip = clip;
		m_camera.gameObject.SetActive(value: true);
		if (m_camera.targetTexture == null)
		{
			CreateRenderTexture();
		}
		m_camera.Render();
		GameObject obj = new GameObject("Clip Recorder: " + clip.clipID.ToMiniString());
		Object.DontDestroyOnLoad(obj.gameObject);
		VideoRecorder videoRecorder = obj.AddComponent<VideoRecorder>();
		videoRecorder.m_targetUnityCamera = m_camera;
		videoRecorder.m_recordMicrophone = true;
		videoRecorder.m_record = true;
		videoRecorder.OutputPath = clip.GetClipDirectory();
		Singleton<RecorderAudioListener>.Instance.SendAudioTo(videoRecorder);
		clip.SetRecorder(videoRecorder);
		contentBuffer = new ContentBuffer();
		clip.SetContentBufffer(contentBuffer);
	}

	private void CreateRenderTexture()
	{
		RenderTexture renderTexture = new RenderTexture(RecordingConstants.VIDEO_RENDER_TEXTURE_WIDTH, RecordingConstants.VIDEO_RENDER_TEXTURE_HEIGHT, 24, GraphicsFormat.R8G8B8A8_UNorm);
		renderTexture.Create();
		m_camera.targetTexture = renderTexture;
		m_cameraScreen.material.SetTexture(BaseMap, renderTexture);
	}

	public void StopRecording()
	{
		if (ContentPolling.CompletePolling(out var eventFrames))
		{
			List<ContentEventFrame> frames = ContentPolling.CombineAndGetBestContentEvent(eventFrames);
			contentBuffer.PushFrame(frames);
		}
		contentBuffer = null;
		if (!isHeldByMe)
		{
			m_camera.gameObject.SetActive(value: false);
		}
		VideoRecorder recorder = m_clip.GetRecorder();
		recorder.m_record = false;
		Singleton<RecorderAudioListener>.Instance.RemoveRecorder(recorder);
	}
}
