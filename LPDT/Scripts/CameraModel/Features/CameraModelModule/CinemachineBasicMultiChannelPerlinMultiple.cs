using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.CameraModelModule
{
	[CameraPipeline(CinemachineCore.Stage.Noise)]
	public class CinemachineBasicMultiChannelPerlinMultiple : CinemachineComponentBase, CinemachineFreeLookModifier.IModifiableNoise
	{
		[Serializable]
		public class WeightedNoiseProfile
		{
			[Tooltip("The type identifier for this noise profile")]
			public NoiseType NoiseType;

			[Tooltip("The NoiseSettings asset to use")]
			public NoiseSettings NoiseProfile;

			[Tooltip("The weight/influence of this noise profile (0-1). All weights will be normalized.")]
			[Range(0f, 1f)]
			public float Weight;
		}

		[Tooltip("Multiple noise profiles that will be blended together based on their weights. Weights are automatically normalized.")]
		public WeightedNoiseProfile[] NoiseProfiles = new WeightedNoiseProfile[0];

		private Dictionary<NoiseType, WeightedNoiseProfile> m_NoiseProfileDict;

		private Dictionary<NoiseType, int> m_NoiseIndexDict;

		[Tooltip("When rotating the camera, offset the camera's pivot position by this much (camera space)")]
		[FormerlySerializedAs("m_PivotOffset")]
		public Vector3 PivotOffset = Vector3.zero;

		[Tooltip("Gain to apply to the amplitudes defined in the NoiseSettings asset.  1 is normal.  Setting this to 0 completely mutes the noise.")]
		[FormerlySerializedAs("m_AmplitudeGain")]
		public float AmplitudeGain = 1f;

		[Tooltip("Scale factor to apply to the frequencies defined in the NoiseSettings asset.  1 is normal.  Larger magnitudes will make the noise shake more rapidly.")]
		[FormerlySerializedAs("m_FrequencyGain")]
		public float FrequencyGain = 1f;

		private bool m_Initialized;

		private float m_NoiseTime;

		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		private Vector3[] m_NoiseOffsets;

		(float, float) CinemachineFreeLookModifier.IModifiableNoise.NoiseAmplitudeFrequency
		{
			get
			{
				return (AmplitudeGain, FrequencyGain);
			}
			set
			{
				(AmplitudeGain, FrequencyGain) = value;
			}
		}

		public override bool IsValid
		{
			get
			{
				if (!base.enabled || NoiseProfiles == null || NoiseProfiles.Length == 0)
				{
					return false;
				}
				WeightedNoiseProfile[] noiseProfiles = NoiseProfiles;
				foreach (WeightedNoiseProfile weightedNoiseProfile in noiseProfiles)
				{
					if (weightedNoiseProfile != null && weightedNoiseProfile.NoiseProfile != null && weightedNoiseProfile.Weight > 0f)
					{
						return true;
					}
				}
				return false;
			}
		}

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Noise;

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!IsValid || deltaTime < 0f)
			{
				m_Initialized = false;
				return;
			}
			if (!m_Initialized)
			{
				Initialize();
			}
			m_NoiseTime += deltaTime * FrequencyGain;
			float num = 0f;
			WeightedNoiseProfile[] noiseProfiles = NoiseProfiles;
			foreach (WeightedNoiseProfile weightedNoiseProfile in noiseProfiles)
			{
				if (weightedNoiseProfile != null && weightedNoiseProfile.NoiseProfile != null && weightedNoiseProfile.Weight > 0f)
				{
					num += weightedNoiseProfile.Weight;
				}
			}
			if (num <= 0f)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			for (int j = 0; j < NoiseProfiles.Length; j++)
			{
				WeightedNoiseProfile weightedNoiseProfile2 = NoiseProfiles[j];
				if (weightedNoiseProfile2 != null && !(weightedNoiseProfile2.NoiseProfile == null) && !(weightedNoiseProfile2.Weight <= 0f))
				{
					float num2 = weightedNoiseProfile2.Weight / num;
					if (m_NoiseOffsets != null && j < m_NoiseOffsets.Length)
					{
						Vector3 combinedFilterResults = NoiseSettings.GetCombinedFilterResults(weightedNoiseProfile2.NoiseProfile.PositionNoise, m_NoiseTime, m_NoiseOffsets[j]);
						Vector3 combinedFilterResults2 = NoiseSettings.GetCombinedFilterResults(weightedNoiseProfile2.NoiseProfile.OrientationNoise, m_NoiseTime, m_NoiseOffsets[j]);
						zero += combinedFilterResults * num2;
						zero2 += combinedFilterResults2 * num2;
					}
				}
			}
			curState.PositionCorrection += curState.GetCorrectedOrientation() * zero * AmplitudeGain;
			Quaternion quaternion = Quaternion.Euler(zero2 * AmplitudeGain);
			if (PivotOffset != Vector3.zero)
			{
				Matrix4x4 matrix4x = Matrix4x4.Translate(-PivotOffset);
				matrix4x = Matrix4x4.Rotate(quaternion) * matrix4x;
				matrix4x = Matrix4x4.Translate(PivotOffset) * matrix4x;
				curState.PositionCorrection += curState.GetCorrectedOrientation() * matrix4x.MultiplyPoint(Vector3.zero);
			}
			curState.OrientationCorrection *= quaternion;
		}

		public void ReSeed()
		{
			if (NoiseProfiles != null && NoiseProfiles.Length != 0)
			{
				m_NoiseOffsets = new Vector3[NoiseProfiles.Length];
				for (int i = 0; i < NoiseProfiles.Length; i++)
				{
					m_NoiseOffsets[i] = new Vector3(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f));
				}
			}
		}

		public void SetNoiseWeight(NoiseType noiseType, float weight)
		{
			BuildDictionaries();
			if (m_NoiseProfileDict != null && m_NoiseProfileDict.ContainsKey(noiseType))
			{
				m_NoiseProfileDict[noiseType].Weight = Mathf.Clamp01(weight);
			}
			else
			{
				Debug.LogWarning($"NoiseType {noiseType} not found in noise profiles.");
			}
		}

		public void SetNoiseWeightByIndex(int index, float weight)
		{
			if (NoiseProfiles != null && index >= 0 && index < NoiseProfiles.Length && NoiseProfiles[index] != null)
			{
				NoiseProfiles[index].Weight = Mathf.Clamp01(weight);
			}
		}

		public float GetNoiseWeight(NoiseType noiseType)
		{
			BuildDictionaries();
			if (m_NoiseProfileDict != null && m_NoiseProfileDict.ContainsKey(noiseType))
			{
				return m_NoiseProfileDict[noiseType].Weight;
			}
			return 0f;
		}

		public float GetNoiseWeightByIndex(int index)
		{
			if (NoiseProfiles != null && index >= 0 && index < NoiseProfiles.Length && NoiseProfiles[index] != null)
			{
				return NoiseProfiles[index].Weight;
			}
			return 0f;
		}

		public WeightedNoiseProfile GetNoiseProfile(NoiseType noiseType)
		{
			BuildDictionaries();
			if (m_NoiseProfileDict != null && m_NoiseProfileDict.ContainsKey(noiseType))
			{
				return m_NoiseProfileDict[noiseType];
			}
			return null;
		}

		public bool HasNoiseType(NoiseType noiseType)
		{
			BuildDictionaries();
			if (m_NoiseProfileDict != null)
			{
				return m_NoiseProfileDict.ContainsKey(noiseType);
			}
			return false;
		}

		public void SetMultipleWeights(Dictionary<NoiseType, float> weights)
		{
			BuildDictionaries();
			foreach (KeyValuePair<NoiseType, float> weight in weights)
			{
				SetNoiseWeight(weight.Key, weight.Value);
			}
		}

		public void TransitionBetweenNoises(NoiseType fromType, NoiseType toType, float t)
		{
			t = Mathf.Clamp01(t);
			SetNoiseWeight(fromType, 1f - t);
			SetNoiseWeight(toType, t);
		}

		public void SetExclusiveNoise(NoiseType noiseType)
		{
			BuildDictionaries();
			WeightedNoiseProfile[] noiseProfiles = NoiseProfiles;
			foreach (WeightedNoiseProfile weightedNoiseProfile in noiseProfiles)
			{
				if (weightedNoiseProfile != null)
				{
					weightedNoiseProfile.Weight = ((weightedNoiseProfile.NoiseType == noiseType) ? 1f : 0f);
				}
			}
		}

		public List<NoiseType> GetActiveNoiseTypes()
		{
			List<NoiseType> list = new List<NoiseType>();
			if (NoiseProfiles != null)
			{
				WeightedNoiseProfile[] noiseProfiles = NoiseProfiles;
				foreach (WeightedNoiseProfile weightedNoiseProfile in noiseProfiles)
				{
					if (weightedNoiseProfile != null && weightedNoiseProfile.Weight > 0f)
					{
						list.Add(weightedNoiseProfile.NoiseType);
					}
				}
			}
			return list;
		}

		private void Initialize()
		{
			m_Initialized = true;
			m_NoiseTime = CinemachineCore.CurrentTime * FrequencyGain;
			if (m_NoiseOffsets == null || m_NoiseOffsets.Length != NoiseProfiles.Length)
			{
				ReSeed();
			}
			BuildDictionaries();
		}

		private void BuildDictionaries()
		{
			if (m_NoiseProfileDict != null && m_NoiseIndexDict != null)
			{
				return;
			}
			m_NoiseProfileDict = new Dictionary<NoiseType, WeightedNoiseProfile>();
			m_NoiseIndexDict = new Dictionary<NoiseType, int>();
			if (NoiseProfiles == null)
			{
				return;
			}
			for (int i = 0; i < NoiseProfiles.Length; i++)
			{
				WeightedNoiseProfile weightedNoiseProfile = NoiseProfiles[i];
				if (weightedNoiseProfile != null)
				{
					if (!m_NoiseProfileDict.ContainsKey(weightedNoiseProfile.NoiseType))
					{
						m_NoiseProfileDict[weightedNoiseProfile.NoiseType] = weightedNoiseProfile;
						m_NoiseIndexDict[weightedNoiseProfile.NoiseType] = i;
					}
					else
					{
						Debug.LogWarning($"Duplicate NoiseType {weightedNoiseProfile.NoiseType} found. Using first occurrence.");
					}
				}
			}
		}

		public void RebuildDictionaries()
		{
			m_NoiseProfileDict = null;
			m_NoiseIndexDict = null;
			BuildDictionaries();
		}
	}
}
