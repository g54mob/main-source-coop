using System;
using System.Collections;
using UnityEngine;

namespace pworld.Scripts
{
	public class PDeathDestroy : MonoBehaviour
	{
		public float delay;

		private PHealth health_g;

		private void Awake()
		{
			health_g = GetComponent<PHealth>();
			PHealth pHealth = health_g;
			pHealth.OnDiedLate = (Action)Delegate.Combine(pHealth.OnDiedLate, (Action)delegate
			{
				StartCoroutine(KillMe());
			});
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		private IEnumerator KillMe()
		{
			if (delay <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return new WaitForSeconds(delay);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
