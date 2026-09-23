using System.Collections.Generic;
using Mimicraft.Analytics;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Voice;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ModelerVoiceView : MonoBehaviour
	{
		[Header("Kayıt")]
		[Tooltip("Hangi mikrofonun kullanılacağı. Ayarlardaki ile AYNI seçimi yazar - buradan değiştirmek ayarlardan değiştirmekle aynı şeydir. Seçenekleri bu bileşen doldurur.")]
		[SerializeField]
		private TMP_Dropdown deviceDropdown;

		[Tooltip("Kayda başlatan/bitiren buton. Basılınca kayıt başlar, tekrar basılınca biter - süre dolunca da kendiliğinden biter.")]
		[SerializeField]
		private Button recordButton;

		[Tooltip("Kayıt butonunun yazısı. Kayıt sırasında 'Durdur' olur; yoksa buton yazısı sabit kalır.")]
		[SerializeField]
		private TextMeshProUGUI recordLabel;

		[Tooltip("Kayıt sırasında soldan sağa çizilen dalga formu. Kayıt bitince son kaydın şekli ekranda kalır; kayıt yokken hareket etmez. Mikrofonun çalıştığını gösteren şey aşağıdaki seviye çubuğudur.")]
		[SerializeField]
		private TauntWaveformView liveWaveform;

		[Tooltip("Mikrofonun anlık seviyesi. Image Type = Filled olmalı.")]
		[SerializeField]
		private Image levelFill;

		[Tooltip("Ses algılanınca çubuğun alacağı renk. Sessizlikte aşağıdaki sönük renk kullanılır.")]
		[SerializeField]
		private Color hearingColor = new Color(0.35f, 0.85f, 0.4f);

		[SerializeField]
		private Color quietColor = new Color(0.55f, 0.55f, 0.55f);

		[Tooltip("Kullanılan sesi dinleten buton. Kayıt alır almaz o kaydı, kütüphaneden başka bir ses seçince onu çalar. Kütüphanedeki satırların kendi dinleme butonları bundan bağımsızdır.")]
		[SerializeField]
		private Button previewButton;

		[Tooltip("İsteğe bağlı: ses çalarken dolan çubuk (Image Type = Filled). Dalga formu zaten çalınan kısmı renklendiriyor; ayrı bir çubuk isteyen için.")]
		[SerializeField]
		private Image previewFill;

		[Tooltip("Durum yazısı: ne yapılacağı, kaydın reddedilme sebebi, kaç saniye kaydedildiği.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[Header("Kütüphane")]
		[Tooltip("Satırların ekleneceği kap - dikey Layout Group'lu bir içerik objesi.")]
		[SerializeField]
		private RectTransform listContent;

		[Tooltip("Bir satırın şablonu. Üzerinde TauntVoiceRowView olmalı. Kapalı bırak: kopyalanır.")]
		[SerializeField]
		private TauntVoiceRowView rowTemplate;

		[Tooltip("Hiç kayıt yokken gösterilecek yazı. Kayıt varken gizlenir.")]
		[SerializeField]
		private GameObject emptyNotice;

		private readonly TauntVoiceRecorder recorder = new TauntVoiceRecorder();

		private readonly List<string> devices = new List<string>();

		private readonly List<TauntVoiceRowView> rows = new List<TauntVoiceRowView>();

		private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

		private AudioSource preview;

		private string statusKey;

		private TauntWaveformView playingOn;

		private TauntVoiceRowView playingRow;

		private float playStartedAt;

		private float playLength;

		private int drawnPeaks = -1;

		private void Awake()
		{
			preview = base.gameObject.AddComponent<AudioSource>();
			preview.playOnAwake = false;
			preview.spatialBlend = 0f;
			preview.outputAudioMixerGroup = AudioLibrary.SfxGroup;
			if (recordButton != null)
			{
				recordButton.onClick.AddListener(ToggleRecording);
			}
			if (previewButton != null)
			{
				previewButton.onClick.AddListener(delegate
				{
					Play(TauntVoiceLibrary.SelectedId, liveWaveform, null);
				});
			}
			UITooltipTrigger.AttachKey(recordButton, "Tooltip.Taunt.Record");
			UITooltipTrigger.AttachKey(previewButton, "Tooltip.Taunt.Preview");
			UITooltipTrigger.AttachKey(deviceDropdown, "Tooltip.Taunt.Device");
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			if (deviceDropdown != null)
			{
				deviceDropdown.onValueChanged.AddListener(PickDevice);
			}
		}

		private void OnEnable()
		{
			RebuildDeviceDropdown();
			statusKey = (recorder.Listen(out var problem) ? "Taunt.Ready" : problem);
			Rebuild();
			ShowSelectedWaveform();
			Refresh();
		}

		private void OnDisable()
		{
			if (recorder.IsRecording)
			{
				Finish();
			}
			preview.Stop();
			StopFollowing();
			recorder.Close();
			Forget();
		}

		private void OnDestroy()
		{
			recorder.Dispose();
			Forget();
		}

		private void Update()
		{
			recorder.Tick();
			if (recorder.IsRecording && recorder.Full)
			{
				Finish();
			}
			DrawLiveWaveform();
			FollowPlayback();
			Refresh();
		}

		private void ShowSelectedWaveform()
		{
			if (!(liveWaveform == null))
			{
				liveWaveform.Show(PeaksOf(TauntVoiceLibrary.SelectedId), 100);
				drawnPeaks = recorder.Peaks.Count;
			}
		}

		private void DrawLiveWaveform()
		{
			if (!(liveWaveform == null) && drawnPeaks != recorder.Peaks.Count)
			{
				drawnPeaks = recorder.Peaks.Count;
				liveWaveform.Show(recorder.Peaks, 100);
			}
		}

		private void RebuildDeviceDropdown()
		{
			if (deviceDropdown == null)
			{
				return;
			}
			devices.Clear();
			string[] array = Microphone.devices;
			if (array != null)
			{
				devices.AddRange(array);
			}
			if (devices.Count == 0)
			{
				Fill(new List<string> { Loc.Get("Audio.NoMicrophone") }, 0);
				deviceDropdown.interactable = false;
				return;
			}
			deviceDropdown.interactable = true;
			int num = devices.IndexOf(GameSettings.MicDevice);
			Fill(new List<string>(devices), (num >= 0) ? num : 0);
			if (num < 0 && !string.IsNullOrEmpty(GameSettings.MicDevice))
			{
				GameSettings.SetMicDevice(devices[0]);
			}
		}

		private void Fill(List<string> labels, int value)
		{
			deviceDropdown.ClearOptions();
			deviceDropdown.AddOptions(labels);
			deviceDropdown.SetValueWithoutNotify(Mathf.Clamp(value, 0, Mathf.Max(0, labels.Count - 1)));
			deviceDropdown.RefreshShownValue();
		}

		private void PickDevice(int index)
		{
			if (index >= 0 && index < devices.Count)
			{
				GameSettings.SetMicDevice(devices[index]);
				if (recorder.IsRecording)
				{
					Finish();
				}
				recorder.Close();
				statusKey = (recorder.Listen(out var problem) ? "Taunt.Ready" : problem);
				Refresh();
			}
		}

		private void ToggleRecording()
		{
			if (recorder.IsRecording)
			{
				Finish();
				return;
			}
			statusKey = (recorder.Begin(out var problem) ? "Taunt.Recording" : problem);
			drawnPeaks = -1;
			Refresh();
		}

		private void Finish()
		{
			if (!recorder.End(out var payload, out var rejection))
			{
				statusKey = rejection;
				Refresh();
				return;
			}
			string value = TauntVoiceLibrary.Add(payload);
			Telemetry.Send("taunt_recorded", ("saved", !string.IsNullOrEmpty(value)));
			if (string.IsNullOrEmpty(value))
			{
				statusKey = ((TauntVoiceLibrary.List().Count >= 20) ? "Taunt.LibraryFull" : "Taunt.SaveFailed");
				Refresh();
				return;
			}
			statusKey = "Taunt.Saved";
			Rebuild();
			Publish();
			Refresh();
		}

		private void Rebuild()
		{
			StopFollowing();
			foreach (TauntVoiceRowView row in rows)
			{
				if (row != null)
				{
					Object.Destroy(row.gameObject);
				}
			}
			rows.Clear();
			List<TauntVoiceLibrary.Entry> list = TauntVoiceLibrary.List();
			string selectedId = TauntVoiceLibrary.SelectedId;
			string forcedSelectedId = TauntVoiceLibrary.ForcedSelectedId;
			if (emptyNotice != null && emptyNotice.activeSelf != (list.Count == 0))
			{
				emptyNotice.SetActive(list.Count == 0);
			}
			if (rowTemplate == null || listContent == null)
			{
				return;
			}
			foreach (TauntVoiceLibrary.Entry item in list)
			{
				TauntVoiceRowView tauntVoiceRowView = Object.Instantiate(rowTemplate, listContent);
				tauntVoiceRowView.gameObject.SetActive(value: true);
				TauntVoiceLibrary.Entry captured = item;
				TauntVoiceRowView boundRow = tauntVoiceRowView;
				tauntVoiceRowView.Bind(captured, PeaksOf(captured.Id), captured.Id == selectedId, captured.Id == forcedSelectedId, delegate
				{
					Play(captured.Id, null, boundRow);
				}, delegate
				{
					Use(captured.Id);
				}, delegate
				{
					UseForced(captured.Id);
				}, delegate
				{
					Remove(captured.Id);
				});
				rows.Add(tauntVoiceRowView);
			}
		}

		private void Use(string id)
		{
			TauntVoiceLibrary.SelectedId = id;
			statusKey = "Taunt.Wearing";
			Rebuild();
			ShowSelectedWaveform();
			Publish();
			Refresh();
		}

		private void UseForced(string id)
		{
			TauntVoiceLibrary.ForcedSelectedId = id;
			statusKey = "Taunt.WearingForced";
			Rebuild();
			Publish();
			Refresh();
		}

		private void Remove(string id)
		{
			TauntVoiceLibrary.Delete(id);
			if (clips.TryGetValue(id, out var value))
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
				clips.Remove(id);
			}
			statusKey = "Taunt.Deleted";
			Rebuild();
			ShowSelectedWaveform();
			Publish();
			Refresh();
		}

		private void Play(string id, TauntWaveformView waveform, TauntVoiceRowView row)
		{
			AudioClip audioClip = ClipOf(id);
			if (!(audioClip == null))
			{
				StopFollowing();
				preview.Stop();
				preview.PlayOneShot(audioClip);
				playingOn = waveform;
				playingRow = row;
				playStartedAt = Time.unscaledTime;
				playLength = audioClip.length;
			}
		}

		private void FollowPlayback()
		{
			if (playingOn == null && playingRow == null)
			{
				if (previewFill != null && previewFill.fillAmount != 0f)
				{
					previewFill.fillAmount = 0f;
				}
				return;
			}
			float num = Time.unscaledTime - playStartedAt;
			float num2 = ((playLength > 0f) ? Mathf.Clamp01(num / playLength) : 1f);
			if (playingOn != null)
			{
				playingOn.Progress = num2;
			}
			if (playingRow != null)
			{
				playingRow.SetProgress(num2);
			}
			if (previewFill != null)
			{
				previewFill.fillAmount = num2;
			}
			if (num >= playLength)
			{
				StopFollowing();
			}
		}

		private void StopFollowing()
		{
			if (playingOn != null)
			{
				playingOn.Progress = 0f;
			}
			if (playingRow != null)
			{
				playingRow.SetProgress(0f);
			}
			if (previewFill != null)
			{
				previewFill.fillAmount = 0f;
			}
			playingOn = null;
			playingRow = null;
		}

		private AudioClip ClipOf(string id)
		{
			if (clips.TryGetValue(id, out var value))
			{
				return value;
			}
			byte[] array = TauntVoiceLibrary.Read(id);
			float[] samples;
			string rejection;
			AudioClip audioClip = ((array.Length != 0 && TauntVoiceClip.TryDecode(array, out samples, out rejection)) ? TauntVoiceClip.ToClip(samples, id) : null);
			clips[id] = audioClip;
			return audioClip;
		}

		private float[] PeaksOf(string id)
		{
			byte[] array = TauntVoiceLibrary.Read(id);
			if (array.Length == 0 || !TauntVoiceClip.TryDecode(array, out var samples, out var _))
			{
				return null;
			}
			return TauntVoiceClip.Peaks(samples);
		}

		private void Forget()
		{
			foreach (AudioClip value in clips.Values)
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
			}
			clips.Clear();
		}

		private static void Publish()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			NetworkObject networkObject = ((singleton != null && singleton.IsClient) ? singleton.LocalClient.PlayerObject : null);
			if (networkObject != null && networkObject.TryGetComponent<PlayerTauntVoice>(out var component))
			{
				component.SubmitSaved();
			}
		}

		private void Refresh()
		{
			if (levelFill != null)
			{
				levelFill.fillAmount = Mathf.Clamp01(recorder.Level * 6f);
				levelFill.color = (recorder.Hearing ? hearingColor : quietColor);
			}
			if (recordLabel != null)
			{
				recordLabel.text = Loc.Get(recorder.IsRecording ? "Taunt.Stop" : "Taunt.Record");
			}
			if (previewButton != null)
			{
				previewButton.interactable = !recorder.IsRecording && !string.IsNullOrEmpty(TauntVoiceLibrary.SelectedId);
			}
			if (!(statusLabel == null))
			{
				statusLabel.text = (recorder.IsRecording ? Loc.Format("Taunt.RecordingSeconds", recorder.Seconds.ToString("0.0"), 2f.ToString("0.0")) : (string.IsNullOrEmpty(statusKey) ? "" : Loc.Get(statusKey)));
			}
		}
	}
}
