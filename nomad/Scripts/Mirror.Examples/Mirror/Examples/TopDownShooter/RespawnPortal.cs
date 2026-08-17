using System.Collections;
using UnityEngine;

namespace Mirror.Examples.TopDownShooter
{
	public class RespawnPortal : MonoBehaviour
	{
		public float rotationSpeed = 360f;

		public float shrinkDuration = 1f;

		public AudioSource soundEffect;

		private Vector3 originalScale;

		private float shrinkTimer;

		private void Awake()
		{
			originalScale = base.transform.localScale;
			shrinkTimer = shrinkDuration;
		}

		private void OnEnable()
		{
			base.transform.localScale = originalScale;
			shrinkTimer = shrinkDuration;
			StartCoroutine(StartEffect());
		}

		private IEnumerator StartEffect()
		{
			soundEffect.Play();
			while (shrinkTimer > 0f)
			{
				base.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
				if (shrinkTimer > 0f)
				{
					shrinkTimer -= Time.deltaTime;
					float num = Mathf.Clamp01(shrinkTimer / shrinkDuration);
					base.transform.localScale = originalScale * num;
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
}
