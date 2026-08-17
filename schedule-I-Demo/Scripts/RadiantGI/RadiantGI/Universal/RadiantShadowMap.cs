using UnityEngine;

namespace RadiantGI.Universal
{
	[ExecuteInEditMode]
	public class RadiantShadowMap : MonoBehaviour
	{
		private static class ShaderParams
		{
			public static int RadiantShadowMapColors = Shader.PropertyToID("_RadiantShadowMapColors");

			public static int RadiantShadowMapNormals = Shader.PropertyToID("_RadiantShadowMapNormals");

			public static int RadiantShadowMapWorldPos = Shader.PropertyToID("_RadiantShadowMapWorldPos");

			public static int RadiantWorldToShadowMap = Shader.PropertyToID("_RadiantWorldToShadowMap");

			public static int ClipToWorld = Shader.PropertyToID("_ClipToWorld");

			public static int ClipDir = Shader.PropertyToID("_ClipDir");

			public static int FarClipPlane = Shader.PropertyToID("_FarClipPlane");
		}

		public enum ShadowMapResolution
		{
			[InspectorName("64")]
			_64 = 0,
			[InspectorName("128")]
			_128 = 1,
			[InspectorName("256")]
			_256 = 2,
			[InspectorName("512")]
			_512 = 3,
			[InspectorName("1024")]
			_1024 = 4,
			[InspectorName("2048")]
			_2048 = 5
		}

		private const string RADIANT_GO_NAME = "RadiantGI Capture Camera";

		public static bool installed;

		public Transform target;

		[Tooltip("The capture extents around target")]
		public float targetCaptureSize = 25f;

		public ShadowMapResolution resolution = ShadowMapResolution._512;

		private Light thisLight;

		public Camera captureCamera;

		private Material captureMat;

		private Quaternion lastRotation;

		private Vector3 lastTargetPos;

		private float lastCaptureSize;

		public RenderTexture rtColors;

		public RenderTexture rtWorldPos;

		public RenderTexture rtNormals;

		private bool needShoot;

		private void OnEnable()
		{
			thisLight = GetComponent<Light>();
			if (thisLight == null || thisLight.type != LightType.Directional)
			{
				Debug.LogError("Radiant Shadow Map script must be added to a directional light!");
				return;
			}
			if (captureMat == null)
			{
				captureMat = new Material(Shader.Find("Hidden/Kronnect/RadiantGICapture"));
			}
			SetupCamera();
			lastTargetPos = new Vector3(float.MaxValue, 0f, 0f);
			installed = true;
		}

		private void OnValidate()
		{
			targetCaptureSize = Mathf.Max(targetCaptureSize, 5f);
		}

		private void OnDestroy()
		{
			Remove();
		}

		private void Remove()
		{
			installed = false;
			if (captureCamera != null && "RadiantGI Capture Camera".Equals(captureCamera.name))
			{
				Object.DestroyImmediate(captureCamera.gameObject);
			}
			if (captureMat != null)
			{
				Object.DestroyImmediate(captureMat);
			}
			DestroyRT(rtColors);
			DestroyRT(rtWorldPos);
			DestroyRT(rtNormals);
		}

		private void SetupCamera()
		{
			if (captureCamera == null)
			{
				captureCamera = GetComponentInChildren<Camera>();
			}
			if (!(captureCamera != null))
			{
				GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("RadiantGI/CaptureCamera"));
				gameObject.name = "RadiantGI Capture Camera";
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				captureCamera = gameObject.GetComponent<Camera>();
			}
		}

		private void LateUpdate()
		{
			if (thisLight == null)
			{
				Remove();
				return;
			}
			if (target == null)
			{
				target = Camera.main.transform;
				if (target == null)
				{
					return;
				}
			}
			if (captureCamera == null)
			{
				SetupCamera();
				if (captureCamera == null)
				{
					return;
				}
			}
			Quaternion rotation = base.transform.rotation;
			if (lastCaptureSize != targetCaptureSize || lastRotation != rotation || (lastTargetPos - target.position).sqrMagnitude > 25f)
			{
				needShoot = true;
			}
			int num = 1 << (int)(resolution + 6);
			if (rtColors == null || rtNormals == null || rtWorldPos == null || rtColors.width != num)
			{
				DestroyRT(rtColors);
				DestroyRT(rtNormals);
				DestroyRT(rtWorldPos);
				if (rtColors == null)
				{
					RenderTextureDescriptor desc = new RenderTextureDescriptor(num, num, RenderTextureFormat.ARGBHalf, 0);
					desc.msaaSamples = 1;
					desc.useMipMap = false;
					rtColors = new RenderTexture(desc);
					rtColors.Create();
					rtNormals = new RenderTexture(desc);
					rtNormals.Create();
					rtWorldPos = new RenderTexture(desc);
					rtWorldPos.Create();
				}
				captureCamera.targetTexture = rtColors;
				needShoot = true;
			}
			if (needShoot)
			{
				needShoot = false;
				CaptureScene();
			}
			Shader.SetGlobalMatrix(ShaderParams.RadiantWorldToShadowMap, captureCamera.projectionMatrix * captureCamera.worldToCameraMatrix);
		}

		private void CaptureScene()
		{
			lastRotation = base.transform.rotation;
			lastTargetPos = target.position;
			lastCaptureSize = targetCaptureSize;
			float farClipPlane = captureCamera.farClipPlane;
			Vector3 vector = ((target != null) ? target.transform.position : Vector3.zero);
			captureCamera.transform.localRotation = Quaternion.identity;
			captureCamera.transform.localPosition = vector + new Vector3(0f, 0f, farClipPlane * -0.5f);
			captureCamera.orthographicSize = targetCaptureSize;
			captureCamera.Render();
			captureMat.SetMatrix(ShaderParams.ClipToWorld, captureCamera.cameraToWorldMatrix * captureCamera.projectionMatrix.inverse);
			captureMat.SetVector(ShaderParams.ClipDir, base.transform.forward);
			captureMat.SetFloat(ShaderParams.FarClipPlane, farClipPlane);
			Graphics.Blit(rtColors, rtWorldPos, captureMat, 0);
			Shader.SetGlobalTexture(ShaderParams.RadiantShadowMapWorldPos, rtWorldPos);
			Graphics.Blit(rtColors, rtNormals, captureMat, 1);
			Shader.SetGlobalTexture(ShaderParams.RadiantShadowMapColors, rtColors);
			Shader.SetGlobalTexture(ShaderParams.RadiantShadowMapNormals, rtNormals);
		}

		private void DestroyRT(RenderTexture rt)
		{
			if (!(rt == null))
			{
				rt.Release();
				Object.DestroyImmediate(rt);
			}
		}
	}
}
