using System;
using System.Collections;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroLightningModule : EnviroModule
	{
		public EnviroLightning Settings;

		public EnviroLightningModule preset;

		public bool showLightningControls;

		private bool spawned;

		private float lightningStart = -999f;

		public override void UpdateModule()
		{
			if (active && Application.isPlaying && Settings.lightningStorm && Settings.prefab != null)
			{
				CastLightningBoltRandom();
			}
		}

		public void CastLightningBolt(Vector3 from, Vector3 to)
		{
			if (Settings.prefab != null)
			{
				GameObject gameObject = ((!(Settings.customLightningEffect != null)) ? UnityEngine.Object.Instantiate(Settings.prefab, from, Quaternion.identity).gameObject : UnityEngine.Object.Instantiate(Settings.customLightningEffect, from, Quaternion.identity));
				gameObject.GetComponent<ILightningEffect>().CastBolt(from, to);
				if (EnviroManager.instance.Audio != null)
				{
					EnviroManager.instance.StartCoroutine(PlayThunderSFX(0.05f));
				}
				if (EnviroManager.instance.VolumetricClouds != null)
				{
					EnviroManager.instance.StartCoroutine(CloudsFlash(0.25f, from));
				}
			}
			else
			{
				Debug.Log("Please assign a lightning prefab in your Enviro Ligthning module!");
			}
		}

		public void CastLightningBoltRandom()
		{
			if (!spawned)
			{
				Vector2 vector = UnityEngine.Random.insideUnitCircle * Settings.randomSpawnRange;
				Vector2 vector2 = UnityEngine.Random.insideUnitCircle * Settings.randomTargetRange;
				float num = 0f;
				if (EnviroManager.instance.Objects.worldAnchor != null)
				{
					num = EnviroManager.instance.Objects.worldAnchor.transform.position.y;
				}
				float num2 = 2000f;
				if (EnviroManager.instance.VolumetricClouds != null)
				{
					num2 = Mathf.Max(EnviroManager.instance.VolumetricClouds.settingsVolume.bottomCloudsHeight + 1000f, 1000f);
				}
				Vector3 spwn = new Vector3(vector.x + EnviroManager.instance.transform.position.x, num + num2, vector.y + EnviroManager.instance.transform.position.z);
				Vector3 targ = new Vector3(vector2.x + spwn.x, num, vector2.y + spwn.z);
				EnviroManager.instance.StartCoroutine(LightningStorm(spwn, targ));
			}
		}

		private IEnumerator LightningStorm(Vector3 spwn, Vector3 targ)
		{
			spawned = true;
			CastLightningBolt(spwn, targ);
			yield return new WaitForSeconds(UnityEngine.Random.Range(Settings.randomLightingDelay, Settings.randomLightingDelay * 2f));
			spawned = false;
		}

		private IEnumerator PlayThunderSFX(float delay)
		{
			yield return new WaitForSeconds(delay);
			EnviroManager.instance.Audio.PlayRandomThunderSFX();
		}

		private IEnumerator CloudsFlash(float delay, Vector3 from)
		{
			yield return new WaitForSeconds(delay);
			lightningStart = Time.time;
			Shader.SetGlobalVector("_LightningCenter", from);
			Shader.SetGlobalFloat("_LightningRadius", Settings.cloudsLightningRadius);
			Shader.SetGlobalFloat("_LightningStart", lightningStart);
			Shader.SetGlobalFloat("_LightningDuration", Settings.cloudsLightningDuration);
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroLightning>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroLightningModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroLightning>(JsonUtility.ToJson(Settings));
		}
	}
}
