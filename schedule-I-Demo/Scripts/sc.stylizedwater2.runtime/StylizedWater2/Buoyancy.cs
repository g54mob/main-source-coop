using System;
using Unity.Mathematics;
using UnityEngine;

namespace StylizedWater2
{
	public static class Buoyancy
	{
		public struct BuoyancySample
		{
			public int hashCode;

			public Vector3[] inputPositions;

			public Vector3[] outputOffset;

			public Vector3[] outputNormal;

			public void SetSamplePositions(Vector3[] positions)
			{
				inputPositions = positions;
				outputOffset = new Vector3[positions.Length];
				outputNormal = new Vector3[positions.Length];
			}
		}

		internal static bool ComputeReadbackAvailable;

		private static WaveParameters waveParameters = new WaveParameters();

		private static Material lastMaterial;

		private static readonly int CustomTimeID = Shader.PropertyToID("_CustomTime");

		private static readonly int TimeParametersID = Shader.PropertyToID("_TimeParameters");

		private const string WavesKeyword = "_WAVES";

		private static float customTimeValue = -1f;

		private static float4 sine;

		private static float4 cosine;

		private static float4 dotABCD;

		private static float4 AB;

		private static float4 CD;

		private static float4 direction1;

		private static float4 direction2;

		private static float4 TIME;

		private static float2 planarPosition;

		private static float4 amp = new float4(0.3f, 0.35f, 0.25f, 0.25f);

		private static float4 freq = new float4(1.3f, 1.35f, 1.25f, 1.25f);

		private static float4 speed = new float4(1.2f, 1.375f, 1.1f, 1f);

		private static float4 dir1 = new float4(0.3f, 0.85f, 0.85f, 0.25f);

		private static float4 dir2 = new float4(0.1f, 0.9f, -0.5f, -0.5f);

		private static float4 steepness = new float4(12f, 12f, 12f, 12f);

		private static float4 frequency;

		private static float3 offsets;

		private static RaycastHit hit = default(RaycastHit);

		private static Vector3 m_offset;

		private static float _TimeParameters
		{
			get
			{
				if (customTimeValue > 0f)
				{
					return customTimeValue;
				}
				return Time.time;
			}
		}

		private static void GetMaterialParameters(Material mat)
		{
			waveParameters.Update(mat);
		}

		public static void SetCustomTime(float value)
		{
			customTimeValue = value;
			Shader.SetGlobalFloat(CustomTimeID, customTimeValue);
		}

		private static float Dot2(float2 a, float2 b)
		{
			return math.dot(a, b);
		}

		private static float Dot3(float3 a, float3 b)
		{
			return math.dot(a, b);
		}

		private static float Dot4(float4 a, float4 b)
		{
			return math.dot(a, b);
		}

		private static float Sine(float t)
		{
			return math.sin(t);
		}

		private static float Cosine(float t)
		{
			return math.cos(t);
		}

		private static void Vector4Sin(ref float4 input, float4 a, float4 b)
		{
			input.x = Sine(a.x + b.x);
			input.y = Sine(a.y + b.y);
			input.z = Sine(a.z + b.z);
			input.w = Sine(a.w + b.w);
		}

		private static void Vector4Cosin(ref float4 input, float4 a, float4 b)
		{
			input.x = Cosine(a.x + b.x);
			input.y = Cosine(a.y + b.y);
			input.z = Cosine(a.z + b.z);
			input.w = Cosine(a.w + b.w);
		}

		private static float4 MultiplyVec4(float4 a, float4 b)
		{
			return a * b;
		}

		public static float3 FindWaterLevelIntersection(float3 origin, float3 direction, float waterLevel)
		{
			float num = Mathf.Acos(Dot3(direction, Vector3.up)) * 180f / MathF.PI;
			float num2 = (waterLevel - origin.y) / Cosine(MathF.PI / 180f * num);
			return origin + direction * num2;
		}

		public static void Raycast(WaterObject waterObject, float3 origin, float3 direction, bool dynamicMaterial, out RaycastHit hit)
		{
			Raycast(waterObject.material, waterObject.transform.position.y, origin, direction, dynamicMaterial, out hit);
		}

		public static void Raycast(Material waterMat, float waterLevel, float3 origin, float3 direction, bool dynamicMaterial, out RaycastHit hit)
		{
			float3 float5 = FindWaterLevelIntersection(origin, direction, waterLevel);
			Vector3 normal;
			float y = SampleWaves(float5, waterMat, waterLevel, 1f, dynamicMaterial, out normal);
			float5.y = y;
			hit = Buoyancy.hit;
			hit.normal = normal;
			hit.point = float5;
		}

		public static float SampleWaves(Vector3 position, WaterObject waterObject, float rollStrength, bool dynamicMaterial, out Vector3 normal)
		{
			return SampleWaves(position, waterObject.material, waterObject.transform.position.y, rollStrength, dynamicMaterial, out normal);
		}

		private static void RecalculateParameters()
		{
			direction1 = MultiplyVec4(dir1, waveParameters.direction);
			direction2 = MultiplyVec4(dir2, waveParameters.direction);
			frequency = freq * (1f - waveParameters.distance) * 3f;
			AB.x = steepness.x * waveParameters.steepness * direction1.x * amp.x;
			AB.y = steepness.x * waveParameters.steepness * direction1.y * amp.x;
			AB.z = steepness.x * waveParameters.steepness * direction1.z * amp.y;
			AB.w = steepness.x * waveParameters.steepness * direction1.w * amp.y;
			CD.x = steepness.z * waveParameters.steepness * direction2.x * amp.z;
			CD.y = steepness.z * waveParameters.steepness * direction2.y * amp.z;
			CD.z = steepness.w * waveParameters.steepness * direction2.z * amp.w;
			CD.w = steepness.w * waveParameters.steepness * direction2.w * amp.w;
		}

		private static void SampleWaves(Vector3 position, Material waterMat, float waterLevel, float rollStrength, bool dynamicMaterial, out Vector3 offset, out Vector3 normal)
		{
			if (!dynamicMaterial && Application.isPlaying)
			{
				if (lastMaterial == null || !lastMaterial.Equals(waterMat))
				{
					GetMaterialParameters(waterMat);
					lastMaterial = waterMat;
				}
			}
			else
			{
				GetMaterialParameters(waterMat);
			}
			TIME = _TimeParameters * (0f - waveParameters.animationSpeed) * waveParameters.speed * speed;
			RecalculateParameters();
			offsets = float3.zero;
			planarPosition.x = position.x;
			planarPosition.y = position.z;
			for (int i = 0; i <= waveParameters.count; i++)
			{
				float num = 1f + (float)i / (float)waveParameters.count;
				frequency *= num;
				dotABCD.x = Dot2(direction1.xy, planarPosition) * frequency.x;
				dotABCD.y = Dot2(direction1.zw, planarPosition) * frequency.y;
				dotABCD.z = Dot2(direction2.xy, planarPosition) * frequency.z;
				dotABCD.w = Dot2(direction2.zw, planarPosition) * frequency.w;
				sine.x = Sine(dotABCD.x + TIME.x);
				sine.y = Sine(dotABCD.y + TIME.y);
				sine.z = Sine(dotABCD.z + TIME.z);
				sine.w = Sine(dotABCD.w + TIME.w);
				cosine.x = Cosine(dotABCD.x + TIME.x);
				cosine.y = Cosine(dotABCD.y + TIME.y);
				cosine.z = Cosine(dotABCD.z + TIME.z);
				cosine.w = Cosine(dotABCD.w + TIME.w);
				offsets.x += Dot4(cosine, new float4(AB.x, AB.z, CD.x, CD.z));
				offsets.y += Dot4(sine, amp);
				offsets.z += Dot4(cosine, new float4(AB.y, AB.w, CD.y, CD.w));
			}
			rollStrength *= math.lerp(0.001f, 0.1f, waveParameters.steepness);
			normal.x = (0f - offsets.x) * rollStrength * waveParameters.height;
			normal.y = 2f;
			normal.z = (0f - offsets.z) * rollStrength * waveParameters.height;
			normal = math.normalize(normal);
			offsets.y /= waveParameters.count;
			offsets.y = offsets.y * waveParameters.height + waterLevel;
			offset = offsets;
		}

		public static float SampleWaves(Vector3 position, Material waterMat, float waterLevel, float rollStrength, bool dynamicMaterial, out Vector3 normal)
		{
			SampleWaves(position, waterMat, waterLevel, rollStrength, dynamicMaterial, out m_offset, out normal);
			return m_offset.y;
		}

		public static void SampleWaves(ref BuoyancySample sample, Material waterMat, float waterLevel, bool dynamicMaterial)
		{
			if (!ComputeReadbackAvailable)
			{
				for (int i = 0; i < sample.inputPositions.Length; i++)
				{
					SampleWaves(sample.inputPositions[i], waterMat, waterLevel, 1f, dynamicMaterial, out sample.outputOffset[i], out sample.outputNormal[i]);
				}
			}
		}

		public static bool CanTouchWater(float3 position, WaterObject waterObject)
		{
			if (!waterObject)
			{
				return false;
			}
			return position.y < waterObject.transform.position.y + WaveParameters.GetMaxWaveHeight(waterObject.material);
		}

		public static bool CanTouchWater(float3 position, Material waterMaterial, float waterLevel)
		{
			return position.y < waterLevel + WaveParameters.GetMaxWaveHeight(waterMaterial);
		}
	}
}
