using System.Collections.Generic;
using UnityEngine;

namespace Enviro
{
	[AddComponentMenu("Enviro 3/Weather Zone")]
	[ExecuteInEditMode]
	public class EnviroZone : MonoBehaviour
	{
		public EnviroWeatherType currentWeatherType;

		public EnviroWeatherType nextWeatherType;

		public bool autoWeatherChanges = true;

		public float weatherChangeIntervall = 2f;

		public double nextWeatherUpdate;

		public List<EnviroZoneWeather> weatherTypeList = new List<EnviroZoneWeather>();

		public Vector3 zoneScale = Vector3.one;

		public Color zoneGizmoColor;

		private BoxCollider zoneCollider;

		private void OnEnable()
		{
			if (zoneCollider == null)
			{
				zoneCollider = base.gameObject.GetComponent<BoxCollider>();
				if (zoneCollider == null)
				{
					zoneCollider = base.gameObject.AddComponent<BoxCollider>();
				}
			}
			zoneCollider.isTrigger = true;
			if (EnviroManager.instance != null && EnviroManager.instance.Weather != null)
			{
				bool flag = false;
				for (int i = 0; i < EnviroManager.instance.zones.Count; i++)
				{
					if (EnviroManager.instance.zones[i] == this)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					EnviroManager.instance.Weather.RegisterZone(this);
				}
			}
			nextWeatherUpdate = EnviroManager.instance.Time.GetDateInHours() + (double)weatherChangeIntervall;
		}

		private void OnDisable()
		{
			if (!(EnviroManager.instance != null) || !(EnviroManager.instance.Weather != null))
			{
				return;
			}
			for (int i = 0; i < EnviroManager.instance.zones.Count; i++)
			{
				if (EnviroManager.instance.zones[i] == this)
				{
					EnviroManager.instance.Weather.RemoveZone(this);
				}
			}
		}

		public void UpdateZoneScale()
		{
			zoneCollider.size = zoneScale;
		}

		public void AddWeatherType(EnviroWeatherType wType)
		{
			EnviroZoneWeather enviroZoneWeather = new EnviroZoneWeather();
			enviroZoneWeather.weatherType = wType;
			weatherTypeList.Add(enviroZoneWeather);
		}

		public void RemoveWeatherZoneType(EnviroZoneWeather wType)
		{
			weatherTypeList.Remove(wType);
		}

		public void ChangeZoneWeatherInstant(EnviroWeatherType type)
		{
			if (EnviroManager.instance != null && currentWeatherType != type)
			{
				EnviroManager.instance.NotifyZoneWeatherChanged(type, this);
				if (EnviroManager.instance.currentZone == this && EnviroManager.instance.Weather != null)
				{
					EnviroManager.instance.Weather.targetWeatherType = type;
				}
			}
			currentWeatherType = type;
		}

		public void ChangeZoneWeather(EnviroWeatherType type)
		{
			nextWeatherType = type;
		}

		private void ChooseNextWeatherRandom()
		{
			float num = Random.Range(0f, 100f * (float)weatherTypeList.Count);
			bool flag = false;
			for (int i = 0; i < weatherTypeList.Count; i++)
			{
				if (weatherTypeList[i].seasonalProbability && EnviroManager.instance != null && EnviroManager.instance.Environment != null)
				{
					switch (EnviroManager.instance.Environment.Settings.season)
					{
					case EnviroEnvironment.Seasons.Spring:
						if (weatherTypeList[i].probabilitySpring > 0f && num <= weatherTypeList[i].probabilitySpring * (float)weatherTypeList.Count)
						{
							ChangeZoneWeather(weatherTypeList[i].weatherType);
							flag = true;
							return;
						}
						break;
					case EnviroEnvironment.Seasons.Summer:
						if (weatherTypeList[i].probabilitySummer > 0f && num <= weatherTypeList[i].probabilitySummer * (float)weatherTypeList.Count)
						{
							ChangeZoneWeather(weatherTypeList[i].weatherType);
							flag = true;
							return;
						}
						break;
					case EnviroEnvironment.Seasons.Autumn:
						if (weatherTypeList[i].probabilityAutumn > 0f && num <= weatherTypeList[i].probabilityAutumn * (float)weatherTypeList.Count)
						{
							ChangeZoneWeather(weatherTypeList[i].weatherType);
							flag = true;
							return;
						}
						break;
					case EnviroEnvironment.Seasons.Winter:
						if (weatherTypeList[i].probabilityWinter > 0f && num <= weatherTypeList[i].probabilityWinter * (float)weatherTypeList.Count)
						{
							ChangeZoneWeather(weatherTypeList[i].weatherType);
							flag = true;
							return;
						}
						break;
					}
				}
				else if (num <= weatherTypeList[i].probability * (float)weatherTypeList.Count)
				{
					ChangeZoneWeather(weatherTypeList[i].weatherType);
					flag = true;
					return;
				}
				num -= 100f;
			}
			if (!flag)
			{
				ChangeZoneWeather(currentWeatherType);
			}
		}

		private void UpdateZoneWeather()
		{
			if (!(EnviroManager.instance.Time != null))
			{
				return;
			}
			double dateInHours = EnviroManager.instance.Time.GetDateInHours();
			if (dateInHours >= nextWeatherUpdate)
			{
				if (nextWeatherType != null)
				{
					ChangeZoneWeatherInstant(nextWeatherType);
				}
				else
				{
					ChangeZoneWeatherInstant(currentWeatherType);
				}
				ChooseNextWeatherRandom();
				nextWeatherUpdate = dateInHours + (double)weatherChangeIntervall;
			}
		}

		private void Update()
		{
			UpdateZoneScale();
			if (Application.isPlaying && !(EnviroManager.instance == null) && !(EnviroManager.instance.Weather == null))
			{
				if (autoWeatherChanges && EnviroManager.instance.Weather.globalAutoWeatherChange)
				{
					UpdateZoneWeather();
				}
				if (EnviroManager.instance.currentZone == this && EnviroManager.instance.Weather.targetWeatherType != currentWeatherType)
				{
					EnviroManager.instance.Weather.targetWeatherType = currentWeatherType;
				}
			}
		}

		private void OnTriggerEnter(Collider col)
		{
			if (!(EnviroManager.instance == null) && !(EnviroManager.instance.Weather == null) && (bool)col.gameObject.GetComponent<EnviroManager>())
			{
				EnviroManager.instance.currentZone = this;
			}
		}

		private void OnTriggerExit(Collider col)
		{
			if (!(EnviroManager.instance == null) && !(EnviroManager.instance.Weather == null) && (bool)col.gameObject.GetComponent<EnviroManager>() && EnviroManager.instance.currentZone == this)
			{
				if (EnviroManager.instance.defaultZone != null)
				{
					EnviroManager.instance.currentZone = EnviroManager.instance.defaultZone;
				}
				else
				{
					EnviroManager.instance.currentZone = null;
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = zoneGizmoColor;
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
			Gizmos.DrawCube(Vector3.zero, new Vector3(zoneScale.x, zoneScale.y, zoneScale.z));
		}
	}
}
