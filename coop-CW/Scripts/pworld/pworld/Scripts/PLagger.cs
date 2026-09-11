using System.Diagnostics;
using UnityEngine;

namespace pworld.Scripts
{
	public class PLagger : MonoBehaviour
	{
		public float pow = 1f;

		public int octaves;

		public float strength;

		public float time;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; (float)i < Mathf.Pow(Perlin.Fbm(Time.realtimeSinceStartup, octaves), pow) * strength; i++)
			{
				UnityEngine.Debug.LogWarning("sdf");
			}
			stopwatch.Stop();
			time = Mathf.Lerp(time, stopwatch.ElapsedMilliseconds, 0.016f);
		}
	}
}
