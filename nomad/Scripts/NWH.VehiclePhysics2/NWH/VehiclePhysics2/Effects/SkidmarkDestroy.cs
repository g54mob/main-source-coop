using UnityEngine;
using UnityEngine.Rendering;

namespace NWH.VehiclePhysics2.Effects
{
	[RequireComponent(typeof(MeshRenderer))]
	public class SkidmarkDestroy : MonoBehaviour
	{
		[Tooltip("    Distance at which the GameObject will be destroyed.")]
		public float distanceThreshold = 100f;

		[Tooltip("Time after which the GameObject will be destroyed.")]
		public float timeThreshold = 20f;

		[Tooltip("True if the skidmark is still the currently active skidmark")]
		public bool skidmarkIsBeingUsed;

		[Tooltip("    Transform to which the object belongs to.")]
		public Transform targetTransform;

		[Tooltip("Set to true to trigger the fade out and destroy even with the next check.")]
		public bool destroyFlag;

		private float _fadeOutTimer;

		private float _fadeOutDuration = 5f;

		private MeshRenderer _meshRenderer;

		private float _initMatAlpha;

		private float _lifeTimer;

		private void Start()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			if (GraphicsSettings.defaultRenderPipeline == null)
			{
				_initMatAlpha = _meshRenderer.material.color.a;
			}
			_fadeOutDuration = _initMatAlpha * 10f;
			_fadeOutTimer = 0f;
			InvokeRepeating("Check", Random.Range(1f, 2f), 1f);
		}

		private void Fade()
		{
			float num = _fadeOutTimer / _fadeOutDuration;
			if (num >= 1f)
			{
				Object.Destroy(base.gameObject);
			}
			else if (GraphicsSettings.defaultRenderPipeline == null)
			{
				Material material = _meshRenderer.material;
				Color color = material.color;
				material.color = new Color(color.r, color.g, color.b, _initMatAlpha * Mathf.Clamp01(1f - num));
			}
			_fadeOutTimer += 0.05f;
		}

		private void OnDestroy()
		{
			CancelInvoke();
			Object.Destroy(GetComponent<MeshFilter>().sharedMesh);
			Object.Destroy(GetComponent<MeshRenderer>().material);
		}

		private void Check()
		{
			if (targetTransform == null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			bool flag = Vector3.Distance(base.transform.position, targetTransform.position) > distanceThreshold;
			bool flag2 = timeThreshold > 0f && _lifeTimer >= timeThreshold;
			if (!skidmarkIsBeingUsed && (flag || flag2 || destroyFlag))
			{
				InvokeRepeating("Fade", 0f, 0.05f);
			}
			_lifeTimer += 1f;
		}
	}
}
