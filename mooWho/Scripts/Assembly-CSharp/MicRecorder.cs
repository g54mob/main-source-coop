using System;
using UnityEngine;

public class MicRecorder : MonoBehaviour
{
	[Header("Kayıt Ayarları")]
	public int maxSeconds = 3;

	[Tooltip("Artık kaydı reddetmiyor — sadece Debug.Log'daki referans/tanı değeri. 'Too quiet' kararı RoleAssignmentUI'de Dissonance amplitude tepe değeriyle veriliyor.")]
	public float silenceRmsThreshold = 0.01f;

	[Header("Trim (mouse click temizleme)")]
	[Tooltip("Kayıt başından kesilecek süre (sn)")]
	public float trimStart = 0.02f;

	[Tooltip("Kayıt sonundan kesilecek süre (sn)")]
	public float trimEnd = 0.1f;

	private string _device;

	private AudioClip _recordingClip;

	private int _sampleRate;

	public bool IsRecording { get; private set; }

	public bool MicrophoneAvailable
	{
		get
		{
			if (Microphone.devices != null)
			{
				return Microphone.devices.Length != 0;
			}
			return false;
		}
	}

	private string ResolveDevice()
	{
		string text = ((AudioSettingsManager.Instance != null) ? AudioSettingsManager.Instance.SelectedMicrophone : null);
		if (!string.IsNullOrEmpty(text) && Array.IndexOf(Microphone.devices, text) >= 0)
		{
			return text;
		}
		return Microphone.devices[0];
	}

	public bool StartRecording()
	{
		if (IsRecording)
		{
			return false;
		}
		if (!MicrophoneAvailable)
		{
			Debug.LogWarning("[MicRecorder] Mikrofon bulunamadı.");
			return false;
		}
		_device = ResolveDevice();
		Microphone.GetDeviceCaps(_device, out var minFreq, out var maxFreq);
		_sampleRate = ((maxFreq == 0) ? 24000 : Mathf.Clamp(24000, minFreq, maxFreq));
		_recordingClip = Microphone.Start(_device, loop: false, maxSeconds, _sampleRate);
		IsRecording = true;
		return true;
	}

	public bool StopRecording(out byte[] outPcm, out int outSampleRate, out int outChannels)
	{
		outPcm = null;
		outSampleRate = _sampleRate;
		outChannels = 1;
		if (!IsRecording)
		{
			return false;
		}
		int position = Microphone.GetPosition(_device);
		Microphone.End(_device);
		IsRecording = false;
		if (_recordingClip == null || position <= 0)
		{
			return false;
		}
		int channels = _recordingClip.channels;
		float[] array = new float[position * channels];
		_recordingClip.GetData(array, 0);
		array = TrimSamples(array, _sampleRate, channels, trimStart, trimEnd);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		float num = ComputeRms(array);
		Debug.Log($"[MicRecorder] Kayıt RMS: {num:F4} (referans eşik: {silenceRmsThreshold}), " + $"trim sonrası sample: {array.Length}");
		outPcm = FloatTo16BitPcm(array);
		outSampleRate = _sampleRate;
		outChannels = channels;
		return true;
	}

	public void CancelRecording()
	{
		if (IsRecording)
		{
			Microphone.End(_device);
			IsRecording = false;
		}
	}

	public float GetCurrentLevel()
	{
		if (!IsRecording || _recordingClip == null)
		{
			return 0f;
		}
		int position = Microphone.GetPosition(_device);
		if (position < 128)
		{
			return 0f;
		}
		float[] array = new float[128];
		_recordingClip.GetData(array, Mathf.Max(0, position - 128));
		return ComputeRms(array);
	}

	private static float[] TrimSamples(float[] samples, int sampleRate, int channels, float startSec, float endSec)
	{
		if (samples == null || samples.Length == 0)
		{
			return samples;
		}
		int b = Mathf.RoundToInt(startSec * (float)sampleRate) * channels;
		int b2 = Mathf.RoundToInt(endSec * (float)sampleRate) * channels;
		b = Mathf.Max(0, b);
		b2 = Mathf.Max(0, b2);
		if (b + b2 >= samples.Length)
		{
			Debug.LogWarning("[MicRecorder] Trim kaydın tamamını keserdi, atlanıyor.");
			return samples;
		}
		int num = samples.Length - b - b2;
		float[] array = new float[num];
		Array.Copy(samples, b, array, 0, num);
		return array;
	}

	private static float ComputeRms(float[] samples)
	{
		if (samples == null || samples.Length == 0)
		{
			return 0f;
		}
		double num = 0.0;
		for (int i = 0; i < samples.Length; i++)
		{
			num += (double)(samples[i] * samples[i]);
		}
		return Mathf.Sqrt((float)(num / (double)samples.Length));
	}

	private static byte[] FloatTo16BitPcm(float[] samples)
	{
		byte[] array = new byte[samples.Length * 2];
		int num = 0;
		for (int i = 0; i < samples.Length; i++)
		{
			short num2 = (short)Mathf.Clamp(samples[i] * 32767f, -32768f, 32767f);
			array[num++] = (byte)(num2 & 0xFF);
			array[num++] = (byte)((num2 >> 8) & 0xFF);
		}
		return array;
	}

	public static AudioClip PcmToAudioClip(byte[] pcm, int sampleRate, int channels, string name = "VoiceClip")
	{
		int num = pcm.Length / 2;
		float[] array = new float[num];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			short num3 = (short)(pcm[num2] | (pcm[num2 + 1] << 8));
			array[i] = (float)num3 / 32768f;
			num2 += 2;
		}
		int lengthSamples = Mathf.Max(1, num / Mathf.Max(1, channels));
		AudioClip audioClip = AudioClip.Create(name, lengthSamples, channels, sampleRate, stream: false);
		audioClip.SetData(array, 0);
		return audioClip;
	}
}
