using UnityEngine;
using UnityEngine.Serialization;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PScaleShake : MonoBehaviour, IPForceTaker
	{
		public bool useTimeScale;

		public Vector3 velocity;

		[FormerlySerializedAs("drag")]
		public float damp = 2f;

		public float spring = 2f;

		private Vector3 localIdleScale = Vector3.one;

		private RectTransform rectT;

		private float scaleFactor;

		private Vector3 tarInLocal;

		public Vector3 TarInLocal
		{
			get
			{
				return tarInLocal;
			}
			set
			{
				tarInLocal = value;
				scaleFactor = tarInLocal.magnitude / localIdleScale.magnitude;
			}
		}

		public Vector3 LocalIdleScale
		{
			get
			{
				return localIdleScale;
			}
			set
			{
				localIdleScale = value;
				ScaleFactor = scaleFactor;
			}
		}

		public float ScaleFactor
		{
			get
			{
				return scaleFactor;
			}
			set
			{
				scaleFactor = value;
				TarInLocal = LocalIdleScale * scaleFactor;
			}
		}

		private void Awake()
		{
			LocalIdleScale = base.transform.localScale;
			TarInLocal = LocalIdleScale;
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			velocity = FRILerp.Lerp(velocity, (tarInLocal - base.transform.localScale) * spring, damp, useTimeScale);
			if ((bool)rectT)
			{
				rectT.localScale += velocity * deltaTime;
			}
			else
			{
				base.transform.localScale += velocity * deltaTime;
			}
		}

		public void AddForce(Vector3 force)
		{
			velocity += force;
		}

		public void SetTarByFactor(float f)
		{
			TarInLocal = LocalIdleScale * f;
		}
	}
}
