using UnityEngine;

namespace pworld.Scripts
{
	public class PParticleDestroy : MonoBehaviour
	{
		private ParticleSystem ps_g;

		private void Awake()
		{
			ps_g = GetComponent<ParticleSystem>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (!ps_g.isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
		}
	}
}
