using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MicRecorder))]
public class VoiceRecordTester : MonoBehaviour
{
	[Header("Hangi hayvan için kaydediyoruz")]
	public AnimalType recordFor = AnimalType.Cow;

	private MicRecorder _rec;

	private void Awake()
	{
		_rec = GetComponent<MicRecorder>();
	}

	[ContextMenu("1) Start Recording")]
	private void StartRec()
	{
		if (!Application.isPlaying)
		{
			Debug.LogWarning("Play mode gerekli.");
		}
		else if (_rec.StartRecording())
		{
			Debug.Log($"[Tester] Kayıt başladı: {recordFor} (max {_rec.maxSeconds}sn). Konuş!");
			StartCoroutine(AutoStop());
		}
	}

	private IEnumerator AutoStop()
	{
		yield return new WaitForSeconds(_rec.maxSeconds);
		if (_rec.IsRecording)
		{
			Debug.Log("[Tester] Max süre doldu, otomatik durduruluyor.");
			StopRec();
		}
	}

	[ContextMenu("2) Stop Recording & Store")]
	private void StopRec()
	{
		if (Application.isPlaying)
		{
			if (!_rec.StopRecording(out var outPcm, out var outSampleRate, out var outChannels))
			{
				Debug.LogWarning("[Tester] Kayıt boş/sessiz ya da geçersiz — depolanmadı.");
			}
			else if (VoiceClipStore.Instance == null)
			{
				Debug.LogError("[Tester] VoiceClipStore sahnede yok!");
			}
			else
			{
				VoiceClipStore.Instance.SetClipFromPcm(recordFor, outPcm, outSampleRate, outChannels);
			}
		}
	}
}
