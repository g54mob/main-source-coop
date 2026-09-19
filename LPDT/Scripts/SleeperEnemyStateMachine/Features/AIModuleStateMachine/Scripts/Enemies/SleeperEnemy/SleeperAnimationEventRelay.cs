using FMODUnity;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public class SleeperAnimationEventRelay : MonoBehaviour
	{
		[SerializeField]
		private EventReference _investigateRoarReference;

		[SerializeField]
		private EventReference _stepReference1;

		[SerializeField]
		private EventReference _attackHitReference;

		[SerializeField]
		private ParticleSystem _particleSystem;

		[SerializeField]
		private ParticleSystem _foamParticleSystem;

		private SleeperEnemyContext _context;

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context)
		{
			_context = context;
		}

		public void OnInvestigateScream()
		{
			if (!(_context == null))
			{
				_context.PlayOccludedOneShot(_investigateRoarReference);
				_foamParticleSystem.Play(withChildren: true);
			}
		}

		public void OnStep()
		{
			if (!(_context == null))
			{
				_context.PlayOccludedOneShot(_stepReference1);
				_particleSystem.Play(withChildren: true);
			}
		}

		public void OnAttackHit()
		{
			if (!(_context == null))
			{
				_context.PlayOccludedOneShot(_attackHitReference);
			}
		}
	}
}
