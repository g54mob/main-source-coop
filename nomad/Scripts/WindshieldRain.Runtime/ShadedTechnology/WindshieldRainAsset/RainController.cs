using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	public class RainController : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float m_CurrentRainAmount;

		public bool m_StormEnabled;

		public float m_StormRainAmount = 1f;

		public bool m_DisableRainScriptsWhenNotRaining = true;

		public float maxParticlesAmount = 1000f;

		public AnimationCurve particleAmountCurve;

		public WindshieldRain[] windshieldRains;

		public Wipers[] wipersScripts;

		public DropletsAcceleration[] dropsAccelerationScripts;

		public SimpleRainManager[] simpleRainManagers;

		public AnimationCurve dropletsSpawnAmountCurve;

		public AnimationCurve dropletsSpawnRateCurve;

		public ParticleSystem rainParticles;

		public Material[] rainMaterials;

		public float materialRainChangeSpeed = 0.1f;

		public float rainStrengthChangeSpeed = 0.1f;

		private float currentMaterialsRainAmount;

		private float currentMaterialsRainStrength;

		private bool isRainEnabled;

		private float lastRainAmountBeforeStorm;

		private float lastRainAmount;

		private bool lastStormEnabled;

		private void EnableDisableRainScripts(bool enable)
		{
			WindshieldRain[] array = windshieldRains;
			foreach (WindshieldRain windshieldRain in array)
			{
				if (!(windshieldRain == null))
				{
					windshieldRain.enabled = enable;
					if (!enable)
					{
						windshieldRain.ResetRain();
					}
				}
			}
			Wipers[] array2 = wipersScripts;
			foreach (Wipers wipers in array2)
			{
				if (!(wipers == null))
				{
					wipers.enabled = enable;
				}
			}
			DropletsAcceleration[] array3 = dropsAccelerationScripts;
			foreach (DropletsAcceleration dropletsAcceleration in array3)
			{
				if (!(dropletsAcceleration == null))
				{
					dropletsAcceleration.enabled = enable;
				}
			}
			SimpleRainManager[] array4 = simpleRainManagers;
			foreach (SimpleRainManager simpleRainManager in array4)
			{
				if (!(simpleRainManager == null))
				{
					simpleRainManager.enabled = enable;
				}
			}
		}

		private void UpdateMaterialsRainAmount(float deltaTime)
		{
			float f = m_CurrentRainAmount - currentMaterialsRainAmount;
			currentMaterialsRainAmount += Mathf.Min(Mathf.Abs(f), materialRainChangeSpeed * deltaTime) * Mathf.Sign(f);
			if (m_DisableRainScriptsWhenNotRaining)
			{
				float f2 = (float)((currentMaterialsRainAmount > 0f) ? 1 : 0) - currentMaterialsRainStrength;
				currentMaterialsRainStrength += Mathf.Min(Mathf.Abs(f2), rainStrengthChangeSpeed * deltaTime) * Mathf.Sign(f2);
			}
			SetMaterialsRainAmount(currentMaterialsRainAmount, m_DisableRainScriptsWhenNotRaining ? currentMaterialsRainStrength : 1f);
			if (m_DisableRainScriptsWhenNotRaining && isRainEnabled != currentMaterialsRainStrength > 0f)
			{
				isRainEnabled = currentMaterialsRainStrength > 0f;
				EnableDisableRainScripts(isRainEnabled);
			}
		}

		private void SetMaterialsRainAmount(float rainAmount, float rainStrength)
		{
			Material[] array = rainMaterials;
			foreach (Material obj in array)
			{
				obj.SetFloat("_RainAmount", rainAmount);
				obj.SetFloat("_RainStrength", rainStrength);
			}
		}

		public void SetRainAmount(float rainAmount)
		{
			lastRainAmount = m_CurrentRainAmount;
			m_CurrentRainAmount = rainAmount;
			if (rainParticles != null)
			{
				ParticleSystem.EmissionModule emission = rainParticles.emission;
				emission.rateOverTime = particleAmountCurve.Evaluate(rainAmount) * maxParticlesAmount;
			}
			WindshieldRain[] array = windshieldRains;
			foreach (WindshieldRain windshieldRain in array)
			{
				if (!(windshieldRain == null))
				{
					windshieldRain.m_SpawnAmount = dropletsSpawnAmountCurve.Evaluate(rainAmount);
					windshieldRain.m_SpawnRate = Mathf.Max(0.001f, dropletsSpawnRateCurve.Evaluate(rainAmount));
					windshieldRain.UpdateShaderValues();
				}
			}
		}

		public void ChangeStorm()
		{
			if (m_StormEnabled)
			{
				lastRainAmountBeforeStorm = m_CurrentRainAmount;
				SetRainAmount(1f);
				WindshieldRain[] array = windshieldRains;
				foreach (WindshieldRain windshieldRain in array)
				{
					if (!(windshieldRain == null))
					{
						windshieldRain.m_SpawnAmount = m_StormRainAmount;
						windshieldRain.UpdateShaderValues();
					}
				}
			}
			else
			{
				SetRainAmount(lastRainAmountBeforeStorm);
			}
		}

		private void Start()
		{
			currentMaterialsRainAmount = m_CurrentRainAmount;
			isRainEnabled = m_CurrentRainAmount > 0f;
			currentMaterialsRainStrength = (isRainEnabled ? 1 : 0);
			lastRainAmountBeforeStorm = m_CurrentRainAmount;
			lastRainAmount = m_CurrentRainAmount;
			lastStormEnabled = m_StormEnabled;
			if (m_DisableRainScriptsWhenNotRaining)
			{
				EnableDisableRainScripts(isRainEnabled);
			}
		}

		private void Update()
		{
			if (lastStormEnabled != m_StormEnabled)
			{
				lastStormEnabled = m_StormEnabled;
				ChangeStorm();
			}
			if (lastRainAmount != m_CurrentRainAmount)
			{
				SetRainAmount(m_CurrentRainAmount);
			}
			UpdateMaterialsRainAmount(Time.deltaTime);
		}
	}
}
