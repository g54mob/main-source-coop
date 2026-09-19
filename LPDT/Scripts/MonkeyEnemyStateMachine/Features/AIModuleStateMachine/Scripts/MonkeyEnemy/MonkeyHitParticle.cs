using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyHitParticle : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystem;

		private void OnEnable()
		{
			Play();
		}

		public void Play()
		{
			_particleSystem.Play();
		}
	}
}
