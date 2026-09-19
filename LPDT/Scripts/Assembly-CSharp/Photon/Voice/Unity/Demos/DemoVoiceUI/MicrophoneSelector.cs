using System;
using System.Collections.Generic;
using FMODUnity;
using Photon.Voice.FMOD;
using Photon.Voice.Unity.UtilityScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Photon.Voice.Unity.Demos.DemoVoiceUI
{
	public class MicrophoneSelector : VoiceComponent
	{
		public class MicrophoneSelectorEvent : UnityEvent<MicType, DeviceInfo>
		{
		}

		public MicrophoneSelectorEvent onValueChanged = new MicrophoneSelectorEvent();

		private List<MicRef> micOptions;

		[SerializeField]
		private Dropdown micDropdown;

		[SerializeField]
		private Slider micLevelSlider;

		[SerializeField]
		private Recorder recorder;

		[SerializeField]
		[FormerlySerializedAs("RefreshButton")]
		private GameObject refreshButton;

		private Image fillArea;

		private Color defaultFillColor = Color.white;

		private Color speakingFillColor = Color.green;

		private IDeviceEnumerator unityMicEnum;

		private IDeviceEnumerator photonMicEnum;

		private IDeviceEnumerator fmodMicEnum;

		private Func<IAudioDesc> fmodInputFactory;

		protected override void Awake()
		{
			base.Awake();
			unityMicEnum = new AudioInEnumerator(base.Logger);
			fmodMicEnum = new Photon.Voice.FMOD.AudioInEnumerator(RuntimeManager.CoreSystem, base.Logger);
			photonMicEnum = Platform.CreateAudioInEnumerator(base.Logger);
			photonMicEnum.OnReady = delegate
			{
				SetupMicDropdown();
				SetCurrentValue();
			};
			refreshButton.GetComponentInChildren<Button>().onClick.AddListener(RefreshMicrophones);
			fillArea = micLevelSlider.fillRect.GetComponent<Image>();
			defaultFillColor = fillArea.color;
		}

		private void Update()
		{
			if (recorder != null)
			{
				micLevelSlider.value = recorder.LevelMeter.CurrentPeakAmp;
				fillArea.color = (recorder.IsCurrentlyTransmitting ? speakingFillColor : defaultFillColor);
			}
		}

		private void OnEnable()
		{
			MicrophonePermission.MicrophonePermissionCallback += OnMicrophonePermissionCallback;
		}

		private void OnMicrophonePermissionCallback(bool granted)
		{
			RefreshMicrophones();
		}

		private void OnDisable()
		{
			MicrophonePermission.MicrophonePermissionCallback -= OnMicrophonePermissionCallback;
		}

		private void SetupMicDropdown()
		{
			micDropdown.ClearOptions();
			micOptions = new List<MicRef>();
			List<string> list = new List<string>();
			micOptions.Add(new MicRef(MicType.Unity, DeviceInfo.Default));
			list.Add($"[Unity]\u00a0[Default]");
			foreach (DeviceInfo item in unityMicEnum)
			{
				micOptions.Add(new MicRef(MicType.Unity, item));
				list.Add($"[Unity]\u00a0{item}");
			}
			micOptions.Add(new MicRef(MicType.Photon, DeviceInfo.Default));
			list.Add($"[Photon]\u00a0[Default]");
			foreach (DeviceInfo item2 in photonMicEnum)
			{
				micOptions.Add(new MicRef(MicType.Photon, item2));
				list.Add($"[Photon]\u00a0{item2}");
			}
			foreach (DeviceInfo item3 in fmodMicEnum)
			{
				micOptions.Add(new MicRef(MicType.FMOD, item3));
				list.Add($"[FMOD]\u00a0{item3}");
			}
			micDropdown.AddOptions(list);
			micDropdown.onValueChanged.RemoveAllListeners();
			micDropdown.onValueChanged.AddListener(delegate
			{
				SwitchToSelectedMic();
			});
		}

		public void SwitchToSelectedMic()
		{
			MicRef mic = micOptions[micDropdown.value];
			switch (mic.MicType)
			{
			case MicType.Unity:
				recorder.SourceType = Recorder.InputSourceType.Microphone;
				recorder.MicrophoneType = Recorder.MicType.Unity;
				recorder.MicrophoneDevice = mic.Device;
				break;
			case MicType.Photon:
				recorder.SourceType = Recorder.InputSourceType.Microphone;
				recorder.MicrophoneType = Recorder.MicType.Photon;
				recorder.MicrophoneDevice = mic.Device;
				break;
			case MicType.FMOD:
				recorder.SourceType = Recorder.InputSourceType.Factory;
				recorder.InputFactory = (fmodInputFactory = () => new AudioInReader<short>(RuntimeManager.CoreSystem, (!mic.Device.IsDefault) ? mic.Device.IDInt : 0, (int)recorder.SamplingRate, base.Logger));
				break;
			}
			onValueChanged?.Invoke(mic.MicType, mic.Device);
		}

		private void SetCurrentValue()
		{
			if (micOptions == null)
			{
				Debug.LogWarning("micOptions list is null");
				return;
			}
			micDropdown.gameObject.SetActive(value: true);
			refreshButton.SetActive(value: true);
			for (int i = 0; i < micOptions.Count; i++)
			{
				MicRef micRef = micOptions[i];
				if ((micRef.MicType == MicType.Unity && recorder.SourceType == Recorder.InputSourceType.Microphone && recorder.MicrophoneType == Recorder.MicType.Unity) || (micRef.MicType == MicType.Photon && recorder.SourceType == Recorder.InputSourceType.Microphone && recorder.MicrophoneType == Recorder.MicType.Photon) || (micRef.MicType == MicType.FMOD && recorder.SourceType == Recorder.InputSourceType.Factory && recorder.InputFactory == fmodInputFactory))
				{
					micDropdown.value = i;
					break;
				}
			}
		}

		public void RefreshMicrophones()
		{
			unityMicEnum.Refresh();
			photonMicEnum.Refresh();
			fmodMicEnum.Refresh();
		}

		private void PhotonVoiceCreated()
		{
			RefreshMicrophones();
		}
	}
}
