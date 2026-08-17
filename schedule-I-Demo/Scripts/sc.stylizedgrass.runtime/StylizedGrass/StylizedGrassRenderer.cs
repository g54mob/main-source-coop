using UnityEngine;

namespace StylizedGrass
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Grass/Stylized Grass Renderer")]
	public class StylizedGrassRenderer : MonoBehaviour
	{
		public static StylizedGrassRenderer Instance;

		public bool debug;

		public RenderTexture vectorRT;

		[Tooltip("When a color map is assigned, this will be set as the active color map.\n\nHaving the Color Map Renderer component present would not longer be required.")]
		public GrassColorMap colorMap;

		[Tooltip("When enabled the grass Ambient and Gust strength values are multiplied by the WindZone's Main value")]
		public bool listenToWindZone;

		public WindZone windZone;

		public static int _BendMapUV = Shader.PropertyToID("_BendMapUV");

		private static int _GlobalWindParams = Shader.PropertyToID("_GlobalWindParams");

		private static int _GlobalWindDirection = Shader.PropertyToID("_GlobalWindDirection");

		private double lastFrameTime;

		private double timeOffset;

		private Vector3 lastDirection;

		private Vector3 windDirection;

		public void OnEnable()
		{
			Instance = this;
			if ((bool)colorMap)
			{
				colorMap.SetActive();
			}
			else if (!GrassColorMapRenderer.Instance)
			{
				GrassColorMap.DisableGlobally();
			}
		}

		public void OnDisable()
		{
			Instance = null;
			Shader.SetGlobalVector(_BendMapUV, Vector4.zero);
			Shader.SetGlobalVector(_GlobalWindParams, Vector4.zero);
		}

		public static void SetWindZone(WindZone windZone)
		{
			if (!Instance)
			{
				Debug.LogWarning("Tried to set Stylized Grass Renderer wind zone, but no instance is present");
			}
			else
			{
				Instance.windZone = windZone;
			}
		}

		private void Update()
		{
			UpdateWind();
		}

		private void UpdateWind()
		{
			if (listenToWindZone)
			{
				if ((bool)windZone)
				{
					double num = (double)Time.time - lastFrameTime;
					lastFrameTime = Time.time;
					timeOffset += num * (double)windZone.windMain;
					windDirection = windZone.transform.rotation * Vector3.forward;
					windDirection = Vector3.Lerp(lastDirection, windDirection, (float)num).normalized;
					lastDirection = windDirection;
					Shader.SetGlobalVector(_GlobalWindParams, new Vector4((float)timeOffset, windZone.windMain, windZone.windTurbulence, 1f));
					Shader.SetGlobalVector(_GlobalWindDirection, lastDirection);
				}
			}
			else
			{
				Shader.SetGlobalVector(_GlobalWindParams, Vector4.zero);
			}
		}

		private void OnDrawGizmosSelected()
		{
			GrassBendingFeature.RenderBendVectors.DrawOrthographicViewGizmo();
		}

		private void OnDrawGizmos()
		{
			if (listenToWindZone && (bool)windZone)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(windZone.transform.position, windZone.transform.position + windDirection * 5f);
			}
		}
	}
}
