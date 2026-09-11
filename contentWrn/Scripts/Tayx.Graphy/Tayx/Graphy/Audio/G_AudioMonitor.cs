using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tayx.Graphy.Audio
{
	public class G_AudioMonitor : MonoBehaviour
	{
		private const float m_refValue = 1f;

		private GraphyManager m_graphyManager;

		private AudioListener m_audioListener;

		private GraphyManager.LookForAudioListener m_findAudioListenerInCameraIfNull = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

		private FFTWindow m_FFTWindow = FFTWindow.Blackman;

		private int m_spectrumSize = 512;

		public float[] Spectrum { get; private set; }

		public float[] SpectrumHighestValues { get; private set; }

		public float MaxDB { get; private set; }

		public bool SpectrumDataAvailable => m_audioListener != null;

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			if (m_audioListener != null)
			{
				AudioListener.GetOutputData(Spectrum, 0);
				float num = 0f;
				for (int i = 0; i < Spectrum.Length; i++)
				{
					num += Spectrum[i] * Spectrum[i];
				}
				float num2 = Mathf.Sqrt(num / (float)Spectrum.Length);
				MaxDB = 20f * Mathf.Log10(num2 / 1f);
				if (MaxDB < -80f)
				{
					MaxDB = -80f;
				}
				AudioListener.GetSpectrumData(Spectrum, 0, m_FFTWindow);
				for (int j = 0; j < Spectrum.Length; j++)
				{
					if (Spectrum[j] > SpectrumHighestValues[j])
					{
						SpectrumHighestValues[j] = Spectrum[j];
					}
					else
					{
						SpectrumHighestValues[j] = Mathf.Clamp(SpectrumHighestValues[j] - SpectrumHighestValues[j] * Time.deltaTime * 2f, 0f, 1f);
					}
				}
			}
			else if (m_audioListener == null && m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ALWAYS)
			{
				m_audioListener = FindAudioListener();
			}
		}

		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		public void UpdateParameters()
		{
			m_findAudioListenerInCameraIfNull = m_graphyManager.FindAudioListenerInCameraIfNull;
			m_audioListener = m_graphyManager.AudioListener;
			m_FFTWindow = m_graphyManager.FftWindow;
			m_spectrumSize = m_graphyManager.SpectrumSize;
			if (m_audioListener == null && m_findAudioListenerInCameraIfNull != GraphyManager.LookForAudioListener.NEVER)
			{
				m_audioListener = FindAudioListener();
			}
			Spectrum = new float[m_spectrumSize];
			SpectrumHighestValues = new float[m_spectrumSize];
		}

		public float lin2dB(float linear)
		{
			return Mathf.Clamp(Mathf.Log10(linear) * 20f, -160f, 0f);
		}

		public float dBNormalized(float db)
		{
			return (db + 160f) / 160f;
		}

		private AudioListener FindAudioListener()
		{
			Camera main = Camera.main;
			if (main != null && main.TryGetComponent<AudioListener>(out var component))
			{
				return component;
			}
			return null;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ON_SCENE_LOAD)
			{
				m_audioListener = FindAudioListener();
			}
		}

		private void Init()
		{
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			UpdateParameters();
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}
}
