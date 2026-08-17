using System;
using System.Collections.Generic;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	[HelpURL("https://man572142s-organization.gitbook.io/broaudio/core-features/no-code-components/spectrum-analyzer")]
	[AddComponentMenu("BroAudio/SpectrumAnalyzer")]
	public class SpectrumAnalyzer : MonoBehaviour
	{
		public enum Channel
		{
			Left = 0,
			Right = 1
		}

		public enum Metering
		{
			Peak = 0,
			RMS = 1,
			Average = 2
		}

		[Serializable]
		public class Band
		{
			public static class NameOf
			{
				public const string Weighted = "_weighted";
			}

			public float Frequency;

			[SerializeField]
			[Min(1f)]
			private float _weighted;

			public float Amplitube { get; private set; } = 0.0001f;

			public float DecibelVolume { get; private set; } = -80f;

			public void SetVolume(float amp, float dB)
			{
				Amplitube = amp;
				DecibelVolume = dB;
			}
		}

		public static class NameOf
		{
			public const string SoundSource = "_soundSource";

			public const string ResolutionScale = "_resolutionScale";

			public const string Channel = "_channel";

			public const string WindowType = "_windowType";

			public const string Bands = "_bands";

			public const string Metering = "_metering";

			public const string Attack = "_attack";

			public const string Decay = "_decay";

			public const string Smooth = "_smooth";
		}

		public const float MaxVolumeChange = 20f;

		[SerializeField]
		private SoundSource _soundSource;

		[Space]
		[SerializeField]
		[Range(6f, 13f)]
		private int _resolutionScale = 10;

		[SerializeField]
		private Channel _channel;

		[SerializeField]
		private FFTWindow _windowType;

		[SerializeField]
		private Metering _metering;

		[SerializeField]
		private int _attack = 100;

		[SerializeField]
		private int _decay = 1500;

		[SerializeField]
		private int _smooth;

		[SerializeField]
		private Band[] _bands;

		private float[] _spectrum;

		private float _harmonic;

		private IAudioPlayer _player;

		private bool _isUsingSoundSource;

		private int SpectrumSampleCount => 1 << _resolutionScale;

		public int BandCount => _bands.Length;

		public IReadOnlyList<Band> Bands => _bands;

		public IReadOnlyList<float> Spectrum => _spectrum;

		public event Action<IReadOnlyList<Band>> OnUpdate;

		public void SetSource(IAudioPlayer audioPlayer)
		{
			_player = audioPlayer;
		}

		private void Start()
		{
			_spectrum = new float[SpectrumSampleCount];
			float num = (float)AudioSettings.outputSampleRate / 2f;
			_harmonic = num / (float)SpectrumSampleCount;
			_isUsingSoundSource = _soundSource != null;
		}

		private void Update()
		{
			if (_player == null && _isUsingSoundSource)
			{
				_player = _soundSource.CurrentPlayer;
			}
			if (_player != null && _player.IsPlaying)
			{
				_player.GetSpectrumData(_spectrum, (int)_channel, _windowType);
				UpdateSpectrum();
			}
		}

		private void UpdateSpectrum()
		{
			for (int i = 0; i < _bands.Length; i++)
			{
				float minFreq = ((i > 0) ? _bands[i - 1].Frequency : 10f);
				float num = GetLatestAmp(minFreq, _bands[i].Frequency).ToDecibel();
				float decibelVolume = _bands[i].DecibelVolume;
				float num2 = num - decibelVolume;
				float num3 = Mathf.Sign(num2);
				float num4 = ((num2 > 0f) ? _attack : _decay);
				float num6;
				if (_smooth > 0)
				{
					float num5 = num2 / (float)_smooth;
					num6 = Mathf.Clamp(20f * num5, -20f, 20f);
				}
				else
				{
					num6 = 20f * num3;
				}
				float deltaTime = Utility.GetDeltaTime();
				float num7 = ((num4 > 0f) ? (deltaTime * 1000f * (num6 / num4)) : 3.4028235E+38f);
				decibelVolume = ((!(num2 * num3 <= num7)) ? (decibelVolume + num7) : num);
				_bands[i].SetVolume(decibelVolume.ToNormalizeVolume(), decibelVolume);
			}
			this.OnUpdate?.Invoke(_bands);
			float GetAverage(RangeInt range)
			{
				float num8 = 0f;
				for (int j = range.start; j <= range.end; j++)
				{
					num8 += _spectrum[j];
				}
				return num8 / (float)range.length;
			}
			float GetLatestAmp(float minFreq2, float maxFreq)
			{
				RangeInt frequencyRangeIndex = GetFrequencyRangeIndex(minFreq2, maxFreq);
				return _metering switch
				{
					Metering.Peak => GetPeak(frequencyRangeIndex), 
					Metering.RMS => GetRMS(frequencyRangeIndex), 
					Metering.Average => GetAverage(frequencyRangeIndex), 
					_ => throw new NotImplementedException(), 
				};
			}
			float GetPeak(RangeInt range)
			{
				float num8 = 0f;
				for (int j = range.start; j <= range.end; j++)
				{
					if (_spectrum[j] > num8)
					{
						num8 = _spectrum[j];
					}
				}
				return num8;
			}
			float GetRMS(RangeInt range)
			{
				float num8 = 0f;
				for (int j = range.start; j <= range.end; j++)
				{
					num8 += Mathf.Pow(_spectrum[j], 2f);
				}
				return Mathf.Sqrt(num8 / (float)range.length);
			}
		}

		private RangeInt GetFrequencyRangeIndex(float minFreq, float maxFreq)
		{
			RangeInt result = default(RangeInt);
			result.start = Mathf.CeilToInt(minFreq / _harmonic);
			int num = Mathf.FloorToInt(maxFreq / _harmonic);
			result.length = num - result.start;
			return result;
		}
	}
}
