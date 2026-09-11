using UnityEngine;

namespace pworld.Scripts
{
	public class PKillAfterTime : MonoBehaviour, PTimerReset
	{
		public float time;

		public float timeLeft;

		private void Awake()
		{
			Reset();
		}

		private void Update()
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}

		public void Reset()
		{
			timeLeft = time;
		}
	}
}
