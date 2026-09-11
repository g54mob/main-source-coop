using UnityEngine;

namespace pworld.Scripts
{
	public class PPositionalShake : MonoBehaviour
	{
		public Vector3 velocity;

		public float spring = 15f;

		public float damp = 15f;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			_ = Time.deltaTime;
			velocity = FRILerp.Lerp(velocity, -base.transform.localPosition * spring, damp, useTimeScale: false);
			base.transform.position += velocity * Time.deltaTime;
		}

		private void OnDestroy()
		{
		}
	}
}
