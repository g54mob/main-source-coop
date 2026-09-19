using System;
using System.Collections;
using Dissonance;
using Mirror;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioSettingsManager : MonoBehaviour
{
	[Header("Mixer")]
	[SerializeField]
	private AudioMixer mixer;

	[SerializeField]
	private string masterVolumeParam = "MasterVolume";

	[SerializeField]
	private string musicVolumeParam = "MusicVolume";

	[SerializeField]
	private string mimicVolumeParam = "MimicVolume";

	[Header("Sessizlik Eşiği")]
	[Tooltip("0% seçilince mixer'a yazılacak dB (pratikte duyulmaz)")]
	[SerializeField]
	private float mutedDb = -80f;

	[Header("Master dB Kalibrasyonu (doğrusal, %50→X, %100→Y, %0→mutedDb)")]
	[Tooltip("Master Volume %50'deyken mixer'a yazılacak dB")]
	[SerializeField]
	private float masterDbAt50;

	[Tooltip("Master Volume %100'deyken mixer'a yazılacak dB")]
	[SerializeField]
	private float masterDbAt100 = 4f;

	[Header("Mimic dB Kalibrasyonu (parçalı doğrusal, %0→X, %50→Y, %100→Z)")]
	[Tooltip("Kullanıcı isteği: Mimic Volume %0'dayken (sürgü tam solda) GERÇEK sessizliğe (mutedDb) DÜŞMEZ — bunun yerine bu dB'ye eşdeğer gelir (hafif duyulsun diye).")]
	[SerializeField]
	private float mimicDbAt0 = -35f;

	[Tooltip("Mimic Volume %50'deyken mixer'a yazılacak dB")]
	[SerializeField]
	private float mimicDbAt50 = -2.5f;

	[Tooltip("Mimic Volume %100'deyken mixer'a yazılacak dB")]
	[SerializeField]
	private float mimicDbAt100 = 2.5f;

	[Header("Varsayılanlar (Reset butonu)")]
	[Range(0f, 1f)]
	[SerializeField]
	private float defaultMasterVolume01 = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float defaultMusicVolume01 = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float defaultMimicVolume01 = 0.5f;

	[SerializeField]
	private CommActivationMode defaultVoiceMode = CommActivationMode.PushToTalk;

	[SerializeField]
	private KeyCode defaultPushToTalkKey = KeyCode.V;

	[Tooltip("'Mute NPC Animals' varsayılan kapalı — opt-in bir özellik, kimseye sürpriz yapmasın.")]
	[SerializeField]
	private bool defaultMuteNpcAnimals;

	[Header("Push-To-Talk SFX")]
	[Tooltip("Push-to-talk tuşuna basıp konuşmaya başlayınca çalınacak küçük ses")]
	[SerializeField]
	private AudioClip pushToTalkPressSfx;

	[Tooltip("Push-to-talk tuşunu bırakınca çalınacak küçük ses")]
	[SerializeField]
	private AudioClip pushToTalkReleaseSfx;

	[Range(0f, 1f)]
	[SerializeField]
	private float pushToTalkSfxVolume = 0.6f;

	private const string MasterPrefKey = "Settings.MasterVolume01";

	private const string MusicPrefKey = "Settings.MusicVolume01";

	private const string MimicPrefKey = "Settings.MimicVolume01";

	private const string VoiceModePrefKey = "Settings.VoiceMode";

	private const string MicrophonePrefKey = "Settings.Microphone";

	private const string PushToTalkKeyPrefKey = "Settings.PushToTalkKey";

	private const string MuteNpcAnimalsPrefKey = "Settings.MuteNpcAnimals";

	private VoiceBroadcastTrigger _trigger;

	private DissonanceComms _comms;

	private bool _wasTransmitting;

	private AudioSource _sfxSource;

	private bool _isRebindingPushToTalkKey;

	private PlayerRoleData _localRoleData;

	public static AudioSettingsManager Instance { get; private set; }

	public float MasterVolume01 { get; private set; }

	public float MusicVolume01 { get; private set; }

	public float MimicVolume01 { get; private set; }

	public bool MuteNpcAnimals { get; private set; }

	public CommActivationMode VoiceMode { get; private set; }

	public string SelectedMicrophone { get; private set; } = "";

	public KeyCode PushToTalkKey { get; private set; } = KeyCode.V;

	public bool IsRebindingPushToTalkKey => _isRebindingPushToTalkKey;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		LoadSaved();
		ApplyMaster(MasterVolume01);
		ApplyMusic(MusicVolume01);
		RefreshMimicMuteState();
		ApplyVoiceModeToScene();
		ApplyMicrophoneToScene();
		StartCoroutine(WarmUpMicrophoneCaptureAfterDelay());
		_sfxSource = base.gameObject.AddComponent<AudioSource>();
		_sfxSource.playOnAwake = false;
		_sfxSource.spatialBlend = 0f;
		SceneManager.sceneLoaded += OnSceneLoaded;
		PlayerRoleData.LocalRoleChanged += OnLocalRoleChanged;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		PlayerRoleData.LocalRoleChanged -= OnLocalRoleChanged;
	}

	private IEnumerator WarmUpMicrophoneCaptureAfterDelay()
	{
		yield return new WaitForSecondsRealtime(2f);
		DissonanceComms dissonanceComms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		if (dissonanceComms != null)
		{
			dissonanceComms.ResetMicrophoneCapture();
		}
	}

	private void OnLocalRoleChanged(PlayerRole _)
	{
		RefreshMimicMuteState();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		ApplyMaster(MasterVolume01);
		ApplyMusic(MusicVolume01);
		_localRoleData = null;
		RefreshMimicMuteState();
		ApplyVoiceModeToScene();
		ApplyMicrophoneToScene();
		_wasTransmitting = false;
	}

	private void Update()
	{
		if (_isRebindingPushToTalkKey)
		{
			TickRebindCapture();
		}
		else if (!(_trigger == null))
		{
			if (RoleAssignmentUI.IsAnimalRecordingActive)
			{
				_trigger.IsMuted = true;
			}
			else if (VoiceMode == CommActivationMode.PushToTalk)
			{
				_trigger.IsMuted = !Input.GetKey(PushToTalkKey);
			}
			else
			{
				_trigger.IsMuted = false;
			}
			bool flag = VoiceMode == CommActivationMode.PushToTalk && _trigger.IsTransmitting;
			if (flag && !_wasTransmitting && pushToTalkPressSfx != null)
			{
				_sfxSource.PlayOneShot(pushToTalkPressSfx, pushToTalkSfxVolume);
			}
			else if (!flag && _wasTransmitting && pushToTalkReleaseSfx != null)
			{
				_sfxSource.PlayOneShot(pushToTalkReleaseSfx, pushToTalkSfxVolume);
			}
			_wasTransmitting = flag;
		}
	}

	private void LoadSaved()
	{
		MasterVolume01 = PlayerPrefs.GetFloat("Settings.MasterVolume01", defaultMasterVolume01);
		MusicVolume01 = PlayerPrefs.GetFloat("Settings.MusicVolume01", defaultMusicVolume01);
		MimicVolume01 = PlayerPrefs.GetFloat("Settings.MimicVolume01", defaultMimicVolume01);
		VoiceMode = (CommActivationMode)PlayerPrefs.GetInt("Settings.VoiceMode", (int)defaultVoiceMode);
		if (VoiceMode == CommActivationMode.Open)
		{
			VoiceMode = defaultVoiceMode;
		}
		SelectedMicrophone = PlayerPrefs.GetString("Settings.Microphone", "");
		PushToTalkKey = (KeyCode)PlayerPrefs.GetInt("Settings.PushToTalkKey", (int)defaultPushToTalkKey);
		MuteNpcAnimals = PlayerPrefs.GetInt("Settings.MuteNpcAnimals", defaultMuteNpcAnimals ? 1 : 0) == 1;
	}

	public void PreviewMasterVolume01(float value01)
	{
		MasterVolume01 = Mathf.Clamp01(value01);
		ApplyMaster(MasterVolume01);
	}

	public void PreviewMusicVolume01(float value01)
	{
		MusicVolume01 = Mathf.Clamp01(value01);
		ApplyMusic(MusicVolume01);
	}

	public void PreviewMimicVolume01(float value01)
	{
		MimicVolume01 = Mathf.Clamp01(value01);
		RefreshMimicMuteState();
	}

	public void PreviewMuteNpcAnimals(bool value)
	{
		MuteNpcAnimals = value;
		RefreshMimicMuteState();
	}

	public void PreviewVoiceMode(CommActivationMode mode)
	{
		VoiceMode = mode;
		ApplyVoiceModeToScene();
	}

	public void PreviewMicrophone(string deviceName)
	{
		SelectedMicrophone = deviceName ?? "";
		ApplyMicrophoneToScene();
	}

	private void PreviewPushToTalkKey(KeyCode key)
	{
		PushToTalkKey = key;
	}

	public void BeginRebindPushToTalkKey()
	{
		_isRebindingPushToTalkKey = true;
	}

	public void CancelRebindPushToTalkKey()
	{
		_isRebindingPushToTalkKey = false;
	}

	private void TickRebindCapture()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_isRebindingPushToTalkKey = false;
			return;
		}
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(value))
			{
				PreviewPushToTalkKey(value);
				_isRebindingPushToTalkKey = false;
				break;
			}
		}
	}

	public void SaveCurrent()
	{
		PlayerPrefs.SetFloat("Settings.MasterVolume01", MasterVolume01);
		PlayerPrefs.SetFloat("Settings.MusicVolume01", MusicVolume01);
		PlayerPrefs.SetFloat("Settings.MimicVolume01", MimicVolume01);
		PlayerPrefs.SetInt("Settings.VoiceMode", (int)VoiceMode);
		PlayerPrefs.SetString("Settings.Microphone", SelectedMicrophone ?? "");
		PlayerPrefs.SetInt("Settings.PushToTalkKey", (int)PushToTalkKey);
		PlayerPrefs.SetInt("Settings.MuteNpcAnimals", MuteNpcAnimals ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void ResetToDefaults()
	{
		PreviewMasterVolume01(defaultMasterVolume01);
		PreviewMusicVolume01(defaultMusicVolume01);
		PreviewMimicVolume01(defaultMimicVolume01);
		PreviewPushToTalkKey(defaultPushToTalkKey);
		PreviewVoiceMode(defaultVoiceMode);
		PreviewMicrophone("");
		PreviewMuteNpcAnimals(defaultMuteNpcAnimals);
	}

	private void ApplyMaster(float value01)
	{
		ApplyDbLinear(masterVolumeParam, value01, masterDbAt50, masterDbAt100);
	}

	private void ApplyMusic(float value01)
	{
		ApplyDb(musicVolumeParam, value01);
	}

	private void ApplyMimic(float value01)
	{
		ApplyMimicDb(value01);
	}

	private void RefreshMimicMuteState()
	{
		if (!(mixer == null))
		{
			if (MuteNpcAnimals && IsLocalPlayerAnimal())
			{
				mixer.SetFloat(mimicVolumeParam, mutedDb);
			}
			else
			{
				ApplyMimic(MimicVolume01);
			}
		}
	}

	private bool IsLocalPlayerAnimal()
	{
		if (_localRoleData == null)
		{
			NetworkIdentity localPlayer = NetworkClient.localPlayer;
			if (localPlayer != null)
			{
				_localRoleData = localPlayer.GetComponent<PlayerRoleData>();
			}
		}
		if (_localRoleData != null)
		{
			return _localRoleData.Role == PlayerRole.Animal;
		}
		return false;
	}

	private void ApplyDb(string param, float value01)
	{
		if (!(mixer == null) && !string.IsNullOrEmpty(param))
		{
			float value2 = ((value01 <= 0.0001f) ? mutedDb : (Mathf.Log10(value01) * 20f));
			mixer.SetFloat(param, value2);
		}
	}

	private void ApplyDbLinear(string param, float value01, float dbAt50, float dbAt100)
	{
		if (!(mixer == null) && !string.IsNullOrEmpty(param))
		{
			if (value01 <= 0.0001f)
			{
				mixer.SetFloat(param, mutedDb);
				return;
			}
			float num = (dbAt100 - dbAt50) / 0.5f;
			float value2 = dbAt50 + (value01 - 0.5f) * num;
			mixer.SetFloat(param, value2);
		}
	}

	private void ApplyMimicDb(float value01)
	{
		if (!(mixer == null) && !string.IsNullOrEmpty(mimicVolumeParam))
		{
			float value2 = ((value01 <= 0.5f) ? Mathf.Lerp(mimicDbAt0, mimicDbAt50, value01 / 0.5f) : Mathf.Lerp(mimicDbAt50, mimicDbAt100, (value01 - 0.5f) / 0.5f));
			mixer.SetFloat(mimicVolumeParam, value2);
		}
	}

	private void ApplyVoiceModeToScene()
	{
		_trigger = UnityEngine.Object.FindObjectOfType<VoiceBroadcastTrigger>();
		if (!(_trigger == null))
		{
			if (VoiceMode == CommActivationMode.PushToTalk)
			{
				_trigger.Mode = CommActivationMode.Open;
				_trigger.IsMuted = true;
			}
			else
			{
				_trigger.Mode = VoiceMode;
				_trigger.IsMuted = false;
			}
		}
	}

	private void ApplyMicrophoneToScene()
	{
		_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		if (!(_comms == null))
		{
			string text = (string.IsNullOrEmpty(SelectedMicrophone) ? null : SelectedMicrophone);
			bool num = (string.IsNullOrEmpty(_comms.MicrophoneName) ? null : _comms.MicrophoneName) != text;
			_comms.MicrophoneName = text;
			if (num)
			{
				_comms.ResetMicrophoneCapture();
			}
		}
	}
}
