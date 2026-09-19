using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.LightBaking
{
	public class LightProbeRuntime : MonoBehaviour
	{
		[SerializeField]
		private bool _hasDirectionalLight;

		[SerializeField]
		private List<Light> _lights = new List<Light>();

		[SerializeField]
		private float _positionThreshold = 0.01f;

		private Vector3 _lastPosition;

		private Quaternion _lastRotation;

		private IEnumerator Start()
		{
			_lastPosition = base.transform.position;
			_lastRotation = base.transform.rotation;
			if (!_hasDirectionalLight && RenderSettings.sun != null)
			{
				_lights.Add(RenderSettings.sun);
				_hasDirectionalLight = true;
			}
			yield return ApplyProbes();
		}

		private void Update()
		{
			bool num = Vector3.Distance(base.transform.position, _lastPosition) > _positionThreshold;
			bool flag = Quaternion.Angle(base.transform.rotation, _lastRotation) > _positionThreshold;
			if (num || flag)
			{
				_lastPosition = base.transform.position;
				_lastRotation = base.transform.rotation;
				StopAllCoroutines();
				StartCoroutine(ApplyProbes());
			}
		}

		private IEnumerator ApplyProbes()
		{
			yield return null;
			if (LightmapSettings.lightProbes == null)
			{
				yield break;
			}
			SphericalHarmonicsL2[] bakedProbes = LightmapSettings.lightProbes.bakedProbes;
			Vector3[] positions = LightmapSettings.lightProbes.positions;
			int count = LightmapSettings.lightProbes.count;
			for (int i = 0; i < count; i++)
			{
				bakedProbes[i].Clear();
			}
			Color ambientColor = GetAmbientColor();
			for (int j = 0; j < count; j++)
			{
				bakedProbes[j].AddAmbientLight(ambientColor);
			}
			foreach (Light light in _lights)
			{
				if (light == null)
				{
					continue;
				}
				if (light.type == LightType.Directional)
				{
					for (int k = 0; k < count; k++)
					{
						bakedProbes[k].AddDirectionalLight(-light.transform.forward, light.color, light.intensity);
					}
				}
				else if (light.type == LightType.Point)
				{
					for (int l = 0; l < count; l++)
					{
						SHAddPointLight(positions[l], light.transform.position, light.range, light.color, light.intensity, ref bakedProbes[l]);
					}
				}
			}
			LightmapSettings.lightProbes.bakedProbes = bakedProbes;
		}

		private Color GetAmbientColor()
		{
			return RenderSettings.ambientMode switch
			{
				AmbientMode.Flat => RenderSettings.ambientLight, 
				AmbientMode.Trilight => (RenderSettings.ambientSkyColor + RenderSettings.ambientEquatorColor + RenderSettings.ambientGroundColor) / 3f, 
				AmbientMode.Skybox => RenderSettings.ambientSkyColor, 
				_ => Color.black, 
			};
		}

		private void SHAddPointLight(Vector3 probePosition, Vector3 position, float range, Color color, float intensity, ref SphericalHarmonicsL2 sh)
		{
			Vector3 vector = position - probePosition;
			float num = 1f / (1f + 25f * vector.sqrMagnitude / (range * range));
			sh.AddDirectionalLight(vector.normalized, color, intensity * num);
		}
	}
}
